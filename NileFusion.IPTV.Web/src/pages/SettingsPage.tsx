import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../app/AuthContext'
import { Globe, User, Calendar, Trash2, RefreshCw, LogOut, ShieldAlert, Check } from 'lucide-react'

export default function SettingsPage() {
  const { activeSession, playerInfo, savedProfiles, deleteProfile, logout } = useAuth()
  const navigate = useNavigate()
  
  const [clearing, setClearing] = useState(false)
  const [cleared, setCleared] = useState(false)

  const handleClearData = () => {
    if (window.confirm("Are you sure you want to clear all Nile TV profiles, favorites, and application settings? This action cannot be undone.")) {
      setClearing(true)
      setTimeout(() => {
        localStorage.clear()
        setClearing(false)
        setCleared(true)
        setTimeout(() => {
          window.location.href = '/login'
        }, 1000)
      }, 1000)
    }
  }

  const handleSwitchProfile = (p: typeof savedProfiles[0]) => {
    logout()
    // Redirect to login and fill in inputs, or just force quick login
    // Let's redirect to login which will show the profiles list for immediate click-login
    navigate('/login')
  }

  const expiryString = playerInfo?.user_info?.exp_date 
    ? new Date(Number(playerInfo.user_info.exp_date) * 1000).toLocaleDateString(undefined, { dateStyle: 'long' })
    : 'N/A'

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '2.5rem', fontFamily: 'var(--font-family)', color: 'var(--text-primary)' }}>
      
      {/* Page Title */}
      <div>
        <h1 style={{ fontSize: '2rem', fontWeight: 800, color: '#fff', marginBottom: '0.5rem' }}>Settings</h1>
        <p style={{ color: 'var(--text-secondary)', fontSize: '0.95rem' }}>Manage your IPTV profiles and connection settings.</p>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))', gap: '2rem', alignItems: 'start' }}>
        
        {/* Connection & Account Metadata */}
        <section className="glass-panel" style={{ padding: '2rem', display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
          <h2 style={{ fontSize: '1.25rem', fontWeight: 700, color: '#fff', borderBottom: '1px solid var(--border-light)', paddingBottom: '0.75rem' }}>
            Account Info
          </h2>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '1.2rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Active Username</span>
              <strong style={{ color: '#fff' }}>{activeSession?.username || 'N/A'}</strong>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Server address</span>
              <strong style={{ color: '#fff', maxWidth: '200px', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                {activeSession?.server || 'N/A'}
              </strong>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Server Port</span>
              <strong style={{ color: '#fff' }}>{activeSession?.port || (activeSession?.useHttps ? '443' : '80')}</strong>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Timezone</span>
              <strong style={{ color: '#fff' }}>{playerInfo?.server_info?.timezone || 'N/A'}</strong>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Expiration</span>
              <strong style={{ color: '#fff' }}>{expiryString}</strong>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
              <span style={{ color: 'var(--text-secondary)' }}>Max Connections</span>
              <strong style={{ color: '#fff' }}>{playerInfo?.user_info?.max_connections || '1'}</strong>
            </div>
          </div>
        </section>

        {/* Saved Profiles list */}
        <section className="glass-panel" style={{ padding: '2rem', display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
          <h2 style={{ fontSize: '1.25rem', fontWeight: 700, color: '#fff', borderBottom: '1px solid var(--border-light)', paddingBottom: '0.75rem' }}>
            Manage Profiles
          </h2>

          {savedProfiles.length === 0 ? (
            <p style={{ fontSize: '0.9rem', color: 'var(--text-secondary)' }}>No profiles saved locally.</p>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', maxHeight: '250px', overflowY: 'auto', paddingRight: '0.25rem' }}>
              {savedProfiles.map((p) => {
                const isActive = activeSession?.username === p.username && activeSession?.server === p.server
                
                return (
                  <div
                    key={p.profileName}
                    style={{
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'space-between',
                      padding: '0.75rem 1rem',
                      background: 'var(--bg-card)',
                      border: isActive ? '1px solid var(--accent-color)' : '1px solid var(--border-light)',
                      borderRadius: '8px',
                    }}
                  >
                    <div style={{ overflow: 'hidden' }}>
                      <div style={{ fontWeight: 600, color: '#fff', fontSize: '0.9rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                        {p.profileName}
                        {isActive && (
                          <span style={{
                            fontSize: '0.75rem',
                            padding: '1px 6px',
                            background: 'rgba(99, 102, 241, 0.15)',
                            color: 'var(--accent-color)',
                            borderRadius: '4px',
                            border: '1px solid rgba(99, 102, 241, 0.3)',
                          }}>
                            Active
                          </span>
                        )}
                      </div>
                      <div style={{ fontSize: '0.75rem', color: 'var(--text-secondary)', textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap', maxWidth: '200px' }}>
                        {p.server}:{p.port || (p.useHttps ? '443' : '80')}
                      </div>
                    </div>

                    <div style={{ display: 'flex', gap: '0.5rem', flexShrink: 0 }}>
                      {!isActive && (
                        <button
                          className="btn btn-secondary"
                          style={{ padding: '0.4rem 0.8rem', fontSize: '0.8rem' }}
                          onClick={() => handleSwitchProfile(p)}
                        >
                          Switch
                        </button>
                      )}
                      <button
                        className="btn btn-secondary"
                        style={{ padding: '0.4rem', color: 'var(--text-muted)' }}
                        onClick={() => deleteProfile(p.profileName)}
                        title="Delete Profile"
                      >
                        <Trash2 size={15} style={{ transition: 'color 0.2s' }} onMouseEnter={(e) => e.currentTarget.style.color = '#ef4444'} onMouseLeave={(e) => e.currentTarget.style.color = 'var(--text-muted)'} />
                      </button>
                    </div>
                  </div>
                )
              })}
            </div>
          )}
        </section>

      </div>

      {/* System & Cache control */}
      <section className="glass-panel" style={{ padding: '2rem', display: 'flex', flexDirection: 'column', gap: '1.5rem', maxWidth: '780px', width: '100%' }}>
        <h2 style={{ fontSize: '1.25rem', fontWeight: 700, color: '#fff', borderBottom: '1px solid var(--border-light)', paddingBottom: '0.75rem' }}>
          Application Cache
        </h2>
        
        <p style={{ fontSize: '0.9rem', color: 'var(--text-secondary)', lineHeight: 1.5 }}>
          If you are experiencing slow loading speeds or visual layout discrepancies, you can clear the local storage cache. This will delete all saved user profiles, list favorites, and reset settings.
        </p>

        <div style={{ display: 'flex', gap: '1rem', marginTop: '0.5rem' }}>
          <button 
            className="btn btn-danger"
            onClick={handleClearData}
            disabled={clearing || cleared}
          >
            {clearing ? (
              <>
                <RefreshCw className="spinner" size={16} />
                <span>Clearing settings...</span>
              </>
            ) : cleared ? (
              <>
                <Check size={16} />
                <span>Cleared successfully</span>
              </>
            ) : (
              <>
                <ShieldAlert size={16} />
                <span>Reset Application Cache</span>
              </>
            )}
          </button>
        </div>
      </section>

    </div>
  )
}
