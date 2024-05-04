# Define variáveis
ARG DOTNET_VERSION=8.0
ARG PROJECT_NAME=Api
ARG PROJECT_FILE=${PROJECT_NAME}.csproj

# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY *.csproj./
COPY **/*.csproj./

# Restaura as dependências
RUN dotnet restore ${PROJECT_FILE}

# Copia o código fonte
COPY..

# Compila o projeto
RUN dotnet build ${PROJECT_FILE} -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish ${PROJECT_FILE} -c Release -o /app/publish

# Estágio final, com a imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app
COPY --from=publish /app/publish.
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "${PROJECT_NAME}.dll"]