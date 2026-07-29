# .NET Version Upgrade Scenario

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0 (.NET 10 LTS)

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-At-Once

### Project Structure
- Project Approach: Standard (in-place upgrade)
- Package Management: Update all packages to compatible versions

## Strategy
**Selected**: All-At-Once  
**Rationale**: Only 2 projects, both already on modern .NET (net10.0-windows + net6.0); no legacy .NET Framework boundary complexities; clear dependency structure supports atomic upgrade.

### Execution Constraints
- All projects upgraded simultaneously in a single operation
- No tier ordering or phased rollout
- Single bounded pass for compilation error fixes after updating all project files and packages
- Final validation must confirm zero compilation errors before completing upgrade
- Build validation occurs after the atomic upgrade task completes
