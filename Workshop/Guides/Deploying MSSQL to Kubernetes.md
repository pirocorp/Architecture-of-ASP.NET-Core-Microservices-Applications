# Create PVC (Persistent Volume Claim) for MSSQL

Create file ```local-pvc.yaml``` and paste the following code.

```yaml
apiVersion: v1
# A PersistentVolumeClaim (PVC) is a request for storage by a user and consumes PersistentVolume. 
# A PersistentVolume (PV) is a piece of storage in the cluster that has been provisioned by an administrator 
# or dynamically provisioned using Storage Classes. It is a resource in the cluster just like a node is a 
# cluster resource. PVs are volume plugins like Volumes, but have a lifecycle independent of any individual 
# Pod that uses the PV.
kind: PersistentVolumeClaim
metadata:
  name: mssql-claim
spec:
  # the volume can be mounted as read-write by many nodes.
  accessModes:
    - ReadWriteMany
  resources:
    requests:
      storage: 200Mi
```


Deploy to kubernetes

```bash
kubectl apply -f .\local-pvc.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172647606-58507864-154b-406f-8332-cd9faf0387f7.png)

![image](https://user-images.githubusercontent.com/34960418/172647797-35491577-0a6d-417c-ad1b-4945f6a1ec86.png)


# Create secret for admin password

```bash
kubectl create secret generic mssql-secret --from-literal=SA_PASSWORD="pa55w0rd!"
```

![image](https://user-images.githubusercontent.com/34960418/172649477-d3454e17-2c0e-43c7-9a38-67f503ee9ef4.png)


# Deploy MSSQL to Kubernetes

Create ```mssql-platform-deployment.yaml``` and enter the following code:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-deployment
spec:
  replicas: 1
  selector:
    matchLabels:
      app: mssql
  template:
    metadata:
      labels:
        app: mssql
    spec:
      containers:
        - name: mssql
          image: mcr.microsoft.com/mssql/server:2019-latest
          ports:
            - containerPort: 1433
          # Environment variables
          env:
          - name: MSSQL_PID
            value: "Express"
          - name: ACCEPT_EULA
            value: "Y"
          # This environment variable will be provided from the secret mssql-secret
          - name: SA_PASSWORD
            valueFrom:
              secretKeyRef: 
                name: mssql-secret
                key: SA_PASSWORD
          volumeMounts:
          # SQL Server database location
          - mountPath: /var/opt/mssql/data
            name: mssqldb
      volumes:
      - name: mssqldb
        # Consuming the claim
        persistentVolumeClaim: 
          claimName: mssql-claim
---
# Cluster IP service exposes MSSQL inside the cluster.
apiVersion: v1
kind: Service
metadata:
  name: mssql-clusterip-srv
spec:
  type: ClusterIP
  selector:
    app: mssql
  ports:
  - name: mssql-clusterip-srv-port
    protocol: TCP
    port: 1433
    targetPort: 1433
---
# Load Balancer exposes MSSQL outside the cluster.
apiVersion: v1
kind: Service
metadata:
  name: mssql-loadbalancer
spec:
  type: LoadBalancer
  selector:
    app: mssql
  ports:
  - name: mssql-loadbalancer-srv-port
    protocol: TCP
    port: 1433
    targetPort: 1433
```


Deploy ```mssql-platform-deployment.yaml``` to Kubernetes

```bash
kubectl apply -f .\mssql-platform-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172658660-c022b995-0843-4bd4-a031-1484e1f7c20c.png)

![image](https://user-images.githubusercontent.com/34960418/172659681-66ff698b-cc5c-4b02-8049-732c5852e4ca.png)


