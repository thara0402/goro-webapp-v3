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

## AI-driven Development with GitHub Copilot

This repository uses GitHub Issues or chat requests as the starting point for AI-driven development with GitHub Copilot.

### Choosing an Agent

Whether you start from an issue or a chat request, use the `orchestrator` custom agent for application changes.

| Change target | Agent |
| --- | --- |
| `src/goro-webapp/` and `design/` (except `design/workflow.md`) | Always use `orchestrator` |
| `README.md`, `.github/` (including CI), and `design/workflow.md` | The default agent (no custom agent selected) is allowed as an exception |

Exceptions are determined only by file location; even minor changes in `src/goro-webapp/` require `orchestrator`. Selecting the agent is a manual step, so it cannot be enforced by instructions alone. The reviewer checks the stage record before merging (see [Orchestrator Custom Agent](#orchestrator-custom-agent)).

### Cloud Agent Flow

1. Create a GitHub Issue using the issue template.
2. Include the goal, affected area, acceptance criteria, allowed change scope, out-of-scope items, and validation command (see [Running Tests](#running-tests)).
3. Ask GitHub Copilot Cloud Agent to work on the issue from the issue page. Select the `orchestrator` custom agent for application changes (see [Choosing an Agent](#choosing-an-agent)).
4. The `orchestrator` delegates design, implementation, and review to dedicated agents and opens a pull request. For exception changes, the default agent makes the change and opens a pull request.
5. Review the pull request and GitHub Actions checks in GitHub.
6. Ask the agent for follow-up fixes if needed.
7. Merge the pull request manually after review and successful checks.

### Orchestrator Custom Agent

Select the `orchestrator` custom agent defined in `.github/agents/`. It delegates each stage to a dedicated agent and does not edit files itself.

| Agent | Role |
| --- | --- |
| `orchestrator` | Manages the stages, delegates work, and checks each gate |
| `architect` | Checks the issue against `design/`, updates feature-level design documents, and escalates architecture-level decisions to you |
| `developer` | Implements the application code and adds or updates MSTest unit tests until they pass |
| `reviewer` | Re-runs the tests independently and reviews the changes without editing files |

- **Local sessions (GitHub Copilot App / VS Code):** The orchestrator always stops after the design stage and waits for your approval before implementation.
- **GitHub Copilot Cloud Agent:** The orchestrator continues without stopping, records design decisions in the pull request description, and relies on the final pull request review for approval.
- **Stage record:** Because the orchestrator is also an AI, it can still skip a stage or misjudge a result. Every pull request that changes `src/goro-webapp/` or `design/` (except `design/workflow.md`) must include the stage record from `.github/pull_request_template.md`. Do not merge a pull request with a missing stage record or with empty or "not run" stages.

See `design/workflow.md` for the full workflow and quality gates.

### Secrets

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

Run the .NET test suite from the repository root. Restore dependencies first after cloning or changing packages:

```bash
dotnet restore src/goro-webapp/goro-webapp.slnx
dotnet test src/goro-webapp/goro-webapp.slnx --no-restore
```

With `--no-restore`, missing dependencies can cause the command to finish successfully without running any tests. Do not treat a run with a total of 0 tests as a success; restore and run again.

GitHub Actions runs the `Test ASP.Net Core app - goro-v3` workflow for pull requests targeting `main`. After a pull request is merged, the deployment workflow runs the test suite again before publishing and deploying the application.
