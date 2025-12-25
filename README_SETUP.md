# AutoDocx Test

Dynamic document generation platform built with ASP.NET Core 8 and Angular.

## Project Structure

```
AutoDocx/
├── backend/
│   ├── AutoDocx.API/          # Web API layer
│   ├── AutoDocx.Core/         # Domain entities, DTOs, interfaces
│   └── AutoDocx.Infrastructure/ # Data access, external services
└── frontend/
    └── src/
        ├── app/
        │   ├── components/    # Angular components
        │   ├── services/      # API services
        │   └── models/        # TypeScript models
        └── environments/      # Environment configurations
```

## Backend (.NET)

### Prerequisites
- .NET 8 SDK
- PostgreSQL

### Setup

1. Navigate to backend directory:
```powershell
cd backend
```

2. Restore dependencies:
```powershell
dotnet restore
```

3. Update connection string in `AutoDocx.API/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=autodocx;Username=postgres;Password=yourpassword"
}
```

4. Create database migrations:
```powershell
cd AutoDocx.API
dotnet ef migrations add InitialCreate --project ..\AutoDocx.Infrastructure
dotnet ef database update
```

5. Run the API:
```powershell
dotnet run --project AutoDocx.API
```

API will be available at: `http://localhost:5000`

## Frontend (Angular)

### Prerequisites
- Node.js (v18+)
- npm

### Setup

1. Navigate to frontend directory:
```powershell
cd frontend
```

2. Install dependencies:
```powershell
npm install
```

3. Run the development server:
```powershell
npm start
```

Application will be available at: `http://localhost:4200`

## Features

### Implemented
- ✅ Template CRUD operations
- ✅ Dynamic field schema
- ✅ File storage service
- ✅ Document generation engine
- ✅ RESTful API
- ✅ Angular services and models
- ✅ Template list component

### To Do
- ⏳ Template creation form
- ⏳ Dynamic form generator
- ⏳ Document preview
- ⏳ PDF conversion
- ⏳ Authentication & authorization
- ⏳ User management

## API Endpoints

### Templates
- `GET /api/templates` - Get all templates
- `GET /api/templates/{id}` - Get template by ID
- `POST /api/templates` - Create template
- `PUT /api/templates/{id}` - Update template
- `DELETE /api/templates/{id}` - Delete template

### Documents
- `POST /api/documents/generate` - Generate document
- `GET /api/documents/{id}/download/word` - Download Word
- `GET /api/documents/{id}/download/pdf` - Download PDF
- `GET /api/documents/{id}/preview` - Preview document

## Technology Stack

### Backend
- ASP.NET Core 8
- Entity Framework Core 8
- PostgreSQL
- Open XML SDK (Word processing)
- QuestPDF (PDF generation)

### Frontend
- Angular 17
- TypeScript
- SCSS
- RxJS

## Development

### Backend Structure
- **AutoDocx.API**: Controllers, middleware, configuration
- **AutoDocx.Core**: Entities, DTOs, interfaces (business logic)
- **AutoDocx.Infrastructure**: Repositories, services, database context

### Frontend Structure
- **Components**: UI components
- **Services**: API communication
- **Models**: TypeScript interfaces

## License

MIT
