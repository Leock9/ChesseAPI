# FiapTechChallenge API

![Build Status](https://github.com/Leock9/fiap-tech-challenge/actions/workflows/workflow.yaml/badge.svg)


FiapTechChallenge é uma API robusta construída com .NET 8, utilizando o FastEndpoint para desenvolvimento rápido de APIs. Ela utiliza PostgreSQL para persistência de dados e RabbitMQ para gerenciamento de mensagens, visando oferecer alto desempenho e confiabilidade.

## Características

- Endpoints RESTful para processamento de pedidos e pagamentos.
- Processamento de mensagens assíncronas com RabbitMQ.
- Armazenamento de dados persistente com PostgreSQL.
- Containerização com Docker para fácil desenvolvimento e implementação.

## Pré-requisitos

- Docker e Docker Compose
- SDK do .NET 8

## Início Rápido

1. Clone o repositório:
    ```
    https://github.com/Leock9/fiap-tech-challenge.git
    cd FiapTechChallenge
    ```

2. Construa e execute os containers:
    ```
    docker-compose up --build
    ```

Os serviços estarão disponíveis em:

- **API**: <http://localhost:5000/>
- **Swagger**: <http://localhost:{ServicePort}/swagger> (Rodando via VS irá abrir automaticamente)
- **Gerenciamento RabbitMQ**: <http://localhost:15672/>
- **pgAdmin**: <http://localhost:5050/>

## Arquitetura

O FiapTechChallenge segue um modelo de arquitetura limpa com a seguinte estrutura:

- **Api**: O ponto de entrada para solicitações HTTP.
- **Domain**: Contém a lógica de negócios central e as entidades.
- **Infrastructure**: Implementa os mecanismos de persistência e mensageria.

## Desenvolvimento

Este projeto utiliza o GitActions para seu processo de CI/CD, garantindo um ciclo de vida de desenvolvimento ágil e estável.

## Docker Compose

Nossos serviços são definidos no `docker-compose.yml`. Aqui está uma breve visão geral:

- `api`: Nosso serviço de API construído a partir da imagem `lkhouri/api:v1`.
- `rabbitmq`: Serviço de corretagem de mensagens com o plugin de gerenciamento.
- `pgdb`: Serviço de banco de dados PostgreSQL.
- `pgadmin`: Administração de PostgreSQL baseada na web.

## Contribuindo

Contribuições são bem-vindas! Por favor, leia nossas diretrizes de contribuição antes de enviar pull requests.

## Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
