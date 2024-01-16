# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copia os arquivos de projeto .csproj para o container e restaura as dependências
COPY ["Api/Api.csproj", "Api/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Api/Api.csproj"

# Após a restauração, copie o código fonte para o container
COPY ["Api/", "Api/"]
COPY ["Domain/", "Domain/"]
COPY ["Infrastructure/", "Infrastructure/"]

# Defina o diretório de trabalho para a API e compile o projeto
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publica o projeto
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Stage final, com a imagem de runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]