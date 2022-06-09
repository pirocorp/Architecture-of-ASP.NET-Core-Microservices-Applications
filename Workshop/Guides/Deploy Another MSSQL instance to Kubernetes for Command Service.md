# Create secret for admin password

```bash
kubectl create secret generic commands-mssql-secret --from-literal=SA_PASSWORD="Secur1ty!"
```

![image](https://user-images.githubusercontent.com/34960418/172856350-1a99844b-56c2-43f9-9c49-0f3e76ed905e.png)


# Deploy MSSQL to Kubernetes

Create `mssql-commands-deployment.yaml` and enter the following code:

```yaml
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: commands-mssql-claim
spec:
  accessModes:
    - ReadWriteMany
  resources:
    requests:
      storage: 200Mi
---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: commands-mssql-deployment
spec:
  replicas: 1
  selector:
    matchLabels:
      app: commands-mssql
  template:
    metadata:
      labels:
        app: commands-mssql
    spec:
      containers:
        - name: commands-mssql
          image: mcr.microsoft.com/mssql/server:2019-latest
          ports:
            - containerPort: 1433
          env:
          - name: MSSQL_PID
            value: "Express"
          - name: ACCEPT_EULA
            value: "Y"
          - name: SA_PASSWORD
            valueFrom:
              secretKeyRef: 
                name: commands-mssql-secret
                key: SA_PASSWORD
          volumeMounts:
          - mountPath: /var/opt/mssql/data
            name: mssqldb
      volumes:
      - name: mssqldb
        persistentVolumeClaim: 
          claimName: commands-mssql-claim
---
apiVersion: v1
kind: Service
metadata:
  name: commands-mssql-clusterip-srv
spec:
  type: ClusterIP
  selector:
    app: commands-mssql
  ports:
  - name: commands-mssql-clusterip-port
    protocol: TCP
    port: 1433
    targetPort: 1433
---
apiVersion: v1
kind: Service
metadata:
  name: commands-mssql-loadbalancer
spec:
  type: LoadBalancer
  selector:
    app: commands-mssql
  ports:
  - name: commands-mssql-loadbalancer-port
    protocol: TCP
    port: 1444
    targetPort: 1433
```


Deploy `mssql-commands-deployment.yaml` to Kubernetes

```bash
kubectl apply -f .\mssql-commands-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172859266-b86e2257-6c6c-432e-b2f3-4e5bbcbf12a2.png)

![image](https://user-images.githubusercontent.com/34960418/172861386-1a6826fe-1944-4623-9140-5f8cd9b8f16d.png)


# Update Platform Service To Use MSSQL In Kubernetes

Update `commands-deployment.yaml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata: 
  name: commands-deployment
spec:
  replicas: 1
  selector: 
    matchLabels:
      app: commandservice
  template:
    metadata:
      labels:
        app: commandservice
    spec:
      containers:
        - name: commandservice
          image: pirocorp/workshop-commandservice:latest
          env:
            - name: SA_PASSWORD
              valueFrom:
                secretKeyRef: 
                  name: commands-mssql-secret
                  key: SA_PASSWORD
            - name: ConnectionStrings__CommandsConnection
              value: Server=commands-mssql-clusterip-srv,1433;Initial Catalog=CommandServiceDb;User Id=sa;Password=$(SA_PASSWORD);
---
apiVersion: v1
kind: Service
metadata:
  name: commands-clusterip-srv
spec:
  type: ClusterIP
  selector:
    app: commandservice
  ports:
  - name: commands-clusterip-srv-port
    protocol: TCP
    port: 80
    targetPort: 80
```

# Update Command Service Project

Update ConfigureServices method to use MSSQL

```C# 
services.AddDbContext<CommandDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("CommandsConnection"));
});
```

# Create migrations

In order to create migrations use this connection string `"Server=commands-mssql-clusterip-srv,1444;Initial Catalog=CommandServiceDb;User Id=sa;Password=Secur1ty!;"`

Run the following command to create migrations

```bash
dotnet ef migrations add Initial
```

![image](https://user-images.githubusercontent.com/34960418/172866834-7399bdfb-35f9-48e4-8263-b05befbed86c.png)


# Build image and push it to Docker Hub

Build docker image again

```bash
docker build -t pirocorp/workshop-commandservice -f . ../
```

![image](https://user-images.githubusercontent.com/34960418/172865261-2cfc31f9-5f50-4c41-8f65-11d2856039bb.png)


Push it to the docker hub

```bash
docker push pirocorp/workshop-commandservice
```

![image](https://user-images.githubusercontent.com/34960418/172865539-7c1f8578-a8bc-40f9-b9ab-086a5a248961.png)


# Redeploy commands service in kubernetes with following commands:

```bash
kubectl apply -f .\commands-deployment.yaml
kubectl rollout restart deployment commands-deployment
```

![image](https://user-images.githubusercontent.com/34960418/172865839-c2387fb4-1fe9-402f-90eb-a0360ab26e8d.png)


# Check if CommandServiceDb is created

![image](https://user-images.githubusercontent.com/34960418/172868499-9c53ceee-9413-4e79-beb8-ed8047f3246a.png)
