import React, { useState, useEffect, useRef } from 'react'
import { useParams, useSearchParams, useNavigate } from 'react-router-dom'
import { useAuth } from '../app/AuthContext'
import { buildStreamUrl } from '../services/api'
import { ArrowLeft, Play, Pause, ExternalLink, Copy, Download, AlertTriangle, Check, Info } from 'lucide-react'

export default function PlayerPage() {
  const { type, id } = useParams<{ type: string; id: string }>()
  const [searchParams] = useSearchParams()
  const navigate = useNavigate()
  const { activeSession } = useAuth()

  const videoRef = useRef<HTMLVideoElement>(null)
  
  // Parse parameters from query string
  const name = searchParams.get('name') || 'Stream'
  const ext = searchParams.get('ext') || (type === 'live' ? 'ts' : 'mp4')
  const season = searchParams.get('season')
  const epNum = searchParams.get('ep')

  const [isPlaying, setIsPlaying] = useState(false)
  const [copied, setCopied] = useState(false)
  const [hasError, setHasError] = useState(false)

  // Construct stream URL
  const streamUrl = activeSession && id && type
    ? buildStreamUrl(activeSession, type as any, id, ext)
    : ''

  const handleCopyLink = () => {
    if (!streamUrl) return
    navigator.clipboard.writeText(streamUrl)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const handleVideoError = () => {
    console.warn("HTML5 Video playback failed. This is typical for TS/MKV formats in browser.")
    setHasError(true)
  }

  const togglePlay = () => {
    if (!videoRef.current) return
    if (isPlaying) {
      videoRef.current.pause()
      setIsPlaying(false)
    } else {
      videoRef.current.play().then(() => {
        setIsPlaying(true)
      }).catch((e) => {
        console.error("Play failed", e)
        setHasError(true)
      })
    }
  }

  // Format headers based on media type
  const headerTitle = () => {
    if (type === 'live') return `Live TV • ${name}`
    if (type === 'movies') return `Movie • ${name}`
    if (type === 'series') return `Series • ${name} (S${season} Ep${epNum})`
    return name
  }

  return (
    <div style={{
      position: 'fixed',
      inset: 0,
      background: '#000',
      color: '#fff',
      zIndex: 9999,
      display: 'flex',
      flexDirection: 'column',
      fontFamily: 'var(--font-family)',
    }}>
      
      {/* Top Controller Bar */}
      <header style={{
        padding: '1.25rem 2rem',
        background: 'linear-gradient(to bottom, rgba(0,0,0,0.85) 0%, transparent 100%)',
        display: 'flex',
        alignItems: 'center',
        gap: '1.5rem',
        position: 'absolute',
        top: 0,
        left: 0,
        right: 0,
        zIndex: 10,
      }}>
        <button 
          onClick={() => navigate(-1)}
          style={{
            background: 'rgba(255,255,255,0.1)',
            border: 'none',
            borderRadius: '50%',
            width: '40px',
            height: '40px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#fff',
            cursor: 'pointer',
            transition: 'background 0.2s',
          }}
          onMouseEnter={(e) => { e.currentTarget.style.background = 'rgba(255,255,255,0.2)' }}
          onMouseLeave={(e) => { e.currentTarget.style.background = 'rgba(255,255,255,0.1)' }}
        >
          <ArrowLeft size={20} />
        </button>
        <div>
          <h2 style={{ fontSize: '1.2rem', fontWeight: 700 }}>{headerTitle()}</h2>
          <span style={{ fontSize: '0.75rem', color: 'rgba(255,255,255,0.5)' }}>Format: .{ext.toUpperCase()}</span>
        </div>
      </header>

      {/* Video Stream Element */}
      <div style={{
        flex: 1,
        position: 'relative',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
      }} onClick={togglePlay}>
        
        {streamUrl && (
          <video
            ref={videoRef}
            src={streamUrl}
            autoPlay
            controls
            onPlay={() => setIsPlaying(true)}
            onPause={() => setIsPlaying(false)}
            onError={handleVideoError}
            style={{
              maxWidth: '100%',
              maxHeight: '100%',
              width: '100%',
              height: '100%',
              objectFit: 'contain',
              cursor: 'pointer',
            }}
          />
        )}

        {/* Format Warning Overlay */}
        {(hasError || ext === 'ts' || ext === 'mkv') && (
          <div style={{
            position: 'absolute',
            inset: 0,
            background: 'rgba(0,0,0,0.85)',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            padding: '2rem',
            textAlign: 'center',
            pointerEvents: 'auto',
          }} onClick={(e) => e.stopPropagation()}>
            <div className="glass-panel" style={{ maxWidth: '480px', padding: '2.5rem', display: 'flex', flexDirection: 'column', gap: '1.25rem', alignItems: 'center' }}>
              <AlertTriangle size={48} color="#f59e0b" />
              <div>
                <h3 style={{ fontSize: '1.25rem', color: '#fff', marginBottom: '0.5rem' }}>Browser Compatibility Check</h3>
                <p style={{ fontSize: '0.85rem', color: 'var(--text-secondary)', lineHeight: 1.5 }}>
                  Web browsers generally do not support playing <strong>.{ext.toUpperCase()}</strong> video files natively. For full-quality playback, we recommend using an external media player.
                </p>
              </div>

              <div style={{ width: '100%', display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                {/* Copy stream link */}
                <div style={{ display: 'flex', gap: '0.5rem', width: '100%' }}>
                  <input
                    type="text"
                    readOnly
                    className="form-input"
                    value={streamUrl}
                    style={{ fontSize: '0.8rem' }}
                  />
                  <button 
                    className="btn glow-btn"
                    style={{ flexShrink: 0, padding: '0.75rem' }}
                    onClick={handleCopyLink}
                    title="Copy stream URL"
                  >
                    {copied ? <Check size={16} /> : <Copy size={16} />}
                  </button>
                </div>

                <div style={{ display: 'flex', gap: '1rem', width: '100%', marginTop: '0.5rem' }}>
                  <a
                    href={streamUrl}
                    download
                    className="btn btn-secondary"
                    style={{ flex: 1, textDecoration: 'none', fontSize: '0.85rem' }}
                  >
                    <Download size={15} />
                    Download File
                  </a>
                  <button
                    className="btn btn-secondary"
                    style={{ flex: 1, fontSize: '0.85rem' }}
                    onClick={() => setHasError(false)}
                  >
                    <Play size={15} />
                    Try browser play
                  </button>
                </div>
              </div>

              <div style={{ display: 'flex', alignItems: 'center', gap: '0.4rem', color: 'var(--text-muted)', fontSize: '0.75rem' }}>
                <Info size={14} />
                <span>Tip: You can open this link directly in VLC or PotPlayer.</span>
              </div>
            </div>
          </div>
        )}
      </div>

    </div>
  )
}
