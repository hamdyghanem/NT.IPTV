import React, { useState, useEffect, useMemo, useRef } from 'react'
import { useSearchParams, useNavigate } from 'react-router-dom'
import { useAuth } from '../app/AuthContext'
import { fetchCategories, fetchStreams, getCacheItem, setCacheItem } from '../services/api'
import { StreamCategory } from '../types'
import { Search, Star, Tv, Film, MonitorPlay, Play, ArrowUpDown, AlertCircle, RefreshCw } from 'lucide-react'

type TabType = 'live' | 'movies' | 'series'

export default function BrowsePage() {
  const { activeSession, toggleFavorite, isFavorite } = useAuth()
  const [searchParams] = useSearchParams()
  const navigate = useNavigate()

  const activeTab = (searchParams.get('type') as TabType) || 'live'
  const [categories, setCategories] = useState<StreamCategory[]>([])
  const [selectedCategory, setSelectedCategory] = useState<string>('all')
  const [searchQuery, setSearchQuery] = useState('')
  const [categorySearchQuery, setCategorySearchQuery] = useState('')
  const [sortBy, setSortBy] = useState('name-asc')
  
  const [streams, setStreams] = useState<any[]>([])
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  // Infinite Scroll state
  const [visibleCount, setVisibleCount] = useState(60)

  // Reset category and search on tab switch
  useEffect(() => {
    setSelectedCategory('-1')
    setSearchQuery('')
    setCategorySearchQuery('')
    setVisibleCount(60)
    
    if (activeSession) {
      loadCatalogData()
    }
  }, [activeTab, activeSession])

  // Reset infinite scroll whenever filters change
  useEffect(() => {
    setVisibleCount(60)
  }, [selectedCategory, searchQuery, sortBy])

  // Scroll listener for infinite loading
  useEffect(() => {
    const handleScroll = () => {
      if (window.innerHeight + window.scrollY >= document.documentElement.scrollHeight - 300) {
        setVisibleCount((prev) => prev + 60)
      }
    }
    window.addEventListener('scroll', handleScroll)
    return () => window.removeEventListener('scroll', handleScroll)
  }, [])

  const loadCatalogData = async (forceUpdate = false) => {
    if (!activeSession) return
    setIsLoading(true)
    setError(null)
    
    const userKey = `${activeSession.username}_${activeSession.server}`.replace(/[^a-zA-Z0-9]/g, '_')
    const categoriesCacheKey = `categories_${userKey}_${activeTab}`
    const streamsCacheKey = `streams_${userKey}_${activeTab}`
    
    try {
      let catData: StreamCategory[] | null = null
      let streamData: any[] | null = null

      if (!forceUpdate) {
        catData = await getCacheItem<StreamCategory[]>(categoriesCacheKey)
        streamData = await getCacheItem<any[]>(streamsCacheKey)
      }

      if (!catData || !streamData) {
        catData = await fetchCategories(activeSession, activeTab)
        streamData = await fetchStreams(activeSession, activeTab)

        await setCacheItem(categoriesCacheKey, catData)
        await setCacheItem(streamsCacheKey, streamData)
      }

      setCategories(catData)
      setStreams(streamData)

      // Default category selection: Favorites for all tabs
      setSelectedCategory('-1')
    } catch (err: any) {
      console.error("Failed to load catalog data", err)
      setError(`Failed to fetch catalog. Please check your CORS configuration or proxy server.`)
    } finally {
      setIsLoading(false)
    }
  }

  // Filter and Sort streams
  const filteredStreams = useMemo(() => {
    let result = [...streams]

    // If a search query is present, perform a global/public search across all categories
    if (searchQuery) {
      const q = searchQuery.toLowerCase()
      result = result.filter(item => (item.name || item.title || '').toLowerCase().includes(q))
    } else {
      // Otherwise, filter by the selected category
      if (selectedCategory === '-1') {
        result = result.filter(item => isFavorite(activeTab, item.stream_id || item.series_id))
      } else if (selectedCategory !== 'all') {
        result = result.filter(item => String(item.category_id) === String(selectedCategory))
      }
    }

    // Sorting
    result.sort((a, b) => {
      const nameA = (a.name || a.title || '').toLowerCase()
      const nameB = (b.name || b.title || '').toLowerCase()
      
      switch (sortBy) {
        case 'name-asc':
          return nameA.localeCompare(nameB)
        case 'name-desc':
          return nameB.localeCompare(nameA)
        case 'rating-desc':
          const ratingA = parseFloat(a.rating || a.rating_5based || '0')
          const ratingB = parseFloat(b.rating || b.rating_5based || '0')
          return ratingB - ratingA
        case 'added-desc':
          const addedA = parseInt(a.added || a.last_modified || '0')
          const addedB = parseInt(b.added || b.last_modified || '0')
          return addedB - addedA
        default:
          return 0
      }
    })

    return result
  }, [streams, selectedCategory, searchQuery, sortBy, activeTab, isFavorite])

  // Count items inside categories for badges
  const categoryCounts = useMemo(() => {
    const counts: Record<string, number> = {
      all: streams.length,
      '-1': streams.filter(s => isFavorite(activeTab, s.stream_id || s.series_id)).length
    }

    streams.forEach(s => {
      const catId = String(s.category_id)
      counts[catId] = (counts[catId] || 0) + 1
    })

    return counts;
  }, [streams, activeTab, isFavorite])

  const handleCardClick = (item: any) => {
    const id = item.stream_id || item.series_id
    if (activeTab === 'live') {
      // Live channels play directly
      navigate(`/player/live/${id}`)
    } else {
      // VOD / Series open details
      navigate(`/details/${activeTab}/${id}`)
    }
  }


  const activeCategoryName = useMemo(() => {
    if (searchQuery) return 'Search Results'
    if (selectedCategory === 'all') return 'All Channels'
    if (selectedCategory === '-1') return 'Favorites'
    return categories.find(c => String(c.category_id) === String(selectedCategory))?.category_name || 'Category'
  }, [selectedCategory, categories, searchQuery])

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem', height: '100%' }}>
      

      {/* Main Browse Panel */}
      <div style={{ display: 'grid', gridTemplateColumns: '260px 1fr', gap: '2rem', alignItems: 'start' }} className="browse-layout">
        
        {/* Categories Sidebar */}
        <aside className="glass-panel" style={{
          padding: '1.25rem',
          display: 'flex',
          flexDirection: 'column',
          gap: '1rem',
          maxHeight: 'calc(100vh - 200px)',
          position: 'sticky',
          top: '20px',
          overflowY: 'auto'
        }}>
          <h3 style={{ fontSize: '1rem', color: '#fff', fontWeight: 600 }}>Categories</h3>
          <div style={{ position: 'relative' }}>
            <Search size={14} style={{ position: 'absolute', left: '10px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
            <input
              type="text"
              placeholder="Filter categories..."
              className="form-input"
              style={{
                padding: '0.45rem 0.75rem 0.45rem 2rem',
                fontSize: '0.8rem',
                borderRadius: '6px',
                height: '32px'
              }}
              value={categorySearchQuery}
              onChange={(e) => setCategorySearchQuery(e.target.value)}
            />
          </div>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '0.3rem' }}>
            {/* Special Categories — "All Streams" intentionally hidden for all tabs */}
            
            <button
              onClick={() => setSelectedCategory('-1')}
              style={{
                justifyContent: 'space-between',
                padding: '0.6rem 0.8rem',
                borderRadius: '6px',
                fontSize: '0.85rem',
                textAlign: 'left',
                width: '100%',
                background: selectedCategory === '-1' ? 'rgba(239, 68, 68, 0.1)' : 'transparent',
                color: selectedCategory === '-1' ? '#f87171' : 'var(--text-secondary)',
                borderLeft: selectedCategory === '-1' ? '3px solid #ef4444' : '3px solid transparent',
              }}
            >
              <span style={{ display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                <Star size={14} fill={selectedCategory === '-1' ? '#ef4444' : 'transparent'} />
                Favorites
              </span>
              <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>({categoryCounts['-1']})</span>
            </button>

            <div style={{ height: '1px', background: 'var(--border-light)', margin: '0.5rem 0' }} />

            {/* Dyn Categories */}
            {categories
              .filter(cat => cat.category_name.toLowerCase().includes(categorySearchQuery.toLowerCase()))
              .map((cat) => {
                const count = categoryCounts[String(cat.category_id)] || 0
              if (count === 0) return null // Hide empty categories
              
              return (
                <button
                  key={cat.category_id}
                  onClick={() => setSelectedCategory(cat.category_id)}
                  style={{
                    justifyContent: 'space-between',
                    padding: '0.6rem 0.8rem',
                    borderRadius: '6px',
                    fontSize: '0.85rem',
                    textAlign: 'left',
                    width: '100%',
                    background: selectedCategory === cat.category_id ? 'var(--bg-hover)' : 'transparent',
                    color: selectedCategory === cat.category_id ? '#fff' : 'var(--text-secondary)',
                    borderLeft: selectedCategory === cat.category_id ? '3px solid var(--accent-color)' : '3px solid transparent',
                  }}
                >
                  <span style={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap', marginRight: '0.5rem' }}>
                    {cat.category_name}
                  </span>
                  <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)', flexShrink: 0 }}>
                    ({count})
                  </span>
                </button>
              )
            })}
          </div>
        </aside>

        {/* Catalog Items Grid */}
        <section style={{ display: 'flex', flexDirection: 'column', gap: '1.25rem', minWidth: 0 }}>
          
          {/* Filters Bar */}
          <div className="glass-panel" style={{ padding: '1rem', display: 'flex', gap: '1rem', flexWrap: 'wrap', alignItems: 'center' }}>
            <div style={{ position: 'relative', flex: 1, minWidth: '200px' }}>
              <Search size={16} style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
              <input
                type="text"
                placeholder={`Search ${activeTab}...`}
                className="form-input"
                style={{ paddingLeft: '2.5rem' }}
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
            
            <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', flexShrink: 0 }}>
              <ArrowUpDown size={16} style={{ color: 'var(--text-secondary)' }} />
              <select
                className="form-input"
                style={{ padding: '0.75rem 2rem 0.75rem 1rem', width: '180px' }}
                value={sortBy}
                onChange={(e) => setSortBy(e.target.value)}
              >
                <option value="name-asc">Name (A-Z)</option>
                <option value="name-desc">Name (Z-A)</option>
                {activeTab !== 'live' && <option value="rating-desc">Top Rated</option>}
                {activeTab !== 'live' && <option value="added-desc">Recently Added</option>}
              </select>
            </div>

            <button
              className="btn btn-secondary"
              onClick={() => loadCatalogData(true)}
              title="Force update catalog streams"
              style={{
                display: 'flex',
                alignItems: 'center',
                gap: '0.5rem',
                padding: '0.75rem 1.25rem',
                flexShrink: 0,
                cursor: 'pointer',
              }}
            >
              <RefreshCw 
                size={15} 
                style={{ 
                  animation: isLoading ? 'spin 1s linear infinite' : 'none' 
                }} 
              />
              <span>Force Refresh</span>
            </button>
          </div>

          <h2 style={{ fontSize: '1.25rem', color: '#fff', fontWeight: 600, marginTop: '0.5rem' }}>
            {activeCategoryName} <span style={{ color: 'var(--text-muted)', fontWeight: 400, fontSize: '0.9rem' }}>({filteredStreams.length} items)</span>
          </h2>

          {/* Loader or Error */}
          {isLoading ? (
            <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', justifyContent: 'center', alignItems: 'center', padding: '5rem 0' }}>
              <div className="spinner"></div>
              <p style={{ color: 'var(--text-secondary)' }}>Loading catalog streams...</p>
            </div>
          ) : error ? (
            <div style={{
              display: 'flex',
              alignItems: 'center',
              gap: '0.75rem',
              background: 'rgba(239, 68, 68, 0.1)',
              border: '1px solid rgba(239, 68, 68, 0.2)',
              borderRadius: '8px',
              padding: '1.5rem',
              color: '#f87171',
              marginTop: '1rem'
            }}>
              <AlertCircle size={24} style={{ flexShrink: 0 }} />
              <div>
                <h4 style={{ fontWeight: 700, marginBottom: '0.25rem' }}>API Request Failed</h4>
                <p style={{ fontSize: '0.9rem', color: 'var(--text-secondary)' }}>{error}</p>
              </div>
            </div>
          ) : filteredStreams.length === 0 ? (
            <div style={{ textAlign: 'center', padding: '4rem 2rem', color: 'var(--text-secondary)', border: '1px dashed var(--border-light)', borderRadius: '12px' }}>
              {selectedCategory === '-1' ? (
                <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '0.5rem' }}>
                  <Star size={36} style={{ color: 'var(--text-muted)', marginBottom: '0.5rem' }} />
                  <h4 style={{ color: '#fff', fontWeight: 600 }}>Your Favorites List is Empty</h4>
                  <p style={{ fontSize: '0.85rem', maxWidth: '360px', margin: '0 auto', lineHeight: '1.5' }}>
                    Select a category from the sidebar to browse movies or series, and click the star icon on any card to add it to your favorites.
                  </p>
                </div>
              ) : (
                "No matches found in this category."
              )}
            </div>
          ) : (
            /* Stream Cards Grid */
            <div className="media-grid">
              {filteredStreams.slice(0, visibleCount).map((item) => {
                const id = item.stream_id || item.series_id
                const title = item.name || item.title
                const poster = item.stream_icon || item.cover
                const rating = item.rating || item.rating_5based
                const isFav = isFavorite(activeTab, id)

                return (
                  <div
                    key={id}
                    className="media-card"
                    onClick={() => handleCardClick(item)}
                  >
                    {/* Favorite Star Bubble */}
                    <button
                      className={`fav-btn-bubble ${isFav ? 'active' : ''}`}
                      onClick={(e) => {
                        e.stopPropagation()
                        toggleFavorite(activeTab, id)
                      }}
                    >
                      <Star size={20} fill={isFav ? '#fbbf24' : 'transparent'} />
                    </button>

                    {/* Image / Fallback Icon */}
                    {poster ? (
                      <img 
                        src={poster} 
                        alt={title} 
                        className="media-card-img" 
                        loading="lazy" 
                        onError={(e) => {
                          e.currentTarget.src = '' // Trigger fallback layout
                          e.currentTarget.style.display = 'none'
                        }}
                      />
                    ) : null}

                    {/* Fallback layout if image is missing */}
                    <div style={{
                      position: 'absolute',
                      inset: 0,
                      background: 'linear-gradient(135deg, #1f2937 0%, #111827 100%)',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      zIndex: -1,
                    }}>
                      <div style={{ color: 'var(--text-muted)', display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '0.5rem' }}>
                        {activeTab === 'live' && <Tv size={36} />}
                        {activeTab === 'movies' && <Film size={36} />}
                        {activeTab === 'series' && <MonitorPlay size={36} />}
                      </div>
                    </div>

                    {/* Meta Overlay */}
                    <div className="media-card-overlay">
                      <div className="media-card-title">{title}</div>
                      <div className="media-card-meta">
                        <span style={{ color: 'var(--accent-secondary)', fontWeight: 600 }}>
                          {activeTab === 'live' ? 'Channel' : item.container_extension ? item.container_extension.toUpperCase() : 'Series'}
                        </span>
                        {rating && rating !== '0' && (
                          <span style={{ display: 'flex', alignItems: 'center', gap: '2px', color: '#fbbf24' }}>
                            ★ {parseFloat(rating).toFixed(1)}
                          </span>
                        )}
                      </div>
                    </div>

                    {/* Hover Play Button */}
                    <div style={{
                      position: 'absolute',
                      inset: 0,
                      background: 'rgba(0,0,0,0.4)',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      opacity: 0,
                      transition: 'opacity 0.2s ease',
                      pointerEvents: 'none',
                    }} className="play-hover">
                      <div style={{
                        width: '48px',
                        height: '48px',
                        borderRadius: '50%',
                        background: 'var(--primary-gradient)',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        color: '#fff',
                        boxShadow: '0 4px 12px rgba(99, 102, 241, 0.4)'
                      }}>
                        <Play size={20} fill="#fff" />
                      </div>
                    </div>
                  </div>
                )
              })}
            </div>
          )}

          {/* Infinite Scroll Trigger */}
          {!isLoading && filteredStreams.length > visibleCount && (
            <div style={{ textAlign: 'center', padding: '2rem 0', color: 'var(--text-secondary)', fontSize: '0.85rem' }}>
              Scrolling down to load more content automatically...
            </div>
          )}

        </section>

      </div>

      <style>{`
        .media-card:hover .play-hover {
          opacity: 1 !important;
        }
        @media (max-width: 900px) {
          .browse-layout {
            grid-template-columns: 1fr !important;
          }
          .browse-layout aside {
            position: relative !important;
            top: 0 !important;
            max-height: 200px !important;
          }
        }
      `}</style>

    </div>
  )
}
