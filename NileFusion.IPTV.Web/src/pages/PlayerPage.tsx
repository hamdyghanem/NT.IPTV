import React, { useState, useEffect, useRef } from 'react'
import { useParams, useSearchParams, useNavigate } from 'react-router-dom'
import { useAuth } from '../app/AuthContext'
import { buildStreamUrl } from '../services/api'
import { ArrowLeft, Play, Pause, ExternalLink, Copy, Download, AlertTriangle, Check, Info } from 'lucide-react'
import Hls from 'hls.js'
import mpegts from 'mpegts.js'

export default function PlayerPage() {
  const { type, id } = useParams<{ type: string; id: string }>()
  const [searchParams] = useSearchParams()
  const navigate = useNavigate()
  const { activeSession } = useAuth()

  const videoRef = useRef<HTMLVideoElement>(null)
  const hlsRef = useRef<Hls | null>(null)
  const mpegtsRef = useRef<mpegts.Player | null>(null)
  
  // Parse parameters from query string
  const name = searchParams.get('name') || 'Stream'
  const ext = searchParams.get('ext') || (type === 'live' ? 'ts' : 'mp4')
  const season = searchParams.get('season')
  const epNum = searchParams.get('ep')

  const [isPlaying, setIsPlaying] = useState(false)
  const [copied, setCopied] = useState(false)
  const [hasError, setHasError] = useState(false)

  // Construct stream URL for copy link / download (direct un-proxied URL)
  const streamUrl = activeSession && id && type
    ? buildStreamUrl(activeSession, type as any, id, ext)
    : ''

  const proxiedStreamUrl = activeSession && id && type
    ? buildStreamUrl(activeSession, type as any, id, ext, true)
    : ''

  const destroyPlayers = () => {
    if (hlsRef.current) {
      hlsRef.current.destroy()
      hlsRef.current = null
    }
    if (mpegtsRef.current) {
      try {
        mpegtsRef.current.unload()
        mpegtsRef.current.detachMediaElement()
        mpegtsRef.current.destroy()
      } catch (e) {
        console.warn("Error destroying mpegts player:", e)
      }
      mpegtsRef.current = null
    }
    if (videoRef.current) {
      videoRef.current.removeAttribute('src')
      videoRef.current.load()
    }
  }

  useEffect(() => {
    const video = videoRef.current
    if (!video || !activeSession || !id || !type) return

    // Reset error state
    setHasError(false)

    // Clean up any existing players before starting
    destroyPlayers()

    // Construct proxied URL for Chrome browser playback to bypass CORS
    const browserPlayUrl = buildStreamUrl(activeSession, type as any, id, ext, true)
    if (!browserPlayUrl) return

    const extension = ext.toLowerCase()
    const isTsStream = extension === 'ts' || type === 'live'
    const isM3u8Stream = extension === 'm3u8'

    if (isTsStream) {
      if (mpegts.isSupported()) {
        const player = mpegts.createPlayer({
          type: 'mpegts',
          isLive: type === 'live',
          url: browserPlayUrl,
        }, {
          enableStashBuffer: false,
          liveBufferLatencyChasing: true,
        })
        
        mpegtsRef.current = player
        player.attachMediaElement(video)
        player.load()
        
        try {
          const playPromise = player.play()
          if (playPromise && typeof playPromise.catch === 'function') {
            playPromise.catch((err: any) => {
              console.warn("mpegts.js play failed, fallback:", err)
            })
          }
        } catch (err: any) {
          console.warn("mpegts.js play failed synchronously:", err)
        }

        player.on(mpegts.Events.ERROR, (errType: any, errDetail: any, errInfo: any) => {
          console.error("mpegts.js player error:", errType, errDetail, errInfo)
          setHasError(true)
        })
      } else {
        console.warn("mpegts.js is not supported, attempting native fallback")
        video.src = browserPlayUrl
      }
    } else if (isM3u8Stream) {
      if (Hls.isSupported()) {
        const hls = new Hls()
        hlsRef.current = hls
        hls.loadSource(browserPlayUrl)
        hls.attachMedia(video)
        
        hls.on(Hls.Events.MANIFEST_PARSED, () => {
          video.play().then(() => {
            setIsPlaying(true)
          }).catch(e => console.error("HLS play failed:", e))
        })

        hls.on(Hls.Events.ERROR, (event, data) => {
          if (data.fatal) {
            console.error("Fatal HLS error:", data)
            setHasError(true)
          }
        })
      } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = browserPlayUrl
      } else {
        console.warn("hls.js is not supported, attempting native fallback")
        video.src = browserPlayUrl
      }
    } else {
      // Other formats (MP4, etc.)
      video.src = browserPlayUrl
      video.play().then(() => {
        setIsPlaying(true)
      }).catch((err) => {
        console.warn("Native video play failed, might need user interaction or codec is unsupported:", err)
      })
    }

    return () => {
      destroyPlayers()
    }
  }, [activeSession, id, type, ext])

  const handleCopyLink = () => {
    if (!streamUrl) return
    navigator.clipboard.writeText(streamUrl)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const handleVideoError = () => {
    console.warn("HTML5 Video playback failed.")
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
        {(hasError || ext === 'mkv') && (
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
                    href={proxiedStreamUrl}
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
