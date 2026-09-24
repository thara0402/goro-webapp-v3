# Kodoku no Gourmet Pilgrimage Site

[![Build and deploy](https://github.com/thara0402/goro-webapp-v3/actions/workflows/main_goro-v3.yml/badge.svg)](https://github.com/thara0402/goro-webapp-v3/actions/workflows/main_goro-v3.yml)

This website provides information about restaurants featured in the TV drama "Kodoku no Gourmet," starring Yutaka Matsushige.

## Creating Issues from Chat

This project uses GitHub Issues as the starting point for AI-driven development. When asking chat to create an issue, include the issue type, title, goal, affected area, and acceptance criteria.

Example:

```text
Create a GitHub Issue.
Type: Feature request.
Title: Add a restaurant status filter to the restaurant list.
Goal: Make it easier to find restaurants that are currently visitable.
Affected area: Home page and restaurant data.
Acceptance criteria: Users can filter by active, closed, temporarily closed, and unknown statuses; the status filter works together with the existing season filter; dotnet test passes.
```

## AI-driven Development with GitHub Copilot Cloud Agent

This repository can use GitHub Issues as the starting point for AI-driven development with GitHub Copilot Cloud Agent.

Recommended flow:

1. Create a GitHub Issue using the issue template.
2. Include the goal, affected area, acceptance criteria, allowed change scope, out-of-scope items, and validation command.
3. Ask GitHub Copilot Cloud Agent to work on the issue from the issue page.
4. The agent checks the design documents, updates design files if needed, implements the change, adds or updates tests, and opens a pull request.
5. Review the pull request and GitHub Actions checks in GitHub.
6. Ask the agent for follow-up fixes if needed.
7. Merge the pull request manually after review and successful checks.

Validation command for normal unit-test-driven development:

```bash
dotnet test src/goro-webapp/goro-webapp.slnx --no-restore
```

The current unit tests do not require production secrets. Do not provide production Cosmos DB, Google API, Application Insights, or Key Vault secrets to the agent for normal unit-test-driven development. Production secrets are handled by Azure App Service, Azure Key Vault, and GitHub Actions deployment settings.

## Running in Visual Studio

### Prerequisites

- Visual Studio with the "ASP.NET and web development" workload
- .NET 10 SDK

### Clone the Repository

```powershell
cd C:\develop
git clone https://github.com/thara0402/goro-webapp-v3.git
```

### Open the Solution

Open the following file in Visual Studio:

```text
goro-webapp-v3\src\goro-webapp\goro-webapp.slnx
```

### Configure User Secrets

Right-click the `goro-webapp` project and select **Manage User Secrets**.

```json
{
  "WebApp": {
    "AppInsightsConnectionString": "Application Insights connection string",
    "CosmosConnection": "Azure Cosmos DB connection string",
    "GoogleMapsApiKey": "Google Maps API key",
    "GoogleGeocodingApiKey": "Google Geocoding API key"
  }
}
```

### Run the Application

Set `goro-webapp` as the startup project, select `https` in the Visual Studio toolbar, and press `F5`.

Open the following URL in your browser:

```text
https://localhost:7159
```

## Running Tests

Run the .NET test suite from the repository root:

```powershell
dotnet test .\src\goro-webapp\goro-webapp.slnx --no-restore
```

GitHub Actions runs the `Test ASP.Net Core app - goro-v3` workflow for pull requests targeting `main`. After a pull request is merged, the deployment workflow runs the test suite again before publishing and deploying the application.
