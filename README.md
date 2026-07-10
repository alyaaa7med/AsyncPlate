# AsyncPlate

 A production-inspired restaurant management system built with **ASP.NET Core**, following **Clean Architecture** and modern backend engineering practices.


<p align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-6DB33F?style=for-the-badge)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge)
![Hangfire](https://img.shields.io/badge/Hangfire-FF6C37?style=for-the-badge)
![QuestPDF](https://img.shields.io/badge/QuestPDF-2E7D32?style=for-the-badge)
![FluentValidation](https://img.shields.io/badge/FluentValidation-0F6CBD?style=for-the-badge)
![AutoMapper](https://img.shields.io/badge/AutoMapper-DD0031?style=for-the-badge)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)
![Mailtrap](https://img.shields.io/badge/Mailtrap-22C55E?style=for-the-badge)


</p>



# About

AsyncPlate is a production-inspired restaurant management platform that demonstrates modern backend engineering practices. It manages menus, orders, inventory, kitchen operations, notifications, reporting, and real-time communication while emphasizing clean architecture, scalability, and maintainability


#  Features

- **Authentication & Authorization** — JWT, Refresh Tokens, and role-based access control.
- **Menu Management** — Products, categories, extras, offers, and availability.
- **Order Management** — Complete order lifecycle with real-time status updates.
- **Kitchen Operations** — Instant order delivery and cooking workflow via SignalR.
-  **Inventory Management** — Suppliers, ingredients, recipes, and stock tracking.
-  **Notification System** — Real-time and email notifications with OTP support.
- **Reporting** — Automated PDF reports powered by QuestPDF and Hangfire.

# Database ERD

The system uses a relational database designed to maintain data integrity, minimize redundancy, and efficiently model restaurant operations.

<p align="center">
    <img src="screenshots/ERD.drawio.png" width="900" />
</p>




# Solution Structure
| | |
|---|---|
| AsyncPlate is built on **Clean Architecture**, organizing the solution into four independent layers with a clear separation of responsibilities. Dependencies always point inward, ensuring the Domain layer remains isolated from infrastructure and framework-specific concerns. This design makes the application easier to maintain, test, and extend. | <img src="screenshots/cleanarch.png" width="320"> |



```text
AsyncPlate/
│
├── AsyncPlate.API      # Presentation Layer (HTTP/WebSocket Gateway Entry point)
│   ├── Controllers
│   ├── Middlewares
│   └── Configuration
│
├── AsyncPlate.Application       # Use Case Layer (Core business orchestration)
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   ├── Mapping
│   └── Validation
│
├── AsyncPlate.Domain        # Core Layer (Zero external dependencies)
│   ├── Entities
│   └── Exceptions
│
└── AsyncPlate.Infrastructure       # Infrastructure Layer (External tools & data storage)
    ├── Data
    ├── Repositories
    ├── Services
    ├── Hubs
    ├── Jobs
    └── Configurations
```



# Engineering Decisions

| Decision | Rationale |
|----------|-----------|
| **Repository + Unit of Work** | Encapsulates data access and coordinates database transactions to maintain consistency across multiple operations. |
| **Asynchronous Programming** | Database operations and external services are implemented using `async`/`await` to improve scalability by avoiding blocked threads during I/O operations. |
| **Database Query Optimization** | Read queries were optimized using projection, selective eager loading, `AsNoTracking()` for read-only operations, and appropriate database indexing to improve performance while avoiding the N+1 query problem. |
| **SignalR** | Provides real-time updates for order status, menu changes, inventory availability, and notifications without client polling. |
| **Hangfire** | Handles long-running and scheduled background tasks such as sending emails and report generation without blocking user requests. 


# Screenshots
The following screenshots demonstrate testing some features of AsyncPlate.

<table>
 
  <tr>
    <td><img src="screenshots/Screenshot (257).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (258).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (259).png" width="100%"></td>
  </tr>
  <tr>
    <td><img src="screenshots/Screenshot (260).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (261).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (262).png" width="100%"></td>
  </tr>
  <tr>
    <td><img src="screenshots/Screenshot (263).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (264).png" width="100%"></td>
 <td><img src="screenshots/Screenshot (267).png" width="100%"></td>
  </tr>
  
    
  
  <tr>
    <td><img src="screenshots/Screenshot (269).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (270).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (271).png" width="100%"></td>
  </tr>
  <tr>
    <td><img src="screenshots/Screenshot (272).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (273).png" width="100%"></td>
    <td><img src="screenshots/Screenshot (274).png" width="100%"></td>
  </tr>
</table>
  
  
# Getting Started

Follow these steps to configure and run AsyncPlate locally.

## 1. Configure Application Settings

Create or update `AsyncPlate.API/appsettings.Development.json` with your local configuration values.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your default DB connection"
  },

  "Jwt": {
    "Key": "your-jwt-secret-key",
    "Issuer": "your issuer",
    "Audience": "your audience",
    "ExpireMinutes": 60
  },

  "MailSandBoxService": {
    "ApiToken": "your mail api token",
    "InboxId": 123456,
    "SenderEmail": "your-email@example.com",
    "SenderName": "AsyncPlate Team"
  }
}
```

> **⚠️ Security:** Never commit configuration files containing secrets or API keys. Keep them in `appsettings.Development.json` (which should be ignored by Git) or use environment variables in production.

---

## 2. Apply Database Migrations

Create the database and apply the latest Entity Framework Core migrations.

```bash
update-database
```

---

## 3. Run the Application

Start the API using:

```bash
dotnet run --project AsyncPlate.API --launch-profile https
```

---

## 4. Access Swagger UI

After the application starts successfully, open the Swagger UI in your browser:

```text
https://localhost:51499/swagger/index.html
```

> **Note:** The port number may differ depending on your local development environment.

# Author

**Alyaa Ahmed**

- GitHub: https://github.com/alyaaa7med
- LinkedIn: https://www.linkedin.com/in/alyaa-ahmed12/

# License

This project is licensed under the MIT License. See the [LICENSE](License.txt) file for details.