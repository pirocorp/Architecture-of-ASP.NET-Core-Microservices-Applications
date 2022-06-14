# Open `platforms-deployment.yaml`file, and for the `ClusterIP` service in the `ports` section, add another point that gRPC will use.

```yaml
apiVersion: v1
kind: Service
metadata:
  name: platforms-clusterip-srv
spec:
  type: ClusterIP
  selector:
    app: platformservice
  ports:
  - name: platforms-clusterip-srv-port
    protocol: TCP
    port: 80
    targetPort: 80
  # port that will be used by gRPC
  - name: platforms-grpc-port
    protocol: TCP
    port: 555
    targetPort: 555
```


# Open `appsettings.Production.json` in the Platform Service project. Add the following configuration for the Kestrel web server.

```json
  "Kestrel": {
    "Endpoints": {
      "Grpc": {
        "Protocols": "Http2",
        "Url": "http://platforms-clusterip-srv:555"
      },
      "WebApi": {
        "Protocols": "Http1",
        "Url": "http://platforms-clusterip-srv:80"
      }
    }
  }
```
