# Yape Antifraud Solution

Yape is a .NET-based solution designed to handle transactions and anti-fraud operations. It consists of multiple projects, including APIs, background workers, and infrastructure components, all working together to provide a robust and scalable system.

## Table of Contents
- [Overview](#overview)
- [Projects](#projects)
- [Prerequisites](#prerequisites)
- [Running the Solution](#running-the-solution)
- [Testing](#testing)

---

## Overview

The Yape solution is built using .NET 8 and follows a microservices architecture. It includes:
- APIs for handling transactions and anti-fraud operations.
- Background workers for processing tasks asynchronously.
- Infrastructure components for database access, messaging, and external API integrations.

---

## Projects

### 1. **API Projects**
- **Yape.API.Transaction**: Handles transaction-related operations.
- **Yape.API.Antifraud**: Manages anti-fraud operations.

### 2. **Background Workers**
- **Yape.Transaction.Worker**: Processes transaction-related background tasks.
- **Yape.AntiFraud.Worker**: Handles anti-fraud background tasks.

### 3. **Infrastructure**
- **Yape.Infrastructure.Kafka**: Provides Kafka-based messaging for communication between services.
- **Yape.Infrastructure.Postgresql**: Implements database access using PostgreSQL.
- **Yape.Infrastructure.TransactionApi**: Handles external transaction API integrations.
- **Yape.Infrastructure.AntiFraudApi**: Manages external anti-fraud API integrations.

### 4. **Domain**
- **Yape.Domain**: Contains core domain models, interfaces, and constants shared across the solution.

### 5. **Application**
- **Yape.Application.Transaction**: Implements transaction-related business logic.
- **Yape.Application.Antifraud**: Implements anti-fraud-related business logic.

---

## Prerequisites

Before running the solution, ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (for running services in containers)
- [PostgreSQL](https://www.postgresql.org/) (if not using Docker for the database)
- [Kafka](https://kafka.apache.org/) (for messaging)

---

## Running the Solution local environment

1. Open solution Yape.Antifraud.sln into Visual Studio 2022
2. Right click in docker-compose project/file /Debug/Start new instance
3. Test Api Trasanction using postman collection include in repo [link](./postman-collection/Yape.API.Transaction.postman_collection.json)
4. Test Api Antifraud using postman collection include in repo [link](./postman-collection/Yape.API.AntiFraud.postman_collection.json)


---

## Testing

Unit tests are located in the `tests` directory. To run the tests, execute:

dotnet test