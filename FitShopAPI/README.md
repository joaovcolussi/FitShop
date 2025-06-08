# FitShop API - Backend C#

Este é o backend da aplicação FitShop desenvolvido em ASP.NET Core 8.0 com Entity Framework Core.

## Configuração do Projeto

### Pré-requisitos
- .NET 8.0 SDK
- SQL Server LocalDB (ou SQL Server)
- Visual Studio 2022 ou VS Code

### Instalação

1. **Clone o repositório e navegue para a pasta da API:**
```bash
cd FitShopAPI
```

2. **Restaure os pacotes NuGet:**
```bash
dotnet restore
```

3. **Configure a string de conexão:**
   - Edite o arquivo `appsettings.json` se necessário
   - Por padrão, usa SQL Server LocalDB

4. **Execute as migrações do banco de dados:**
```bash
dotnet ef database update
```

5. **Execute a aplicação:**
```bash
dotnet run
```

A API estará disponível em:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5000`

### Estrutura do Banco de Dados

O projeto implementa todas as tabelas especificadas:

- **Clientes**: Informações dos clientes
- **Produtos**: Catálogo de produtos com suporte a promoções e novidades
- **Categorias**: Categorias dos produtos
- **Pedidos**: Pedidos realizados
- **PedidoProdutos**: Relação many-to-many entre pedidos e produtos
- **Promocoes**: Promoções específicas de produtos
- **MaisComprados**: Estatísticas de produtos mais comprados
- **MaisPesquisados**: Estatísticas de produtos mais pesquisados
- **ListasProdutos**: Listas personalizadas de produtos

### Endpoints Principais

#### Produtos
- `GET /api/produtos` - Lista produtos (com filtros opcionais)
- `GET /api/produtos/{id}` - Busca produto por ID
- `GET /api/produtos/promocoes` - Lista produtos em promoção
- `GET /api/produtos/novidades` - Lista novidades
- `POST /api/produtos` - Cria novo produto
- `PUT /api/produtos/{id}` - Atualiza produto
- `DELETE /api/produtos/{id}` - Remove produto

#### Categorias
- `GET /api/categorias` - Lista categorias
- `GET /api/categorias/{id}` - Busca categoria por ID
- `POST /api/categorias` - Cria nova categoria
- `PUT /api/categorias/{id}` - Atualiza categoria
- `DELETE /api/categorias/{id}` - Remove categoria

#### WhatsApp
- `POST /api/whatsapp/enviar-pedido` - Gera link do WhatsApp para pedido

### Funcionalidades Implementadas

1. **CRUD completo** para produtos e categorias
2. **Filtros de busca** por termo e categoria
3. **Suporte a promoções** com preços promocionais
4. **Sistema de novidades** para destacar produtos
5. **Integração com WhatsApp** para envio de pedidos
6. **Estatísticas** de produtos mais pesquisados e comprados
7. **CORS configurado** para integração com o frontend Vue.js
8. **Swagger** para documentação da API

### Configuração do Frontend

Para conectar o frontend Vue.js com esta API, certifique-se de que:

1. A API esteja rodando na porta correta (7001 por padrão)
2. O arquivo `src/services/api.ts` tenha a URL correta da API
3. O CORS esteja configurado para permitir requisições do frontend

### Dados de Exemplo

O banco é populado automaticamente com produtos de exemplo quando criado, incluindo:
- 8 produtos variados (suplementos, acessórios, roupas)
- 5 categorias padrão
- Produtos com promoções e novidades configuradas

### Desenvolvimento

Para adicionar novas funcionalidades:

1. **Modelos**: Adicione em `Models/`
2. **DTOs**: Adicione em `DTOs/`
3. **Controllers**: Adicione em `Controllers/`
4. **Configuração do EF**: Atualize `Data/FitShopContext.cs`

### Comandos Úteis

```bash
# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove

# Gerar script SQL
dotnet ef script
```