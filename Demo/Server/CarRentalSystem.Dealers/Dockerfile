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
RUN dotnet build "CarRentalSystem.Dealers.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "CarRentalSystem.Dealers.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentalSystem.Dealers.dll"]