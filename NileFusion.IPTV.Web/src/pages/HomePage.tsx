import React from 'react'
import { useAuth } from '../app/AuthContext'
import { Link } from 'react-router-dom'
import { Tv, Film, MonitorPlay, Settings, Calendar, ShieldCheck, Activity, Radio } from 'lucide-react'

export default function HomePage() {
  const { activeSession, playerInfo } = useAuth()

  const userInfo = playerInfo?.user_info
  const serverInfo = playerInfo?.server_info

  const expiryDate = userInfo?.exp_date 
    ? new Date(Number(userInfo.exp_date) * 1000).toLocaleDateString(undefined, { dateStyle: 'long' })
    : 'Unlimited'

  const createdDate = userInfo?.created_at
    ? new Date(Number(userInfo.created_at) * 1000).toLocaleDateString(undefined, { dateStyle: 'medium' })
    : 'Unknown'

  const stats = [
    { 
      label: 'Account Status', 
      value: userInfo?.status || 'Active', 
      icon: <ShieldCheck size={24} color="#10b981" />,
      glow: 'rgba(16, 185, 129, 0.15)',
      border: 'rgba(16, 185, 129, 0.3)'
    },
    { 
      label: 'Expiration Date', 
      value: expiryDate, 
      icon: <Calendar size={24} color="#6366f1" />,
      glow: 'rgba(99, 102, 241, 0.15)',
      border: 'rgba(99, 102, 241, 0.3)'
    },
    { 
      label: 'Active Connections', 
      value: `${userInfo?.active_cons || 0} / ${userInfo?.max_connections || 1}`, 
      icon: <Activity size={24} color="#06b6d4" />,
      glow: 'rgba(6, 182, 212, 0.15)',
      border: 'rgba(6, 182, 212, 0.3)'
    },
    { 
      label: 'Allowed Formats', 
      value: userInfo?.allowed_output_formats?.join(', ') || 'ts, mp4, mkv', 
      icon: <Radio size={24} color="#d946ef" />,
      glow: 'rgba(217, 70, 239, 0.15)',
      border: 'rgba(217, 70, 239, 0.3)'
    },
  ]

  const categories = [
    {
      title: 'Live TV Channels',
      desc: 'Watch real-time live channels sorted by categories.',
      to: '/browse?type=live',
      icon: <Tv size={36} />,
      bg: 'linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%)',
    },
    {
      title: 'VOD Movies',
      desc: 'Browse and watch full movies on demand.',
      to: '/browse?type=movies',
      icon: <Film size={36} />,
      bg: 'linear-gradient(135deg, #10b981 0%, #047857 100%)',
    },
    {
      title: 'TV Series',
      desc: 'Follow your favorite series with season lists.',
      to: '/browse?type=series',
      icon: <MonitorPlay size={36} />,
      bg: 'linear-gradient(135deg, #8b5cf6 0%, #6d28d9 100%)',
    },
    {
      title: 'Settings & Profiles',
      desc: 'Manage profiles, themes, and playback options.',
      to: '/settings',
      icon: <Settings size={36} />,
      bg: 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)',
    },
  ]

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '2.5rem' }}>
      
      {/* Header Banner */}
      <div>
        <h1 style={{ fontSize: '2rem', fontWeight: 800, color: '#fff', marginBottom: '0.5rem' }}>
          Welcome back, {activeSession?.username}!
        </h1>
        <p style={{ color: 'var(--text-secondary)', fontSize: '0.95rem' }}>
          Connected to server: <strong style={{ color: '#fff' }}>{activeSession?.server}</strong> (Protocol: {serverInfo?.server_protocol || (activeSession?.useHttps ? 'https' : 'http')})
        </p>
      </div>

      {/* Subscription Info Cards */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))', gap: '1.5rem' }}>
        {stats.map((stat, i) => (
          <div 
            key={i} 
            className="glass-panel" 
            style={{
              padding: '1.5rem',
              display: 'flex',
              alignItems: 'center',
              gap: '1.25rem',
              background: stat.glow,
              borderColor: stat.border,
            }}
          >
            <div style={{
              width: '48px',
              height: '48px',
              borderRadius: '10px',
              background: 'rgba(0,0,0,0.2)',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              flexShrink: 0
            }}>
              {stat.icon}
            </div>
            <div>
              <div style={{ fontSize: '0.75rem', fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.05em', color: 'var(--text-secondary)', marginBottom: '0.2rem' }}>
                {stat.label}
              </div>
              <div style={{ fontSize: '1.2rem', fontWeight: 700, color: '#fff' }}>
                {stat.value}
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Navigation Panels */}
      <div>
        <h2 style={{ fontSize: '1.4rem', fontWeight: 700, color: '#fff', marginBottom: '1.25rem' }}>
          Explore Catalogs
        </h2>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '1.5rem' }}>
          {categories.map((cat, i) => (
            <Link 
              key={i} 
              to={cat.to} 
              style={{
                textDecoration: 'none',
                color: 'inherit',
                borderRadius: '12px',
                overflow: 'hidden',
                display: 'flex',
                flexDirection: 'column',
                boxShadow: 'var(--shadow-lg)',
                border: '1px solid var(--border-light)',
                background: 'var(--bg-card)',
                transition: 'var(--transition-smooth)',
              }}
              className="dashboard-card"
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-6px)'
                e.currentTarget.style.borderColor = 'var(--accent-color)'
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)'
                e.currentTarget.style.borderColor = 'var(--border-light)'
              }}
            >
              {/* Card Banner */}
              <div style={{
                background: cat.bg,
                padding: '2rem 1.5rem',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                color: '#fff',
              }}>
                {cat.icon}
              </div>
              {/* Card Details */}
              <div style={{ padding: '1.5rem', display: 'flex', flexDirection: 'column', gap: '0.5rem', flex: 1 }}>
                <h3 style={{ fontSize: '1.15rem', fontWeight: 700, color: '#fff' }}>{cat.title}</h3>
                <p style={{ fontSize: '0.85rem', color: 'var(--text-secondary)', lineHeight: '1.4' }}>{cat.desc}</p>
              </div>
            </Link>
          ))}
        </div>
      </div>

      {/* User Info Footnote */}
      <div className="glass-panel" style={{ padding: '1.5rem', display: 'flex', justifyContent: 'space-between', flexWrap: 'wrap', gap: '1rem', fontSize: '0.85rem', color: 'var(--text-secondary)' }}>
        <div>Account Creation Date: <strong style={{ color: '#fff' }}>{createdDate}</strong></div>
        <div>Timezone: <strong style={{ color: '#fff' }}>{serverInfo?.timezone || 'N/A'}</strong></div>
        <div>Time on Server: <strong style={{ color: '#fff' }}>{serverInfo?.time_now || 'N/A'}</strong></div>
      </div>

    </div>
  )
}
