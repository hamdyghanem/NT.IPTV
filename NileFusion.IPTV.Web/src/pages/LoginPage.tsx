import React, { useState, useRef, useEffect } from 'react'
import { useAuth } from '../app/AuthContext'
import { Server, User, Lock, Trash2, Eye, EyeOff, AlertCircle, Play, Globe } from 'lucide-react'
import lionzLogo from '../assets/Lionz-TV-Logo.png'
import defaultLogo from '../assets/logo.png'

function setCookie(name: string, value: string, days = 30) {
  const expires = new Date()
  expires.setTime(expires.getTime() + days * 24 * 60 * 60 * 1000)
  document.cookie = `${name}=${encodeURIComponent(value)};expires=${expires.toUTCString()};path=/`
}

function getCookie(name: string): string {
  const nameEQ = `${name}=`
  const ca = document.cookie.split(';')
  for (let i = 0; i < ca.length; i++) {
    let c = ca[i]
    while (c.charAt(0) === ' ') c = c.substring(1, c.length)
    if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length))
  }
  return ''
}

export default function LoginPage() {
  const { login, savedProfiles, deleteProfile } = useAuth()
  
  const [profileName, setProfileName] = useState(() => getCookie('nilefusion_last_profile_name') || '')
  const [server, setServer] = useState(() => getCookie('nilefusion_last_server') || '')
  const [port, setPort] = useState(() => getCookie('nilefusion_last_port') || '8080')
  const [username, setUsername] = useState(() => getCookie('nilefusion_last_username') || '')
  const [password, setPassword] = useState('')
  const [useHttps, setUseHttps] = useState(() => getCookie('nilefusion_last_use_https') === 'true')
  const [saveProfile, setSaveProfile] = useState(true)
  
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [isConnecting, setIsConnecting] = useState(false)

  // ── Connection log ──────────────────────────────────────────────
  type LogLine = { text: string; status: 'info' | 'ok' | 'err' }
  const [logLines, setLogLines] = useState<LogLine[]>([])
  const logRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (logRef.current) logRef.current.scrollTop = logRef.current.scrollHeight
  }, [logLines])

  const stamp = () => new Date().toLocaleTimeString('en-GB', { hour12: false })
  const pushLog = (text: string, status: LogLine['status'] = 'info') =>
    setLogLines(prev => [...prev, { text: `[${stamp()}]  ${text}`, status }])
  const sleep = (ms: number) => new Promise(r => setTimeout(r, ms))
  // ───────────────────────────────────────────────────────────────

  const handleConnect = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    setLogLines([])

    if (!server || !username || !password) {
      setError('Please fill in Server, Username, and Password.')
      return
    }

    const cleanServer = server.replace(/^https?:\/\//i, '').trim()
    const displayPort = port.trim() || (useHttps ? '443' : '80')
    const session = { server: cleanServer, port: port.trim(), username: username.trim(), password: password.trim(), useHttps }
    const finalProfileName = profileName.trim() || cleanServer

    setIsConnecting(true)
    try {
      pushLog(`Resolving  ${cleanServer}:${displayPort} …`)
      await sleep(380)
      pushLog(`Establishing connection …`)
      await sleep(250)
      pushLog(`Sending authentication request …`)

      await login(session, saveProfile, finalProfileName)

      pushLog(`✓  Authenticated as "${username.trim()}"`, 'ok')
      await sleep(180)
      pushLog(`✓  Session established`, 'ok')
      await sleep(150)
      pushLog(`✓  All systems go — launching dashboard …`, 'ok')

      setCookie('nilefusion_last_profile_name', finalProfileName)
      setCookie('nilefusion_last_server', cleanServer)
      setCookie('nilefusion_last_port', port.trim())
      setCookie('nilefusion_last_username', username.trim())
      setCookie('nilefusion_last_use_https', String(useHttps))
    } catch (err: any) {
      pushLog(`✗  ${err.message || 'Connection failed'}`, 'err')
      setError(err.message || 'Failed to connect. Please check your credentials and server details.')
    } finally {
      setIsConnecting(false)
    }
  }

  const handleQuickLogin = async (profile: typeof savedProfiles[0]) => {
    setError(null)
    setLogLines([])
    setIsConnecting(true)

    setProfileName(profile.profileName)
    setServer(profile.server)
    setPort(profile.port)
    setUsername(profile.username)
    setPassword(profile.password)
    setUseHttps(profile.useHttps)

    const displayPort = profile.port || (profile.useHttps ? '443' : '80')
    try {
      pushLog(`Loading profile “${profile.profileName}” …`)
      await sleep(280)
      pushLog(`Connecting to ${profile.server}:${displayPort} …`)
      await sleep(300)
      pushLog(`Authenticating …`)

      await login(
        { server: profile.server, port: profile.port, username: profile.username, password: profile.password, useHttps: profile.useHttps },
        true,
        profile.profileName
      )

      pushLog(`✓  Authenticated as "${profile.username}"`, 'ok')
      await sleep(150)
      pushLog(`✓  Session established — launching dashboard …`, 'ok')

      setCookie('nilefusion_last_profile_name', profile.profileName)
      setCookie('nilefusion_last_server', profile.server)
      setCookie('nilefusion_last_port', profile.port)
      setCookie('nilefusion_last_username', profile.username)
      setCookie('nilefusion_last_use_https', String(profile.useHttps))
    } catch (err: any) {
      pushLog(`✗  ${err.message || 'Connection failed'}`, 'err')
      setError(err.message || 'Failed to connect using selected profile.')
    } finally {
      setIsConnecting(false)
    }
  }

  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      padding: '2rem',
      background: 'radial-gradient(circle at top right, #2a0808 0%, var(--bg-darker) 100%)',
      position: 'relative',
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
            <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '0.75rem' }}>
              <img
                src={server.toLowerCase().includes('lionztv') ? lionzLogo : defaultLogo}
                alt={server.toLowerCase().includes('lionztv') ? 'Lionz TV Logo' : 'NileFusion IPTV'}
                style={{ maxHeight: '72px', width: 'auto', display: 'block', filter: 'drop-shadow(0 0 10px rgba(255,255,255,0.12))' }}
              />
            </div>
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

            {/* Predefined server suggestions */}
            <datalist id="server-suggestions">
              <option value="Lionztv.com" />
            </datalist>

            <div style={{ display: 'grid', gridTemplateColumns: '1fr 100px', gap: '1rem' }}>
              <div className="form-group">
                <label>Server address</label>
                <div style={{ position: 'relative' }}>
                  <Server size={16} style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)', pointerEvents: 'none', zIndex: 1 }} />
                  <input 
                    type="text"
                    list="server-suggestions"
                    className="form-input" 
                    style={{ paddingLeft: '2.25rem' }}
                    placeholder="provider.com" 
                    value={server}
                    onChange={(e) => setServer(e.target.value)}
                    disabled={isConnecting}
                    required
                    autoComplete="off"
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

            {/* ── Animated Connection Log Terminal ── */}
            {logLines.length > 0 && (
              <div
                ref={logRef}
                style={{
                  background: '#05060a',
                  border: '1px solid rgba(229,57,53,0.25)',
                  borderRadius: '8px',
                  padding: '0.85rem 1rem',
                  fontFamily: "'Courier New', Consolas, monospace",
                  fontSize: '0.78rem',
                  maxHeight: '140px',
                  overflowY: 'auto',
                  marginBottom: '1.25rem',
                  display: 'flex',
                  flexDirection: 'column',
                  gap: '0.22rem',
                  boxShadow: '0 0 20px rgba(229,57,53,0.08)',
                }}
              >
                {logLines.map((line, i) => (
                  <div
                    key={i}
                    style={{
                      color: line.status === 'ok'
                        ? '#fbbf24'
                        : line.status === 'err'
                        ? '#f87171'
                        : '#6b7280',
                      display: 'flex',
                      alignItems: 'flex-start',
                      gap: '0.4rem',
                      animation: 'fadeInLog 0.25s ease',
                      lineHeight: 1.5,
                    }}
                  >
                    <span style={{ color: line.status === 'ok' ? '#e53935' : line.status === 'err' ? '#ef4444' : '#374151', flexShrink: 0 }}>›</span>
                    {line.text}
                  </div>
                ))}
                {isConnecting && (
                  <span className="blink-cursor" style={{ marginTop: '0.1rem', display: 'inline-block', fontSize: '0.85rem' }}>█</span>
                )}
              </div>
            )}

            <button
              type="submit"
              className="glow-btn"
              style={{ width: '100%', padding: '0.85rem' }}
              disabled={isConnecting}
            >
              {isConnecting ? (
                <>
                  <div className="spinner" style={{ width: 18, height: 18, borderTopColor: '#fff' }} />
                  <span>Connecting…</span>
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

      {/* Powered by NileFusion */}
      <a
        href="https://NileFusion.com"
        target="_blank"
        rel="noopener noreferrer"
        style={{
          position: 'absolute',
          bottom: '1.5rem',
          display: 'flex',
          alignItems: 'center',
          gap: '0.45rem',
          textDecoration: 'none',
          padding: '0.4rem 0.75rem',
          borderRadius: '6px',
          transition: 'background 0.2s ease',
        }}
        onMouseEnter={e => (e.currentTarget.style.background = 'rgba(99,102,241,0.08)')}
        onMouseLeave={e => (e.currentTarget.style.background = 'transparent')}
      >
        <img
          src="/favicon.png"
          alt="NileFusion"
          style={{ width: '16px', height: '16px', borderRadius: '3px', objectFit: 'cover' }}
        />
        <span style={{ fontSize: '0.72rem', color: 'rgba(255,255,255,0.35)', letterSpacing: '0.02em' }}>
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
  )
}
