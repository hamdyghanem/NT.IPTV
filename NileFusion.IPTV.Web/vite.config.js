import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import http from 'node:http';
import https from 'node:https';
// Built-in CORS proxy — uses Node http/https so it:
//   • bypasses browser CORS entirely (server-side fetch)
//   • accepts self-signed SSL certs (rejectUnauthorized: false)
//   • has a 20-second timeout to avoid hanging
function corsProxyPlugin() {
    return {
        name: 'cors-proxy',
        configureServer(server) {
            server.middlewares.use('/proxy', (req, res) => {
                const rawUrl = new URL(req.url ?? '', 'http://localhost').searchParams.get('url');
                if (!rawUrl) {
                    res.statusCode = 400;
                    res.end(JSON.stringify({ error: 'Missing ?url= parameter' }));
                    return;
                }
                let targetUrl;
                try {
                    targetUrl = new URL(rawUrl);
                }
                catch {
                    res.statusCode = 400;
                    res.end(JSON.stringify({ error: 'Invalid target URL' }));
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
                    // Accept self-signed / expired certs that IPTV providers commonly use
                    ...(isHttps ? { rejectUnauthorized: false } : {}),
                };
                const proxyReq = transport.request(options, (proxyRes) => {
                    res.statusCode = proxyRes.statusCode ?? 200;
                    const ct = proxyRes.headers['content-type'] ?? 'application/json';
                    res.setHeader('Content-Type', ct);
                    res.setHeader('Access-Control-Allow-Origin', '*');
                    proxyRes.pipe(res);
                });
                proxyReq.on('timeout', () => {
                    proxyReq.destroy();
                    if (!res.headersSent) {
                        res.statusCode = 504;
                        res.end(JSON.stringify({ error: 'Gateway timeout — server did not respond in 20 s' }));
                    }
                });
                proxyReq.on('error', (err) => {
                    if (!res.headersSent) {
                        res.statusCode = 502;
                        res.end(JSON.stringify({ error: 'Proxy request failed', detail: err.message }));
                    }
                });
                proxyReq.end();
            });
        },
    };
}
export default defineConfig({
    plugins: [react(), corsProxyPlugin()],
});
