# ASCLAB .NET Core PoC - LAB Insurance Sales Portal

[![Build Status](https://travis-ci.org/asc-lab/dotnetcore-microservices-poc.svg?branch=master)](https://travis-ci.org/asc-lab/dotnetcore-microservices-poc)

This is an example of a very simplified insurance sales system made in a microservice architecture using:

* .NET Core 2.1
* Entity Framework Core
* MediatR
* Marten
* Eureka
* Ocelot
* JWT Tokens
* RestEase
* RawRabbit
* NHibernate
* Polly
* NEST (ElasticSearch client)
* Dapper
* DynamicExpresso
* SignalR

**Comprehensive guide describing exactly the architecture, applied design patterns and technologies can be found on our blog:**

- [Part I The Plan](https://altkomsoftware.pl/en/blog/building-microservices-on-net-core-1/)
- [Part 2 Shaping microservice internal architecture with CQRS and MediatR](https://altkomsoftware.pl/en/blog/microservices-net-core-cqrs-mediatr/)
- [Part 3 Service Discovery with Eureka](https://altkomsoftware.pl/en/blog/service-discovery-eureka/)
- [Part 4 Building API Gateways With Ocelot](https://altkomsoftware.pl/en/blog/building-api-gateways-with-ocelot/)
- [Part 5 Marten An Ideal Repository For Your Domain Aggregates](https://altkomsoftware.pl/en/blog/building-microservices-domain-aggregates/)
- [Part 6 Real time server client communication with SignalR and RabbitMQ](https://altkomsoftware.pl/en/blog/building-microservices-6/)

## Business Case

We are going to build very simplified system for insurance agents to sell various kind of insurance products.
Insurance agents will have to log in and system will present them with list of products they can sell. Agents will be able to view products and find a product appropriate for their customers. Then they can create an offer and system will calculate a price based on provided parameters. \
Finally agent will be able to confirm the sale by converting offer to policy and printing pdf certificate. \
Portal will also give them ability to search and view offer and policies. \
Portal will also have some basic social network features like chat for agents.

## Architecture overview

<p align="center">
    <img alt="NET Microservices Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-microservices-poc/master/readme-images/dotnetcore-microservices-architecture.png" />
</p>

* **Web** - a VueJS Single Page Application that provides insurance agents ability to select appropriate product for their customers, calculate price, create an offer and conclude the sales process by converting offer to policy. This application also provides search and view functions for policies and offers. Frontend talks to backend services via `agent-portal-api-gateway`.

* **Agent Portal API Gateway** - is a special microservice whose main purpose it to hide complexity of the underlying back office services structure from client application. Usually we create a dedicated API gateway for each client app. In case in the future we add a Xamarin mobile app to our system, we will need to build a dedicated API gateway for it. API gateway provides also security barrier and does not allow unauthenticated request to be passed to backend services. Another popular usage of API gateways is content aggregation from multiple services.

* **Auth Service** - a service responsible for users authentication. Our security system will be based on JWT tokens. Once user identifies himself correctly, auth service issues a token that is further use to check user permission and available products.

* **Chat Service** - a service that uses SignalR to give agents ability to chat with each other.

* **Payment Service** - main responsibilities: create Policy Account, show Policy Account list, register in payments from bank statement file. \
This module is taking care of a managing policy accounts. Once the policy is created, an account is created in this service with expected money income.  PaymentService also has an implementation of a scheduled process where CSV file with payments is imported and payments are assigned to policy accounts. This component shows asynchronous communication between services using RabbitMQ and ability to create background jobs. It also features accessing database using Dapper.

* **Policy Service** - creates offers, converts offers to insurance policies. \
In this service we demonstrated usage of CQRS pattern for better read/write operation isolation. This service demonstrates two ways of communication between services: synchronous REST based calls to `PricingService` through HTTP Client to get the price, and asynchronous event based using RabbitMQ to publish information about newly created policies. In this service we also access RDBMS using NHibernate.

* **Policy Search Service** - provides insurance policy search. \
This module listens for events from RabbitMQ, converts received DTOs to “read model” and indexes given model in ElasticSearch to provide advanced search capabilities.

* **Pricing Service** - a service responsible for calculation of price for given insurance product based on its parametrization. \
For each product a tariff should be defined. The tariff is a set of rules on the basis of which the price is calculated. DynamicExpresso was used to parse the rules. During the policy purchase process, the `PolicyService` connects with this service to calculate a price. Price is calculated based on user’s answers for defined questions.

* **Product Service** - simple insurance product catalog. \
It provides basic information about each insurance product and its parameters that can be customized while creating an offer for a customer.

* **Document Service** - this service uses JS Report to generate pdf certificates (not implemented yet).

Each business microservice has also **.Api project** (`PaymentService.Api`, `PolicyService.Api` etc.), where we defined commands, events, queries and operations and **.Test project** (`PaymentService.Test`, `PolicyService.Test`) with unit and integration tests.

## Build

Build all projects from command line without test:

```bash
cd scripts
./build-without-tests.sh
```

Build all projects from command line with test:

```bash
cd scripts
./build.sh
```

## Running

You must install Docker & Docker Compose before. \
Scripts have been divided into two parts:

* [`infra.yml`](scripts/infra.yml) runs the necessary infrastructure
* [`app.yml`](scripts/app.yml) is used to run the application.

You can use scripts to run/stop/down all containers.

To run the whole solution:

```bash
./infra-run.sh
./app-run.sh
```

## Run

Go to folder with specific service (`PolicyService`, `ProductService` etc) and use `dotnet run` command.
