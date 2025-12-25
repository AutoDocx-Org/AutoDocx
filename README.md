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

# Available endpoints:
🏠 Home/Welcome: http://localhost:5000
📚 Swagger Documentation: http://localhost:5000/swagger
❤️ Health Check: http://localhost:5000/health
📄 Templates API: http://localhost:5000/api/templates
📝 Documents API: http://localhost:5000/api/documents
Open http://localhost:5000/swagger in your browser to see and test all your API endpoints!

# To run the frontend:
cd frontend
npm install
npm start