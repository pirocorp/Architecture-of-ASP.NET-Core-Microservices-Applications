# Dockerize the Command Service

Create Dockerfile in the root folder of the CommandService project with the following content. 

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj for the microservice and restore as distinct layers
COPY ./CommandService/*.csproj /app
COPY ./Rules.ruleset /
COPY ./stylecop.json /
RUN cd /app
RUN dotnet restore

# Copy csproj for the common library and restore as distinct layers
RUN mkdir /Common
COPY ./Common/*.csproj /Common
RUN cd /Common
RUN dotnet restore

# Copy everything else and build
COPY ./CommandService /app
COPY ./Common /Common
RUN cd /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CommandService.dll"]
```


Tag the docker image to your dockerhub and build it. In the root folder of your project, run the following command:

```bash
docker build -t pirocorp/workshop-commandservice -f . ../
```


Push the docker image to dockerhub

```bash
docker push pirocorp/workshop-commandservice
```


# Create ```commands-deployment.yaml``` file

In ```K8s``` folder, create a new file ```commands-deployment.yaml``` and paste the following code:

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
  name: commands-deployment
spec:
  # The desired pod quantity
  replicas: 1
  # For which pods is this deployment
  selector: 
    matchLabels:
      app: commandservice
  # "Hidden" pod specification
  template:
    metadata:
      labels:
        app: commandservice
    spec:
      containers:
        - name: commandservice
          image: pirocorp/workshop-commandservice:latest
---
apiVersion: v1
# An abstract way to expose an application running on a set of Pods as a network service.
kind: Service
# Service name and other metadata
metadata:
  name: commands-clusterip-srv
spec:
  # Exposes the Service on a cluster-internal IP. Choosing this value makes the Service only reachable from within the cluster. This is the default ServiceType
  type: ClusterIP
  selector:
    app: commandservice
  ports:
  - name: commands-clusterip-srv-port
    protocol: TCP
    port: 80
    targetPort: 80
```


# Deploy to Kubernetes

Apply ```platforms-deployment.yaml``` again with the following command

```bash
kubectl apply -f .\platforms-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172602287-25a6b510-9e64-4042-a0a8-cfda8c9e900d.png)


To force Kubernetes to redeploy the image and pull it from the docker hub use

```bash
kubectl rollout restart deployment
```

![image](https://user-images.githubusercontent.com/34960418/172608378-0f4c136d-1067-4ddf-bd6f-cf91c0b1151b.png)

![image](https://user-images.githubusercontent.com/34960418/172608547-8aad175c-5a2a-4c14-92dc-a26aecf6d7b1.png)


Deploy ```commands-deployment.yaml``` to Kubernetes

```bash
kubectl apply -f commands-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172609222-675d6742-f589-45e1-9afd-f0f24dcfd1e2.png)


# Test that deployment is correct and Platform Service is comunicating with Commands Service

![image](https://user-images.githubusercontent.com/34960418/172610144-44f5e8f4-8d51-4d8b-9ef3-801b29ec0a0d.png)

![image](https://user-images.githubusercontent.com/34960418/172610541-9754bbfd-ffeb-4e4d-acdc-cd0fe086a59c.png)

![image](https://user-images.githubusercontent.com/34960418/172610043-4bf82180-1d81-4866-ab4c-94edca44e6bf.png)
