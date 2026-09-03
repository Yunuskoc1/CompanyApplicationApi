FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CompanyApplicationApi.csproj", "."]
RUN dotnet restore "./CompanyApplicationApi.csproj"
COPY . .
RUN dotnet build "./CompanyApplicationApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./CompanyApplicationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Data/City.json ./Data/
COPY Data/District.json ./Data/
ENTRYPOINT ["dotnet", "CompanyApplicationApi.dll"]