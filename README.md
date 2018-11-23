# ASCLAB .NET Core PoC - LAB Insurance Sales Portal

This is an example of a very simplified insurance sales system made in a microservice architecture using .NET Core 2.1, MediatR, Entity Framework Core, Marten, RestEase, RawRabbit, NHibernate, Polly, NEST, Dapper, DynamicExpresso.

## Architecture overview

<p align="center">
    <img alt="Architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-microservices-poc/master/readme-images/architecture.png" />
</p>

* **PaymentService** - main responsibilities: create Policy Account, show Policy Account list, register in payments from bank statement file. \
This module is taking care of a managing policy accounts. Once the policy is created, an account is created in this service with expected money income.  PaymentService also has an implementation of a scheduled process where CSV file with payments is imported and payments are assigned to policy accounts. This component shows asynchronous communication between services using RabbitMQ and ability to create background jobs using Micronaut. It also features accessing database using Dapper.

* **PolicyService** - creates offers, converts offers to insurance policies. \
In this service we demonstrated usage of CQRS pattern for better read/write operation isolation. This service demonstrates two ways of communication between services: synchronous REST based calls to `PricingService` through HTTP Client to get the price, and asynchronous event based using RabbitMQ to publish information about newly created policies. In this service we also access RDBMS using NHibernate.

* **PolicySearchService** - provides insurance policy search. \
This module listens for events from RabbitMQ, converts received DTOs to “read model” (used later in search) and saves this in ElasticSearch. It also exposes REST endpoint for search policies.

* **PricingService** - calculates price for selected insurance product. \
For each product a tariff should be defined. The tariff is a set of rules on the basis of which the price is calculated. DynamicExpresso was used to parse the rules. During the policy purchase process, the `PolicyService` connects with this service to calculate a price. Price is calculated based on user’s answers for defined questions.

* **ProductService** - simple insurance product catalog. \
Information about products are stored in in-memory database. Each product has code, name, image, description, cover list and question list (affect the price defined by the tariff).

* **web-vue** - SPA application built with Vue.js and Bootstrap for Vue.

Each business microservice has also **.Api project** (`PaymentService.Api`, `PolicyService.Api` etc.), where we defined commands, events, queries and operations.