import React, { useState } from 'react'
import { Outlet, NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../../app/AuthContext'
import { Home, Tv, Film, MonitorPlay, Settings, LogOut, Menu, X, User } from 'lucide-react'

export default function Layout() {
  const { activeSession, playerInfo, logout } = useAuth()
  const navigate = useNavigate()
  const [mobileOpen, setMobileOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const navLinks = [
    { to: '/', label: 'Dashboard', icon: <Home size={18} /> },
    { to: '/browse?type=live', label: 'Live TV', icon: <Tv size={18} /> },
    { to: '/browse?type=movies', label: 'Movies', icon: <Film size={18} /> },
    { to: '/browse?type=series', label: 'Series', icon: <MonitorPlay size={18} /> },
    { to: '/settings', label: 'Settings', icon: <Settings size={18} /> },
  ]

  const expiryString = playerInfo?.user_info?.exp_date 
    ? new Date(Number(playerInfo.user_info.exp_date) * 1000).toLocaleDateString()
    : 'N/A'

  return (
    <div className="layout-container" style={{ minHeight: '100vh', background: 'var(--bg-main)', color: 'var(--text-primary)' }}>
      
      {/* Mobile Header */}
      <header style={{
        display: 'none',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '1rem',
        background: 'var(--bg-card)',
        borderBottom: '1px solid var(--border-light)',
        position: 'sticky',
        top: 0,
        zIndex: 100,
      }} className="mobile-header">
        <h2 style={{ fontSize: '1.25rem', fontWeight: 800, background: 'var(--primary-gradient)', WebkitBackgroundClip: 'text', WebkitTextFillColor: 'transparent' }}>
          NileFusion IPTV
        </h2>
        <button 
          onClick={() => setMobileOpen(!mobileOpen)}
          style={{ background: 'transparent', border: 'none', color: '#fff', cursor: 'pointer' }}
        >
          {mobileOpen ? <X size={24} /> : <Menu size={24} />}
        </button>
      </header>

      {/* Sidebar Navigation */}
      <aside 
        style={{
          width: '260px',
          position: 'fixed',
          top: 0,
          bottom: 0,
          left: 0,
          background: 'var(--sidebar-gradient)',
          borderRight: '1px solid var(--border-light)',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'space-between',
          padding: '2rem 1.5rem',
          zIndex: 90,
          transform: mobileOpen ? 'translateX(0)' : undefined,
          transition: 'var(--transition-smooth)',
        }}
        className={`sidebar-nav ${mobileOpen ? 'mobile-open' : ''}`}
      >
        <div style={{ display: 'flex', flexDirection: 'column', gap: '2rem' }}>
          {/* Logo */}
          <div>
            <h2 style={{ fontSize: '1.5rem', fontWeight: 800, background: 'var(--primary-gradient)', WebkitBackgroundClip: 'text', WebkitTextFillColor: 'transparent', textAlign: 'center' }}>
              NileFusion IPTV
            </h2>
            <div style={{ height: '1px', background: 'var(--border-light)', marginTop: '1.5rem' }} />
          </div>

          {/* Navigation Links */}
          <nav style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            {navLinks.map((link) => (
              <NavLink
                key={link.to}
                to={link.to}
                onClick={() => setMobileOpen(false)}
                style={({ isActive }) => ({
                  display: 'flex',
                  alignItems: 'center',
                  gap: '0.75rem',
                  padding: '0.8rem 1rem',
                  borderRadius: '8px',
                  color: isActive ? '#fff' : 'var(--text-secondary)',
                  background: isActive ? 'var(--bg-hover)' : 'transparent',
                  borderLeft: isActive ? '3px solid var(--accent-color)' : '3px solid transparent',
                  textDecoration: 'none',
                  fontWeight: 500,
                  fontSize: '0.95rem',
                  transition: 'var(--transition-smooth)',
                })}
                className="nav-link"
              >
                {link.icon}
                <span>{link.label}</span>
              </NavLink>
            ))}
          </nav>
        </div>

        {/* Sidebar Footer (User Info & Logout) */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div style={{ height: '1px', background: 'var(--border-light)', marginBottom: '0.5rem' }} />
          
          {activeSession && (
            <div style={{
              display: 'flex',
              alignItems: 'center',
              gap: '0.75rem',
              padding: '0.5rem',
              background: 'rgba(255,255,255,0.02)',
              borderRadius: '8px',
              border: '1px solid var(--border-light)'
            }}>
              <div style={{
                width: '36px',
                height: '36px',
                borderRadius: '50%',
                background: 'var(--primary-gradient)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                color: '#fff',
                fontWeight: 'bold',
              }}>
                <User size={18} />
              </div>
              <div style={{ overflow: 'hidden' }}>
                <div style={{ fontSize: '0.85rem', fontWeight: 600, color: '#fff', textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }}>
                  {activeSession.username}
                </div>
                <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                  Expires: {expiryString}
                </div>
              </div>
            </div>
          )}

          <button 
            onClick={handleLogout}
            className="btn btn-secondary"
            style={{ width: '100%', justifyContent: 'center' }}
          >
            <LogOut size={16} />
            <span>Logout</span>
          </button>
        </div>
      </aside>

      {/* Main Content Area */}
      <main className="main-content">
        <Outlet />
      </main>

      {/* CSS details to manage responsiveness */}
      <style>{`
        @media (max-width: 768px) {
          .mobile-header {
            display: flex !important;
          }
          .sidebar-nav {
            transform: translateX(-100%);
            top: 60px !important;
            width: 100% !important;
            border-right: none !important;
            border-bottom: 1px solid var(--border-light);
            background: var(--bg-main) !important;
          }
          .sidebar-nav.mobile-open {
            transform: translateX(0);
          }
        }
      `}</style>

    </div>
  )
}
