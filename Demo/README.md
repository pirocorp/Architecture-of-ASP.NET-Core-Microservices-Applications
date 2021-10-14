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

Identity service is identity micro service

```yml
  identity: 
```

Identity service must have docker file. Can be created from visual studio. Right button over microservice assembly add docker support and choose docker os. Generated docker file for identity micro service

```dockerfile
# First Stage
# A Dockerfile must begin with a FROM instruction. Specifies the “base image” to use to build our image.
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# Sets the directory as the working directory for all following commands, 
# so every command is run from inside the app directory.
WORKDIR /app
# Expose exposes the given port outside the container.
EXPOSE 80
EXPOSE 443

# Second Stage
# Specifies the SDK for the base image to use.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# Changes workdir to src
WORKDIR /src
# COPY <src> <dest>
COPY ["CarRentalSystem.Identity/CarRentalSystem.Identity.csproj", "CarRentalSystem.Identity/"]
COPY ["CarRentalSystem.Common/CarRentalSystem.Common.csproj", "CarRentalSystem.Common/"]
# Executes command restore - restores all NuGet packages.
RUN dotnet restore "CarRentalSystem.Identity/CarRentalSystem.Identity.csproj"
# The COPY . . copies the entire project, recursively into the container for the build.
COPY . .
# Changes workdir to /src/CarRentalSystem.Identity
WORKDIR "/src/CarRentalSystem.Identity"
# Executes command build with output dir /app/build
RUN dotnet build "CarRentalSystem.Identity.csproj" -c Release -o /app/build

# Third Stage continious previous stage and renameing it to publish
FROM build AS publish
# Executes command publish with output dir /app/publish
RUN dotnet publish "CarRentalSystem.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Identity.dll"]
```

Networks are used to allow comunications between containers. Only containers in one network can comunicate with each other freely.

```yml
networks:
  carrentalsystem-network:
```

Volumes are persistance storages for containers because containers are immutable.
```yml
volumes:
  sqldata:
```
