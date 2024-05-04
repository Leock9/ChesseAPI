# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY..
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Publica o projeto
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage final, com a imagem de runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish.
ENTRYPOINT ["dotnet", "Api.dll"]