# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A .NET microservices proof-of-concept for a simplified insurance sales system, demonstrating
service discovery (Eureka/Steeltoe), API gateway (Ocelot), CQRS (MediatR), event-driven
communication (RabbitMQ with a transactional outbox), and multiple data-access technologies
chosen deliberately per service (EF Core, NHibernate, Dapper, Marten, NEST/ElasticSearch).
Frontend is a VueJS SPA (`Web/`); a `BlazorWasmClient` also exists.

## Commands

### Build & test (backend)

`.Api` projects must build before their owning service and before any other service that
depends on them (e.g. `PolicyService` depends on `PricingService.Api`), so build order matters:

```bash
cd scripts
./build-without-tests.sh   # builds all .Api projects, then all services (no tests)
./build.sh                 # same, plus builds and runs all *.Test projects
```

Build/test a single service directly with the SDK:

```bash
dotnet build PolicyService.Api        # build the service's contract (Commands/Queries/Events) project first if referenced elsewhere
dotnet build PolicyService
dotnet test PolicyService.Test
dotnet test PolicyService.Test --filter "FullyQualifiedName~ClassName.MethodName"   # single test
```

Integration tests (e.g. `PricingService.IntegrationTest`) use Testcontainers (spins up
PostgreSQL in Docker) plus Alba / `Microsoft.AspNetCore.Mvc.Testing` — Docker must be running
to execute these.

### Run a single service

```bash
cd PolicyService   # or any other service folder
dotnet run
```

Each service reads `hosting.json` and `appsettings.json` from its own folder and registers
itself with Eureka on startup, so Eureka should be running first (see below) for
service-to-service calls to resolve.

### Run the full stack with Docker

```bash
cd scripts
./infra-run.sh   # infra.yml: Postgres, RabbitMQ, ElasticSearch, Eureka, etc.
./app-run.sh     # app.yml: builds & runs all microservices + the Vue web app
```

App is then at http://localhost:8080. `./infra-down.sh` / `./app-down.sh` and
`./infra-logs.sh` / `./app-logs.sh` are also available. If ElasticSearch fails to start:
`sudo sysctl -w vm.max_map_count=262144`.

### Manual (non-Docker) setup

Requires PostgreSQL >= 10, RabbitMQ, and Elasticsearch >= 6 installed locally, plus running
Eureka via `cd eureka-server && ./gradlew bootRun`. Databases are created with
`postgres/createdatabases.sql` (creates `lab_user` and the `lab_netmicro_payments`,
`lab_netmicro_jobs`, `lab_netmicro_policy`, `lab_netmicro_pricing` databases).

### Frontend (`Web/`, VueJS)

```bash
cd Web
yarn install     # or npm install — both a package-lock.json and yarn.lock are present
yarn serve       # vue-cli-service serve
yarn build
yarn lint
```

## Architecture

### Service map

- **Web** — VueJS SPA for insurance agents (browse products, price & create offers, convert
  offers to policies, search policies/offers, chat). Talks only to `AgentPortalApiGateway`.
- **AgentPortalApiGateway** — Ocelot-based gateway; hides backend service topology from the
  client, enforces authentication, and is the single entry point for `Web`. Routing config is
  in `AgentPortalApiGateway/ocelot.json` (`ocelot.docker.json` for the containerized topology).
- **AuthService** — issues JWT tokens; user store is `AuthService/DataAccess/InsuranceAgentsInMemoryDb.cs`
  (in-memory, not a real DB).
- **ChatService** — SignalR-based chat between agents.
- **ProductService** — insurance product catalog (products + their configurable parameters).
- **PricingService** — calculates price for a product from agent-supplied answers; tariffs are
  rule sets parsed and evaluated with DynamicExpresso.
- **PolicyService** — creates offers and converts them to policies. The reference
  implementation of CQRS via MediatR in this repo, calls `PricingService` synchronously over
  REST (RestEase + Steeltoe service discovery + Polly retry, see
  `PolicyService/RestClients/`), and publishes policy-created/terminated events asynchronously
  via RabbitMQ using a **transactional outbox** (`PolicyService/Messaging/RabbitMq/Outbox/`:
  events are written to an `Outbox` table in the same DB transaction as the domain change, then
  a background `OutboxSendingService` publishes them to RabbitMQ). Persistence is NHibernate.
- **PolicySearchService** — consumes policy events from RabbitMQ, projects them into a read
  model, and indexes it in ElasticSearch for search.
- **PaymentService** — creates/lists policy accounts, imports a CSV bank-statement file as a
  scheduled background job and reconciles payments against accounts. Uses Dapper and consumes
  RabbitMQ events.
- **DashboardService** — consumes sales events, indexes them in ElasticSearch, and exposes
  aggregation-based sales stats (by product/time/agent) rendered with ChartJS.
- **DocumentService** (referenced in README, not present in this checkout) — PDF certificate
  generation via JS Report.
- **eureka-server** — Spring Boot/Gradle service registry all backend services register with
  via Steeltoe's discovery client.

### The `.Api` project pattern

Every business microservice that other services or the gateway need to call has a matching
`<Service>.Api` project (`PolicyService.Api`, `PricingService.Api`, `PaymentService.Api`, etc.)
containing only the public contract: MediatR `Commands`/`Queries` + their result types, `Events`,
and DTOs — no implementation. Other services reference the `.Api` project (not the service
project) to build typed RestEase clients or to deserialize published events, which is why
`.Api` projects must build first and why cross-service coupling should stay at that contract
layer rather than reaching into a service's implementation project.

### Test project pattern

Each service typically has:
- `<Service>` — implementation
- `<Service>.Api` — contracts (see above)
- `<Service>.Test` — xUnit unit tests
- some services additionally have `<Service>.IntegrationTest` (e.g. `PricingService`) using
  Testcontainers + Alba/`Mvc.Testing` for real-database, real-HTTP-pipeline tests.

### Cross-cutting conventions

- All backend services target **.NET 9**, use Serilog for logging, Steeltoe for Eureka
  discovery, and Swagger in Development.
- Synchronous inter-service calls: RestEase-generated typed clients over an
  `HttpClient`/`DiscoveryHttpClientHandler` (resolves the target via Eureka) wrapped in a Polly
  retry policy — see `PolicyService/RestClients/PricingClient.cs` as the canonical example.
- Asynchronous inter-service communication: RabbitMQ. Producers that must guarantee delivery
  alongside a DB transaction use the outbox pattern (`PolicyService`); simpler
  publish/subscribe is used elsewhere (`PolicySearchService`, `PaymentService`, `DashboardService`).
- Each service owns its own database/schema; there's no shared database.
