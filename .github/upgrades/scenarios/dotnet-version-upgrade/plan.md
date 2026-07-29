# .NET 10 Upgrade Plan

Target Framework: **.NET 10 (net10.0)**

## Overview

Upgrade strategy for 2-project solution containing a WinForms desktop application and a JavaScript/web component.
- **NT.IPTV** (WinForms app, net10.0-windows) — already on target framework
- **NileFusion.IPTV.Web** (JavaScript/Vite project, net6.0 tag) — requires framework tag update

Both projects are SDK-style, well-structured, with minimal dependencies. Straightforward modern-to-modern upgrade.

## Strategy

### Selected Strategy

**All-At-Once** — All projects upgraded simultaneously in a single operation.

**Rationale**: Only 2 projects, both already on modern .NET (net10.0-windows + net6.0); no legacy .NET Framework boundary complexities; clear dependency structure supports atomic upgrade. This approach is fastest and eliminates multi-targeting overhead.

## Execution Plan

### 01-prerequisites: Verify SDK and toolchain readiness

**Objective**: Ensure the build environment fully supports .NET 10 development and the proposed target frameworks.

Verify that:
- .NET 10 SDK is installed on the development machine
- Visual Studio can access the required SDKs
- global.json (if present) is compatible with .NET 10
- Build tools and design-time support are available

**Done when**:
- .NET 10 SDK validated and available for all projects
- No toolchain conflicts or missing SDKs
- Solution loads and projects are recognized by Visual Studio

---

### 02-update-project-files: Update target frameworks and SDK metadata

**Objective**: Update all project files to target .NET 10.

For each project:
- Update `<TargetFramework>` property to `net10.0` (or `net10.0-windows` for Windows-targeting projects)
- Verify SDK metadata and conditional compilation symbols
- Update any framework-specific imports or build properties
- Confirm SDK-style format is retained

For the WinForms application, verify `UseWindowsForms`, `EnableWindowsTargeting`, and platform-specific settings remain intact.

**Done when**:
- All project files updated with correct target framework
- No syntax errors in project files
- Project files still load successfully in Visual Studio
- Build metadata correctly reflects .NET 10 targets

---

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

---

### 04-fix-compilation-errors: Resolve breaking changes and code issues

**Objective**: Fix all compilation errors introduced by the framework and package updates.

Address:
- API changes and deprecations between .NET versions
- Package API breaking changes (methods, namespaces, signatures)
- Removed or renamed types and members
- Conditional compilation symbols and platform-specific code
- Serialization, configuration, or middleware changes related to package updates

**Done when**:
- Solution builds with zero compiler errors
- All code compiles successfully across both projects
- No `#error` preprocessor directives remain
- Compiler warnings from code changes are identified and documented (will be addressed post-upgrade)

---

### 05-validation: Build solution and verify upgrade completeness

**Objective**: Perform final validation that the upgrade is complete and functional.

- Full solution build with all projects
- Verify all project outputs are generated correctly
- Check for runtime compatibility issues (if test suite exists, execute it)
- Document any remaining post-upgrade recommendations or deferred work items

**Done when**:
- Solution builds successfully with no errors
- All project output artifacts generated
- If unit tests exist: all tests pass
- Upgrade summary documented

---

## Success Criteria

- [ ] All projects target .NET 10 (net10.0)
- [ ] All NuGet packages updated to compatible versions
- [ ] Solution builds with zero compiler errors
- [ ] No blocking compile warnings introduced by code changes
- [ ] All tests pass (if test projects present)

