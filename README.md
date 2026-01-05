# Task Management Application

A modern full-stack task management application built with .NET 8 and React + TypeScript.

## Prerequisites

Before running the application, ensure you have the following installed:

- **Docker & Docker Compose** (for containerized execution)
- **.NET 8 SDK** (for local execution)
- **Node.js 20+** (for local execution)

---

## Getting Started

This project supports two execution modes: **Docker** (recommended) or **Local CLI** (for development and debugging).

### Option 1: Docker (Recommended)

The simplest way to start both Backend and Frontend without manual configuration.

1. **Navigate to the project root:**
   ```bash
   cd TaskManagement
   ```

2. **Start the containers:**
   ```bash
   docker-compose up --build
   ```

3. **Access the application:**
   - **Frontend:** http://localhost:5173
   - **API (Swagger):** http://localhost:5000/swagger

> **Note:** In Docker mode, the API runs on HTTP port 5000, and the Frontend connects automatically via the configured `.env` file.

---

### Option 2: Local Execution (Development)

Use this method for debugging locally with Visual Studio (IIS Express) or CLI.

#### 1. Start the Backend

Open a terminal and navigate to the API project folder:

```bash
cd TaskManagement.Api/src/TaskManagement.Api
dotnet run
```

The API will start at: **https://localhost:7064**

#### 2. Start the Frontend

Open a new terminal and navigate to the Frontend folder:

```bash
cd TaskManagement.Web
npm install
npm run dev:local
```

> **Important:** The `npm run dev:local` command connects to the local HTTPS backend on port **7064** instead of the Docker default.

#### 3. Access the application

- **Frontend:** http://localhost:5173
- **API (Swagger):** https://localhost:7064/swagger

---

## Tech Stack

- **Backend:** .NET 8 Web API
- **Frontend:** React 19 + TypeScript + Vite
- **Containerization:** Docker & Docker Compose

---

## Architecture and Design Patterns

### Architecture: Pragmatic Clean Architecture

The solution follows the **Pragmatic Clean Architecture** style, widely adopted in the .NET ecosystem.

While derived from the core principles of **Ports & Adapters (Hexagonal)** and **Robert C. Martin's Clean Architecture**, I explicitly chose this pragmatic structure (API as the entry point referencing Application/Infra) over the strict canonical approach.

**Reason:** It aligns better with standard .NET conventions (Dependency Injection setup in `Program.cs`), improving maintainability and Developer Experience (DX) without sacrificing the core principle of Domain isolation.

#### Project Structure

- **Domain:** Contains the core entities, repository interfaces, and static domain validation logic
- **Application:** Holds the business logic through explicitly defined Command and Query Handlers
- **Infrastructure:** Implements the data access layer using Entity Framework Core
- **API:** The entry point containing Controllers and DI configuration

### Key Design Decisions

#### 1. Explicit Handler Injection

The application utilizes **explicit Handler injection** directly into Controllers using the `[FromServices]` attribute.

**Benefit:** This approach ensures a transparent dependency chain. Unlike event-based or mediator patterns that can obscure the execution flow, explicit injection makes the code significantly easier to navigate and debug, strictly adhering to the **"Explicit Dependencies"** principle.

#### 2. Lightweight Command Validation

Command validation is implemented using specialized, stateless validation classes (e.g., `AddTaskItemCommandValidation`).

**Benefit:** This keeps the Application layer lightweight and performant, avoiding the overhead of heavy external libraries for simple validation rules while maintaining a clean separation of concerns.

#### 3. Static Domain Validation

Business rules and data integrity checks are centralized in static validation methods within the Domain layer (`TaskItemDomainValidation`).

**Benefit:** This pattern ensures that the Domain Entity remains the **"single source of truth"** for validity. It protects the domain invariants against any entry point, preventing the instantiation of invalid entities without the complexity of stateful domain services.

---

## Testing Strategy

The project includes a robust test suite (**xUnit**) with a specific focus on:

### Unit Testing
Covering domain validation rules and individual business logic components.

### Scenario-Based Testing
I implemented **Scenario Tests** (e.g., `TaskItemHandlersTest`) that exercise the full lifecycle of a feature—from writing data via an Add Handler to retrieving it via a Get Query and modifying it via a Toggle Handler.

**Value:** This strategy validates the consistency between the Command and Query models, ensuring that data persisted by write operations is correctly projected and retrievable by read operations.

---

## Frontend Architecture (React + Vite)

The frontend was built using **React 19** and **TypeScript**, focused on performance, type safety, and a clear separation between UI and Business Logic.

### 1. State Management Strategy (Server vs. Client State)

Instead of using a global state manager (like Redux) for everything, I adopted **TanStack Query (React Query)**.

**Reason:** This library specifically handles **Server State** (caching, deduplication, background updates, and loading states). This drastically reduces boilerplate code and ensures the UI is always in sync with the backend without manual `useEffect` fetching logic.

### 2. Component & Logic Separation (Custom Hooks)

I implemented the **Custom Hook Pattern** to separate logic from presentation.

**Benefit:** Components remain focused on rendering UI (JSX). Complex logic, such as form handling or data transformations, is extracted into hooks (e.g., `useTaskItems`). This makes the code more testable and reusable.

### 3. Styling: Utility-First (Tailwind CSS)

I chose **Tailwind CSS** for styling.

**Reason:** The utility-first approach allows for rapid UI development and ensures design consistency through a centralized configuration. It also provides superior performance by generating minimal CSS bundles at build time, avoiding the runtime overhead associated with traditional CSS-in-JS solutions.

### 4. API Layer

HTTP communication is centralized via **Axios**.

**Implementation:** A singleton Axios instance is configured with base URLs that adapt to the environment (Docker HTTP vs. Local HTTPS). This centralization allows for easy implementation of interceptors for global error handling.

### 5. Developer Experience (Vite)

The project uses **Vite** as the build tool.

**Reason:** It provides instant server start and **Hot Module Replacement (HMR)**, significantly speeding up the development feedback loop compared to Webpack-based solutions.

---

## Notes

- Make sure no other services are using ports **5000**, **5173**, or **7064**