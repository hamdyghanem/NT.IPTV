# Upgrade Options — NT.IPTV

Assessment: 2 projects (1 on net10.0-windows, 1 on net6.0); 1 issue identified; no complex dependencies.

## Strategy

### Upgrade Strategy
Solution has 2 projects with straightforward modernization paths. Since both are already on modern .NET (net6.0+), an atomic All-at-Once upgrade is the fastest and simplest approach. No Framework-to-Core boundary mechanics required.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects simultaneously in a single atomic pass; fastest approach, no multi-targeting overhead. |
| Top-Down | Upgrade entry-point applications first with multi-targeted libraries; longer process but incremental buildability. |

## Project Structure

### Project Approach
Both projects are application/web components with no library layers. Standard project-in-place upgrade applies.

| Value | Description |
|-------|-------------|
| **Standard (in-place upgrade)** (selected) | Upgrade each project in place, replacing its target framework directly. |

### Package Management  
All packages referenced are compatible with .NET 10 targets, or updates are available.

| Value | Description |
|-------|-------------|
| **Update all packages to compatible versions** (selected) | Update packages to latest versions supporting .NET 10. |

