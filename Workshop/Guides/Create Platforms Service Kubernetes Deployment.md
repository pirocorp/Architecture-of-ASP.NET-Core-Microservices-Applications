# Create Platforms Kubernetes Deployment

## Create ```platforms-deployment.yaml``` file

Create in your solution folder subfolder named ```K8s``` and put in it file ```platforms-deployment.yaml```. In the file, paste the following code:

```yaml
apiVersion: apps/v1
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
    targetport: 80
```


## Run the deployment in Docker desktop with enabled 1 node kubernetes cluster. 

Open Docker Desktop ang got to settings menu.

![image](https://user-images.githubusercontent.com/34960418/172050884-16d40fe9-8b0a-49dc-92b9-d9185b207272.png)


Make sure the "Enable Kubernetes" option is enabled. Otherwise, select it and click on the Apply & Restart button.

![image](https://user-images.githubusercontent.com/34960418/172050931-eca9d2fc-f945-4089-974d-22cf8ae555d2.png)


After a while, both docker and Kubernetes indicators will turn green.

![image](https://user-images.githubusercontent.com/34960418/172051081-ea205f02-ab45-4cea-a49a-e215accb9781.png)


To see all Kubernetes contexts and the active one, run the following command:

```bash
kubectl config get-contexts
```

![image](https://user-images.githubusercontent.com/34960418/172051236-143da098-9480-418b-ae60-6cea9285a9cc.png)


To change to Docker Desktop context, run:

```bash
kubectl config use-context docker-desktop
```

![image](https://user-images.githubusercontent.com/34960418/172051295-ea81a483-7e63-46fa-88c4-f7a0e7ab9e9e.png)


Verify that the context is Docker Desktop with: 

```bash
kubectl config get-contexts
```

![image](https://user-images.githubusercontent.com/34960418/172051339-707241ff-bf3b-4633-90fa-1aca6e897e3d.png)


The ```kubectl``` should be +/- 1 minor version from the docker desktop version.

```bash
kubectl version
```

![image](https://user-images.githubusercontent.com/34960418/172051405-c120d4f3-0c1e-4073-94a5-f1cb87d7ed84.png)


Run the following command to see all nodes in the Docker Desktop's Kubernetes cluster:

```bash
kubectl get nodes
```

![image](https://user-images.githubusercontent.com/34960418/172051651-571a2d08-9e6c-4cd7-8b7d-4c9bc93aebdc.png)


Deploy ```platforms-deployment.yaml``` to Kubernetes

```bash
kubectl apply -f platforms-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172052050-d549b90c-0a9d-4674-a158-f46cdd80ecda.png)


Check deployment with:

```bash
kubectl get deployments
```

![image](https://user-images.githubusercontent.com/34960418/172052125-8b00ca51-bc65-48de-b76e-361d60da170d.png)


Check pods with:

```bash
kubectl get pods
```

![image](https://user-images.githubusercontent.com/34960418/172052261-d8f95693-6970-43f9-aca5-9ad1fb860c86.png)


Delete deployment with:

```bash
kubectl delete deployment platforms-deployment
```

![image](https://user-images.githubusercontent.com/34960418/172052861-ea364039-22f9-49b7-aeba-b8417c1e8858.png)

or 

```bash
kubectl delete -f .\platforms-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172052933-cfd931df-6cb4-43fe-9793-e161919c21a6.png)
