# NileFusion.IPTV React Migration Plan

## 1. Project Goal
The current application is a Windows Forms desktop client named NT.IPTV. The intended new direction is a web-based React application named **NileFusion.IPTV** that keeps the same core workflows:

- Login and profile management
- Live channels, movies, and series browsing
- Search and favorites
- Detail pages for movies and series
- Stream playback
- Downloading movies/series and collecting media links
- Settings and local persistence

---

## 2. Current Application Summary
The desktop project is centered around the following main entry points:

- [NT.IPTV/Program.cs](NT.IPTV/Program.cs) starts the app with the login screen.
- [NT.IPTV/frmLogin.cs](NT.IPTV/frmLogin.cs) handles connection/login and profile loading.
- [NT.IPTV/frmCategories.cs](NT.IPTV/frmCategories.cs) is the main catalog screen for categories and media browsing.
- [NT.IPTV/frmMovieData.cs](NT.IPTV/frmMovieData.cs) shows movie/series details and actions.
- [NT.IPTV/frmDownloader.cs](NT.IPTV/frmDownloader.cs) handles file downloads.
- [NT.IPTV/frmGetDownloadLinks.cs](NT.IPTV/frmGetDownloadLinks.cs) shows downloadable links.
- [NT.IPTV/frmPlayMovie.cs](NT.IPTV/frmPlayMovie.cs) is used for media playback.

The shared logic is mainly in [NT.IPTV/Utilities/clsCore.cs](NT.IPTV/Utilities/clsCore.cs).

---

## 3. Feature Inventory and React Mapping

| Current behavior | Current desktop logic | React equivalent |
|---|---|---|
| Login | [NT.IPTV/frmLogin.cs](NT.IPTV/frmLogin.cs) | Login page + saved profile selector |
| Save/load user profiles | [NT.IPTV/Utilities/clsCore.cs](NT.IPTV/Utilities/clsCore.cs) | Local storage / JSON persistence |
| Categories loading | [NT.IPTV/Utilities/clsCore.cs](NT.IPTV/Utilities/clsCore.cs) | Dashboard with tabs for Live / Movies / Series |
| Stream catalog browsing | [NT.IPTV/frmCategories.cs](NT.IPTV/frmCategories.cs) | Grid/list cards with search/filter/sort |
| Favorites | [NT.IPTV/Controls/ChannelControl.cs](NT.IPTV/Controls/ChannelControl.cs) | Favorite toggles stored per user |
| Movie details | [NT.IPTV/frmMovieData.cs](NT.IPTV/frmMovieData.cs) | Detail page with synopsis, cast, trailer, metadata |
| Series details and seasons | [NT.IPTV/frmMovieData.cs](NT.IPTV/frmMovieData.cs) | Expandable season/episode UI |
| Stream playback | [NT.IPTV/frmPlayMovie.cs](NT.IPTV/frmPlayMovie.cs) | Embedded player or external player launch flow |
| Download movie | [NT.IPTV/frmDownloader.cs](NT.IPTV/frmDownloader.cs) | Background download manager UI |
| Download series | [NT.IPTV/frmDownloader.cs](NT.IPTV/frmDownloader.cs) | Multi-episode download workflow |
| Copy/show links | [NT.IPTV/frmGetDownloadLinks.cs](NT.IPTV/frmGetDownloadLinks.cs) | Link drawer / modal |
| Global search | [NT.IPTV/frmGlobalSearch.cs](NT.IPTV/frmGlobalSearch.cs) | Search page or global search modal |
| Settings | [NT.IPTV/Utilities/AppSettings.cs](NT.IPTV/Utilities/AppSettings.cs) | Settings page + persisted preferences |

---

## 4. Core Business Logic Already Present
The app already contains most of the needed domain logic:

### Authentication
The login flow builds a connection string using the user credentials and checks the Xtream-style API.

Relevant code:
- [NT.IPTV/Utilities/clsCore.cs](NT.IPTV/Utilities/clsCore.cs)
- [NT.IPTV/Models/PlayerInfo.cs](NT.IPTV/Models/PlayerInfo.cs)
- [NT.IPTV/Models/UserInfo.cs](NT.IPTV/Models/UserInfo.cs)

### Media categories and lists
The application loads these sets:
- Live categories
- VOD categories
- Series categories
- Live streams
- VOD streams
- Series streams

Relevant code:
- [NT.IPTV/Utilities/clsCore.cs](NT.IPTV/Utilities/clsCore.cs)
- [NT.IPTV/Models/Items/Channesl/StreamChannel.cs](NT.IPTV/Models/Items/Channesl/StreamChannel.cs)
- [NT.IPTV/Models/Items/Channesl/StreamVideo.cs](NT.IPTV/Models/Items/Channesl/StreamVideo.cs)
- [NT.IPTV/Models/Items/Channesl/StreamSeries.cs](NT.IPTV/Models/Items/Channesl/StreamSeries.cs)

### Metadata and detail models
The project already defines models for:
- Movies
- Series
- Seasons
- Episodes
- Stream URLs

Relevant code:
- [NT.IPTV/Models/Items/StreamObject/WatchMovie.cs](NT.IPTV/Models/Items/StreamObject/WatchMovie.cs)
- [NT.IPTV/Models/Items/StreamObject/WatchSeries.cs](NT.IPTV/Models/Items/StreamObject/WatchSeries.cs)

### Download behavior
The desktop app downloads media using progress tracking and creates output folders.

Relevant code:
- [NT.IPTV/frmDownloader.cs](NT.IPTV/frmDownloader.cs)
- [NT.IPTV/Utilities/HttpClientDownloadWithProgress.cs](NT.IPTV/Utilities/HttpClientDownloadWithProgress.cs)

---

## 5. API Endpoints Used by the Current App
The current app relies on Xtream-style endpoints. For the React/web version, the architecture should be adapted so the browser does not directly depend on unsafe credential handling.

### Recommended architecture for web
1. Prefer a small backend proxy/API layer for authentication and API calls.
2. Keep the frontend only responsible for UI, routing, state, and rendering.
3. Use the backend to build secure request URLs, handle credentials, and optionally cache responses.
4. If a backend is not desired initially, allow a limited client-side mode only for development/testing.

### Recommended endpoint groups
- `player_api.php` for authentication and session data
- `get_live_categories`
- `get_vod_categories`
- `get_series_categories`
- `get_live_streams`
- `get_vod_streams`
- `get_series`
- `get_vod_info`
- `get_series_info`
- `xmltv.php` for EPG data (if needed)

### Web-specific security considerations
- Credentials should not be stored in plain browser code for long-term use.
- Prefer server-side session storage or encrypted token storage.
- The frontend should never assume direct access to the IPTV host is safe or allowed.
- If CORS blocks certain endpoints, the proxy backend becomes the correct solution.

### Important note
The desktop app builds URLs from the logged-in server info and user credentials. The React version should preserve that behavior in a safe and testable way, but the browser architecture should be designed for web constraints.

---

## 6. Required React Frontend Features

### Authentication
- Login screen with:
  - username
  - password
  - server
  - port
  - HTTPS toggle
  - optional profile save

### Browsing UI
- Sidebar or top navigation for:
  - Live
  - Movies
  - Series
  - Favorites
  - Search
- Category chips or side list
- Grid cards for media items

### Media detail UI
- Poster/backdrop display
- Plot, director, genre, duration, cast
- Trailer button
- Play button
- Download button
- Episode/season navigation for series

### Playback
- React player support (HTML5 or external player flow)
- Use a secure player strategy depending on the stream source
- If using VLC-style native playback, keep that as a desktop fallback or external action

### Downloads
- Show progress for current download
- Allow movie downloads and season downloads
- Keep download links usable in the UI
- Store downloads in a user-accessible folder or browser download area

### Search and filters
- Search by title
- Sort by name, rating, release date
- Favorites-first ordering

### Persistent state
- Save last logged-in profile
- Save favorite categories/items
- Save UI settings like thumbnail size and theme

---

## 7. Suggested React Architecture

### Frontend stack
- React + TypeScript
- Vite (recommended for speed and simplicity)
- React Router
- TanStack Query (for API data caching and loading states)
- Zustand or Redux Toolkit (for global app state)
- Tailwind CSS or a custom design system
- Axios or fetch wrappers
- Optional: React Query Devtools, Persist middleware, and route-level code splitting

### Recommended backend/web split
- **Frontend**: handles login UI, streaming UI, browsing, favorites, cards, search, and settings.
- **Backend/API layer**: handles IPTV API requests, credential validation, rate limiting, caching, and response shaping.
- **Optional edge service**: can be used for download job orchestration, proxying stream URLs, or secure link generation.

### Suggested folder structure
- `src/components` for reusable UI parts
- `src/pages` for route-level screens
- `src/hooks` for API hooks
- `src/store` for global state
- `src/services` for API service layer
- `src/types` for model definitions
- `src/utils` for formatting and helpers
- `src/lib` for shared constants, env config, and adapters
- `src/features` for domain-specific modules such as auth, catalog, player, and downloads

### Recommended state flow
- Authentication state is loaded first
- After login, fetch categories and initial media sets
- Cache catalogs so switching tabs is fast
- Separate detail fetches from list fetches
- Use optimistic updates for favorites and watch progress when appropriate

### Web-specific UX requirements
- Use responsive layouts for mobile and desktop
- Support lazy loading and infinite scroll for large lists
- Use skeleton loading states instead of blocking full-page waits
- Keep the player in a dedicated route or modal so navigation does not interrupt playback
- Allow deep links to movie, series, and category pages

### Deployment considerations
- Build frontend as a static web app or SPA
- If using a backend proxy, deploy backend separately from the frontend
- Store environment-specific endpoints in config files or deployment secrets
- Support both development (localhost) and production hosts with proper CORS settings

---

## 8. Data Model Mapping
The current C# model concepts should become TypeScript interfaces.

### Core user/session model
- `UserInfo`
- `PlayerInfo`
- connection settings

### Catalog models
- `StreamCategory`
- `StreamChannel`
- `StreamVideo`
- `StreamSeries`

### Detail models
- `WatchMovie`
- `WatchSeries`
- `Season`
- `EpisodeData`

### Important migration rule
The React model should match the API response shape closely, while keeping UI-friendly names.

---

## 9. Migration Steps

### Phase 1: Audit and API isolation
- Confirm exact API response formats
- Decide whether browser calls will go directly or through a proxy
- Document the authentication flow

### Phase 2: Create the React shell
- Setup Vite React app
- Add routing and global layout
- Add theme, card components, tabs, and search bar

### Phase 3: Implement authentication
- Login form
- Profile save/load
- Token/session handling

### Phase 4: Implement catalog browsing
- Category loading
- Grid/list view
- Favorites handling
- Sorting and filtering

### Phase 5: Implement details and playback
- Movie and series detail pages
- Season/episode rendering
- Trailer support
- Player integration

### Phase 6: Implement downloads
- Download UI
- Link retrieval
- Progress tracking
- Download history

### Phase 7: Testing and polish
- Error handling
- Loading skeletons
- Responsive design
- Accessibility

---

## 10. Risks and Considerations

### Security
If the React app calls the Xtream API directly from the browser, credentials and server details may be exposed in client-side code. A backend proxy is safer.

### Playback behavior
Native video playback is easier in the desktop app. In the web app, playback must be handled carefully depending on stream type and browser restrictions. A good architecture should separate the media URL fetch stage from the actual playback UI.

### Large media catalogs
The app may need efficient paging, virtualization, or lazy loading for large libraries. The React architecture should avoid fetching full datasets unnecessarily.

### Downloading from browser
Browser downloads are different from desktop file management, so the download UX should be designed with clear progress and file destination handling. A server job queue may be needed for reliable long downloads.

### Browser limitations
Some stream formats may not play directly in browsers, so the web app should support fallback options such as external player launch or proxy playback routes.

### SEO and shareability
If movie and series detail pages should be shareable, the app must support proper route metadata and potentially server-side rendering or static generation for metadata-heavy pages.

---

## 11. Recommended Final Direction
The best long-term path is:

- Keep the current desktop project as a backup/reference
- Build a new React frontend named **NileFusion.IPTV**
- Add a lightweight backend or API proxy for secure credential handling and media request shaping
- Use a clean separation between UI, API, playback, and download management
- Preserve the same user experience for:
  - login
  - live channels
  - movies and series
  - favorites
  - detail views
  - media downloads
- Design the app so it works well in the browser while still allowing fallback flows for unsupported media formats

---

## 12. Concrete Starter Scaffold for NileFusion.IPTV

### Recommended repository layout
- `NileFusion.IPTV/`
  - `apps/`
    - `web/` → React + Vite frontend
    - `api/` → optional backend proxy service
  - `packages/`
    - `shared/` → shared types, constants, and API contracts
  - `docs/` → migration notes, API docs, and deployment notes

### Web app structure
- `apps/web/src/`
  - `app/` → router, global providers, layout shell
  - `pages/` → login, home, browse, details, player, settings
  - `features/`
    - `auth/`
    - `catalog/`
    - `player/`
    - `downloads/`
  - `components/`
    - `ui/` → reusable buttons, cards, modals, forms
    - `layout/` → sidebar, header, top nav
  - `services/` → API clients and adapters
  - `hooks/` → custom hooks for data fetching and local state
  - `store/` → Zustand/Redux state
  - `types/` → TypeScript interfaces and models
  - `utils/` → formatting, string helpers, URL builders

### API backend structure (optional but recommended)
- `apps/api/src/`
  - `routes/` → auth, categories, streams, details, downloads
  - `controllers/`
  - `services/`
  - `middleware/`
  - `config/`

### Initial implementation order
1. Create the `apps/web` React project.
2. Add routing, layout, and theme support.
3. Build the auth flow and profile persistence.
4. Implement the media catalog pages.
5. Add movie/series details and trailers.
6. Add player integration and fallback behavior.
7. Implement downloads and user settings.

### Suggested tooling
- `Vite` for the frontend bootstrap
- `TypeScript` for safety and maintainability
- `React Router` for navigation
- `TanStack Query` for API caching
- `Zustand` for global state
- `Axios` or `fetch` wrappers for API calls
- `Tailwind CSS` or a small component library for styling

### Environment configuration
- `VITE_API_BASE_URL`
- `VITE_PROXY_MODE`
- `VITE_ENABLE_PLAYER_FALLBACK`
- `VITE_DOWNLOAD_API_URL`

### Recommended first milestone
The first working milestone should be:
- login screen
- saved profile selection
- category browsing
- movie/series list view
- detail page

---

## 13. Suggested Next Implementation Actions
1. Create the `apps/web` React/Vite project for NileFusion.IPTV.
2. Build the API service layer for the Xtream endpoints.
3. Recreate the login page and saved profile flow.
4. Rebuild the category/browser screens.
5. Recreate detail pages and media actions.
6. Add download management.
7. Test the app against real IPTV data.
