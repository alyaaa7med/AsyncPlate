# 🍽️ AsyncPlate 

AsyncPlate is an asynchronous restaurant management backend system designed to handle complex business workflows, real-time customer notifications, and automated background processing. 


## 🚀 Core Features 

### 1. Authentication & Access Control

- Secure user authentication using JWT tokens.
- Role-based access control to manage user permissions.

### 2. Menu & Recipe Management

- Manage menu, categories, and products.
- Support customizable extra add-ons.
- Manage recipes and their ingredients.

### 3. Order Management
 Handle the complete order lifecycle.
- Track orders in real time.
- Provide instant updates to customers and chefs.
### 4. Inventory & Supplier Management
- Monitor inventory levels automatically.
- Detect low-stock items.
- Send daily inventory reports to suppliers.

 ### 5. Customer Engagement
- Create and manage promotional offers.
- Send targeted offers to VIP customers.
- Allow customers to submit ratings and reviews.

### 6. Notifications & Real-Time Features

- Deliver real-time order updates.
- Broadcast instant notifications to users.
- Improve communication between customers, chefs, and admins.

### 7. Automation & Analytics

- Automate repetitive business tasks using background jobs.
- Generate daily business reports and analytics.

## 🗃️ Database Schema & ERD


<p align="center">
  <img src="screenshots/Async Plate DB diagram.JPG" alt="AsyncPlate Entity Relationship Diagram" width="850">
</p>

## 🏗️ Architectural Overview

The project is structured into four layers to isolate core business rules from external frameworks, databases, and third-party services. Dependencies flow strictly inward toward the Domain layer.

```text
AsyncPlate/
│
├── 🔹 AsyncPlate.Domain/                  # Core Layer (Zero external dependencies)
│   ├── Entities/                        # Database entities (e.g., Order, Product, User)
│   └── Exceptions/                      # Custom core domain exceptions
│
├── 🔹 AsyncPlate.Application/             # Use Case Layer (Core business orchestration)
│   ├── Common/                          # Shared application utilities and helper extensions
│   ├── DTOs/                            # Decoupled Data Transfer Objects for client payloads
│   ├── Exceptions/                      # Application-specific workflow exceptions
│   ├── Interfaces/                      # Repository contracts and Unit of Work abstractions
│   │   ├── Repositories/                # IOrderRepo, IProductRepo, IInventoryRepo, etc.
│   │   └── Services/                    # Core business engine contracts
│   ├── Jobs/                            # Background processing contracts (IInventoryJob, IOrderJob, etc.)
│   ├── Mapping/                         # Object-to-object mapping profiles (e.g., AutoMapper)
│   └── Services/                        # Application Business Services
│       ├── Implementation/              # Concrete application logic (e.g., InventoryService)
│       └── Interfaces/                  # Local service interfaces
│
├── 🔹 AsyncPlate.Infrastructure/          # Infrastructure Layer (External tools & data storage)
│   ├── Configurations/                  # Entity Framework Core fluent API schema maps
│   ├── Data/                            # Repositories implementation & AppDbContext
│   ├── Hubs/                            # Real-time WebSocket hubs (NotificationHub for SignalR)
│   ├── Migrations/                      # EF Core database schema version snapshots
│   ├── Services/                        # Concrete Third-Party/OS implementations
│   │                                    # (JwtTokenService, MailTrapEmailService, MediaService, PdfService, SignalRNotificationSender)
│   ├── Settings/                        # Infrastructure-specific strongly-typed option configs
│   └── Jobs/                            # Concrete job task runner implementations (e.g., Hangfire/Hosted Services)
│
└── 🔹 AsyncPlate.API/                     # Presentation Layer (HTTP/WebSocket Gateway Entry point)
    ├── Connected Services/              # Visual Studio connected service configurations
    ├── Controllers/                     # REST API endpoints invoking Application use cases
    ├── Middlewares/                     # Global exception interceptors and logging pipelines
    ├── Models/                          # Presentation-specific request/view models


```

## 🚦 Getting Started 

### 1. Configure Environment Variables

Create `AsyncPlate.API/appsettings.Development.json` and add the following settings with your local values.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your default DB connection"
  },

  "Jwt": {
    "Key": "your-jwt-secret-key",
    "Issuer": "your issuer",
    "Audience": "your audience",
    "ExpireMinutes": number of minutes
  },

  "MailSandBoxService": {
    "ApiToken": "your mail api-token",
    "InboxId": your id,
    "SenderEmail": "your-email@example.com",
    "SenderName": "AsyncPlate Team"
  }
}
```

> **⚠️ Security Note:** never commit this file on github add it to the ignored files.


### 2. Apply Database Migrations

Run the following command to create and update the database schema:

```bash
update-database
```


### 3. Run the Application

Start the API using:

```bash
dotnet run --project AsyncPlate.API --launch-profile https
```

### 4. Open Swagger UI

Once the application is running, open Swagger in your browser:

```text
https://localhost:51499/swagger/index.html
```

> **Note:** The port number may vary depending on your local configuration.