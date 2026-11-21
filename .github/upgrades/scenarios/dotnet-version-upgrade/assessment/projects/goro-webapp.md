# goro-webapp\goro-webapp.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net10.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 30
- **Number of Files with Incidents**: 3
- **Lines of Code**: 1047
- **Estimated LOC to modify**: 7+ (at least 0.7% of the project)

## Related Projects

**Depended on by (1)** — projects that reference this one:

- [C:\Users\原敏之\\.copilot\repos\copilot-worktrees\goro-webapp-v3\thara0402-redesigned-spork\src\goro-webapp\goro-webapp.Tests\goro-webapp.Tests.csproj](../projects/goro-webapp.Tests.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;goro-webapp.Tests.csproj</b><br/><small>net10.0</small>"]
        click P1 "../projects/goro-webapp.Tests.md"
    end
    subgraph current["goro-webapp.csproj"]
        MAIN["<b>📦&nbsp;goro-webapp.csproj</b><br/><small>net10.0</small>"]
        click MAIN "../projects/goro-webapp.md"
    end
    P1 --> MAIN

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 1 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 6 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 428 |  |
| ***Total APIs Analyzed*** | ***435*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 16.2.0 | 17.0.0 | 🔵 Optional | NuGet パッケージにセキュリティの脆弱性が含まれています |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

