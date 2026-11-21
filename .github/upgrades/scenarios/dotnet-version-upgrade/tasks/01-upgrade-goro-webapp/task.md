# 01-upgrade-goro-webapp: Upgrade the application and test projects to .NET 11

Upgrade `goro-webapp` and `goro-webapp.Tests` together from `net10.0` to `net11.0`. Both projects are SDK-style, so no project-format conversion is needed. The assessment identifies 1 package finding in the application and 2 deprecated MSTest package findings in the test project; it also reports 1 binary-incompatible API in the application, 7 behavioral API notices across the solution, and 1 behavioral notice in the test project. Research the detailed assessment findings and package availability, resolve package/security and API compatibility issues inline, and retain the behavioral notices as review items unless investigation shows a required change. The confirmed Test Coverage selection is Skip, so do not add generated baseline work.

Include SDK/toolchain and `global.json` compatibility in this atomic upgrade. Package and API resolution belongs in this task, as does updating any test-project references affected by the application upgrade. Behavioral API notices are advisory; do not assume they require code changes or separate manual runtime-test tasks.

**Done when**: Both projects target `net11.0`; package compatibility findings are resolved or their remaining blockers are explicitly reported; required API compatibility fixes are complete; the whole solution builds without errors; and the applicable existing test suite passes.

## Research findings

- **Execution scope:** `goro-webapp/goro-webapp/goro-webapp.csproj` is the application project and `goro-webapp/goro-webapp.Tests/goro-webapp.Tests.csproj` is the sole test project. The test project references the application project; both are already SDK-style. No `// STUB:` markers were found in the source tree.
- **Current/toolchain state:** Both project TFMs are `net10.0`; target is `net11.0`. SDK `11.0.100` and .NET 11 runtime are installed. Visual Studio MSBuild is available. No `global.json` exists, so there is no pinned SDK entry to retarget.
- **Package state:** `AutoMapper` is `16.2.0` and the assessment identifies a known-vulnerability finding with supported target `17.0.0`; update it to `17.0.0`. The test project uses `MSTest.Sdk/4.0.2` and package resolution reports `MSTest.TestFramework` and `MSTest.TestAdapter` at `4.0.2`; the supported .NET 11-compatible replacement is `4.5.3`, so update the SDK to `MSTest.Sdk/4.5.3`. `Moq 4.20.72` and `Microsoft.NET.Test.Sdk 18.0.1` are compatible. The package-management skill confirms these project files use direct non-CPM package references.
- **API state:** `builder.Services.Configure<MySettings>(...)` is the single binary-incompatible finding. Preserve configuration binding semantics by replacing the incompatible overload with `builder.Services.AddOptions<MySettings>().Bind(builder.Configuration.GetSection("WebApp"));`, so the configuration binder registers the same options section without the removed annotation/API shape. The behavioral API notices are: `HttpContent`, `Uri`, `UseExceptionHandler`, `JsonDocument`, and the test `HttpContent` call. Local code uses standard JSON parsing, URI construction/escaping, exception-handler endpoint routing, and mock HTTP content; no documented trigger reviewed as significant or requiring a code rewrite.

## Behavioral API review

- **HttpContent / JsonDocument:** The geocode client reads JSON as text and parses it with `JsonDocument.Parse`; nullable `JsonDocument` deserialization behavior is not exercised by this code path. Disposition: applicable behavior, low impact / existing tests cover HTTP status and JSON parsing, so retain as an advisory review item and run the existing suite.
- **Uri / Uri(string):** Key Vault URI and geocode URL construction use string-based URI APIs. Triggered behavior may differ for malformed/escaping input. Disposition: uncertain for external malformed-input edge cases; no required code rewrite established, run existing tests and leave unverified edge cases as a user check.
- **UseExceptionHandler(string):** The app uses the string overload for `/Home/Error`; existing routing and middleware behavior is a normal local trigger. Disposition: applicable, low impact; no code rewrite indicated, existing suite/build are the available checks.
- **Test HttpContent:** Tests construct `StringContent` JSON responses. Disposition: not applicable to a special nullable deserialization or new read behavior observed in the test setup; no test edit required.

## Decomposition assessment

- Loaded scenario Execution stage: `.../dotnet-version-upgrade/execution.md`.
- Evaluated Breakdown Hints: `breakdown-hints/common.md` and `breakdown-hints/test.md`; framework-specific hint files do not apply.
- Verdict: **atomic**. The task is an explicitly scoped all-at-once upgrade of two dependent SDK-style projects. It has no package replacement research task, no stubs, no independent package replacement batch, and no significant test rework. The test hint requires test-reference/TFM updates to remain in the same atomic task, which this task does.

## Planned edits

1. Change both project TFMs from `net10.0` to `net11.0`.
2. Update `AutoMapper` from `16.2.0` to `17.0.0`.
3. Update `MSTest.Sdk` from `4.0.2` to `4.5.3`.
4. Replace the incompatible options configuration call in `Program.cs` with `AddOptions<T>().Bind(...)`.
