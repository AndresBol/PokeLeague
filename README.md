<p align="center">
  <img src="PokeLeague.Web/wwwroot/media/light/isologo.png" alt="PokeLeague Banner" width="400"/>
</p>

<h1 align="center">PokeLeague</h1>

<p align="center">
  A web-based Pokémon card marketplace for collectors and traders, featuring auction management, bidding, and card cataloging - built with ASP.NET Core MVC and SQL Server.
  <br/>
  <strong>Programming VI Final Project</strong> - Universidad Técnica Nacional (UTN)
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-.NET%209.0-purple?logo=dotnet&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/SQL%20Server-2022-blue?logo=microsoftsqlserver&logoColor=white" alt="SQL Server"/>
  <img src="https://img.shields.io/badge/UI-ASP.NET%20Core%20MVC-green?logo=dotnet&logoColor=white" alt="ASP.NET Core MVC"/>
  <img src="https://img.shields.io/badge/ORM-Entity%20Framework%20Core-orange?logo=nuget&logoColor=white" alt="EF Core"/>
  <img src="https://img.shields.io/badge/CSS-Bootstrap%205-7952B3?logo=bootstrap&logoColor=white" alt="Bootstrap"/>
</p>

---

## About

PokeLeague is a full-stack web application built as a marketplace platform for Pokémon trading card enthusiasts. Users can register their card collections with detailed metadata (set, rarity, language, condition, images), list them for auction with configurable pricing rules, and place competitive bids on cards from other collectors. The platform manages the complete auction lifecycle, from scheduling through active bidding to completion and purchase order generation.

This project was developed as the final assignment for the Programming VI course, demonstrating skills in layered architecture, object-relational mapping, dependency injection, design patterns (Repository, Service, DTO/AutoMapper), and responsive web UI development.

## Features

- **Card catalog management**: Register Pokémon cards with name, description, condition grade, set, rarity, language, multiple images, and category tags
- **Guided card creation wizard**: Step-by-step card registration flow for a streamlined user experience
- **Image carousel display**: Browse card listings with multi-image carousels and condition badges
- **Auction system**: Create time-bound auctions with base price and minimum bid increment rules
- **Auction lifecycle tracking**: Automatic status resolution - Scheduled, In Progress, Finished, or Canceled - based on date logic
- **Competitive bidding**: Place bids on active auctions with real-time validation against minimum increase thresholds
- **Purchase order generation**: Automatic purchase record creation when auctions close
- **User management**: View, edit, and block/unblock user accounts
- **Category tagging**: Assign multiple categories to cards via a many-to-many relationship
- **Smart form validation**: Server-side and client-side validation with SweetAlert2 notifications
- **Soft-delete pattern**: All entities support logical activation/deactivation (`is_active` flag)
- **Responsive UI**: Bootstrap 5 layout with Bootstrap Icons, Tom-Select enhanced dropdowns, and dark/light branding assets

## Tech Stack

| Layer         | Technology                           |
| ------------- | ------------------------------------ |
| Language      | C# (.NET 9.0)                        |
| Web Framework | ASP.NET Core MVC                     |
| Database      | Microsoft SQL Server 2022            |
| ORM           | Entity Framework Core 9.x            |
| Mapping       | AutoMapper 16.0.0                    |
| Frontend      | Bootstrap 5, Bootstrap Icons, jQuery |
| Dropdowns     | Tom-Select 2.4.3                     |
| Notifications | SweetAlert2                          |
| IDE           | Visual Studio                        |

## Architecture

The project follows a three-tier layered architecture with clear separation of concerns and full dependency injection:

```
PokeLeague/
├── PokeLeague.Web/                    # Presentation Layer
│   ├── Controllers/                   #   MVC Controllers (Home, User, Card, Auction)
│   ├── Views/                         #   Razor views organized by feature
│   │   ├── Home/                      #     Landing page
│   │   ├── Card/                      #     Card CRUD + guided creation wizard
│   │   ├── Auction/                   #     Auction CRUD + bid management
│   │   ├── User/                      #     User profiles & management
│   │   └── Shared/                    #     Layout, partials, error page
│   ├── Util/                          #   SweetAlert2 notification helper
│   ├── wwwroot/                       #   Static assets (CSS, JS, media, libraries)
│   └── Program.cs                     #   DI registration & pipeline configuration
│
├── PokeLeague.Application/           # Business Logic Layer
│   ├── Services/
│   │   ├── Interfaces/               #   Service contracts (IServiceCard, IServiceAuction, etc.)
│   │   └── Implementations/          #   Business rules, validation, orchestration
│   ├── DTOs/                         #   Data Transfer Objects (12 record types)
│   └── Profiles/                     #   AutoMapper entity ↔ DTO profiles (12 profiles)
│
├── PokeLeague.Infraestructure/       # Data Access Layer
│   ├── Data/
│   │   └── PokeLeagueContext.cs      #   EF Core DbContext with Fluent API configuration
│   ├── Models/                       #   Entity classes (12 domain models)
│   └── Repository/
│       ├── Interfaces/               #   Repository contracts
│       └── Implementations/          #   EF Core repository implementations
│
└── db_query.sql                      # Full database creation script
```

**Design Patterns:**

| Pattern                  | Implementation                                                                                     |
| ------------------------ | -------------------------------------------------------------------------------------------------- |
| **Repository**           | Generic CRUD repositories per entity with EF Core, eager loading, and `AsNoTracking` for reads     |
| **Service Layer**        | Business logic encapsulated in services with validation and auction status resolution              |
| **DTO / AutoMapper**     | 12 DTO records with display annotations, mapped via AutoMapper profiles for clean layer boundaries |
| **Dependency Injection** | All repositories and services registered as Transient in `Program.cs`                              |
| **Soft Delete**          | `is_active` flag on all entities for logical deletion without data loss                            |

## RESTful API Integration - Guided Card Creation

A key highlight of the project is the **Guided Create** wizard, a multi-step cascading form that consumes the [TCGdex REST API](https://tcgdex.dev/) to let users register real Pokémon cards with verified metadata.

**How it works:**

The user picks options step by step, and each selection triggers a `fetch()` call to the TCGdex API to load the next dropdown. So only real, valid data is ever shown:

```
Language -> Series -> Set -> Rarity -> Card -> Details + Image Preview
```

1. **Pick a language** -> fetches all available series from the API
2. **Pick a series** -> fetches only the sets within that series
3. **Pick a set** -> fetches the rarities that exist in that set
4. **Pick a rarity** (optional) -> fetches matching cards
5. **Pick a card** -> auto-fills the name, rarity, and shows the official card image
6. **Complete the form** -> add description, condition grade, upload photos, select categories
7. **Submit** -> `POST /Card/Create` saves the card, images, and category links in a single EF Core transaction

**Key technical details for recruiters:**

| Aspect                   | Detail                                                                                                                                         |
| ------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| **External API**         | Consumes [TCGdex v2 REST API](https://api.tcgdex.net/v2) - series, sets, rarities, and card lookup endpoints                                   |
| **Cascading filters**    | Each step filters the next dropdown; only series/sets/rarities with actual cards are shown (validated with `pagination:itemsPerPage=1` probes) |
| **Multi-language**       | Fetches card data in the user's selected language; falls back to the English endpoint for rarity normalization                                 |
| **Parallel fetching**    | Uses `Promise.all()` to fetch localized + English card data simultaneously                                                                     |
| **Searchable dropdowns** | Tom-Select integration provides autocomplete search across hundreds of series/sets                                                             |
| **Graceful degradation** | If the external API is unreachable, the wizard redirects to a manual card creation form                                                        |
| **Image preview**        | `FileReader` API renders client-side thumbnails before upload                                                                                  |
| **Anti-forgery**         | Form submission protected with `[ValidateAntiForgeryToken]`                                                                                    |

The project also exposes **internal JSON endpoints** for AJAX-driven features in the auction module:

```csharp
[HttpGet] GetCardsByUser(int userId)    // Returns user's cards as JSON for auction creation
[HttpGet] HasActiveAuction(int cardId)  // Checks if a card already has an active auction
```

## Database Schema

The SQL Server database (`PokeLeague`) includes the following tables:

`user` · `role` · `card` · `auction` · `auction_bid` · `category` · `category_card` · `image` · `language` · `set` · `rarity` · `purchase_order`

**Entity Relationships:**

```
role ──1:M──> user ──1:M──> card ──1:M──> image
                │             │
                │             ├──M:1──> set
                │             ├──M:1──> rarity
                │             ├──M:1──> language
                │             ├──M:M──> category  (via category_card)
                │             └──1:M──> auction ──1:M──> auction_bid
                │                          └──1:1──> purchase_order
                │
                ├──1:M──> auction
                ├──1:M──> auction_bid
                └──1:M──> purchase_order
```

The full creation script is available in [`db_query.sql`](db_query.sql).

## Prerequisites

- Visual Studio 2022+ with ASP.NET and web development workload
- Microsoft SQL Server 2022
- .NET 9.0 SDK

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/AndresBol/PokeLeague.git
   ```

2. **Set up the database**
   - Open SQL Server Management Studio
   - Execute the script [`db_query.sql`](db_query.sql) to create the database and tables

3. **Configure the connection**
   - Update credentials in `appsettings.Development.json` if needed:
     ```json
     {
       "ConnectionStrings": {
         "SqlServerDataBase": "Server=localhost;Database=PokeLeague;Integrated Security=True;TrustServerCertificate=True;user id=sa;password=your_password;Encrypt=false;"
       }
     }
     ```

4. **Restore NuGet packages**
   - Open the solution `PokeLeague.sln` in Visual Studio
   - Right-click the solution -> Restore NuGet Packages

5. **Build and run**
   - Build the solution
   - Run the `PokeLeague.Web` project
   - Navigate to `https://localhost:7205`

## Authors

- **Andrés Bolaños** Student ID: 119090051
- **Josué Calderón** Student ID: 207770303

Universidad Técnica Nacional (UTN)

---

<p align="center">
  <sub>Built with ASP.NET Core MVC as an academic project - 2026</sub>
</p>
