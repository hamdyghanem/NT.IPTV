import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import http from 'node:http';
import https from 'node:https';
import { Buffer } from 'node:buffer';
// Built-in CORS proxy — uses Node http/https so it:
//   • bypasses browser CORS entirely (server-side fetch)
//   • accepts self-signed SSL certs (rejectUnauthorized: false)
//   • has a 30-second timeout to avoid hanging
//   • rewrites .m3u8 playlist segment URLs to also go through this proxy
//     so HLS.js segment fetches never hit browser CORS restrictions
function corsProxyPlugin() {
    return {
        name: 'cors-proxy',
        configureServer(server) {
            server.middlewares.use('/proxy', (req, res) => {
                const initialUrl = new URL(req.url ?? '', 'http://localhost').searchParams.get('url');
                if (!initialUrl) {
                    res.statusCode = 400;
                    res.end(JSON.stringify({ error: 'Missing ?url= parameter' }));
                    return;
                }
                const performProxyRequest = (urlStr, redirectCount = 0) => {
                    if (redirectCount > 5) {
                        res.statusCode = 502;
                        res.end(JSON.stringify({ error: 'Too many redirects' }));
                        return;
                    }
                    let targetUrl;
                    try {
                        targetUrl = new URL(urlStr);
                    }
                    catch {
                        res.statusCode = 400;
                        res.end(JSON.stringify({ error: 'Invalid target URL: ' + urlStr }));
                        return;
                    }
                    const isHttps = targetUrl.protocol === 'https:';
                    const transport = isHttps ? https : http;
                    const options = {
                        hostname: targetUrl.hostname,
                        port: targetUrl.port || (isHttps ? 443 : 80),
                        path: targetUrl.pathname + targetUrl.search,
                        method: 'GET',
                        timeout: 30000,
                        headers: {
                            'User-Agent': req.headers['user-agent'] || 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)',
                        },
                        ...(isHttps ? { rejectUnauthorized: false } : {}),
                    };
                    const proxyReq = transport.request(options, (proxyRes) => {
                        const statusCode = proxyRes.statusCode ?? 200;
                        // Intercept HTTP redirects (301, 302, 303, 307, 308) and follow them server-side
                        if ([301, 302, 303, 307, 308].includes(statusCode)) {
                            const location = proxyRes.headers['location'];
                            if (location) {
                                try {
                                    const resolvedLocation = new URL(location, urlStr).href;
                                    performProxyRequest(resolvedLocation, redirectCount + 1);
                                    return;
                                }
                                catch (e) {
                                    console.error('Failed to resolve redirect location:', location, e.message);
                                }
                            }
                        }
                        const ct = (proxyRes.headers['content-type'] ?? '').toLowerCase();
                        const isM3u8 = ct.includes('mpegurl') || urlStr.includes('.m3u8');
                        res.setHeader('Access-Control-Allow-Origin', '*');
                        if (isM3u8) {
                            // Buffer the full playlist and rewrite segment URLs so that
                            // HLS.js requests all segments through this proxy (no browser CORS)
                            const chunks = [];
                            proxyRes.on('data', (chunk) => chunks.push(chunk));
                            proxyRes.on('end', () => {
                                const playlist = Buffer.concat(chunks).toString('utf8');
                                const rewritten = playlist.split('\n').map(line => {
                                    const trimmed = line.trim();
                                    if (trimmed === '' || trimmed.startsWith('#'))
                                        return line;
                                    try {
                                        const absUrl = new URL(trimmed, urlStr).href;
                                        return `/proxy?url=${encodeURIComponent(absUrl)}`;
                                    }
                                    catch {
                                        return line;
                                    }
                                }).join('\n');
                                if (!res.headersSent) {
                                    res.statusCode = statusCode;
                                    res.setHeader('Content-Type', 'application/vnd.apple.mpegurl');
                                    res.end(rewritten);
                                }
                            });
                        }
                        else {
                            // All other content (TS segments, JSON API, images) — stream directly
                            res.statusCode = statusCode;
                            res.setHeader('Content-Type', ct || 'application/octet-stream');
                            proxyRes.pipe(res);
                        }
                    });
                    proxyReq.on('timeout', () => {
                        proxyReq.destroy();
                        if (!res.headersSent) {
                            res.statusCode = 504;
                            res.end(JSON.stringify({ error: 'Gateway timeout — server did not respond in 30 s' }));
                        }
                    });
                    proxyReq.on('error', (err) => {
                        if (!res.headersSent) {
                            res.statusCode = 502;
                            res.end(JSON.stringify({ error: 'Proxy request failed', detail: err.message }));
                        }
                    });
                    proxyReq.end();
                };
                performProxyRequest(initialUrl);
            });
        },
    };
}
export default defineConfig({
    plugins: [react(), corsProxyPlugin()],
});
