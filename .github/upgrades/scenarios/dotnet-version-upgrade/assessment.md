be# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
  - [Binding Redirect Configuration](#binding-redirect-configuration)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [NileFusion.IPTV.Web\NileFusion.IPTV.Web.esproj](#nilefusioniptvwebnilefusioniptvwebesproj)
  - [NT.IPTV\NT.IPTV.csproj](#ntiptvntiptvcsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 2 | 1 require upgrade |
| Total NuGet Packages | 4 | All compatible |
| Total Code Files | 47 |  |
| Total Code Files with Incidents | 1 |  |
| Total Lines of Code | 7319 |  |
| Total Number of Issues | 1 |  |
| Estimated LOC to modify | 0+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [NileFusion.IPTV.Web\NileFusion.IPTV.Web.esproj](#nilefusioniptvwebnilefusioniptvwebesproj) | net6.0 | 🟢 Low | 0 | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [NT.IPTV\NT.IPTV.csproj](#ntiptvntiptvcsproj) | net10.0-windows | ✅ None | 0 | 0 | 0 |  | WinForms, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 4 | 100.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 0 | 0.0% |
| ***Total NuGet Packages*** | ***4*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| LibVLCSharp.WinForms | 3.10.0 |  | [NT.IPTV.csproj](#ntiptvntiptvcsproj) | ✅Compatible |
| Microsoft.Web.WebView2 | 1.0.2420.47 |  | [NT.IPTV.csproj](#ntiptvntiptvcsproj) | ✅Compatible |
| Newtonsoft.Json | 13.0.3 |  | [NT.IPTV.csproj](#ntiptvntiptvcsproj) | ✅Compatible |
| VideoLAN.LibVLC.Windows | 3.0.23.1 |  | [NT.IPTV.csproj](#ntiptvntiptvcsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;NT.IPTV.csproj</b><br/><small>net10.0-windows</small>"]
    P2["<b>📦&nbsp;NileFusion.IPTV.Web.esproj</b><br/><small>net6.0</small>"]
    click P1 "#ntiptvntiptvcsproj"
    click P2 "#nilefusioniptvwebnilefusioniptvwebesproj"

```

## Project Details

<a id="nilefusioniptvwebnilefusioniptvwebesproj"></a>
### NileFusion.IPTV.Web\NileFusion.IPTV.Web.esproj

#### Project Info

- **Current Target Framework:** net6.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 0
- **Dependants**: 0
- **Number of Files**: 0
- **Number of Files with Incidents**: 1
- **Lines of Code**: 0
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["NileFusion.IPTV.Web.esproj"]
        MAIN["<b>📦&nbsp;NileFusion.IPTV.Web.esproj</b><br/><small>net6.0</small>"]
        click MAIN "#nilefusioniptvwebnilefusioniptvwebesproj"
    end

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="ntiptvntiptvcsproj"></a>
### NT.IPTV\NT.IPTV.csproj

#### Project Info

- **Current Target Framework:** net10.0-windows✅
- **SDK-style**: True
- **Project Kind:** WinForms
- **Dependencies**: 0
- **Dependants**: 0
- **Number of Files**: 64
- **Lines of Code**: 7319
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["NT.IPTV.csproj"]
        MAIN["<b>📦&nbsp;NT.IPTV.csproj</b><br/><small>net10.0-windows</small>"]
        click MAIN "#ntiptvntiptvcsproj"
    end

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

