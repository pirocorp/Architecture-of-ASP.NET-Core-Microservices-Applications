
For multiple hosts if the cluster uses Kubernetes version >= 1.19.x, then its suggested to create 2 ingress resources, using yaml examples shown below. These examples are in conformity with the `networking.kubernetes.io/v1` api.


- [Create an ingress controller in Azure Kubernetes Service (AKS)](https://docs.microsoft.com/en-us/azure/aks/ingress-basic?tabs=azure-cli)
- [NGINX Ingress Controller Basic usage Host Based Routing](https://kubernetes.github.io/ingress-nginx/user-guide/basic-usage/)
