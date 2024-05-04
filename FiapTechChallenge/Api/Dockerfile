# Usa a imagem base do.NET SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho como /src
WORKDIR /src

# Copia os arquivos de projeto.csproj para o container
COPY Api/Api.csproj Api/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Restaura as dependências
RUN dotnet restore Api/Api.csproj
RUN dotnet restore Domain/Domain.csproj
RUN dotnet restore Infrastructure/Infrastructure.csproj

# Copia todo o código-fonte para o container
COPY..

# Compila o projeto API
WORKDIR "/src/Api"
RUN dotnet build Api.csproj -c Release -o /app/build

# Compila os projetos Domain e Infrastructure
WORKDIR "/src/Domain"
RUN dotnet build Domain.csproj -c Release -o /app/build
WORKDIR "/src/Infrastructure"
RUN dotnet build Infrastructure.csproj -c Release -o /app/build

# Cria a imagem de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/build.
ENTRYPOINT ["dotnet", "Api.dll"]