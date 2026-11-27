# Sistema de Doações

Projeto desenvolvido em C# com MySQL para gerenciamento de doações, doadores, pessoas necessitadas e itens.

## Tecnologias utilizadas

- C#
- .NET 10
- MySQL
- MySql.Data

## Pré-requisitos

Antes de executar o projeto, instale:

1. [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
2. MySQL Server
3. MySQL Workbench (opcional, para facilitar a execução dos scripts SQL)

## 1. Clonar o repositório

Abra o terminal e execute:

git clone https://github.com/Danifalha11/PI.git


Entre na pasta:

cd PI


## 2. Criar o banco de dados

1. Abra o MySQL Workbench.
2. Conecte-se ao servidor MySQL local.
3. Abra o arquivo `Banco.sql`, localizado na pasta do projeto.
4. Execute todo o script.

O script cria automaticamente o banco `SistemaDoacoes` e as seguintes tabelas:

- Doador
- Necessitado
- InstituicaoDoacao
- Item

## 3. Configurar a conexão

Abra o arquivo `Conexao.cs`.

Configure os dados de acesso ao seu servidor MySQL:


private readonly string connectionString =
    "Server=localhost;Database=SistemaDoacoes;Uid=root;Pwd=SUA_SENHA;";


Substitua `SUA_SENHA` pela senha configurada no seu MySQL.

**Importante:** não compartilhe senhas ou credenciais pessoais no repositório.

## 4. Restaurar as dependências

Na pasta do projeto, execute:

dotnet restore


## 5. Executar o programa

Para iniciar o sistema, execute:


dotnet run


Se a conexão estiver configurada corretamente, o programa deverá exibir a mensagem:


Conectado ao banco com sucesso!

Em seguida, o menu principal será iniciado.

## Estrutura do projeto

| Arquivo | Função 
|---|---|
| `Program.cs` | Inicialização do programa 
| `Conexao.cs` | Conexão com o MySQL 
| `Banco.sql` | Criação do banco e das tabelas 
| `DoadorDAO.cs` | Operações relacionadas aos doadores 
| `NecessitadoDAO.cs` | Operações relacionadas aos necessitados 
| `ItemDAO.cs` | Operações relacionadas aos itens 
| `Menus.cs` | Menus e interação com o usuário 
| `SiteDoacao.cs` | Classe relacionada ao sistema de doações 

## Observações

- O MySQL Server precisa estar em execução antes de iniciar o programa.
- O banco de dados deve ser criado antes da execução.
- Os IDs utilizados nos relacionamentos precisam existir nas respectivas tabelas.
