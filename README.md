# 📝 BloggingPlatformAPI

A RESTful API built with ASP.NET Core and Entity Framework Core, designed to manage blog posts, categories, and tags. It demonstrates clean architecture, efficient many-to-many relationship handling, and a code-first approach using MySQL via Pomelo.

---

## 🚀 Features

- 🧾 CRUD operations for blog posts
- 🗂 Dynamic category assignment by name
- 🏷 Tagging system with many-to-many support
- 🧼 DTO-based data contracts
- 📅 Automatic handling of creation and update timestamps
- 🐬 MySQL integration with Pomelo provider

---

## 🛠️ Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **Pomelo.EntityFrameworkCore.MySql**
- **MySQL**
- **C#**

---

## 📁 Project Structure

```
BloggingPlatformAPI/
├── Controllers/           # API Controllers
├── Data/                  # DbContext and config
├── Dtos/                  # DTO definitions
├── Entities/              # Entity models
├── Mapping/               # Entity <-> DTO mapping
├── Migrations/            # EF Core migrations
├── appsettings.json       # Configuration
└── Program.cs             # App entry point
```

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- Visual Studio / VS Code

### Installation Steps

1. **Clone the repository:**

   ```bash
   git clone https://github.com/OmarKhaled1504/BloggingPlatformAPI.git
   cd BloggingPlatformAPI
   ```

2. **Configure the database connection:**

   In `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;port=3306;database=BloggingPlatform;user=root;password=yourpassword;"
   }
   ```

   > 💡 Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables to avoid committing passwords.

3. **Apply migrations:**

   ```bash
   dotnet ef database update
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

   Visit `https://localhost:5001` or `http://localhost:5000`.

---

## 📬 API Endpoints

### Posts

- `GET /api/posts` – Get all posts
- `GET /api/posts/{id}` – Get a specific post
- `POST /api/posts` – Create a post
- `PUT /api/posts/{id}` – Update a post
- `DELETE /api/posts/{id}` – Delete a post

### Sample POST Request

```json
{
  "title": "Understanding ASP.NET Core",
  "content": "ASP.NET Core is a powerful web framework...",
  "category": "Web Development",
  "tags": ["ASP.NET", "C#", "EF Core"]
}
```

---

## 🗃️ Database Schema Overview

| Entity     | Fields |
|------------|--------|
| `Post`     | Id, Title, Content, CategoryId, CreatedAt, UpdatedAt |
| `Category` | Id, Name |
| `Tag`      | Id, Name |
| `PostTag`  | PostId, TagId (junction table) |

---

## 🧪 Testing

Use [Postman](https://www.postman.com/) or Swagger UI (auto-enabled in dev) to test endpoints.

---

## 🤝 Contributing

Contributions are welcome! Fork the project and submit a pull request.

---

## 📄 License

Licensed under the [MIT License](LICENSE).

---

## 📫 Contact

Created by [Omar Khaled](https://github.com/OmarKhaled1504)
