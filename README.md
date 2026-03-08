# RequestsForAdvancePaymentsApi

English version: `README.en.md`

API para gerenciamento de solicitacoes de antecipacao de recebiveis.

## Visao geral

Esta API foi estruturada com foco em simplicidade e evolucao incremental, usando:

- Clean Architecture (inspirada)
- DDD Lite
- Vertical Slice + CQRS leve
- Controllers finos

Principais funcionalidades atuais:

- Criar solicitacao de antecipacao
- Listar solicitacoes por `creator_id`
- Listar todas as solicitacoes com paginacao
- Aprovar ou recusar solicitacao

Regras de negocio implementadas:

- `valor_solicitado` deve ser maior que `100.00`
- Um `creator` nao pode ter mais de uma solicitacao pendente
- Taxa fixa de antecipacao: `5%`
- Status inicial no retorno: `pendentef`

## Como rodar localmente

Voce pode subir de duas formas. A mais simples e via Docker.

### Opcao 1: Docker Compose (recomendado)

Pre-requisitos:

- Docker Desktop em execucao

Comando:

```bash
docker compose up --build -d
```

Pronto. A API ficara disponivel em:

- `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

Para parar:

```bash
docker compose down
```

### Opcao 2: .NET local

Pre-requisitos:

- .NET SDK 10

Comandos:

```bash
dotnet restore Anticipation.sln
dotnet run --project src/Anticipation.API/Anticipation.API.csproj
```

A API abrira usando o `launchSettings`/config local.

## Endpoints principais

- `POST /api/v1/anticipations`
- `GET /api/v1/anticipations?page=1&pageSize=20`
- `GET /api/v1/anticipations/creator/{creatorId}`
- `PUT /api/v1/anticipations/{id}/approve`
- `PUT /api/v1/anticipations/{id}/reject`

## Inclusao de dados para teste funcional

Depois de subir a API, voce pode popular dados aleatorios com o script Python:

```bash
python tools/functional-seeding/seed_anticipations.py
```

Documentacao do script:

- `tools/functional-seeding/README.md`