# Merge Multiple YAML Files Into One


## Create a new file `kustomization.yaml` in the same folder.

```yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

resources:
  - platforms-deployment.yaml
  - platforms-np-srv.yaml
  - commands-deployment.yaml
  - https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.2.0/deploy/static/provider/cloud/deploy.yaml
  - ingress-srv.yaml
  - local-pvc.yaml
  - mssql-platform-deployment.yaml
  - mssql-commands-deployment.yaml
  - rabbitmq-deployment.yaml
```

## Compile into single file

```bash
kubectl kustomize build conf-files > compiled.yaml
```
