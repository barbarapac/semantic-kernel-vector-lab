# Semantic Kernel MCP Service

Este é um servidor MCP (Model Context Protocol) que atua como uma ponte entre IDEs compatíveis com MCP e uma API Web baseada em Semantic Kernel. A aplicação permite que assistentes de IA em IDEs interajam com funcionalidades de busca vetorial e gerenciamento de produtos através de ferramentas MCP.

## Funcionalidades

O servidor MCP expõe as seguintes ferramentas:

### Ferramentas Disponíveis

- **CreateProductAsync**: Registra um novo produto na Semantic Kernel Web API
  - Parâmetros: título, categoria, resumo e descrição
  - Utiliza busca vetorial para indexação e recuperação

- **SearchRecommendationsAsync**: Busca recomendações baseadas em prompt
  - Utiliza IA para gerar recomendações contextuais
  - Retorna lista de recomendações com título e categoria

### Configuração

A aplicação se conecta a uma API Semantic Kernel através da variável de ambiente `API_BASE_ADDRESS` (padrão: `http://localhost:5042`).

## Desenvolvimento Local

### Configuração no Claude Desktop

1. Abra o Claude Desktop
2. Vá em **Configurações** → **Desenvolvedor**
3. Clique em **Edit Config**
4. Adicione a seguinte configuração:

```json
{
  "mcpServers":{
    "Semantic.Kernel.MCP.Service":{
      "command": "npx",
      "args": [
        "mcp-remote",
        "http://localhost:5010/"
      ]
    }
  }
}
```
5. Salve a configuração e reinicie o Claude Desktop

### Variáveis de Ambiente

- `API_BASE_ADDRESS`: URL base da API Semantic Kernel (padrão: `http://localhost:5042`)

### Executando o Servidor

1. Certifique-se de que a API Semantic Kernel esteja rodando na porta 5042
2. Execute o projeto: `dotnet run --urls="http://localhost:5010"`
3. Configure o Claude Desktop ou seu IDE conforme as instruções acima
4. O servidor estará disponível em `http://localhost:5010`

### Testando no Claude Desktop

Após configurar o servidor MCP no Claude Desktop, você pode testar as funcionalidades diretamente no chat:

- **Criar Produto**: "Crie um produto chamado 'Notebook Gamer' na categoria 'Eletrônicos' com resumo 'Notebook para jogos' e descrição completa"
- **Buscar Recomendações**: "Busque recomendações para 'tecnologias de desenvolvimento web'" ou "Sugira categorias de tecnologia"

O Claude irá automaticamente detectar e utilizar as ferramentas `CreateProductAsync` e `SearchRecommendationsAsync` do servidor MCP.

## Dependências

- **ModelContextProtocol.AspNetCore**: SDK para servidores MCP em ASP.NET Core
- **Microsoft.Extensions.AI.Abstractions**: Abstrações para integração com IA
- **System.Net.ServerSentEvents**: Suporte a Server-Sent Events