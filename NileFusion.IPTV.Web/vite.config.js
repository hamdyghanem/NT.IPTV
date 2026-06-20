import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
// Built-in CORS proxy — forwards /proxy?url=<encoded> to the real IPTV server
// server-side (Node), so the browser never sees a cross-origin request.
function corsProxyPlugin() {
    return {
        name: 'cors-proxy',
        configureServer(server) {
            server.middlewares.use('/proxy', async (req, res) => {
                const rawUrl = new URL(req.url ?? '', 'http://localhost').searchParams.get('url');
                if (!rawUrl) {
                    res.statusCode = 400;
                    res.end(JSON.stringify({ error: 'Missing ?url= parameter' }));
                    return;
                }
                try {
                    const upstream = await fetch(rawUrl);
                    const body = await upstream.text();
                    res.statusCode = upstream.status;
                    res.setHeader('Content-Type', upstream.headers.get('Content-Type') ?? 'application/json');
                    res.setHeader('Access-Control-Allow-Origin', '*');
                    res.end(body);
                }
                catch (err) {
                    res.statusCode = 502;
                    res.end(JSON.stringify({ error: 'Proxy fetch failed', detail: String(err?.message ?? err) }));
                }
            });
        },
    };
}
export default defineConfig({
    plugins: [react(), corsProxyPlugin()],
});
