import React, { useState } from 'react'
import { Outlet, NavLink, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../../app/AuthContext'
import { Home, Tv, Film, MonitorPlay, Settings, LogOut, Menu, X, User } from 'lucide-react'
import nilefusionLogo from '../../assets/logo.png'

export default function Layout() {
  const { activeSession, playerInfo, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
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

  const getLinkStyles = (label: string, isActive: boolean) => {
    switch (label) {
      case 'Live TV':
        return {
          background: isActive 
            ? 'linear-gradient(135deg, rgba(6, 182, 212, 0.25) 0%, rgba(6, 182, 212, 0.05) 100%)' 
            : 'rgba(6, 182, 212, 0.03)',
          borderLeft: isActive ? '3.5px solid #06b6d4' : '3.5px solid transparent',
          color: isActive ? '#22d3ee' : 'rgba(255, 255, 255, 0.65)',
          textShadow: isActive ? '0 0 10px rgba(6, 182, 212, 0.4)' : 'none',
        }
      case 'Movies':
        return {
          background: isActive 
            ? 'linear-gradient(135deg, rgba(236, 72, 153, 0.25) 0%, rgba(236, 72, 153, 0.05) 100%)' 
            : 'rgba(236, 72, 153, 0.03)',
          borderLeft: isActive ? '3.5px solid #ec4899' : '3.5px solid transparent',
          color: isActive ? '#f472b6' : 'rgba(255, 255, 255, 0.65)',
          textShadow: isActive ? '0 0 10px rgba(236, 72, 153, 0.4)' : 'none',
        }
      case 'Series':
        return {
          background: isActive 
            ? 'linear-gradient(135deg, rgba(245, 158, 11, 0.25) 0%, rgba(245, 158, 11, 0.05) 100%)' 
            : 'rgba(245, 158, 11, 0.03)',
          borderLeft: isActive ? '3.5px solid #fbbf24' : '3.5px solid transparent',
          color: isActive ? '#fbbf24' : 'rgba(255, 255, 255, 0.65)',
          textShadow: isActive ? '0 0 10px rgba(251, 191, 36, 0.4)' : 'none',
        }
      default:
        return {
          background: isActive ? 'rgba(255, 255, 255, 0.08)' : 'transparent',
          borderLeft: isActive ? '3.5px solid var(--text-secondary)' : '3.5px solid transparent',
          color: isActive ? '#fff' : 'var(--text-secondary)',
        }
    }
  }

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
          Nile TV
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
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '0.5rem' }}>
              <img
                src="/favicon.png"
                alt="Nile TV"
                style={{ width: '32px', height: '32px', borderRadius: '6px', objectFit: 'cover', filter: 'drop-shadow(0 0 6px rgba(229,57,53,0.4))' }}
              />
              <h2 style={{ fontSize: '1.4rem', fontWeight: 800, background: 'var(--primary-gradient)', WebkitBackgroundClip: 'text', WebkitTextFillColor: 'transparent' }}>
                Nile TV
              </h2>
            </div>
            <div style={{ height: '1px', background: 'var(--border-light)', marginTop: '1.5rem' }} />
          </div>

          {/* Navigation Links */}
          <nav style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            {navLinks.map((link) => {
              // For browse links, match both path AND query param
              const isBrowseLink = link.to.startsWith('/browse?')
              const isActive = isBrowseLink
                ? location.pathname + location.search === link.to
                : location.pathname === link.to && !location.search

              const customStyles = getLinkStyles(link.label, isActive)
              return (
                <NavLink
                  key={link.to}
                  to={link.to}
                  onClick={() => setMobileOpen(false)}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: '0.75rem',
                    padding: '0.8rem 1rem',
                    borderRadius: '8px',
                    textDecoration: 'none',
                    fontWeight: 600,
                    fontSize: '0.95rem',
                    transition: 'var(--transition-smooth)',
                    ...customStyles,
                  }}
                  className={`nav-link nav-link-${link.label.toLowerCase().replace(/\s+/g, '-')}`}
                >
                  {link.icon}
                  <span>{link.label}</span>
                </NavLink>
              )
            })}
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

          {/* Powered by NileFusion */}
          <a
            href="https://NileFusion.com"
            target="_blank"
            rel="noopener noreferrer"
            className="nilefusion-credit"
            style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '0.4rem',
              textDecoration: 'none',
              padding: '0.5rem',
              borderRadius: '6px',
              transition: 'var(--transition-smooth)',
            }}
          >
            <img
              src={nilefusionLogo}
              alt="NileFusion"
              style={{ width: '16px', height: '16px', borderRadius: '3px', objectFit: 'cover' }}
            />
            <span style={{
              fontSize: '0.72rem',
              color: 'var(--text-muted)',
              letterSpacing: '0.02em',
            }}>
              Powered by{' '}
              <span style={{
                background: 'var(--primary-gradient)',
                WebkitBackgroundClip: 'text',
                WebkitTextFillColor: 'transparent',
                fontWeight: 700,
              }}>
                NileFusion
              </span>
            </span>
          </a>
        </div>
      </aside>

      {/* Main Content Area */}
      <main className="main-content">
        <Outlet />
      </main>

      {/* CSS details to manage responsiveness */}
      <style>{`
        .nav-link:hover {
          transform: translateX(6px);
        }
        .nav-link-live-tv:hover {
          background: rgba(6, 182, 212, 0.3) !important;
          color: #22d3ee !important;
          filter: drop-shadow(0 0 5px rgba(6, 182, 212, 0.4));
        }
        .nav-link-movies:hover {
          background: rgba(236, 72, 153, 0.3) !important;
          color: #f472b6 !important;
          filter: drop-shadow(0 0 5px rgba(236, 72, 153, 0.4));
        }
        .nav-link-series:hover {
          background: rgba(245, 158, 11, 0.3) !important;
          color: #fbbf24 !important;
          filter: drop-shadow(0 0 5px rgba(245, 158, 11, 0.4));
        }
        .nav-link-dashboard:hover, .nav-link-settings:hover {
          background: rgba(255, 255, 255, 0.15) !important;
          color: #fff !important;
        }
        .nilefusion-credit:hover {
          background: rgba(99, 102, 241, 0.08);
          filter: drop-shadow(0 0 6px rgba(99, 102, 241, 0.25));
        }
        .nilefusion-credit:hover img {
          transform: scale(1.15);
          transition: transform 0.2s ease;
        }

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
