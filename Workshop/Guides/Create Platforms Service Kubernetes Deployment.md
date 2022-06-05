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
