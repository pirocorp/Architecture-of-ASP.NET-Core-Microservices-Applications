Create `rabbitmq-deployment.yaml` and put the following code in it.

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: rabbitmq-deployment
spec:
  replicas: 1
  selector:
    matchLabels:
      app: rabbitmq
  template:
    metadata:
      labels:
        app: rabbitmq
    spec:
      containers:
        - name: rabbitmq
          image: rabbitmq:3-management
          ports:
            - containerPort: 15672
              name: rabbitmq-management-port
            - containerPort: 5672
              name: rabbitmq-messaging-port
---
apiVersion: v1
kind: Service
metadata:
  name: rabbitmq-clusterip-srv
spec:
  type: ClusterIP
  selector:
    app: rabbitmq
  ports:
  - name: rabbitmq-management-port
    protocol: TCP
    port: 15672
    targetPort: 15672
  - name: rabbitmq-messaging-port
    protocol: TCP
    port: 5672
    targetPort: 5672
---
apiVersion: v1
kind: Service
metadata:
  name: rabbitmq-loadbalancer
spec:
  type: LoadBalancer
  selector:
    app: rabbitmq
  ports:
  - name: rabbitmq-management-port
    protocol: TCP
    port: 15672
    targetPort: 15672
  - name: rabbitmq-messaging-port
    protocol: TCP
    port: 5672
    targetPort: 5672
```
