# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 11 (STS) (`net11.0`)

## Source Control
- **Source Branch**: thara0402-redesigned-spork
- **Working Branch**: upgrade-dotnet-11
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options

### Strategy
- Upgrade Strategy: All-at-Once

### Compatibility
- Unsupported Packages: Resolve Inline (2 incompatible packages)
- Unsupported API Handling: Fix Inline

### Reliability
- Test Coverage: Skip

## Strategy
**Selected**: All-at-Once
**Rationale**: Both projects target net10.0, are SDK-style, and have a simple two-project dependency relationship; the assessment identifies a contained set of package and API compatibility work.

### Execution Constraints
- Upgrade both projects in one atomic operation; do not introduce tier ordering or a phased rollout.
- Verify the .NET 11 SDK/toolchain and global.json compatibility as part of the upgrade.
- Update both target frameworks, resolve package compatibility and security findings inline, and fix API compatibility issues inline.
- After the atomic changes, verify the whole solution builds without errors and run the applicable existing test suite; no generated pre-upgrade test baseline is in scope.

**Commit Strategy**: Single Commit at End (recommended for All-at-Once; supersedes the pre-initialization value After Each Task).
