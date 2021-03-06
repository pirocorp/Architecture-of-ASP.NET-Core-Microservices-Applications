# Creating Docker Compose File

Defines version of the yml file

```yml
version: "3.8"
```

Services are all managed containers

```yml
services:
```

Data service (container) in this service will be sqlserver. 

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

Messages service (container) here will be RabbitMQ

```yml
messages:
  container_name: rabbitmq
  image: rabbitmq:3-management
  ports:
    - "15672:15672"
    - "5672:5672"
  hostname: rabbitmq
  environment:
    - RABBITMQ_ERLANG_COOKIE=Rand0mR4bbitMqCo0k1e
    - RABBITMQ_DEFAULT_USER=rabbitmquser
    - RABBITMQ_DEFAULT_PASS=rabbitmqPassword12!
  volumes:
    - rabbitmq:/var/lib/rabbitmq/mnesia
  networks:
    - carrentalsystem-network
```

Identity service (container)

```yml
identity:
  container_name: identity
  # build will build image from a docker file.
  build:
    # commands in your Dockerfile then act as if they are relative to the context 
    # regardless of where the Dockerfile actually is
    context: ./Server
    # dockerfile location relative to context
    dockerfile: ./CarRentalSystem.Identity/Dockerfile
  ports:
    - "5001:80"
  # Environment file location
  env_file: Server/CarRentalSystem.Common/Common.env
  # environment variables they can be accessed programabily from the application
  environment:
    # Connection string to sqlserver in the container
    - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=CarRentalDatabase.Identity;User Id=sa; Password=yourStrongPassword12!@;MultipleActiveResultSets=true
  # will restart service (container) if it fails
  restart: on-failure
  volumes:
    # From microsoft documentation fix Data Protection Keys storage warning.
    - ./.aspnet/identity/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
  networks:
    - carrentalsystem-network
  # the services (containers) on which this service (container) depends will be started first and when the services (containers) are running this container (service) will be started
  depends_on: 
    - data
```

Dealers service (container)

```yml
dealers:    
  container_name: dealers  
  build:
    context: ./Server
    dockerfile: ./CarRentalSystem.Dealers/Dockerfile
  ports:
    - "5003:80"
  env_file: Server/CarRentalSystem.Common/Common.env
  environment:
    - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=CarRentalDatabase.Dealers;User Id=sa; Password=yourStrongPassword12!@;MultipleActiveResultSets=true
  restart: on-failure
  volumes:
    - ./.aspnet/dealers/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
  networks:
    - carrentalsystem-network
  depends_on: 
    - data
```

Statistics service (container)

```yml
statistics:    
  container_name: statistics  
  build:
    context: ./Server
    dockerfile: ./CarRentalSystem.Statistics/Dockerfile
  ports:
    - "5005:80"
  env_file: Server/CarRentalSystem.Common/Common.env
  environment:
    - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=CarRentalDatabase.Statistics;User Id=sa; Password=yourStrongPassword12!@;MultipleActiveResultSets=true
  restart: on-failure
  volumes:
    - ./.aspnet/statistics/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
  networks:
    - carrentalsystem-network
  depends_on: 
    - data
```

Notification service (container)

```yml
notifications:
  container_name: notifications
  build:
      context: ./Server
      dockerfile: ./CarRentalSystem.Notifications/Dockerfile
  ports: 
      - "50011:80"
  env_file: 
      - Server/CarRentalSystem.Common/Common.env
  environment:
      - NotificationSettings__AllowedOrigins=http://localhost
      - MessageQueueSettings__Host=rabbitmq
      - MessageQueueSettings__UserName=rabbitmquser
      - MessageQueueSettings__Password=rabbitmqPassword12!
  restart: on-failure
  volumes:
      - ./.aspnet/notifications/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
  networks: 
      - carrentalsystem-network
  depends_on:
      - messages
```

Client service (container)

```yml
client:
  container_name: client
  build:
      context: ./Client
      dockerfile: ./Dockerfile
      args:
          configuration: development
  ports: 
      - "80:80"
  restart: on-failure
  depends_on:
      - identity
      - dealers
      - statistics
      - notifications
```

Admin service (container)

```yml
admin:
  container_name: admin
  build:
      context: ./Server
      dockerfile: ./CarRentalSystem.Admin/Dockerfile
  ports: 
      - "5000:80"
  env_file: Server/CarRentalSystem.Common/Common.env
  environment:
      - ServiceEndpoints__Identity=http://identity
      - ServiceEndpoints__Dealers=http://dealers
      - ServiceEndpoints__Statistics=http://statistics
  restart: on-failure
  volumes:
      - ./.aspnet/admin/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
  networks: 
      - carrentalsystem-network
  depends_on:
      - identity
      - dealers
      - statistics
```

Watchdog service (container)

```yml
watchdog:
  container_name: watchdog
  build:
      context: ./Server
      dockerfile: ./CarRentalSystem.Watchdog/Dockerfile
  ports: 
      - "5500:80"
  environment:
      - HealthChecks-UI__HealthChecks__0__Name=Identity
      - HealthChecks-UI__HealthChecks__0__Uri=http://identity/health
      - HealthChecks-UI__HealthChecks__1__Name=Dealers
      - HealthChecks-UI__HealthChecks__1__Uri=http://dealers/health
      - HealthChecks-UI__HealthChecks__2__Name=Statistics
      - HealthChecks-UI__HealthChecks__2__Uri=http://statistics/health
      - HealthChecks-UI__HealthChecks__3__Name=Notifications
      - HealthChecks-UI__HealthChecks__3__Uri=http://notifications/health
      - HealthChecks-UI__HealthChecks__4__Name=Admin
      - HealthChecks-UI__HealthChecks__4__Uri=http://admin/health
  restart: on-failure
  networks: 
      - carrentalsystem-network
  depends_on:
      - identity
      - dealers
      - statistics
      - notifications
      - admin
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
  rabbitmq:
```

### Dockerfiles

Can be created from visual studio. Right button over microservice assembly add docker support and choose docker os. 

Dockerfile generated by VisualStudio uses multistage builds. The multistage build allows container images to be created in stages that produce intermediate images.

#### Identity service dockerfile

```dockerfile
# First Stage
# A Dockerfile must begin with a FROM instruction. Specifies the ???base image??? to use to build our image.
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

# Final Stage continious initial stage and renameing it to final
FROM base AS final
# Changes workdir to /app
WORKDIR /app
# opies the contents of output directory from the publish stage into the /app/publish directory of final stage. 
COPY --from=publish /app/publish .1
# command that will start the application
ENTRYPOINT ["dotnet", "CarRentalSystem.Identity.dll"]
```

##### First Stage

The lines in the Dockerfile begin with image from Microsoft Container Registry (mcr.microsoft.com) and create an intermediate image ```base``` that exposes ports 80 and 443, and sets the working directory to ```/app```.

##### Second Stage

You can see that the ```build``` stage starts from a different original image from the registry (```sdk``` rather than ```aspnet```), rather than continuing from base. The ```sdk``` image has all the build tools, and for that reason it's a lot bigger than the aspnet image, which only contains runtime components. The reason for using a separate image becomes clear when you look at the rest of the Dockerfile.

##### Final Stage

The final stage starts again from ```base```, and includes the ```COPY --from=publish``` to copy the published output to the final image. This process makes it possible for the final image to be a lot smaller, since it doesn't need to include all of the build tools that were in the ```sdk``` image.

#### Dealers service dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRentalSystem.Dealers/CarRentalSystem.Dealers.csproj", "CarRentalSystem.Dealers/"]
COPY ["CarRentalSystem.Common/CarRentalSystem.Common.csproj", "CarRentalSystem.Common/"]
RUN dotnet restore "CarRentalSystem.Dealers/CarRentalSystem.Dealers.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Dealers"
RUN dotnet build "CarRentalSystem.Dealers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Dealers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Dealers.dll"]
```

#### Statistics service dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRentalSystem.Statistics/CarRentalSystem.Statistics.csproj", "CarRentalSystem.Statistics/"]
COPY ["CarRentalSystem.Common/CarRentalSystem.Common.csproj", "CarRentalSystem.Common/"]
RUN dotnet restore "CarRentalSystem.Statistics/CarRentalSystem.Statistics.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Statistics"
RUN dotnet build "CarRentalSystem.Statistics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Statistics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Statistics.dll"]
```

#### Notifications service dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRentalSystem.Notifications/CarRentalSystem.Notifications.csproj", "CarRentalSystem.Notifications/"]
COPY ["CarRentalSystem.Common/CarRentalSystem.Common.csproj", "CarRentalSystem.Common/"]
RUN dotnet restore "CarRentalSystem.Notifications/CarRentalSystem.Notifications.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Notifications"
RUN dotnet build "CarRentalSystem.Notifications.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Notifications.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Notifications.dll"]
```

#### Client dockerfile

```dockerfile
# base image
FROM node:12.2.0 as build

# set working directory
WORKDIR /app

# add `/app/node_modules/.bin` to $PATH
ENV PATH /app/node_modules/.bin:$PATH

# install and cache app dependencies
COPY package.json /app/package.json
RUN npm install
RUN npm install -g @angular/cli@10.1.2

# add app
COPY . /app

# generate build
ARG configuration=production

RUN ng build --output-path=dist --configuration=$configuration

# base image
FROM nginx:1.16.0-alpine

# Remove default Nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy Nginx configuration
COPY ./nginx.conf /etc/nginx/nginx.conf

# copy artifact build from the 'build environment'
COPY --from=build /app/dist /usr/share/nginx/html

# expose port 80
EXPOSE 80

# run nginx
CMD ["nginx", "-g", "daemon off;"]
```

#### Admin service dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRentalSystem.Admin/CarRentalSystem.Admin.csproj", "CarRentalSystem.Admin/"]
COPY ["CarRentalSystem.Common/CarRentalSystem.Common.csproj", "CarRentalSystem.Common/"]
RUN dotnet restore "CarRentalSystem.Admin/CarRentalSystem.Admin.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Admin"
RUN dotnet build "CarRentalSystem.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Admin.dll"]
```

#### Watchdog service dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRentalSystem.Watchdog/CarRentalSystem.Watchdog.csproj", "CarRentalSystem.Watchdog/"]
RUN dotnet restore "CarRentalSystem.Watchdog/CarRentalSystem.Watchdog.csproj"
COPY . .
WORKDIR "/src/CarRentalSystem.Watchdog"
RUN dotnet build "CarRentalSystem.Watchdog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Watchdog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Watchdog.dll"]
```
