
# Information System

A Windows Forms application for managing a business information system. The application uses Oracle database for backend data management and supports user authentication, inventory management, and order processing.

---

## Features

- **User Authentication**: Supports user login and logout with role-based access control.
- **Inventory Management**:
  - Manage products and materials.
  - Track inventory and adjust stock levels.
- **Order Management**:
  - Create and manage customer orders.
  - Process material orders.
- **Role-Based Interface**:
  - Admin-specific functions.
  - Custom views for different user roles (e.g., Warehouse Manager, Economist).
- **Database Integration**: Uses Oracle Managed Data Access for database operations.

---

## Prerequisites

- .NET Framework
- Oracle Database with the appropriate schema
- Oracle.ManagedDataAccess library

---

## Installation

1. Clone this repository:
   ```bash
   git clone <repository_url>
   ```

2. Open the solution (`.sln`) file in Visual Studio.

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Update the database connection string in `DBconnect.cs`:
   ```csharp
   private OracleConnection connection = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = YOUR_HOST)(PORT = YOUR_PORT))(CONNECT_DATA = (SID = YOUR_SID))); User Id = YOUR_USER; Password=YOUR_PASSWORD");
   ```

---

## Usage

1. Build and run the application in Visual Studio.

2. Log in using valid credentials.

3. Navigate through the tabs to manage users, inventory, and orders.

---

## File Overview

- **`Program.cs`**: Entry point for the application. Initializes the main form.
- **`Form1.cs`**: Handles user login and provides a gateway to the main system.
- **`DBconnect.cs`**: Contains methods to manage Oracle database connections.
- **`informacny_system.cs`**: Implements the main application logic, including user role management, inventory operations, and order handling.
- **`Form1.Designer.cs`**: Auto-generated code for designing the login form.

---

## Database Requirements

Ensure that the Oracle database has the following tables:
- **`zamestnanci`**: Stores employee details.
- **`produkty`**: Stores product information.
- **`zakaznici`**: Stores customer details.
- **`objednavky`**: Stores order information.
- **`sklad_produktov`**: Tracks product inventory.

 - ** ! will be updated in future
---

## License

This project is licensed under the [MIT License](LICENSE).
