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

# Copy everything else and build
COPY ./PlatformService /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
```

## Create docker image

Tag the docker image to your dockerhub. In the root folder of your project run the following command: 

```bash
docker build -t <your docker hub id>/platformservice -f . ../
```

![image](https://user-images.githubusercontent.com/34960418/171868210-dd2ca3c4-7879-447e-8eaa-f20b10efc237.png)


