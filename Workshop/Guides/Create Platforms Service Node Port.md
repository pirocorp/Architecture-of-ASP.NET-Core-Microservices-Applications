# Create Platforms Service Node Port

## Create ```platforms-np-srv.yaml``` file. 

In folder ```K8s``` and put in it file ```platforms-np-srv.yaml```. In the file, paste the following code:

```yaml
apiVersion: v1
kind: Service
# Service name and other metadata
metadata:
  name: platforms-np-srv
spec:
  # A node port exposes the service on a static port on the node IP address. NodePorts are in the 30000-32767 range by default
  type: NodePort
  # The set of Pods targeted by a Service is usually determined by a selector.
  selector:
    app: platformservice
  ports: 
    - name: platforms-np-srv-port
      protocol: TCP
      # External port
      nodePort: 32000
      # Internal port
      port: 80
      # Pod port
      targetPort: 80
```


## Deploy ```platforms-np-srv.yaml``` to Kubernetes

```bash
kubectl apply -f platforms-np-srv.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172363556-c6fee1eb-cfd1-49ec-bbbd-42aa96f3d2b3.png)


Check services with:

```bash
 kubectl get services
```

![image](https://user-images.githubusercontent.com/34960418/172364010-efc4c776-4563-4c99-869b-9e2dbfa75c7a.png)
