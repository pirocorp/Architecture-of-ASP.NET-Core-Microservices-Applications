# Update `platforms-deployment.yaml` to following code:

```yaml
apiVersion: apps/v1
# A Deployment provides declarative updates for Pods and ReplicaSets.
# You describe a desired state in a Deployment, and the Deployment Controller 
# changes the actual state to the desired state at a controlled rate. 
# You can define Deployments to create new ReplicaSets, or to remove existing 
# Deployments and adopt all their resources with new Deployments.
kind: Deployment
# Deployment name and other metadata
metadata: 
  name: platforms-deployment
spec:
  # The desired pod quantity
  replicas: 1
  # For which pods is this deployment
  selector: 
    matchLabels:
      app: platformservice
  # "Hidden" pod specification
  template:
    metadata:
      labels:
        app: platformservice
    spec:
      containers:
        - name: platformservice
          image: pirocorp/workshop-platformservice:latest
          env:
          # This environment variable will be provided from the secret mssql-secret
            - name: SA_PASSWORD
              valueFrom:
                secretKeyRef: 
                  name: mssql-secret
                  key: SA_PASSWORD
            - name: ConnectionStrings__PlatformsConnection
              value: Server=mssql-clusterip-srv,1433;Initial Catalog=PlatformServiceDb;User Id=sa;Password=$(SA_PASSWORD);
---
apiVersion: v1
# An abstract way to expose an application running on a set of Pods as a network service.
kind: Service
# Service name and other metadata
metadata:
  name: platforms-clusterip-srv
spec:
  # Exposes the Service on a cluster-internal IP. Choosing this value makes the Service only reachable from within the cluster. This is the default ServiceType
  type: ClusterIP
  selector:
    app: platformservice
  ports:
  - name: platforms-clusterip-srv-port
    protocol: TCP
    port: 80
    targetPort: 80
```

Depploy with following commands:

```bash
kubectl apply -f  .\platforms-deployment.yaml
kubectl rollout restart deployment platforms-deployment
```

![image](https://user-images.githubusercontent.com/34960418/172683487-f2f3a9cb-86b7-45b1-bff2-576e37b1e8f9.png)


# Chack database for initialized data

![image](https://user-images.githubusercontent.com/34960418/172683648-43b8f7cc-a69e-4bbf-a038-6e79d19cc895.png)
