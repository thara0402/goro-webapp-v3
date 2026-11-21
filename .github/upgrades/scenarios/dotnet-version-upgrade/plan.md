# .NET Version Upgrade Plan

## Overview

**Target**: Upgrade `goro-webapp` and `goro-webapp.Tests` from `net10.0` to `net11.0`.
**Scope**: Small two-project solution with an ASP.NET Core application and its test project; both are SDK-style, with 1,649 total lines of code.

### Selected Strategy
**All-at-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 2 projects, both on .NET 10, with a straightforward dependency structure and a bounded set of package and API compatibility findings.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Upgrade Strategy | All-at-Once | Both projects target net10.0 and the dependency graph contains only two projects. |
| Unsupported Packages | Resolve Inline | The assessment reports 2 incompatible package findings, suitable for inline resolution in this small upgrade. |
| Unsupported API Handling | Fix Inline | The assessment reports 1 binary-incompatible API and a limited set of behavioral notices across the two projects. |
| Test Coverage | Skip | The confirmed selection skips generated pre-upgrade test-baseline work. |

## Tasks

### 01-upgrade-goro-webapp: Upgrade the application and test projects to .NET 11

Upgrade `goro-webapp` and `goro-webapp.Tests` together from `net10.0` to `net11.0`. Both projects are SDK-style, so no project-format conversion is needed. The assessment identifies 1 package finding in the application and 2 deprecated MSTest package findings in the test project; it also reports 1 binary-incompatible API in the application, 7 behavioral API notices across the solution, and 1 behavioral notice in the test project. Research the detailed assessment findings and package availability, resolve package/security and API compatibility issues inline, and retain the behavioral notices as review items unless investigation shows a required change. The confirmed Test Coverage selection is Skip, so do not add generated baseline work.

Include SDK/toolchain and `global.json` compatibility in this atomic upgrade. Package and API resolution belongs in this task, as does updating any test-project references affected by the application upgrade. Behavioral API notices are advisory; do not assume they require code changes or separate manual runtime-test tasks.

**Done when**: Both projects target `net11.0`; package compatibility findings are resolved or their remaining blockers are explicitly reported; required API compatibility fixes are complete; the whole solution builds without errors; and the applicable existing test suite passes.
