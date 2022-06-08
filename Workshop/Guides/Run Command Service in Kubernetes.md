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

In ```K8s``` folder create new file ```commands-deployment.yaml``` and paste the following code:

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
