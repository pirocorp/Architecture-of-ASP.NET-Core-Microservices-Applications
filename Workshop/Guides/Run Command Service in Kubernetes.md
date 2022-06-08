# Create Dockerfile for the Command Service

Create Dockerfile in the root folder of the CommandService project with the following content. 

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./CommandService/*.csproj /app
COPY ./Rules.ruleset /
COPY ./stylecop.json /
RUN cd /app
RUN dotnet restore

# Copy everything else and build
COPY ./CommandService /app
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
