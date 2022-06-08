# Create Dockerfile for the Command Service

Create Dockerfile in the root folder of the CommandService project with the following content. 

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj for the microservice and restore as distinct layers
COPY ./CommandService/*.csproj /app
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
COPY ./CommandService /app
COPY ./Common /Common
RUN cd /app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CommandService.dll"]
```


Tag the docker image to your dockerhub and build it. In the root folder of your project, run the following command:

```bash
docker build -t pirocorp/workshop-commandservice -f . ../
```


Push the docker image to dockerhub

```bash
docker push pirocorp/workshop-commandservice
```
