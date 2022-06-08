# Dockerize PlatformService

## Create Dockerfile in the root folder of the PlatformService project

Add the following content to the docker file:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./PlatformService/*.csproj /app
COPY ./Rules.ruleset /
COPY ./stylecop.json /
RUN cd /app
RUN dotnet restore

# Copy csproj for the common library and restore as distinct layers
RUN mkdir /Common
COPY ./Common/*.csproj /Common
RUN cd /Common
RUN dotnet restore

# Copy everything else and build
COPY ./PlatformService /app
COPY ./Common /Common
RUN cd /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
```

## Create docker image

Tag the docker image to your dockerhub and build it. In the root folder of your project, run the following command:

```bash
docker build -t pirocorp/workshop-platformservice -f . ../
```

![image](https://user-images.githubusercontent.com/34960418/171869347-c749d7ee-3534-4ef6-a37c-f457935e8528.png)


Run docker container

```bash
docker run -p 8080:80 pirocorp/workshop-platformservice
```

![image](https://user-images.githubusercontent.com/34960418/171884708-6fa00663-5c04-4bc7-baf3-fecb09e39d8b.png)


Push docker image to dockerhub

```bash
docker push pirocorp/workshop-platformservice
```

![image](https://user-images.githubusercontent.com/34960418/171895117-dfbd51e8-24f3-4571-ad9d-9a54e2c303b2.png)
