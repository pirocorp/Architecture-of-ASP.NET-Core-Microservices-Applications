# Create Platforms Kubernetes Deployment

Create in your solution folder subfolder named ```K8s``` and put in it file ```platforms-deployment.yaml```. In the file paste following code:

```yaml
apiVersion: apps/v1
kind: Deployment
# Deployment name
metadata: 
  name: platforms-deployment
spec:
  # Desired pods quantity
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
