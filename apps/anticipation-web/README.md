# Anticipation Web

Frontend React para operacao da API de antecipacoes.

## Funcionalidades

- Listar solicitacoes existentes (paginado)
- Criar nova solicitacao via formulario
- Aprovar antecipacao pendente
- Reprovar antecipacao pendente

## Requisitos

- Node.js 18+
- API executando em `http://localhost:8080`

## Executar com Docker Compose (front + back)

Na raiz do repositorio:

```bash
docker compose up --build -d
```

Observacoes:

- O backend expoe `GET /healthz` para healthcheck.
- O frontend aguarda o backend ficar `healthy` antes de iniciar.

URLs:

- Frontend: `http://localhost:3000`
- API (acesso direto): `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

No modo Docker, o frontend usa proxy interno `/api` para conversar com `anticipation-api:8080`.

## Configuracao

Opcionalmente, configure a URL da API no `.env`:

```bash
VITE_API_BASE_URL=http://localhost:8080
```

## Executar localmente

```bash
npm install
npm run dev
```

## Build de producao

```bash
npm run build
```
