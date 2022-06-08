# Deploy nginx Ingress Load Balancer

To deploy Deploy nginx Ingress Load Balancer run the following command

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.2.0/deploy/static/provider/cloud/deploy.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172620366-9d8b619a-aa1c-4356-a516-4734313ac0f0.png)

![image](https://user-images.githubusercontent.com/34960418/172621855-62d723f5-3317-47c9-87f1-81520ccc9323.png)


# Configure nginx Ingress Load Balancer

Create ```ingress-srv.yaml``` file and put the following code in it:

```yaml
apiVersion: networking.k8s.io/v1
# An API object that manages external access to the services in a cluster, typically HTTP.
# Ingress may provide load balancing, SSL termination and name-based virtual hosting.
kind: Ingress
metadata:
  name: ingress-srv
  annotations: 
    kubernetes.io/ingress.class: nginx
    nginx.ingress.kubernetes.io/use-regex: 'true'
spec:
  # Simple fanout
  # A fanout configuration routes traffic from a single IP address to more than one Service, 
  # based on the HTTP URI being requested. An Ingress allows you to keep the number 
  # of load balancers down to a minimum.
  rules:
  # Each HTTP rule contains the following information:
  # An optional host. If no host is specified, so the rule applies to all inbound HTTP traffic 
  # through the IP address specified. If a host is provided, the rules apply to that host.
    - host: pirocorp.com
      http: 
      # A list of paths, each of which has an associated backend defined with a service.name and
      # a service.port.name or service.port.number. Both the host and path must match the content
      # of an incoming request before the load balancer directs traffic to the referenced Service.
        paths:
          - path: /api/platforms
            pathType: Prefix
            backend:
              service:
                name: platforms-clusterip-srv
                port: 
                  number: 80
          - path: /api/c/platforms
            pathType: Prefix
            backend:
              service:
                name: commands-clusterip-srv
                port:
                  number: 80
```


Edit your hosts file (for windows users the file is located in C:\Windows\System32\drivers\etc) and add following text in it

```
# Docker Desktop Kubernetes Workshop
127.0.0.1 pirocorp.com
```

![image](https://user-images.githubusercontent.com/34960418/172630536-6b3626b1-6c95-4cb2-89f3-15a8b9c8ca5b.png)


Check the name resolution for `pirocorp.com`

```bash
ping pirocorp.com
```

![image](https://user-images.githubusercontent.com/34960418/172631341-85d13c25-b72d-469f-937b-17f43a1ed5e2.png)


Apply ```ingress-srv.yaml``` to kubernetes

```bash
kubectl apply -f .\ingress-srv.yaml
```

![image](https://user-images.githubusercontent.com/34960418/172632390-25292c7a-fab5-4ccf-b0ef-d87c83e1ee4d.png)



