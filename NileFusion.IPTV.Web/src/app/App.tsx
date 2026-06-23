import { Routes, Route, Navigate, Outlet } from 'react-router-dom'
import { AuthProvider, useAuth } from './AuthContext'
import Layout from '../components/layout/Layout'
import HomePage from '../pages/HomePage'
import LoginPage from '../pages/LoginPage'
import BrowsePage from '../pages/BrowsePage'
import DetailsPage from '../pages/DetailsPage'
import PlayerPage from '../pages/PlayerPage'
import SettingsPage from '../pages/SettingsPage'

function ProtectedRoute() {
  const { activeSession, isLoading } = useAuth()

  if (isLoading) {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', justifyContent: 'center', alignItems: 'center', height: '100vh', background: '#0f1117', color: '#fff' }}>
        <div className="spinner"></div>
        <p style={{ fontFamily: 'Inter, sans-serif', color: '#9ca3af' }}>Connecting to session...</p>
      </div>
    )
  }

  return activeSession ? <Outlet /> : <Navigate to="/login" replace />
}

function LoginRoute() {
  const { activeSession, isLoading } = useAuth()

  if (isLoading) return null

  return activeSession ? <Navigate to="/" replace /> : <LoginPage />
}

import { DownloadProvider } from './DownloadContext'

function App() {
  return (
    <AuthProvider>
      <DownloadProvider>
        <Routes>
          <Route path="/login" element={<LoginRoute />} />
          <Route element={<ProtectedRoute />}>
            {/* Player is fullscreen — render it outside Layout so no sidebar appears */}
            <Route path="/player/:type/:id" element={<PlayerPage />} />
            <Route element={<Layout />}>
              <Route path="/" element={<HomePage />} />
              <Route path="/browse" element={<BrowsePage />} />
              <Route path="/details/:type/:id" element={<DetailsPage />} />
              <Route path="/settings" element={<SettingsPage />} />
            </Route>
          </Route>
        </Routes>
      </DownloadProvider>
    </AuthProvider>
  )
}

export default App
