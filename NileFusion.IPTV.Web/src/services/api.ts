import { ApiSession, PlayerInfoResponse, StreamCategory, StreamChannel, StreamVideo, StreamSeries, WatchMovie, WatchSeries } from '../types';

const API_BASE = (import.meta as ImportMeta & { env?: Record<string, string> }).env?.VITE_API_BASE_URL || '';

export async function fetchJson<T>(input: string, init?: RequestInit): Promise<T> {
  let response: Response
  try {
    response = await fetch(input, {
      ...init,
      headers: {
        'Content-Type': 'application/json',
        ...(init?.headers || {}),
      },
    })
  } catch (networkErr: any) {
    throw new Error('Network error — could not reach the proxy server. Is the dev server running?')
  }

  if (!response.ok) {
    // Try to read a structured error from our proxy
    let detail = ''
    try {
      const body = await response.clone().json() as any
      detail = body?.detail || body?.error || ''
    } catch { /* ignore */ }

    if (response.status === 502) {
      throw new Error(
        detail
          ? `Cannot reach IPTV server: ${detail}`
          : 'Cannot reach IPTV server (502). Check the server address, port, and that the server is online.'
      )
    }
    if (response.status === 504) {
      throw new Error('IPTV server timed out. The server may be offline or the address is wrong.')
    }
    throw new Error(`Request failed: ${response.status}`)
  }

  return response.json() as Promise<T>
}

export function buildXtreamUrl(
  session: ApiSession,
  action?: string,
  extraParams: Record<string, string> = {}
): string {
  const protocol = session.useHttps ? 'https' : 'http';
  // Strip protocol from server name if user typed it in the server field
  const serverClean = session.server.replace(/^https?:\/\//i, '');
  const targetHost = `${serverClean}${session.port ? `:${session.port}` : ''}`;
  const baseUrl = `${protocol}://${targetHost}/player_api.php`;

  const params = new URLSearchParams({
    username: session.username,
    password: session.password,
    ...extraParams,
  });

  if (action) {
    params.append('action', action);
  }

  const directUrl = `${baseUrl}?${params.toString()}`;

  // If a custom proxy base URL is configured (for production), use it
  if (API_BASE) {
    if (API_BASE.includes('?')) {
      return `${API_BASE}&url=${encodeURIComponent(directUrl)}`;
    } else {
      return `${API_BASE}/${directUrl.replace(/^https?:\/\//i, '')}`;
    }
  }

  // In dev (and any local deploy), route through the built-in Vite CORS proxy
  return `/proxy?url=${encodeURIComponent(directUrl)}`;
}

export async function testConnection(session: ApiSession): Promise<PlayerInfoResponse> {
  // Xtream Codes login authentication is done by calling player_api.php with credentials but no action parameter
  const url = buildXtreamUrl(session);
  const data = await fetchJson<PlayerInfoResponse>(url);
  return data;
}

export async function fetchCategories(
  session: ApiSession,
  type: 'live' | 'movies' | 'series'
): Promise<StreamCategory[]> {
  let action = 'get_live_categories';
  if (type === 'movies') action = 'get_vod_categories';
  else if (type === 'series') action = 'get_series_categories';

  const url = buildXtreamUrl(session, action);
  return fetchJson<StreamCategory[]>(url);
}

export async function fetchStreams(
  session: ApiSession,
  type: 'live' | 'movies' | 'series',
  categoryId?: string
): Promise<any[]> {
  let action = 'get_live_streams';
  if (type === 'movies') action = 'get_vod_streams';
  else if (type === 'series') action = 'get_series';

  const params: Record<string, string> = {};
  if (categoryId && categoryId !== '-1') {
    params['category_id'] = categoryId;
  }

  const url = buildXtreamUrl(session, action, params);
  
  if (type === 'live') {
    return fetchJson<StreamChannel[]>(url);
  } else if (type === 'movies') {
    return fetchJson<StreamVideo[]>(url);
  } else {
    return fetchJson<StreamSeries[]>(url);
  }
}

export async function fetchMovieDetails(
  session: ApiSession,
  streamId: string | number
): Promise<WatchMovie> {
  const url = buildXtreamUrl(session, 'get_vod_info', { vod_id: String(streamId) });
  return fetchJson<WatchMovie>(url);
}

export async function fetchSeriesDetails(
  session: ApiSession,
  seriesId: string | number
): Promise<WatchSeries> {
  const url = buildXtreamUrl(session, 'get_series_info', { series_id: String(seriesId) });
  return fetchJson<WatchSeries>(url);
}

export function buildStreamUrl(
  session: ApiSession,
  type: 'live' | 'movies' | 'series',
  streamId: string | number,
  extension: string,
  forceProxy = false
): string {
  const protocol = session.useHttps ? 'https' : 'http';
  const serverClean = session.server.replace(/^https?:\/\//i, '');
  const targetHost = `${serverClean}${session.port ? `:${session.port}` : ''}`;
  
  let path = '';
  if (type === 'live') {
    path = `live/${session.username}/${session.password}/${streamId}.ts`;
  } else if (type === 'movies') {
    path = `movie/${session.username}/${session.password}/${streamId}.${extension || 'ts'}`;
  } else if (type === 'series') {
    path = `series/${session.username}/${session.password}/${streamId}.${extension || 'ts'}`;
  }

  const directUrl = `${protocol}://${targetHost}/${path}`;

  if (API_BASE) {
    if (API_BASE.includes('?')) {
      return `${API_BASE}&url=${encodeURIComponent(directUrl)}`;
    } else {
      return `${API_BASE}/${directUrl.replace(/^https?:\/\//i, '')}`;
    }
  }

  if (forceProxy) {
    return `/proxy?url=${encodeURIComponent(directUrl)}`;
  }

  return directUrl;
}

// Simple IndexedDB wrapper for caching stream catalog
const DB_NAME = 'nilefusion_db';
const DB_VERSION = 1;
const STORE_NAME = 'catalog_cache';

function getDB(): Promise<IDBDatabase> {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open(DB_NAME, DB_VERSION);
    request.onupgradeneeded = () => {
      const db = request.result;
      if (!db.objectStoreNames.contains(STORE_NAME)) {
        db.createObjectStore(STORE_NAME);
      }
    };
    request.onsuccess = () => resolve(request.result);
    request.onerror = () => reject(request.error);
  });
}

export async function setCacheItem(key: string, val: any): Promise<void> {
  try {
    const db = await getDB();
    return new Promise((resolve, reject) => {
      const tx = db.transaction(STORE_NAME, 'readwrite');
      const store = tx.objectStore(STORE_NAME);
      const req = store.put(val, key);
      req.onsuccess = () => resolve();
      req.onerror = () => reject(req.error);
    });
  } catch (e) {
    console.warn("IndexedDB cache save failed", e);
  }
}

export async function getCacheItem<T>(key: string): Promise<T | null> {
  try {
    const db = await getDB();
    return new Promise((resolve, reject) => {
      const tx = db.transaction(STORE_NAME, 'readonly');
      const store = tx.objectStore(STORE_NAME);
      const req = store.get(key);
      req.onsuccess = () => resolve((req.result as T) || null);
      req.onerror = () => reject(req.error);
    });
  } catch (e) {
    console.warn("IndexedDB cache load failed", e);
    return null;
  }
}

export async function clearCache(): Promise<void> {
  try {
    const db = await getDB();
    return new Promise((resolve, reject) => {
      const tx = db.transaction(STORE_NAME, 'readwrite');
      const store = tx.objectStore(STORE_NAME);
      const req = store.clear();
      req.onsuccess = () => resolve();
      req.onerror = () => reject(req.error);
    });
  } catch (e) {
    console.warn("IndexedDB cache clear failed", e);
  }
}

