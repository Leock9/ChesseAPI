# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto.csproj para o container
COPY ["Api/Api.csproj", "Api/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "Api/Api.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"

# Compila os projetos
WORKDIR "/src/Domain"
RUN dotnet build "Domain.csproj" -c Release -o /app/build
WORKDIR "/src/Infrastructure"
RUN dotnet build "Infrastructure.csproj" -c Release -o /app/build
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