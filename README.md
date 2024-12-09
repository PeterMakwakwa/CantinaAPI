# CantinaAPI

## Overview

Cantina API is a robust, production-ready REST API built using ASP.NET Core 8. It provides functionality for managing menu items (dishes and drinks), user authentication and authorization, and rating and reviewing menu items. This API adheres to best practices, is designed with maintainability and scalability in mind, and follows SOLID principles.
The only think that is questionable on the production readiness is the use of in-memory cache. This is not recommended for production use, as it is not distributed and does not scale well. For production use, a distributed cache like Redis should be used. The storing of secrets, i would have loved to use something like Vault to store all my secrets or even have them in the environment variables

---

## Features

- **Menu Item Management**:
  - Create, view, list, update, and delete dishes and drinks.
  - Menu items have a name, description, price, and image.
  - Supports searching for menu items.

- **Rating and Review**:
  - Customers can rate and review menu items.

- **Authentication & Authorization**:
  - User registration and login with password-based authentication.
  - Supports OAuth2-based Single Sign-On (SSO) with Google.
  - Password policies and brute-force attack prevention.
  - Role-based permission handling.

- **Cache Optimization**:
  - Menu items are cached for optimal performance.
  - Cache settings are configurable.

- **Secure and Validated**:
  - Entity validation for all data models.
  - Prevention against brute-force attacks using rate-limiting.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- Docker Desjtop
- MSSQL Server (via Docker)

---

## Setup Instructions


### 1. Create SQL server container using Docker 
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=******!" -p 1433:1433 -v "sqlvolume:/var/opt/mssql" --name sqlserver mcr.microsoft.com/mssql/server:2019-latest

you can also use the docker-compose file: docker-compose.yaml included in the project directory to create the container
```

### 2. Clone the CantinaAPI Repository
```bash
git clone https://github.com/PeterMakwakwa/CantinaAPI.git
cd cantina-api

```
Please note that the project include the unit tests, so you can run the tests to see if everything is working as expected