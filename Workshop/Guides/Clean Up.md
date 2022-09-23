# Clean Up

## [Configure `kustomization.yaml`](Merge%20Multiple%20YAML%20Files%20Into%20One.md)


## Delete all yaml configurations

Execute the following command

```bash
kubectl delete -k ./K8s
```

![image](https://user-images.githubusercontent.com/34960418/191967184-76dbbad8-f2e9-42db-9a45-87e1c79a3ca9.png)


## Check pods, services

```bash
kubectl get pods,svc
```

![image](https://user-images.githubusercontent.com/34960418/191967624-32eb896a-960d-46fa-abf9-b968239af0a0.png)


## Check secrets

The secrets were created with `kubectl create secret` and need to be removed manually.

```bash
kubectl get secrets
```

![image](https://user-images.githubusercontent.com/34960418/191967785-47a5f355-47ad-46a8-9399-6143cf889131.png)


## Remove secrets

```bash
kubectl delete secret mssql-secret
kubectl delete secret commands-mssql-secret
```

## Check all

![image](https://user-images.githubusercontent.com/34960418/191968521-9ef9ab68-9c61-4f12-ab0f-da5f1d3660b0.png)
