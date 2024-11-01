# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CrudDapperGraphQL.csproj", "."]
RUN dotnet restore "./CrudDapperGraphQL.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./CrudDapperGraphQL.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CrudDapperGraphQL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .

ENV BookLibraryServiceAccount="***"
ENV	BookLibraryDbConnection="***"
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "CrudDapperGraphQL.dll"]

#=== Build WebApi from Dockerfile === (run from Dockerfile place)
#
#$ docker build -t cruddappergraphql:v1.0 .
#
#=== Network ===
#
#$ docker network create app-network
#
#=== SQL Express ===
#= {run command without -p 1433:1433 not to be exposed to }
#
#$ docker run \
#-e "ACCEPT_EULA=Y" \
#-e "MSSQL_SA_PASSWORD=***" \
#-e "MSSQL_COLLATION=SQL_Latin1_General_CP1_CI_AS" \
#-v sqldata2022:/var/opt/mssql \
#-d -p 1433:1433 \
#--name sqlexpress2022 \
#--network=app-network \
#--hostname sqlexpress2022host \
#mcr.microsoft.com/mssql/server:2022-latest
#
#=== WebApi ===
#
#$ docker run \
#--name cruddappergraphql \
#--network=app-network \
#-p 8080:8080 \
#-d cruddappergraphql:v1.0
#
#== Inspect & fix connection == 
#
#$ docker inspect sqlserver2022
#$ docker inspect cruddappergraphql
#
#$ docker network connect app-network cruddappergraphql
#
#=== check ===
#http://localhost:8080/swagger/index.html