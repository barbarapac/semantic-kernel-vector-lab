# Semantic Kernel Vector Lab

Projeto experimental com Microsoft Semantic Kernel voltado ao desenvolvimento de competências em IA aplicada, explorando o ciclo completo de geração de embeddings, indexação em pgvector (PostgreSQL) e recuperação de contexto para uso em aplicações conversacionais e RAG.

## 🚀 Tecnologias

- **.NET 10** - Framework web
- **Microsoft Semantic Kernel** - Integração com IA
- **Ollama** - Modelo local de embeddings (`mxbai-embed-large`)
- **PostgreSQL + pgvector** - Banco vetorial
- **Entity Framework Core** - ORM
- **MediatR** - Padrão CQRS e mediação
- **Clean Architecture** - Separação de responsabilidades

## 🏢 Arquitetura

O projeto segue os princípios da **Clean Architecture** com separação clara de responsabilidades:

- **Domain**: Entidades e interfaces (sem dependências externas)
- **Application**: Lógica de negócio usando CQRS com MediatR
- **Infrastructure**: Implementações de repositórios e serviços externos
- **WebApi**: Endpoints organizados e configuração da API

### Fluxo de Comunicação
```
WebApi (Endpoints) → MediatR → Application (Commands/Queries) → Domain Interfaces → Infrastructure
```

## 📋 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Ollama](https://ollama.ai/) instalado localmente

## ⚙️ Configuração

### 1. Instalar o modelo Ollama
```bash
ollama pull mxbai-embed-large
```

### 2. Subir o banco PostgreSQL
```bash
docker-compose up -d
```

### 3. Executar a aplicação
```bash
cd src/Semantic.Kernel.Vector.WebApi
dotnet run
```

## 🔗 Endpoints

### Gerar embeddings para produtos existentes
```http
GET /v1/seed
```

### Criar novo produto
```http
POST /v1/products
Content-Type: application/json

{
  "title": "Blue Mountain Coffee",
  "category": "premium",
  "summary": "Rare Jamaican coffee",
  "description": "Smooth and mild flavor profile"
}
```

### Busca semântica
```http
POST /v1/prompt
Content-Type: application/json

{
  "prompt": "strong coffee"
}
```

## 🎯 Como Funciona

1. **Geração de Embeddings**: Converte texto em vetores usando Ollama
2. **Armazenamento**: Salva embeddings no PostgreSQL com pgvector
3. **Busca Semântica**: Encontra produtos similares usando distância coseno

## 📊 Dataset

A aplicação inclui 15 tipos de café pré-configurados com diferentes categorias:
- `robust` - Cafés encorpados
- `soft` - Cafés suaves  
- `intense` - Cafés intensos
- `arabic` - Cafés arábicos

## 🔍 Exemplo de Uso

1. Execute `GET /v1/seed` para gerar embeddings dos produtos
2. Faça uma busca: `POST /v1/prompt` com `{"prompt": "fruity coffee"}`
3. Receba recomendações baseadas em similaridade semântica

## 📝 Notas

- O modelo `mxbai-embed-large` gera vetores de 1024 dimensões
- O índice HNSW otimiza consultas vetoriais no PostgreSQL
- A busca retorna os 3 produtos mais similares