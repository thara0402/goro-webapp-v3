# goro-webapp.Tests\goro-webapp.Tests.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net10.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 5
- **Number of Files with Incidents**: 2
- **Lines of Code**: 602
- **Estimated LOC to modify**: 1+ (at least 0.2% of the project)

## Related Projects

**Depends on (1)** — projects this one references:

- [C:\Users\原敏之\\.copilot\repos\copilot-worktrees\goro-webapp-v3\thara0402-redesigned-spork\src\goro-webapp\goro-webapp\goro-webapp.csproj](../projects/goro-webapp.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["goro-webapp.Tests.csproj"]
        MAIN["<b>📦&nbsp;goro-webapp.Tests.csproj</b><br/><small>net10.0</small>"]
        click MAIN "../projects/goro-webapp.Tests.md"
    end
    subgraph downstream["Dependencies (1)"]
        P2["<b>📦&nbsp;goro-webapp.csproj</b><br/><small>net10.0</small>"]
        click P2 "../projects/goro-webapp.md"
    end
    MAIN --> P2

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 243 |  |
| ***Total APIs Analyzed*** | ***244*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| MSTest.TestAdapter | 4.0.2 | — | 🔵 Optional | NuGet パッケージは非推奨です |
| MSTest.TestFramework | 4.0.2 | — | 🔵 Optional | NuGet パッケージは非推奨です |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

