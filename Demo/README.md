# Creating Docker Compose File

Defines version of the yml file

```yml
version: "3.8"
```

Services are all managed containers

```yml
services:
```

Data service in this service will be sqlserver. 
```yml
data:
  # Set container name comunication between services (containers) is by name
  container_name: sqlserver  
  # Specifies which image to be used for the container (images are like blueprints)
  image: mcr.microsoft.com/mssql/server:2019-latest
  # Docker host port will be mapped to container port (out:in). 
  # Docker host port (out) will be exposed for comunication.
  ports:
    - "1433:1433"
  # Environment variables
  environment:
    - ACCEPT_EULA=Y
    - SA_PASSWORD=yourStrongPassword12!@
  volumes:
    # volume sqldata is mapped to /var/opt/mssql folder in the container.
    - sqldata:/var/opt/mssql
  networks:
    # this container (service) is part of carrentalsystem-network
    - carrentalsystem-network
```

Networks are used to allow comunications between containers. Only containers in one network can comunicate with each other freely.

```yml
networks:
```

Volumes are persistance storages for containers because containers are immutable.
```yml
volumes:
  sqldata:
```
