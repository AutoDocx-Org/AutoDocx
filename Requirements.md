✅ Requirements (Final Understanding)

✔ Users can create multiple templates
✔ Templates can be edited later
✔ No fixed object / no fixed fields
✔ Each template defines its own dynamic form
✔ User fills form → document auto-generated
✔ Output: Word/PDF Preview before download
✔ Low-cost / mostly free

🧠 Key Design Idea (Important)

👉 Template = Document + Field Definitions

Each template contains:
Word document with placeholders
Field schema (JSON) that defines the form
This is exactly how DocuSign / Adobe Sign / HR tools work internally.

🏗️ Final Tech Stack (Still Low Cost)
🔹 Backend

ASP.NET Core 8 Web API

Libraries:
Word → Open XML SDK
PDF → QuestPDF
JSON → Native System.Text.Json

🔹 Frontend

Angular
Dynamic forms using JSON schema
Preview support

🔹 Database (Free)

PostgreSQL (Supabase / Railway free tier)

🔹 Storage

Azure Blob (Free tier) OR local storage

📦 Data Model (Very Important)
🔹 Template Table
Template
--------
Id (UUID)
Name
Description
WordFilePath
CreatedAt
UpdatedAt

🔹 TemplateField Table
TemplateField
--------------
Id
TemplateId
FieldKey        -- FirstName
Label           -- First Name
Type            -- text, date, select
IsRequired
OptionsJson     -- for dropdowns
Placeholder     -- {{FirstName}}


OR store fields as JSON:

[
  {
    "key": "firstName",
    "label": "First Name",
    "type": "text",
    "placeholder": "{{FirstName}}",
    "required": true
  },
  {
    "key": "gender",
    "label": "Gender",
    "type": "select",
    "options": ["Male","Female"]
  }
]

🧩 Flow (End-to-End)
🔹 1. Create Template (User)

User uploads:

Word file
Defines fields via UI

Example UI:

Field Name
Label
Type (text/date/dropdown)
Placeholder

Saved as:

Word file
Field JSON schema

🔹 2. Edit Template

User can:

Edit fields
Replace Word file
Add/remove placeholders

🔹 3. Fill Form (Dynamic)

Frontend:

Reads field schema
Auto-generates form
No fixed object ✔

🔹 4. Generate Document

Backend:

Loads Word template
Replaces placeholders dynamically
Saves filled document

🔹 5. Preview + Download

Convert to PDF
Show preview
Allow Word & PDF download

📄 Placeholder Replacement (Dynamic)
foreach (var text in doc.MainDocumentPart.Document.Descendants<Text>())
{
    foreach (var field in fields)
    {
        text.Text = text.Text.Replace(
            field.Placeholder,
            request.Data[field.Key]?.ToString() ?? ""
        );
    }
}


Works for any number of fields ✔

🧪 Example API Contract
Generate Document
POST /api/document/generate
{
  "templateId": "uuid",
  "data": {
    "firstName": "Chetan",
    "lastName": "Deore",
    "gender": "Male",
    "joiningDate": "2025-12-21"
  }
}

👁️ Preview Strategy (Best)

✔ Generate PDF
✔ Return preview URL
✔ Render in iframe

🔐 Security & Validation

✔ Validate missing placeholders
✔ Field-type validation
✔ Template ownership check
✔ Size limits

💰 Cost Estimate
Component	Cost
Backend	Free
Frontend	Free
DB	Free
Storage	Free
Total	₹0 – ₹300
🚀 Recommended MVP Plan (2 Weeks)
Week 1

Template CRUD
Field schema builder
Word upload
Week 2
Dynamic form generation
Document generation
Preview + download

🎯 This Architecture Is Scalable

✔ Unlimited templates
✔ Unlimited fields
✔ Any document type
✔ Multi-tenant ready
✔ Enterprise-grade design






