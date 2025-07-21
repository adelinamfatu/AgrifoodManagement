# 🌾 AgrifoodManagement

**AgrifoodManagement** is a full-featured ASP.NET Core MVC application designed to manage and sell agro-food resources. The platform supports both **producers** (e.g., local farmers) and **buyers** (individuals or businesses), offering powerful tools for stock management, product promotion, demand forecasting, and personalized shopping experiences.

---

## 📁 Solution Structure

The solution contains **four main projects**, each with a clear responsibility:

- **`AgrifoodManagement.Domain`**
  - Contains EF Core context, entity classes, table relationships, and mappings to SQL Server.

- **`AgrifoodManagement.Business`**
  - Implements core business logic using **CQRS** with **MediatR**.
  - Handles DTOs, rules, and database transactions via EF Core.

- **`AgrifoodManagement.Util`**
  - A shared utility library with common config classes, enums, and DTOs used across the solution.

- **`AgrifoodManagement.Web`**
  - ASP.NET Core MVC project hosting the web interface.
  - Defines routes, controllers, views, and service configurations.

---

## ⚙️ Technologies Used

- **Framework**: ASP.NET Core (cross-platform)
- **Language**: C#
- **ORM**: Entity Framework Core
- **Architecture**:
  - MVC (Model-View-Controller)
  - CQRS with MediatR
- **Frontend**: Razor Pages + integrated JavaScript
- **Machine Learning**: ML.NET
- **Chatbot**: Tidio (live chatbot with customizable Q&A)

---

## 👨‍🌾 Features for Producers

- Add and manage product listings with:
  - Expiration dates
  - Images
  - Predefined categories
  - Quantities and units
- Promote products manually or automatically (when nearing expiration)
- Archive, update, or boost listings
- Generate invoices and manage deliveries
- Apply discounts and update stock in real time
- Access an **internal discussion forum** (subscription-based)
- Receive demand forecasts using SSA algorithms:
  - Predictions available by **day**, **week**, or **month**
  - **95% confidence intervals** included

---

## 🛒 Features for Buyers

- Browse producers on an interactive map
- View personalized product recommendations based on:
  - Purchase history
  - Preferences
  - Proximity
- Add items to a wishlist
- Use discount codes at checkout (fixed or percentage)
- Review orders after delivery
- All reviews undergo **automated sentiment analysis**

---

## 🤖 Intelligent Chatbot

Integrated with **Tidio**, the chatbot:
- Provides instant, contextual support
- Is trained to respond to questions related to:
  - Product listing
  - Stock and inventory
  - Promotions and pricing
  - Forecasts and tools
- Continuously learns from user interactions

---

## 🏗️ Architecture Overview

- **Monolithic Deployment**: Single deployable unit ensures lower operational costs and full control
- **MVC Pattern**:
  - `Models` = Data and rules
  - `Views` = UI and layout
  - `Controllers` = Application flow
- **CQRS**: Clear separation between read and write responsibilities via MediatR handlers
- **ML.NET**: Used for demand prediction and analyzing product review sentiment
- **Tidio Chatbot**: Modular, real-time user support system

---

## 🌍 Target Users

- **Producers**: Small farmers and local food suppliers
- **Buyers**: Individuals or businesses seeking local, sustainable products

---

## 📸 Preview

You can find the database structure in [`Baza de date.svg`](./Baza%20de%20date.svg)

---

![Baza de date](https://github.com/user-attachments/assets/e00a71e9-e521-48d2-9fed-f8c52dc7db5a)
