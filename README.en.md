# RequestsForAdvancePaymentsApi

Portuguese version: `README.md`

API for managing receivables anticipation requests.

## Overview

This API is designed with simplicity and incremental evolution in mind, using:

- Inspired Clean Architecture
- DDD Lite
- Vertical Slice + lightweight CQRS
- Thin Controllers

Current core features:

- Create anticipation request
- List requests by `creator_id`
- List all requests with pagination
- Approve or reject a request
- React frontend for operations (listing, create, approve/reject)

Implemented business rules:

- `valor_solicitado` must be greater than `100.00`
- A `creator` cannot have more than one pending request at the same time
- Fixed anticipation fee: `5%`
- Initial status in API response: `pendentef`

## Run Locally

You can run the API in two ways. The easiest is Docker.

### Option 1: Docker Compose (recommended)

Prerequisites:

- Docker Desktop running

Command:

```bash
docker compose up --build -d
```

Notes:

- Backend has a `healthcheck` at `/healthz`.
- Frontend starts only after backend is `healthy`.
- This avoids startup race conditions and early connection errors.

API will be available at:

- Frontend: `http://localhost:3000`
- `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

To stop:

```bash
docker compose down
```

### Option 2: Local .NET run

Prerequisites:

- .NET SDK 10

Commands:

```bash
dotnet restore Anticipation.sln
dotnet run --project src/Anticipation.API/Anticipation.API.csproj
```

The API starts using local `launchSettings` and app configuration.

### Local Frontend (React)

Commands:

```bash
cd apps/anticipation-web
npm install
npm run dev
```

Frontend local URL:

- `http://localhost:5173`

In local dev, the frontend uses `VITE_API_BASE_URL` (see `apps/anticipation-web/.env.example`) to target the API.

## Main Endpoints

- `POST /api/v1/anticipations`
- `GET /api/v1/anticipations?page=1&pageSize=20`
- `GET /api/v1/anticipations/creator/{creatorId}`
- `PUT /api/v1/anticipations/{id}/approve`
- `PUT /api/v1/anticipations/{id}/reject`

## Functional Data Seeding

After starting the API, you can seed random data with the Python script:

```bash
python tools/functional-seeding/seed_anticipations.py
```

Script docs:

- `tools/functional-seeding/README.md`
