# CittaAI Configuration Versioning

A configuration versioning application built using **ASP.NET Core Web API, Entity Framework Core, PostgreSQL, and Angular**.

The application allows users to create immutable configuration versions, view version history, edit existing configurations by creating new versions, and compare two versions to identify added, removed, and modified values.

## Live Application

**Frontend:**  
https://citta-ai-config-version.vercel.app

**Backend API:**  
https://cittaai-config-api.onrender.com

**GitHub Repository:**  
https://github.com/Ranvir-bot/CittaAI_ConfigVersion


## Deployment

| Component | Technology |
|---|---|
| Frontend | Vercel |
| Backend | Render |
| Backend Containerization | Docker |
| Database | Neon PostgreSQL |
| Source Control | GitHub |

## Tech Stack

### Backend

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- PostgreSQL
- Npgsql
- JsonDiffPatch.Net

### Frontend

- Angular 22
- TypeScript
- Reactive Forms
- RxJS
- Angular Router
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

## Database Design

### Configuration

```text
Configuration
-------------------------
Id
Name
CurrentVersionId
CreatedAt
UpdatedAt

ConfigurationVersion
-------------------------
Id
ConfigurationId
VersionNumber
ConfigurationJson
CreatedAt
Author
Comment
PreviousVersionId
```

A configuration can contain multiple immutable versions.

```text
Configuration
     │
     ├── Version 1
     │
     ├── Version 2
     │
     └── Version 3
```

PreviousVersionId maintains the relationship between consecutive versions.

## Frontend
The Angular application contains three main areas:

- Editor
- History
- Diff

### Configuration Editor
- Loads configuration JSON dynamically
- Allows the user to modify the configuration
- Validates JSON
- Saves the configuration as a new version

The UI is not hardcoded to specific configuration properties.

### Version History

Displays:

- Configuration
- Version
- Data
- Created By
- Created At
- Action

The internal database ID is not displayed in the user interface.

### Diff View

Allows users to compare two versions and clearly identify:

- Added values
- Removed values
- Modified values
- Nested changes
- Array changes

### Separation of Concerns

The backend follows a layered architecture:

Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
PostgreSQL

### Controllers

Handle HTTP requests and responses.

### Services

Contain business/application logic.

### Repositories

Handle database access.

### DTOs

Define API request and response contracts.

### Diff

Contains JSON comparison logic.

### Versioning Strategy

The application uses a **full JSON snapshot strategy**.

Each successful save creates a new immutable version containing the complete configuration JSON.

Previous versions are never overwritten.

#### Why Full Snapshots?

- Simple and reliable version retrieval
- Older versions can be accessed directly
- No need to reconstruct a version from a chain of diffs
- Easier to support future restore functionality

#### Trade-off

Full snapshots require more database storage compared with storing only diffs.
For the expected configuration size and number of versions, the simplicity and reliability of full snapshots are preferred.

### Diff Strategy

The application uses:

- **Backend:** JsonDiffPatch.Net
- **Frontend:** json-difference

The diff functionality supports:

- Added fields
- Removed fields
- Modified values
- Nested object changes
- Array changes

The UI displays the old and new values where applicable and provides a readable representation of nested changes.

### Context-Aware Saves

The save request contains a `BaseVersionId`, which identifies the version from which the user started editing.

Before creating a new version, the backend checks whether the supplied base version is still the latest version.

If a newer version already exists, the save is rejected with HTTP `409 Conflict`.

Example:

```text
User A loads Version 2
        ↓
User B creates Version 3
        ↓
User A tries to save using BaseVersionId = 2
        ↓
Backend detects that Version 3 is newer
        ↓
HTTP 409 Conflict
```

## Design Decisions & Trade-offs

### Full Snapshot Storage

The application stores the complete configuration JSON for each version.

**Advantages:**

- Simple implementation
- Fast and predictable version retrieval
- Easy access to historical versions
- Easier future restore functionality

**Trade-off:**

- Requires more storage than diff-only storage

### Docker Deployment

The ASP.NET Core API is containerized using Docker.

This provides a consistent deployment environment between local development and production.

### Managed PostgreSQL

PostgreSQL is hosted on Neon to provide a managed database without requiring database infrastructure to be maintained manually.

## Performance Considerations

The application stores complete JSON snapshots for each version.

The design is intended to support configuration documents up to approximately **1 MB** and version histories of up to **100 versions**.

The API retrieves individual versions directly from PostgreSQL, avoiding the need to reconstruct historical versions from a chain of diffs.

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
├── ConfigurationVersioning.Api
│   ├── Controllers
│   ├── DTOs
│   ├── Data
│   ├── Diff
│   ├── Migrations
│   ├── Models
│   ├── Repositories
│   ├── Services
│   ├── Dockerfile
│   └── Program.cs
│
├── configuration-versioning-ui
│   └── src
│       └── app
│           ├── components
│           │   ├── editor
│           │   ├── history
│           │   └── diff
│           ├── config
│           ├── models
│           └── services
│
├── .gitignore
└── README.md
```