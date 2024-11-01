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
EXPOSE 8081
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .

ENV BookLibraryServiceAccount="**apiuserjsonstring**"
ENV	BookLibraryDbConnection="**dbconnectionstring**"
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "CrudDapperGraphQL.dll"]