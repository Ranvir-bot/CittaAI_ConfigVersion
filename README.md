# CittaAI Configuration Versioning

A configuration versioning application built using **ASP.NET Core Web API, Entity Framework Core, SQL Server, and Angular**.

The application allows users to create immutable configuration versions, view version history, edit existing configurations by creating new versions, and compare two versions to identify added, removed, and modified values.

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
- Maintain `PreviousVersionId` to identify version history
- View configuration version history
- Edit an existing configuration version
- Save edits as a new version without modifying the old version
- Validate configuration JSON
- Handle invalid configuration IDs
- Compare two configuration versions
- Detect:
  - Added fields
  - Removed fields
  - Modified fields
  - Nested object changes
  - Array changes
- Display old and new values for changes
- Display total number of changes
- Prevent comparison of the same version
- Detect stale saves using `BaseVersionId`
- Return HTTP 409 Conflict when a stale version is saved

## API Endpoints

### Save Configuration

```http
POST /config/save
```
### Get Version History
```http
GET /config/versions
```
### Get Version
```http
GET /config/versions/{versionId}
```
### Compare Versions
```http
GET /config/diff?from={versionId}&to={versionId}
```


## Project Structure

```text
CittaAI_ConfigVersion
│
├── ConfigurationVersioning.Api
│   ├── Controllers
│   ├── DTOs
│   ├── Data
│   ├── Diff
│   ├── Migrations
│   ├── Models
│   ├── Repositories
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
```