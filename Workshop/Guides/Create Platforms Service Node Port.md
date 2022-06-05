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
      port: 80
      targetPort: 80
```
