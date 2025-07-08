# Documentação do Projeto SizeApi - Diego Yann

Este documento fornece uma visão geral da estrutura do projeto SizeApi e as instruções para executá-lo localmente.

## Estrutura do Projeto

O projeto segue os princípios da Arquitetura Limpa (Clean Architecture), dividindo as responsabilidades em projetos distintos para garantir baixo acoplamento e alta coesão.

-   `Size.Domain`: Camada de Domínio. É o núcleo do software, contendo as entidades de negócio (`Empresa`, `NotaFiscal`, etc.), enums e as interfaces dos repositórios. Não depende de nenhuma outra camada.

-   `Size.Application`: Camada de Aplicação. Orquestra os objetos de domínio para executar casos de uso. Contém a lógica de negócio, serviços, DTOs (Data Transfer Objects) e validações.

-   `Size.Infrastructure`: Camada de Infraestrutura. Implementa as abstrações definidas na camada de domínio, como os repositórios para acesso a dados (utilizando Entity Framework Core) e a configuração do DbContext.

-   `Size.Api`: Camada de Apresentação. Responsável por expor os endpoints da API (Controllers) para o mundo externo, configurar o pipeline de requisições HTTP e a injeção de dependência.

## Como Executar o Projeto

Existem duas maneiras principais de executar o projeto: utilizando Docker Compose ou diretamente com o .NET CLI.

### 1. Utilizando Docker Compose

Este é o método recomendado para um ambiente de desenvolvimento padronizado.

**Pré-requisitos:**
*   Docker
*   Docker Compose

**Passos:**

1.  Na raiz do projeto, execute o seguinte comando:

    ```bash
    docker-compose up --build
    ```

2.  O comando irá construir a imagem da API, baixar a imagem do SQL Server 2022 Express, e iniciar ambos os contêineres. O health check garante que a API só inicie após o banco de dados estar pronto.

3.  A API estará disponível no endereço: `http://localhost:5000`

### 2. Utilizando .NET CLI

Este método requer que você tenha o ambiente .NET e um banco de dados SQL Server configurado manualmente.

**Pré-requisitos:**
*   .NET 8 SDK
*   SQL Server (Express ou superior) em execução.
*   A connection string no arquivo `appsettings.Development.json` deve apontar para sua instância do SQL Server.

**Passos:**

1.  **Restaurar dependências do .NET:**
    Abra um terminal na raiz do projeto e execute:
    ```bash
    dotnet restore Size.sln
    ```

2.  **Aplicar as migrações do banco de dados:**
    Este comando irá criar o banco de dados (se não existir) e todas as tabelas.
    ```bash
    dotnet ef database update --project Size.Infrastructure/Size.Infrastructure.csproj --startup-project Size.Api/Size.Api.csproj
    ```

3.  **Executar a aplicação:**
    ```bash
    dotnet run --project Size.Api/Size.Api.csproj
    ```

4.  A API estará disponível nos endereços definidos em `Properties/launchSettings.json` (`http://localhost:5000`).
