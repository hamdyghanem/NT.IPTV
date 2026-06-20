import React, { useState, useEffect, useMemo } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useAuth } from '../app/AuthContext'
import { fetchMovieDetails, fetchSeriesDetails, buildStreamUrl } from '../services/api'
import { WatchMovie, WatchSeries, EpisodeData } from '../types'
import { ArrowLeft, Play, Star, Calendar, Clock, Film, ExternalLink, Copy, Check, X, AlertCircle } from 'lucide-react'

export default function DetailsPage() {
  const { type, id } = useParams<{ type: string; id: string }>()
  const navigate = useNavigate()
  const { activeSession, toggleFavorite, isFavorite } = useAuth()

  const [movieDetails, setMovieDetails] = useState<WatchMovie | null>(null)
  const [seriesDetails, setSeriesDetails] = useState<WatchSeries | null>(null)
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  
  // Series-specific states
  const [selectedSeason, setSelectedSeason] = useState<string>('')
  const [activeEpisode, setActiveEpisode] = useState<EpisodeData | null>(null)
  
  // UI overlays
  const [showTrailer, setShowTrailer] = useState(false)
  const [showLinks, setShowLinks] = useState(false)
  const [copied, setCopied] = useState(false)

  const isFav = isFavorite(type === 'movies' ? 'movies' : 'series', id || '')

  useEffect(() => {
    if (activeSession && id && type) {
      loadDetails()
    }
  }, [activeSession, id, type])

  const loadDetails = async () => {
    setIsLoading(true)
    setError(null)
    try {
      if (type === 'movies') {
        const data = await fetchMovieDetails(activeSession!, id!)
        setMovieDetails(data)
      } else if (type === 'series') {
        const data = await fetchSeriesDetails(activeSession!, id!)
        setSeriesDetails(data)
        
        // Select the first season by default
        if (data.episodes) {
          const seasons = Object.keys(data.episodes).sort((a, b) => Number(a) - Number(b))
          if (seasons.length > 0) {
            setSelectedSeason(seasons[0])
          }
        }
      }
    } catch (err: any) {
      console.error("Failed to load details", err)
      setError("Failed to retrieve details. Please check your CORS configuration or proxy settings.")
    } finally {
      setIsLoading(false)
    }
  }

  // Derived fields to simplify UI rendering
  const details = type === 'movies' ? movieDetails : seriesDetails
  const info = details?.info
  
  const poster = type === 'movies' 
    ? movieDetails?.info?.movie_image 
    : seriesDetails?.info?.cover

  const backdrop = info?.backdrop_path && info.backdrop_path.length > 0
    ? info.backdrop_path[0]
    : info?.backdrop || poster

  const title = type === 'movies' 
    ? movieDetails?.movie_data?.name || ''
    : seriesDetails?.info?.name || ''
  const plot = info?.plot || 'No synopsis available.'
  const rating = type === 'movies' 
    ? movieDetails?.movie_data?.added // placeholder if rating is null, but let's check
    : seriesDetails?.info?.tmdb_id

  const genre = info?.genre || 'N/A'
  const director = info?.director || 'N/A'
  const cast = info?.cast || 'N/A'

  const duration = type === 'movies' && movieDetails?.info?.duration
    ? movieDetails.info.duration
    : ''

  const releaseDate = type === 'movies' && movieDetails?.info?.releaseDate
    ? movieDetails.info.releaseDate
    : ''

  const streamUrl = type === 'movies' && movieDetails && activeSession
    ? buildStreamUrl(activeSession, 'movies', movieDetails.movie_data.stream_id, movieDetails.movie_data.container_extension)
    : ''

  // Series details season & episodes list
  const seasons = seriesDetails?.episodes ? Object.keys(seriesDetails.episodes).sort((a, b) => Number(a) - Number(b)) : []
  const episodesList = seriesDetails?.episodes && selectedSeason ? seriesDetails.episodes[selectedSeason] || [] : []

  const handleCopyLink = (url: string) => {
    navigator.clipboard.writeText(url)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  // Youtube trailer URL formatting
  const trailerEmbedUrl = useMemo(() => {
    const trailerId = info?.youtube_trailer
    if (!trailerId) return ''
    
    // Check if it's already a full link or just the video ID
    if (trailerId.includes('youtube.com') || trailerId.includes('youtu.be')) {
      const match = trailerId.match(/(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))([^&?\n]+)/)
      return match ? `https://www.youtube.com/embed/${match[1]}` : trailerId
    }
    return `https://www.youtube.com/embed/${trailerId}`
  }, [info?.youtube_trailer])

  if (isLoading) {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
        <div className="spinner"></div>
        <p style={{ color: 'var(--text-secondary)' }}>Loading catalog details...</p>
      </div>
    )
  }

  if (error || !details) {
    return (
      <div style={{ padding: '2rem', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '80vh', gap: '1.5rem' }}>
        <AlertCircle size={48} color="#ef4444" />
        <div style={{ textAlign: 'center' }}>
          <h2 style={{ fontSize: '1.5rem', color: '#fff', marginBottom: '0.5rem' }}>Error Loading Details</h2>
          <p style={{ color: 'var(--text-secondary)', maxWidth: '400px' }}>{error || 'Unable to load catalog item metadata.'}</p>
        </div>
        <button className="btn btn-secondary" onClick={() => navigate(-1)}>
          <ArrowLeft size={16} />
          <span>Go Back</span>
        </button>
      </div>
    )
  }

  return (
    <div style={{ position: 'relative', minHeight: '100%', paddingBottom: '3rem' }}>
      
      {/* Blurred Backdrop image */}
      {backdrop && (
        <div style={{
          position: 'absolute',
          top: '-2rem',
          left: '-2rem',
          right: '-2rem',
          height: '550px',
          backgroundImage: `url(${backdrop})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center',
          filter: 'blur(30px) brightness(0.22)',
          zIndex: 0,
          pointerEvents: 'none',
          maskImage: 'linear-gradient(to bottom, black 50%, transparent)',
          WebkitMaskImage: 'linear-gradient(to bottom, black 50%, transparent)',
        }} />
      )}

      {/* Details layout */}
      <div style={{ position: 'relative', zIndex: 1, display: 'flex', flexDirection: 'column', gap: '2.5rem' }}>
        
        {/* Navigation & Header Actions */}
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <button className="btn btn-secondary" onClick={() => navigate(-1)} style={{ padding: '0.5rem 1rem' }}>
            <ArrowLeft size={16} />
            <span>Back</span>
          </button>
          
          <button
            className={`btn ${isFav ? 'btn-danger' : 'btn-secondary'}`}
            onClick={() => toggleFavorite(type === 'movies' ? 'movies' : 'series', id!)}
          >
            <Star size={16} fill={isFav ? '#fff' : 'transparent'} />
            <span>{isFav ? 'Remove Favorite' : 'Add Favorite'}</span>
          </button>
        </div>

        {/* Main Details Panel */}
        <div style={{ display: 'grid', gridTemplateColumns: '300px 1fr', gap: '3rem' }} className="details-main-grid">
          
          {/* Cover Art */}
          <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <div className="glass-panel" style={{ borderRadius: '12px', overflow: 'hidden', border: '1px solid var(--border-light)', boxShadow: 'var(--shadow-lg)' }}>
              {poster ? (
                <img src={poster} alt={title} style={{ width: '100%', height: 'auto', display: 'block', aspectRatio: '2/3', objectFit: 'cover' }} />
              ) : (
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', aspectRatio: '2/3', background: 'var(--bg-card)', color: 'var(--text-muted)' }}>
                  <Film size={60} />
                </div>
              )}
            </div>
            
            {/* Quick action triggers */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
              {type === 'movies' && streamUrl && (
                <button className="btn glow-btn" onClick={() => navigate(`/player/movies/${id}`)} style={{ width: '100%', padding: '0.8rem' }}>
                  <Play size={18} fill="#fff" />
                  <span>Play Movie</span>
                </button>
              )}
              {trailerEmbedUrl && (
                <button className="btn btn-secondary" onClick={() => setShowTrailer(true)} style={{ width: '100%' }}>
                  <Film size={16} />
                  <span>Watch Trailer</span>
                </button>
              )}
              <button className="btn btn-secondary" onClick={() => setShowLinks(true)} style={{ width: '100%' }}>
                <ExternalLink size={16} />
                <span>Stream & Download Link</span>
              </button>
            </div>
          </div>

          {/* Metadata details */}
          <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem', color: '#fff' }}>
            <div>
              <h1 style={{ fontSize: '2.5rem', fontWeight: 800, lineHeight: 1.1, marginBottom: '0.5rem' }}>{title}</h1>
              
              <div style={{ display: 'flex', flexWrap: 'wrap', gap: '1rem', alignItems: 'center', fontSize: '0.9rem', color: 'var(--text-secondary)', marginTop: '0.5rem' }}>
                <span style={{ color: 'var(--accent-secondary)', fontWeight: 600 }}>{type === 'movies' ? 'Movie VOD' : 'TV Series'}</span>
                
                {releaseDate && (
                  <span style={{ display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                    <Calendar size={14} />
                    {releaseDate}
                  </span>
                )}
                {duration && (
                  <span style={{ display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                    <Clock size={14} />
                    {duration}
                  </span>
                )}
              </div>
            </div>

            <div style={{ height: '1px', background: 'var(--border-light)' }} />

            {/* Synopsis */}
            <div>
              <h3 style={{ fontSize: '1.1rem', fontWeight: 700, marginBottom: '0.5rem', color: '#fff' }}>Synopsis</h3>
              <p style={{ color: 'var(--text-secondary)', lineHeight: 1.6, fontSize: '0.95rem' }}>{plot}</p>
            </div>

            {/* Additional info grid */}
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1.5rem', fontSize: '0.9rem' }}>
              <div>
                <span style={{ color: 'var(--text-muted)', display: 'block', marginBottom: '0.2rem' }}>Director</span>
                <span style={{ color: 'var(--text-primary)', fontWeight: 500 }}>{director}</span>
              </div>
              <div>
                <span style={{ color: 'var(--text-muted)', display: 'block', marginBottom: '0.2rem' }}>Genre</span>
                <span style={{ color: 'var(--text-primary)', fontWeight: 500 }}>{genre}</span>
              </div>
            </div>

            <div>
              <span style={{ color: 'var(--text-muted)', display: 'block', marginBottom: '0.2rem', fontSize: '0.9rem' }}>Cast</span>
              <p style={{ color: 'var(--text-primary)', fontSize: '0.9rem', lineHeight: '1.5' }}>{cast}</p>
            </div>

          </div>
        </div>

        {/* Series Episodes list */}
        {type === 'series' && seriesDetails && (
          <div style={{ marginTop: '2rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '1px solid var(--border-light)', paddingBottom: '1rem', marginBottom: '1.5rem' }}>
              <h2 style={{ fontSize: '1.4rem', color: '#fff', fontWeight: 700 }}>Seasons & Episodes</h2>
              
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                <span style={{ fontSize: '0.9rem', color: 'var(--text-secondary)' }}>Season:</span>
                <select
                  className="form-input"
                  style={{ width: '140px', padding: '0.5rem 2rem 0.5rem 0.75rem', fontSize: '0.9rem' }}
                  value={selectedSeason}
                  onChange={(e) => setSelectedSeason(e.target.value)}
                >
                  {seasons.map((s) => (
                    <option key={s} value={s}>Season {s}</option>
                  ))}
                </select>
              </div>
            </div>

            {episodesList.length === 0 ? (
              <div style={{ textAlign: 'center', padding: '3rem 0', color: 'var(--text-secondary)', border: '1px dashed var(--border-light)', borderRadius: '12px' }}>
                No episodes found for this season.
              </div>
            ) : (
              <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: '1.5rem' }}>
                {episodesList.map((ep) => {
                  const epUrl = buildStreamUrl(activeSession!, 'series', ep.id, ep.container_extension)

                  return (
                    <div 
                      key={ep.id} 
                      className="glass-panel" 
                      style={{
                        padding: '1.25rem',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'space-between',
                        gap: '1rem',
                        transition: 'var(--transition-smooth)',
                      }}
                      onMouseEnter={(e) => { e.currentTarget.style.borderColor = 'var(--accent-color)' }}
                      onMouseLeave={(e) => { e.currentTarget.style.borderColor = 'var(--border-light)' }}
                    >
                      <div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', gap: '0.5rem', marginBottom: '0.4rem' }}>
                          <span style={{ fontSize: '0.75rem', fontWeight: 600, color: 'var(--accent-secondary)', textTransform: 'uppercase' }}>
                            Episode {ep.episode_num}
                          </span>
                          {ep.info?.duration && (
                            <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                              {ep.info.duration}
                            </span>
                          )}
                        </div>
                        <h4 style={{ fontSize: '0.95rem', fontWeight: 700, color: '#fff', marginBottom: '0.5rem' }}>
                          {ep.title || `Episode ${ep.episode_num}`}
                        </h4>
                        <p style={{ fontSize: '0.8rem', color: 'var(--text-secondary)', lineHeight: 1.4, display: '-webkit-box', WebkitLineClamp: 3, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                          {ep.info?.plot || 'No synopsis details.'}
                        </p>
                      </div>

                      {/* Episode play or download links */}
                      <div style={{ display: 'flex', gap: '0.5rem', borderTop: '1px solid var(--border-light)', paddingTop: '0.75rem' }}>
                        <button 
                          className="btn glow-btn" 
                          style={{ padding: '0.4rem 0.8rem', fontSize: '0.8rem', flex: 1 }}
                          onClick={() => navigate(`/player/series/${ep.id}?season=${selectedSeason}&name=${encodeURIComponent(title)}&ep=${ep.episode_num}`)}
                        >
                          <Play size={12} fill="#fff" />
                          Play
                        </button>
                        <button 
                          className="btn btn-secondary" 
                          style={{ padding: '0.4rem 0.8rem', fontSize: '0.8rem' }}
                          title="Stream URL & Download details"
                          onClick={() => {
                            setActiveEpisode(ep)
                            setShowLinks(true)
                          }}
                        >
                          <ExternalLink size={12} />
                        </button>
                      </div>
                    </div>
                  )
                })}
              </div>
            )}
          </div>
        )}

      </div>

      {/* Youtube Trailer Modal Overlay */}
      {showTrailer && trailerEmbedUrl && (
        <div style={{
          position: 'fixed',
          inset: 0,
          background: 'rgba(0,0,0,0.85)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          zIndex: 999,
        }}>
          <div className="glass-panel" style={{ width: '90%', maxWidth: '850px', position: 'relative', overflow: 'hidden' }}>
            <button 
              onClick={() => setShowTrailer(false)}
              style={{
                position: 'absolute',
                top: '10px',
                right: '10px',
                background: 'rgba(0,0,0,0.5)',
                border: 'none',
                borderRadius: '50%',
                width: '32px',
                height: '32px',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                color: '#fff',
                cursor: 'pointer',
                zIndex: 10,
              }}
            >
              <X size={18} />
            </button>
            <div style={{ position: 'relative', paddingBottom: '56.25%', height: 0 }}>
              <iframe
                src={trailerEmbedUrl}
                title="Trailer Player"
                frameBorder="0"
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowFullScreen
                style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%' }}
              />
            </div>
          </div>
        </div>
      )}

      {/* Stream Links Modal Overlay */}
      {showLinks && (
        <div style={{
          position: 'fixed',
          inset: 0,
          background: 'rgba(0,0,0,0.85)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          zIndex: 999,
        }}>
          <div className="glass-panel" style={{ width: '90%', maxWidth: '520px', padding: '2rem', position: 'relative' }}>
            <button 
              onClick={() => {
                setShowLinks(false)
                setActiveEpisode(null)
              }}
              style={{
                position: 'absolute',
                top: '15px',
                right: '15px',
                background: 'transparent',
                border: 'none',
                color: 'var(--text-secondary)',
                cursor: 'pointer',
              }}
            >
              <X size={20} />
            </button>
            
            <h3 style={{ fontSize: '1.25rem', color: '#fff', marginBottom: '1rem' }}>
              {activeEpisode ? 'Episode Stream Information' : 'Movie Stream Information'}
            </h3>
            
            <div style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem' }}>
              <p style={{ fontSize: '0.85rem', color: 'var(--text-secondary)' }}>
                This direct media link can be copied and opened in external players like <strong>VLC</strong>, <strong>PotPlayer</strong>, or downloaded via your web browser.
              </p>
              
              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Direct Stream URL</label>
                <div style={{ display: 'flex', gap: '0.5rem' }}>
                  <input
                    type="text"
                    readOnly
                    className="form-input"
                    value={activeEpisode && activeSession
                      ? buildStreamUrl(activeSession, 'series', activeEpisode.id, activeEpisode.container_extension)
                      : streamUrl
                    }
                  />
                  <button 
                    className="btn glow-btn"
                    style={{ flexShrink: 0, padding: '0.75rem' }}
                    onClick={() => handleCopyLink(
                      activeEpisode && activeSession
                        ? buildStreamUrl(activeSession, 'series', activeEpisode.id, activeEpisode.container_extension)
                        : streamUrl
                    )}
                  >
                    {copied ? <Check size={16} /> : <Copy size={16} />}
                  </button>
                </div>
              </div>

              <div style={{ display: 'flex', gap: '1rem', marginTop: '0.5rem' }}>
                <a
                  href={activeEpisode && activeSession
                    ? buildStreamUrl(activeSession, 'series', activeEpisode.id, activeEpisode.container_extension)
                    : streamUrl
                  }
                  download
                  target="_blank"
                  rel="noreferrer"
                  className="btn btn-secondary"
                  style={{ flex: 1, textDecoration: 'none' }}
                >
                  <Film size={16} />
                  <span>Download File</span>
                </a>
              </div>
            </div>
          </div>
        </div>
      )}

      <style>{`
        @media (max-width: 768px) {
          .details-main-grid {
            grid-template-columns: 1fr !important;
            gap: 1.5rem !important;
          }
        }
      `}</style>

    </div>
  )
}
