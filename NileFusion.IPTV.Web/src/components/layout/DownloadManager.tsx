import React from 'react';
import { useDownloads } from '../../app/DownloadContext';
import { X, Trash2, CheckCircle, AlertTriangle, Play, Loader, Download } from 'lucide-react';

interface DownloadManagerProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function DownloadManager({ isOpen, onClose }: DownloadManagerProps) {
  const { activeDownloads, downloadHistory, cancelDownload, clearHistory } = useDownloads();

  const activeList = Object.values(activeDownloads);

  if (!isOpen) return null;

  const formatSize = (bytes: number) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  };

  return (
    <div style={{
      position: 'fixed',
      inset: 0,
      background: 'rgba(0, 0, 0, 0.6)',
      backdropFilter: 'blur(4px)',
      zIndex: 1000,
      display: 'flex',
      justifyContent: 'flex-end',
      animation: 'fadeIn 0.2s ease-out',
    }} onClick={onClose}>
      
      <div 
        className="glass-panel" 
        style={{
          width: '100%',
          maxWidth: '420px',
          height: '100%',
          display: 'flex',
          flexDirection: 'column',
          padding: '2rem 1.5rem',
          borderRadius: 0,
          borderLeft: '1px solid var(--border-light)',
          borderRight: 'none',
          borderTop: 'none',
          borderBottom: 'none',
          animation: 'slideLeft 0.3s cubic-bezier(0.16, 1, 0.3, 1)',
        }}
        onClick={(e) => e.stopPropagation()}
      >
        {/* Header */}
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
          <h2 style={{ fontSize: '1.25rem', fontWeight: 800, color: '#fff', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            <Download size={20} className="blink-cursor" />
            Download Manager
          </h2>
          <button 
            onClick={onClose}
            className="btn-secondary"
            style={{ padding: '0.4rem', borderRadius: '50%', background: 'rgba(255,255,255,0.05)', color: '#fff' }}
          >
            <X size={18} />
          </button>
        </div>

        {/* Inner Content scroll area */}
        <div style={{ flex: 1, overflowY: 'auto', display: 'flex', flexDirection: 'column', gap: '2rem' }}>
          
          {/* Active Downloads Section */}
          <div>
            <h3 style={{ fontSize: '0.9rem', fontWeight: 700, color: 'var(--text-secondary)', marginBottom: '1rem', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
              Active Queue ({activeList.length})
            </h3>

            {activeList.length === 0 ? (
              <div style={{ padding: '1.5rem', textAlign: 'center', border: '1px dashed var(--border-light)', borderRadius: '8px', color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                No active downloads.
              </div>
            ) : (
              <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                {activeList.map((task) => (
                  <div 
                    key={task.id} 
                    className="glass-panel" 
                    style={{ padding: '1rem', background: 'rgba(255,255,255,0.02)', borderColor: 'rgba(255,255,255,0.05)', position: 'relative', overflow: 'hidden' }}
                  >
                    {/* Tiny top bar info */}
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', gap: '1rem', marginBottom: '0.5rem' }}>
                      <div style={{ overflow: 'hidden' }}>
                        <div style={{ fontSize: '0.9rem', fontWeight: 600, color: '#fff', textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }}>
                          {task.name}
                        </div>
                        <div style={{ fontSize: '0.75rem', color: 'var(--text-secondary)', display: 'flex', gap: '0.5rem', marginTop: '0.2rem' }}>
                          <span>{task.speed}</span>
                          <span>•</span>
                          <span>{formatSize(task.loaded)} / {task.total > 0 ? formatSize(task.total) : 'Unknown'}</span>
                        </div>
                      </div>
                      
                      <button 
                        onClick={() => cancelDownload(task.id)}
                        className="btn-danger" 
                        style={{ padding: '0.25rem 0.5rem', fontSize: '0.75rem', height: '24px', borderRadius: '4px' }}
                      >
                        Cancel
                      </button>
                    </div>

                    {/* Progress Bar container */}
                    <div style={{ width: '100%', height: '6px', background: 'rgba(255,255,255,0.05)', borderRadius: '3px', overflow: 'hidden', marginTop: '0.75rem' }}>
                      <div style={{
                        width: `${task.progress}%`,
                        height: '100%',
                        background: 'var(--primary-gradient)',
                        borderRadius: '3px',
                        transition: 'width 0.2s ease',
                      }} />
                    </div>

                    <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.72rem', color: 'var(--text-muted)', marginTop: '0.4rem' }}>
                      <span>Status: Downloading</span>
                      <span>{task.progress}%</span>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* History Section */}
          <div style={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
              <h3 style={{ fontSize: '0.9rem', fontWeight: 700, color: 'var(--text-secondary)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
                Download History
              </h3>
              {downloadHistory.length > 0 && (
                <button 
                  onClick={clearHistory}
                  className="nilefusion-credit"
                  style={{ background: 'transparent', border: 'none', padding: '0.25rem 0.5rem', fontSize: '0.75rem', color: '#ef4444', display: 'flex', alignItems: 'center', gap: '0.25rem', cursor: 'pointer' }}
                >
                  <Trash2 size={12} />
                  Clear All
                </button>
              )}
            </div>

            {downloadHistory.length === 0 ? (
              <div style={{ padding: '1.5rem', textAlign: 'center', border: '1px dashed var(--border-light)', borderRadius: '8px', color: 'var(--text-muted)', fontSize: '0.85rem' }}>
                No download history.
              </div>
            ) : (
              <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem', overflowY: 'auto', flex: 1, maxHeight: '350px' }}>
                {downloadHistory.map((item, idx) => (
                  <div 
                    key={idx}
                    style={{
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'space-between',
                      padding: '0.75rem 1rem',
                      background: 'rgba(255,255,255,0.01)',
                      border: '1px solid var(--border-light)',
                      borderRadius: '8px',
                      fontSize: '0.85rem'
                    }}
                  >
                    <div style={{ overflow: 'hidden', paddingRight: '0.5rem' }}>
                      <div style={{ fontWeight: 600, color: '#fff', textOverflow: 'ellipsis', overflow: 'hidden', whiteSpace: 'nowrap' }} title={item.name}>
                        {item.name}
                      </div>
                      <div style={{ fontSize: '0.72rem', color: 'var(--text-muted)', marginTop: '0.2rem' }}>
                        {item.size > 0 ? `${formatSize(item.size)} • ` : ''}
                        {new Date(item.timestamp).toLocaleString(undefined, { dateStyle: 'short', timeStyle: 'short' })}
                      </div>
                    </div>

                    <div style={{ flexShrink: 0 }}>
                      {item.status === 'completed' ? (
                        <span title="Completed"><CheckCircle size={16} color="#10b981" /></span>
                      ) : (
                        <span title="Failed"><AlertTriangle size={16} color="#ef4444" /></span>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

        </div>

      </div>

      <style>{`
        @keyframes slideLeft {
          from { transform: translateX(100%); }
          to { transform: translateX(0); }
        }
      `}</style>
    </div>
  );
}
