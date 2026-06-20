const API_BASE = (import.meta as ImportMeta & { env?: Record<string, string> }).env?.VITE_API_BASE_URL || '';

export async function fetchJson<T>(input: string, init?: RequestInit): Promise<T> {
  const response = await fetch(input, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...(init?.headers || {}),
    },
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return response.json() as Promise<T>;
}

export function buildXtreamUrl(
  session: {
    server: string;
    port: string;
    useHttps: boolean;
    username: string;
    password: string;
  },
  action: string,
) {
  const protocol = session.useHttps ? 'https' : 'http';
  const base = `${protocol}://${session.server}:${session.port}`;
  return `${base}/player_api.php?username=${session.username}&password=${session.password}&action=${action}`;
}

export async function testConnection(session: {
  server: string;
  port: string;
  useHttps: boolean;
  username: string;
  password: string;
}) {
  const url = buildXtreamUrl(session, 'get_live_categories');
  const data = await fetchJson<{ user_info?: { status?: string } }>(url);
  return data;
}
