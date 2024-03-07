# FiapTechChallenge

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

## Arquitetura da Solução

![image](https://github.com/Leock9/fiap-tech-challenge/assets/42394625/b96f6384-62bc-4519-8e8f-a4266a97189c)

- `Clients`: Representam os usuários ou sistemas externos que se conectam à API. Eles se comunicam com a aplicação através da porta 5000, que é a porta na qual o serviço API está ouvindo.
- `API Service`: Este é um objeto Kubernetes do tipo Service que atua como um proxy para enviar tráfego de rede para um ou mais Pods executando a aplicação API. O Service garante que a API possa ser acessada de maneira estável e confiável.
- `Pod`: Aqui temos um Pod que representa a unidade de implantação da aplicação API dentro do cluster Kubernetes. O Pod encapsula um ou mais contêineres da aplicação, isolando-a em seu próprio ambiente de execução.
- `HPA (Horizontal Pod Autoscaler)`: O HPA permite o escalonamento automático do número de réplicas do Pod baseado em métricas específicas, como CPU ou tráfego de rede, para garantir a disponibilidade e a eficiência da aplicação conforme a demanda muda.
- `Gateway Payment`: Responsável por processar pagamentos. Este componente pode interagir com a API e/ou outros serviços do sistema.
- `Services`: O serviço com a porta 5672 provavelmente é um de mensagens RabbitMQ, uma vez que essa é a porta padrão do RabbitMQ. Serviço com a porta 5432 é um banco de dados PostgreSQL, já que esta é a porta padrão para conexões com o PostgreSQL.
- `PVC (Persistent Volume Claim)`: Este é um recurso de armazenamento persistente que está associado ao banco de dados PostgreSQL, garantindo que os dados persistam além do ciclo de vida dos contêineres e Pods.
- `Node` : Um Node é uma máquina física ou virtual que faz parte do cluster Kubernetes e hospeda os Pods.

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
