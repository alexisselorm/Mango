---

# Mango Microservices Architecture

## Overview

This repository showcases a production-style **microservices architecture** built using **.NET**, with a strong emphasis on **modularity**, **message-driven communication**, and **scalable service orchestration**. It includes a set of independent services communicating through **RabbitMQ**, integrated with a **Stripe-based payment system**, and fronted by an **API Gateway powered by Ocelot**.

The project is designed to demonstrate key architectural concepts such as service decoupling, asynchronous communication, and secure payment integration — all while maintaining clean separation of concerns and scalability best practices.

---

## Architecture Highlights

* **Microservices-based design**: Each business domain is encapsulated in its own service (Auth, Product, Order, Email, Coupon, etc.).
* **Ocelot API Gateway**: Routes incoming traffic to appropriate downstream services and manages API composition.
* **RabbitMQ Messaging**: Enables asynchronous communication between services for operations like email notifications and order events.
* **Stripe Payment Integration**: Secure, real-time payment processing via the Coupon and Checkout APIs.
* **Redis Caching**: High-performance caching layer to optimize read-heavy operations.
* **Database per service**: Promotes decoupling and autonomy across services.
* **Docker-ready**: Can be containerized for deployment using Docker Compose or Kubernetes (with extensions).

---

## Services

* `Mango.GatewaySolution`: API Gateway powered by Ocelot.
* `Mango.MessageBus`: Shared service bus abstraction for RabbitMQ messaging.
* `Mango.Services.AuthAPI`: Handles authentication and forwards cart data to message queue.
* `Mango.Services.CouponAPI`: Applies Stripe-based discount coupons.
* `Mango.Services.EmailAPI`: Listens to RabbitMQ for sending transactional emails.
* `Mango.Services.OrderAPI`: Manages order lifecycle and listens for order creation events.
* `Mango.Services.ProductAPI`: Product CRUD operations with support for image uploads.
* `Mango.Services.RewardsAPI`: Updates reward points through RabbitMQ.
* `Mango.Services.ShoppingCartAPI`: Manages cart state and sends order requests.
* `Mango.Web`: Frontend UI (MVC or Razor) to interact with the backend services.

---

## Tech Stack

* **.NET 6**
* **RabbitMQ**
* **Ocelot**
* **Stripe API**
* **Redis**
* **PostgreSQL / SQL Server**
* **Docker**

---

## Setup Instructions

1. Clone the repo:

   ```bash
   git clone https://github.com/alexisselorm/mango.git
   ```

2. Configure environment variables or `appsettings.json` for:

   * RabbitMQ credentials
   * Stripe keys
   * Connection strings

3. Run services locally:

   ```bash
   dotnet build
   dotnet run --project Mango.Web
   ```

---

## Future Improvements

* IdentityServer or JWT-based authentication
* Kubernetes Helm charts for deployment
* CI/CD integration with GitHub Actions
* Observability via Prometheus + Grafana

---

## License

This project is licensed under the MIT License.

---
