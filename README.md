# AutoDocx
dynamic templates, form-to-document automation, Word/PDF generation

# 📁 Complete File Structure
AutoDocx/
├── backend/
│   ├── AutoDocx.sln
│   ├── AutoDocx.API/
│   │   ├── Controllers/ (Templates, Documents)
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── AutoDocx.Core/
│   │   ├── Entities/ (Template, TemplateField)
│   │   ├── DTOs/
│   │   └── Interfaces/
│   └── AutoDocx.Infrastructure/
│       ├── Data/ (DbContext)
│       ├── Repositories/
│       └── Services/
└── frontend/
    ├── package.json
    ├── angular.json
    └── src/
        ├── app/
        │   ├── components/
        │   ├── services/
        │   └── models/
        └── environments/

# To run the backend:
cd backend/AutoDocx.API
dotnet restore
dotnet ef migrations add InitialCreate --project ..\AutoDocx.Infrastructure
dotnet ef database update
dotnet run

# To run the frontend:
cd frontend
npm install
npm start