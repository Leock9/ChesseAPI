FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto.csproj para o container e restaura as dependências
COPY ["Api/Api.csproj", "Api/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Api/Api.csproj"

# Copia todo o código-fonte para o container
COPY ["Api/", "Api/"]
COPY ["Domain/", "Domain/"]
COPY ["Infrastructure/", "Infrastructure/"]

# Defina o diretório de trabalho para a API e compile o projeto
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Use a imagem de runtime do .NET 8.0 para a imagem de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Api.dll"]