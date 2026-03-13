# Restaurant Order Management System

Angular frontend and ASP.NET Core Web API backend for menu, customers, and orders.

## Prerequisites

- **Node.js** (for Angular)
- **.NET SDK** (for backend)
- **Database:** In Development the app uses **SQLite** by default (no install needed). For **SQL Server**, set the connection string in `appsettings.json` or `appsettings.Development.json`.

## Run from scratch

### 1. Backend (API on http://localhost:5006)

```bash
cd backend/RestaurantAPI
dotnet restore
dotnet run
```

- Uses the `http` launch profile: **http://localhost:5006**
- In Development, the app uses **SQLite** (`restaurant.db` in the API folder) by default: the database is created and seeded on first run. No SQL Server required. To use SQL Server instead, set `ConnectionStrings:DefaultConnection` in `appsettings.Development.json` to your SQL Server connection string.
- Swagger UI: **http://localhost:5006/swagger**

### 2. Frontend (Angular on http://localhost:4200)

```bash
cd frontend/restaurant-app
npm install
ng serve
```

- App: **http://localhost:4200**
- Default route redirects to **/menu**. Use the header links: Menu, Order, Admin, Order History.

### 3. Verify

- Open http://localhost:4200/menu — menu should load from the API (or show “No menu items yet” if the DB is empty).
- Add menu items at http://localhost:4200/admin, then refresh the menu page.
- Place an order from the menu page (cart + Place Order). Customer ID is fixed to 1 for demo; create customers via API or Admin if needed.

## Configuration

- **API base URL (frontend):** `frontend/restaurant-app/src/environments/environment.ts` → `apiBaseUrl: 'http://localhost:5006/api/'`
- **Backend port:** `backend/RestaurantAPI/Properties/launchSettings.json` → `applicationUrl` in the `http` profile
- **Database:** `backend/RestaurantAPI/appsettings.json` → `ConnectionStrings:DefaultConnection`

## Project structure

- **backend/RestaurantAPI** — Controllers, Services, Repositories, Models, DTOs, Data (DbContext), Migrations
- **frontend/restaurant-app** — Angular Material app: `pages/` (role-selection, login, register, admin/*, customer/*), `layouts/`, `services/` (auth, cart, menu, order, customer, toast), `guards/`, `interceptors/`
- **database/schema.sql** — Reference SQL schema (optional)
