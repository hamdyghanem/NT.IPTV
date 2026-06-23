import React, { createContext, useContext, useState, useEffect } from 'react';

export interface DownloadTask {
  id: string;
  name: string;
  progress: number;
  speed: string;
  loaded: number;
  total: number;
  status: 'downloading' | 'completed' | 'failed' | 'paused';
  controller: AbortController | null;
}

export interface HistoryItem {
  id: string;
  name: string;
  timestamp: number;
  size: number;
  status: 'completed' | 'failed';
}

interface DownloadContextType {
  activeDownloads: Record<string, DownloadTask>;
  downloadHistory: HistoryItem[];
  startDownload: (id: string, name: string, url: string, extension: string) => Promise<void>;
  cancelDownload: (id: string) => void;
  clearHistory: () => void;
}

const DownloadContext = createContext<DownloadContextType | undefined>(undefined);

export const DownloadProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [activeDownloads, setActiveDownloads] = useState<Record<string, DownloadTask>>({});
  const [downloadHistory, setDownloadHistory] = useState<HistoryItem[]>([]);

  useEffect(() => {
    const storedHistory = localStorage.getItem('nilefusion_download_history');
    if (storedHistory) {
      try {
        setDownloadHistory(JSON.parse(storedHistory));
      } catch (e) {
        console.error("Failed to parse download history", e);
      }
    }
  }, []);

  const saveHistory = (newHistory: HistoryItem[]) => {
    setDownloadHistory(newHistory);
    localStorage.setItem('nilefusion_download_history', JSON.stringify(newHistory));
  };

  const cancelDownload = (id: string) => {
    const task = activeDownloads[id];
    if (task && task.controller) {
      task.controller.abort();
    }
    setActiveDownloads((prev) => {
      const next = { ...prev };
      delete next[id];
      return next;
    });
  };

  const clearHistory = () => {
    saveHistory([]);
  };

  const startDownload = async (id: string, name: string, url: string, extension: string) => {
    // If already downloading, ignore
    if (activeDownloads[id]) return;

    const controller = new AbortController();
    const cleanExtension = extension ? extension.toLowerCase() : 'ts';
    const fileName = `${name.replace(/[^a-zA-Z0-9\s-_]/g, '')}.${cleanExtension}`;

    // Initialize the task
    const newTask: DownloadTask = {
      id,
      name,
      progress: 0,
      speed: '0 KB/s',
      loaded: 0,
      total: 0,
      status: 'downloading',
      controller,
    };

    setActiveDownloads((prev) => ({ ...prev, [id]: newTask }));

    let fileWritable: any = null;
    let fallbackChunks: BlobPart[] = [];
    const isFileSystemAccessSupported = 'showSaveFilePicker' in window;

    try {
      if (isFileSystemAccessSupported) {
        // Trigger showSaveFilePicker immediately (must be synchronous with user click)
        const fileHandle = await (window as any).showSaveFilePicker({
          suggestedName: fileName,
          types: [
            {
              description: 'Video File',
              accept: {
                [`video/${cleanExtension === 'mkv' ? 'x-matroska' : cleanExtension === 'mp4' ? 'mp4' : 'mp2t'}`]: [`.${cleanExtension}`],
              },
            },
          ],
        });
        fileWritable = await fileHandle.createWritable();
      }
    } catch (err: any) {
      // User cancelled picker or error occurred
      console.warn("File picker cancelled or failed, aborting download:", err);
      setActiveDownloads((prev) => {
        const next = { ...prev };
        delete next[id];
        return next;
      });
      return;
    }

    try {
      const response = await fetch(url, { signal: controller.signal });
      if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
      if (!response.body) throw new Error("Response body is not readable");

      const reader = response.body.getReader();
      const contentLength = Number(response.headers.get('content-length')) || 0;

      setActiveDownloads((prev) => {
        if (!prev[id]) return prev;
        return {
          ...prev,
          [id]: { ...prev[id], total: contentLength },
        };
      });

      let loadedBytes = 0;
      let lastLoadedBytes = 0;
      let lastTime = Date.now();

      // Speed calculation interval (every 1s)
      const speedInterval = setInterval(() => {
        const now = Date.now();
        const duration = (now - lastTime) / 1000;
        if (duration <= 0) return;

        const bytesSent = loadedBytes - lastLoadedBytes;
        const speedBps = bytesSent / duration;

        let speedStr = '0 KB/s';
        if (speedBps > 1024 * 1024) {
          speedStr = `${(speedBps / (1024 * 1024)).toFixed(1)} MB/s`;
        } else if (speedBps > 1024) {
          speedStr = `${(speedBps / 1024).toFixed(0)} KB/s`;
        } else {
          speedStr = `${speedBps.toFixed(0)} B/s`;
        }

        lastLoadedBytes = loadedBytes;
        lastTime = now;

        setActiveDownloads((prev) => {
          if (!prev[id]) return prev;
          return {
            ...prev,
            [id]: { ...prev[id], speed: speedStr },
          };
        });
      }, 1000);

      try {
        while (true) {
          const { done, value } = await reader.read();
          if (done) break;

          if (fileWritable) {
            await fileWritable.write(value);
          } else {
            fallbackChunks.push(value);
          }

          loadedBytes += value.length;
          const progress = contentLength > 0 ? Math.round((loadedBytes / contentLength) * 100) : 0;

          setActiveDownloads((prev) => {
            if (!prev[id]) return prev;
            return {
              ...prev,
              [id]: { ...prev[id], progress, loaded: loadedBytes },
            };
          });
        }

        // Close writable file stream if it was used
        if (fileWritable) {
          await fileWritable.close();
        } else {
          // Trigger memory fallback download trigger
          const blob = new Blob(fallbackChunks, { type: `video/${cleanExtension === 'mkv' ? 'x-matroska' : cleanExtension === 'mp4' ? 'mp4' : 'mp2t'}` });
          const downloadUrl = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = downloadUrl;
          a.download = fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          URL.revokeObjectURL(downloadUrl);
        }

        clearInterval(speedInterval);

        // Save to History
        const historyItem: HistoryItem = {
          id,
          name,
          timestamp: Date.now(),
          size: loadedBytes,
          status: 'completed',
        };
        saveHistory([historyItem, ...downloadHistory]);

        // Remove from active downloads
        setActiveDownloads((prev) => {
          const next = { ...prev };
          delete next[id];
          return next;
        });

      } catch (streamErr) {
        clearInterval(speedInterval);
        if (fileWritable) {
          try { await fileWritable.abort(); } catch {}
        }
        throw streamErr;
      }

    } catch (err: any) {
      if (err.name === 'AbortError') {
        console.log(`Download of ${name} was cancelled.`);
        return;
      }
      console.error(`Download failed: ${err.message}`);

      // Add failed to history
      const historyItem: HistoryItem = {
        id,
        name,
        timestamp: Date.now(),
        size: 0,
        status: 'failed',
      };
      saveHistory([historyItem, ...downloadHistory]);

      // Remove from active downloads
      setActiveDownloads((prev) => {
        const next = { ...prev };
        delete next[id];
        return next;
      });
    }
  };

  return (
    <DownloadContext.Provider value={{ activeDownloads, downloadHistory, startDownload, cancelDownload, clearHistory }}>
      {children}
    </DownloadContext.Provider>
  );
};

export const useDownloads = () => {
  const context = useContext(DownloadContext);
  if (context === undefined) {
    throw new Error('useDownloads must be used within a DownloadProvider');
  }
  return context;
};
