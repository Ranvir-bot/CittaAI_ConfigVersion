# CittaAI Configuration Versioning

A configuration versioning application built using **ASP.NET Core Web API, Entity Framework Core, SQL Server, and Angular**.

The application allows users to create configuration versions, view version history, and compare two versions to identify added, removed, and modified values.

## Tech Stack

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Newtonsoft.Json
- JsonDiffPatch.Net

### Frontend

- Angular
- TypeScript
- Reactive Forms
- RxJS
- json-difference

## Features

- Create and save configuration JSON
- Create a new immutable version for every save
- Maintain configuration version numbers
- View configuration version history
- View individual configuration versions
- Compare two configuration versions
- Detect:
  - Added fields
  - Removed fields
  - Modified fields
  - Nested object changes
  - Array changes
- Display old and new values for changes
- Display total number of changes

## Project Structure


CittaAI_ConfigVersion
│
├── ConfigurationVersioning.Api
│   ├── Controllers
│   ├── DTOs
│   ├── Data
│   ├── Diff
│   ├── Migrations
│   ├── Models
│   └── Services
│
├── configuration-versioning-ui
│   └── src
│       └── app
│           ├── components
│           │   ├── editor
│           │   ├── history
│           │   └── diff
│           ├── models
│           └── services
│
├── .gitignore
└── README.md