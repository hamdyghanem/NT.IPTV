# 02-update-project-files: Update target frameworks and SDK metadata

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
