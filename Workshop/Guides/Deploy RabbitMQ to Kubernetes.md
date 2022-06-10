# Deploy RabbitMQ to Kubernetes

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


Run the following command to deploy to Kubernetes

```bash
kubectl apply -f .\rabbitmq-deployment.yaml
```

![image](https://user-images.githubusercontent.com/34960418/173067476-86d22438-a3c9-44f1-9be6-be1def046ef2.png)

![image](https://user-images.githubusercontent.com/34960418/173067716-738178d1-6f71-412a-98d9-506c7d4ddefd.png)


# Test RabbitMQ

Open `http://localhost:15672/`. Use the default username: `guest` and password: `guest`

![image](https://user-images.githubusercontent.com/34960418/173069689-d828ed1f-affb-45a9-ae34-add060e54992.png)

![image](https://user-images.githubusercontent.com/34960418/173069897-8691b19e-37ed-4806-8c49-ae01f2132b51.png)


# Configure PlatformService and CommandService to use RabbitMQ

In `appsettings.Development.json`, add the following code:

```json
"RabbitMQ": {
  "Host": "localhost",
  "Port":  5672
}
```

In `appsettings.Production.json` host will be the name of RabbitMQ's ClusterIP service. 

```json
"RabbitMQ": {
  "Host": "rabbitmq-clusterip-srv",
  "Port": 5672
}
```

Add configuration object class `RabbitMqOptions`.

```csharp
public class RabbitMqOptions
{
    public const string RabbitMq = "RabbitMQ";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }
}
```

In the startup/program file, add the following code:

```csharp
IConfigurationSection rabbitMqOptions = configuration.GetSection(RabbitMqOptions.RabbitMq);
services.Configure<RabbitMqOptions>(rabbitMqOptions);
```

Where RabbitMQ needs configuration, inject the following type:

```csharp
public Service(IOptions<RabbitMqOptions> rabitMqOptions) 
{
  this.rabitMqOptions = rabitMqOptions;
}
```
