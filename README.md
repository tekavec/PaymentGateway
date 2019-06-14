# Payment Gateway

## Overview

**Payment Gateway** is ASP.NET Core based API, which enables merchants to process their payments and to retrieve their payments previously made.

### Business Requirements
1. A merchant should be able to process a payment through the payment gateway and receive either a
successful or unsuccessful response.
2. A merchant should be able to retrieve the details of a previously made payment.

Top-level diagram of actors and components:

![PaymentGateway top level diagram](/img/top-level-diagram.png?raw=true "PaymentGateway top level diagram")

The objective was to built **Payment Gateway** and to simulate **Acquiring Bank** (or Acquirer) with a mock, which can be easily replaced.

This repository includes the following components:
- PaymentGateway: a self hoted Web API  
- Acquirer.Client: a wrapper around Acquirer mock
- Acquirer: a mock which simulates real Acquiring Bank (Docker container)

## Assumptions

These assumptions were made to simplify MVP and would be subject of clarification with stakeholders:

- Acquiring Bank returns very simple JSON object: `{"PaymentId": "{{UUID}}", "IsPaymentSuccessful": {(boolean}}}`
- After a payment was processed by the acquirer, PaymentGateway stores acquirer's unique identifier and processing status but it doesn't store sensitive data - it doesn't store CVV (there would be a separate storage in real app) and it stores only masked Card number
- At the moment, only boolean value is used for acquirer's response; in real app, this would be expanded to distinct values (e.g. enum) with accompanying response message
- In-memory 'database' (plain dictionary) is used for MVP, which means data is not persisted with restarting the application
- Unique identifier of a payment, which is returned by the acquiring bank is not exposed to merchants, but it is saved to the database. I've assumed PaymentGateway should expose only its internal payment unique identifier to merchants
- As much as possible validation should be implemented in the PaymentGateway as we don't want to use unnecessary API calls.
- No money amount limits were implemented in MVP - I assume this would be defined in coordination with the acquiring bank.

## Using PaymentGateway API

### Prerequisites

- [.NET Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- Docker (e.g. [Docker Desktop for Windows](https://hub.docker.com/editions/community/docker-ce-desktop-windows))

### Building the solution

To build solution from the source files, use the following command:
```
dotnet build
```

### Running the solution

Use the following command:
```
dotnet run --project src\PaymentGateway
```

By default, application will be accessible on the port 55166.

### Using Acquiring Bank mock

Acquiring bank simulator can be started with `docker-compose` from the command line:
```
docker-compose up
```

Its configuration file is located in [mock-acquirer-setup.json](/config/mock-acquirer-setup.json). By default, it listens on port `8083` and `/payment` endpoint of type `POST`.
Default configuration:
```
{
  "request": {
    "method": "POST",
    "path": "/payment"
  },
  "response": {
    "statusCode": 200,
    "headers": {
      "Content-Type": [ "application/json" ]
    },
    "body": "{\"PaymentId\": \"{{fake.UUID}}\", \"IsPaymentSuccessful\": true}"
  }
}
```


### User interface

- [Swagger](https://localhost:55166/index.html)
- [Prometheus Metrics](https://localhost:55166/Metrics)

### Testing

Before running the tests, Acquiring Bank mock must be running, otherwise some of integration tests will fail.

#### Using make tool

If you have `bash` and `make` tool installed on your computer, use the following command:

```
make test
```

#### Using docker-compose 

If you have docker-compose installed on your computer, use the following command:
```
docker-compose -f docker-compose.test.yml up
```

#### Using Visual Studio 

All unit tests could be executed via Visual Studio test runners. 
In order to run integration tests, Acquirer mock must be running in CMD window, use the following command:

```
docker-compose up
```

In addition, we need to add entry to hosts file as `Acquirer.Client` uses dockerised service name (`acquirer`).

- new `hosts` file entry (Windows): 
```
127.0.0.1  acquirer
```

### Authorisation

Both endpoints are secured and a valid JWT token must be used to use them. The easiest way is to use Swagger UI - `Account/GenerateToken` endpoint and `Athorize` button. 
In the MVP version and for testing purposes, `Acccount` controller with token generation endpoint is exposed. This would be obviously removed in production and replaced by dedicated authentication & authorization service.
Also, CORS is set to allow any host access.

## Notes

- CI support: in absence of out-of-the-box solutions (e.g. Azure DevOps), `Makefile` targets (`build`, `test`) can be used for automated build and testing
- Serilog logger is used for logging purposes
- Acquirer.Client was implemented to abstract handling with the bank from the payment gateway.

### Points of further improvements:

- replacing in-memory database with the real one
- CORS white/blacklisting
- encryption 
- correlationId in logs
- performance testing included in the CI pipeline (e.g. Gatling/Scala)
- memory caching (currently, only response caching is used for the GET endpoint)
- fine grained metrics (currently, only endpoint counters are implemented and basic setup in `docker-compose.metrics.yml` configured)
- CLI tool for easier web API usage