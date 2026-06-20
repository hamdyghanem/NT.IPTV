import React, { useState } from 'react'
import { useAuth } from '../app/AuthContext'
import { Server, User, Lock, Trash2, Eye, EyeOff, AlertCircle, Play, Globe } from 'lucide-react'

export default function LoginPage() {
  const { login, savedProfiles, deleteProfile } = useAuth()
  
  const [profileName, setProfileName] = useState('')
  const [server, setServer] = useState('')
  const [port, setPort] = useState('')
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [useHttps, setUseHttps] = useState(false)
  const [saveProfile, setSaveProfile] = useState(true)
  
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [isConnecting, setIsConnecting] = useState(false)

  const handleConnect = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)

    if (!server || !username || !password) {
      setError('Please fill in Server, Username, and Password.')
      return
    }

    const cleanServer = server.replace(/^https?:\/\//i, '').trim()
    const session = {
      server: cleanServer,
      port: port.trim(),
      username: username.trim(),
      password: password.trim(),
      useHttps,
    }

    const finalProfileName = profileName.trim() || cleanServer

    setIsConnecting(true)
    try {
      await login(session, saveProfile, finalProfileName)
    } catch (err: any) {
      setError(err.message || 'Failed to connect. Please check your credentials and server details.')
    } finally {
      setIsConnecting(false)
    }
  }

  const handleQuickLogin = async (profile: typeof savedProfiles[0]) => {
    setError(null)
    setIsConnecting(true)
    
    // Auto-fill form fields
    setProfileName(profile.profileName)
    setServer(profile.server)
    setPort(profile.port)
    setUsername(profile.username)
    setPassword(profile.password)
    setUseHttps(profile.useHttps)

    try {
      await login(
        {
          server: profile.server,
          port: profile.port,
          username: profile.username,
          password: profile.password,
          useHttps: profile.useHttps,
        },
        true,
        profile.profileName
      )
    } catch (err: any) {
      setError(err.message || 'Failed to connect using selected profile.')
    } finally {
      setIsConnecting(false)
    }
  }

  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      padding: '2rem',
      background: 'radial-gradient(circle at top right, #1a103c 0%, var(--bg-darker) 100%)',
    }}>
      <div className="glass-panel" style={{
        display: 'grid',
        gridTemplateColumns: savedProfiles.length > 0 ? '300px 480px' : '1fr',
        maxWidth: savedProfiles.length > 0 ? '780px' : '480px',
        width: '100%',
        overflow: 'hidden',
        transition: 'all 0.3s ease',
      }}>
        
        {/* Saved Profiles Section */}
        {savedProfiles.length > 0 && (
          <div style={{
            padding: '2rem',
            borderRight: '1px solid var(--border-light)',
            background: 'rgba(0,0,0,0.2)',
            display: 'flex',
            flexDirection: 'column',
          }}>
            <h3 style={{ fontSize: '1.1rem', marginBottom: '1.25rem', color: '#fff' }}>Profiles</h3>
            <div style={{
              display: 'flex',
              flexDirection: 'column',
              gap: '1rem',
              overflowY: 'auto',
              flex: 1,
              maxHeight: '400px',
              paddingRight: '0.25rem'
            }}>
              {savedProfiles.map((p) => (
                <div 
                  key={p.profileName} 
                  style={{
                    position: 'relative',
                    padding: '0.75rem 1rem',
                    background: 'var(--bg-card)',
                    border: '1px solid var(--border-light)',
                    borderRadius: '8px',
                    cursor: 'pointer',
                    transition: 'var(--transition-smooth)',
                    display: 'flex',
                    flexDirection: 'column',
                    gap: '0.2rem',
                  }}
                  className="profile-card"
                  onClick={() => handleQuickLogin(p)}
                >
                  <div style={{ fontWeight: 600, color: '#fff', fontSize: '0.9rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                    <div style={{ width: 8, height: 8, borderRadius: '50%', background: 'var(--accent-color)' }} />
                    {p.profileName}
                  </div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-secondary)', display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                    <Globe size={12} />
                    {p.server}:{p.port || (p.useHttps ? '443' : '80')}
                  </div>
                  <button 
                    onClick={(e) => {
                      e.stopPropagation()
                      deleteProfile(p.profileName)
                    }}
                    style={{
                      position: 'absolute',
                      right: '8px',
                      top: '50%',
                      transform: 'translateY(-50%)',
                      background: 'transparent',
                      border: 'none',
                      color: 'var(--text-muted)',
                      padding: '4px',
                    }}
                    title="Delete Profile"
                  >
                    <Trash2 size={15} style={{ transition: 'color 0.2s' }} onMouseEnter={(e) => (e.currentTarget.style.color = '#ef4444')} onMouseLeave={(e) => (e.currentTarget.style.color = 'var(--text-muted)')} />
                  </button>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Login Form Section */}
        <div style={{ padding: '2.5rem' }}>
          <div style={{ textAlign: 'center', marginBottom: '2rem' }}>
            <h2 style={{ fontSize: '1.75rem', fontWeight: 800, background: 'var(--primary-gradient)', WebkitBackgroundClip: 'text', WebkitTextFillColor: 'transparent' }}>
              NileFusion IPTV
            </h2>
            <p style={{ color: 'var(--text-secondary)', fontSize: '0.85rem', marginTop: '0.3rem' }}>
              Connect your IPTV subscription catalog
            </p>
          </div>

          {error && (
            <div style={{
              display: 'flex',
              alignItems: 'center',
              gap: '0.5rem',
              background: 'rgba(239, 68, 68, 0.1)',
              border: '1px solid rgba(239, 68, 68, 0.2)',
              borderRadius: '8px',
              padding: '0.75rem 1rem',
              color: '#f87171',
              fontSize: '0.85rem',
              marginBottom: '1.5rem',
            }}>
              <AlertCircle size={18} style={{ flexShrink: 0 }} />
              <span>{error}</span>
            </div>
          )}

          <form onSubmit={handleConnect}>
            <div className="form-group">
              <label>Profile Name (Optional)</label>
              <div style={{ position: 'relative' }}>
                <input 
                  type="text" 
                  className="form-input" 
                  placeholder="e.g. My Server" 
                  value={profileName}
                  onChange={(e) => setProfileName(e.target.value)}
                  disabled={isConnecting}
                />
              </div>
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: '1fr 100px', gap: '1rem' }}>
              <div className="form-group">
                <label>Server address</label>
                <div style={{ position: 'relative' }}>
                  <Server size={16} style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
                  <input 
                    type="text" 
                    className="form-input" 
                    style={{ paddingLeft: '2.25rem' }}
                    placeholder="provider.com" 
                    value={server}
                    onChange={(e) => setServer(e.target.value)}
                    disabled={isConnecting}
                    required
                  />
                </div>
              </div>
              <div className="form-group">
                <label>Port</label>
                <input 
                  type="text" 
                  className="form-input" 
                  placeholder="8080" 
                  value={port}
                  onChange={(e) => setPort(e.target.value)}
                  disabled={isConnecting}
                />
              </div>
            </div>

            <div className="form-group">
              <label>Username</label>
              <div style={{ position: 'relative' }}>
                <User size={16} style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
                <input 
                  type="text" 
                  className="form-input" 
                  style={{ paddingLeft: '2.25rem' }}
                  placeholder="Username" 
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  disabled={isConnecting}
                  required
                />
              </div>
            </div>

            <div className="form-group">
              <label>Password</label>
              <div style={{ position: 'relative' }}>
                <Lock size={16} style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
                <input 
                  type={showPassword ? 'text' : 'password'} 
                  className="form-input" 
                  style={{ paddingLeft: '2.25rem', paddingRight: '2.5rem' }}
                  placeholder="••••••••" 
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  disabled={isConnecting}
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  style={{
                    position: 'absolute',
                    right: '12px',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    background: 'transparent',
                    border: 'none',
                    color: 'var(--text-muted)',
                    padding: 0,
                    cursor: 'pointer',
                  }}
                >
                  {showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                </button>
              </div>
            </div>

            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', margin: '1.5rem 0' }}>
              <label className="checkbox-container">
                <input 
                  type="checkbox" 
                  checked={useHttps}
                  onChange={(e) => setUseHttps(e.target.checked)}
                  disabled={isConnecting}
                />
                <span className="custom-checkbox" />
                <span>Use HTTPS</span>
              </label>

              <label className="checkbox-container">
                <input 
                  type="checkbox" 
                  checked={saveProfile}
                  onChange={(e) => setSaveProfile(e.target.checked)}
                  disabled={isConnecting}
                />
                <span className="custom-checkbox" />
                <span>Save Profile</span>
              </label>
            </div>

            <button 
              type="submit" 
              className="glow-btn" 
              style={{ width: '100%', padding: '0.85rem' }}
              disabled={isConnecting}
            >
              {isConnecting ? (
                <>
                  <div className="spinner" style={{ width: 18, height: 18, borderTopColor: '#fff' }} />
                  <span>Connecting...</span>
                </>
              ) : (
                <>
                  <Play size={16} />
                  <span>Connect Server</span>
                </>
              )}
            </button>
          </form>
        </div>
        
      </div>
    </div>
  )
}
