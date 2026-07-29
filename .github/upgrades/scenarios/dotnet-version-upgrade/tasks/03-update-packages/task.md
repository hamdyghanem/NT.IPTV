# 03-update-packages: Upgrade NuGet packages to .NET 10-compatible versions

### 03-update-packages: Upgrade NuGet packages to .NET 10-compatible versions

**Objective**: Update all NuGet package references to versions compatible with .NET 10.

Review all packages used across both projects and:
- Identify packages with compatible .NET 10 versions available
- Update package references to the latest compatible versions
- Resolve version conflicts between projects (both must use compatible versions)
- Run `dotnet restore` to fetch and validate new package versions

**Done when**:
- All NuGet package references updated to .NET 10-compatible versions
- No unresolved dependency conflicts
- Package restoration succeeds without errors
- `packages.lock.json` (if present) updated
