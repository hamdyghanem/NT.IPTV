import { Routes, Route } from 'react-router-dom'
import Layout from '../components/layout/Layout'
import HomePage from '../pages/HomePage'
import LoginPage from '../pages/LoginPage'
import BrowsePage from '../pages/BrowsePage'
import DetailsPage from '../pages/DetailsPage'
import PlayerPage from '../pages/PlayerPage'
import SettingsPage from '../pages/SettingsPage'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route element={<Layout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/browse" element={<BrowsePage />} />
        <Route path="/details/:type/:id" element={<DetailsPage />} />
        <Route path="/player/:type/:id" element={<PlayerPage />} />
        <Route path="/settings" element={<SettingsPage />} />
      </Route>
    </Routes>
  )
}

export default App
