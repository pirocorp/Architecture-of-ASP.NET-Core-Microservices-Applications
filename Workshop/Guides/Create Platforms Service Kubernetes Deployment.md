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
