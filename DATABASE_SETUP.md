# Database Setup Script for AutoDocx

## PostgreSQL Setup

### 1. Install PostgreSQL
Download and install PostgreSQL from: https://www.postgresql.org/download/

### 2. Create Database

```sql
-- Connect to PostgreSQL
psql -U postgres

-- Create database
CREATE DATABASE autodocx;

-- Connect to the database
\c autodocx;

-- Verify connection
SELECT current_database();
```

### 3. Run Migrations

From the backend/AutoDocx.API directory:

```powershell
dotnet ef migrations add InitialCreate --project ..\AutoDocx.Infrastructure
dotnet ef database update
```

### 4. Verify Tables

```sql
-- List all tables
\dt

-- Should see:
-- Templates
-- TemplateFields
```

### 5. Test Data (Optional)

```sql
-- Insert sample template
INSERT INTO "Templates" ("Id", "Name", "Description", "WordFilePath", "CreatedAt", "UpdatedAt")
VALUES (gen_random_uuid(), 'Sample Template', 'Test template', 'sample.docx', NOW(), NOW());
```

## Connection String

Default connection string in appsettings.json:
```
Host=localhost;Port=5432;Database=autodocx;Username=postgres;Password=postgres
```

Update the password to match your PostgreSQL installation.
