## Load Balancer vs Ingress

> Ingress Controller Services are often provisioned as LoadBalancer type, so that http and https requests can be proxied / routed to specific internal services through an external ip.

Ingress-nginx is not a load balancer but on a broader lever can help you with load balancing.

But there are two things-

1) ingress controller - which is a traffic management solution I would say. Yes it manages the traffic using path based or host based routing. yes it goes to a cluster IP at the backend which again does a load balancing among different pods.
2) ingress- rules written to define how the traffic is to be managed. basically, it tells nginx ingress controller to send traffic to which IP at the backend.

As mentioned [here](https://docs.microsoft.com/en-us/azure/aks/concepts-network#ingress-controllers) as well:

> When you create a LoadBalancer type Service, an underlying Azure load balancer resource is created. The load balancer is configured to distribute traffic to the pods in your Service on a given port. The LoadBalancer only works at layer 4 - the Service is unaware of the actual applications, and can't make any additional routing considerations.
> 
> Ingress controllers work at layer 7, and can use more intelligent rules to distribute application traffic. A common use of an Ingress controller is to route HTTP traffic to different applications based on the inbound URL.

> Either we use path based or host based routing and it always reaches to a specific cluster ip behind which load balancing occurs.

> Because, If it is acting just as a proxy then for any service(especially for gRPC) we can easily replace ingress-nginx and any service mesh with envoy.

More info here as well:

https://github.com/kubernetes/ingress-nginx

https://kubernetes.io/docs/concepts/services-networking/ingress/


## [NGINX Ingress Controller Basic usage Host Based Routing](https://kubernetes.github.io/ingress-nginx/user-guide/basic-usage/)

For multiple hosts if the cluster uses Kubernetes version >= 1.19.x, then its suggested to create 2 ingress resources, using yaml examples shown below. These examples are in conformity with the `networking.kubernetes.io/v1` api.

- [Ingress](https://kubernetes.io/docs/concepts/services-networking/ingress/)
- [Create an ingress controller in Azure Kubernetes Service (AKS)](https://docs.microsoft.com/en-us/azure/aks/ingress-basic?tabs=azure-cli)
- [NGINX Ingress Controller Basic usage Host Based Routing](https://kubernetes.github.io/ingress-nginx/user-guide/basic-usage/)
- [Is ingress-nginx a load balancer or not?](https://docs.microsoft.com/en-us/answers/questions/295210/is-ingress-nginx-is-really-a-load-balancer-or-not.html)
- [Ingress vs Load Balancer](https://stackoverflow.com/questions/45079988/ingress-vs-load-balancer#:~:text=Ingress%20Controller%20Services%20are%20often,not%20strictly%20needed%20for%20this.)
- [What is Application Gateway Ingress Controller?](https://docs.microsoft.com/en-us/azure/application-gateway/ingress-controller-overview)
- [Enable the Ingress Controller add-on for a new AKS cluster with a new Application Gateway instance](https://docs.microsoft.com/en-us/azure/application-gateway/tutorial-ingress-controller-add-on-new)
- [Network concepts for applications in Azure Kubernetes Service (AKS)](https://docs.microsoft.com/en-us/azure/aks/concepts-network#ingress-controllers)

