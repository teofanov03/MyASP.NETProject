# MyASP.NETProject

This is an **ASP.NET Core** web application that implements a small online shop, a blog, and a personal to-do list.

---

## Features

### 🛒 Shop
- Display products
- Add products to the cart (AJAX)
- Increase / decrease product quantity
- Remove products from the cart
- Total price updates automatically
- Session state preserves cart content
- Cart item count shown in navbar

### 📝 Blog
- Users can create blog posts
- Edit or delete **only their own posts**
- Add comments to posts
- Edit or delete **only their own comments**
- View all posts from all users


### ✅ To-Do List
- Users can add tasks
- Mark tasks as completed
- Edit or delete tasks
- Set deadlines for tasks
- **API endpoints for Tasks**:
  - `GET /api/tasksapi` → get all tasks for logged-in user
  - `POST /api/tasksapi` → add a new task
  - `PUT /api/tasksapi/{id}` → edit an existing task
  - `DELETE /api/tasksapi/{id}` → delete a task
  - `POST /api/tasksapi/toggle/{id}` → toggle task completion
- All API responses are returned in **JSON format**
- **Swagger** is integrated for testing API endpoints

---

## Technologies
- ASP.NET Core MVC
- Razor Pages
- jQuery / AJAX
- Bootstrap
- Session state
- ASP.NET Core Web API
- Swagger / OpenAPI for API documentation
