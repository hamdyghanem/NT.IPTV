import { defineConfig, Plugin } from 'vite'
import react from '@vitejs/plugin-react'
import http from 'node:http'
import https from 'node:https'
import { Buffer } from 'node:buffer'

// Built-in CORS proxy — uses Node http/https so it:
//   • bypasses browser CORS entirely (server-side fetch)
//   • accepts self-signed SSL certs (rejectUnauthorized: false)
//   • has a 30-second timeout to avoid hanging
//   • rewrites .m3u8 playlist segment URLs to also go through this proxy
//     so HLS.js segment fetches never hit browser CORS restrictions
function corsProxyPlugin(): Plugin {
  return {
    name: 'cors-proxy',
    configureServer(server) {
      server.middlewares.use('/proxy', (req, res) => {
        const rawUrl = new URL(req.url ?? '', 'http://localhost').searchParams.get('url')
        if (!rawUrl) {
          res.statusCode = 400
          res.end(JSON.stringify({ error: 'Missing ?url= parameter' }))
          return
        }

        let targetUrl: URL
        try {
          targetUrl = new URL(rawUrl)
        } catch {
          res.statusCode = 400
          res.end(JSON.stringify({ error: 'Invalid target URL' }))
          return
        }

        const isHttps = targetUrl.protocol === 'https:'
        const transport = isHttps ? https : http

        const options: http.RequestOptions = {
          hostname: targetUrl.hostname,
          port: targetUrl.port || (isHttps ? 443 : 80),
          path: targetUrl.pathname + targetUrl.search,
          method: 'GET',
          timeout: 30000,
          // Accept self-signed / expired certs that IPTV providers commonly use
          ...(isHttps ? { rejectUnauthorized: false } : {}),
        }

        const proxyReq = transport.request(options, (proxyRes) => {
          const ct = (proxyRes.headers['content-type'] ?? '').toLowerCase()
          const isM3u8 = ct.includes('mpegurl') || rawUrl.includes('.m3u8')

          res.setHeader('Access-Control-Allow-Origin', '*')

          if (isM3u8) {
            // Buffer the full playlist and rewrite segment URLs so that
            // HLS.js requests all segments through this proxy (no browser CORS)
            const chunks: Buffer[] = []
            proxyRes.on('data', (chunk: Buffer) => chunks.push(chunk))
            proxyRes.on('end', () => {
              const playlist = Buffer.concat(chunks).toString('utf8')
              const rewritten = playlist.split('\n').map(line => {
                const trimmed = line.trim()
                if (trimmed === '' || trimmed.startsWith('#')) return line
                try {
                  const absUrl = new URL(trimmed, rawUrl).href
                  return `/proxy?url=${encodeURIComponent(absUrl)}`
                } catch {
                  return line
                }
              }).join('\n')

              if (!res.headersSent) {
                res.statusCode = proxyRes.statusCode ?? 200
                res.setHeader('Content-Type', 'application/vnd.apple.mpegurl')
                res.end(rewritten)
              }
            })
          } else {
            // All other content (TS segments, JSON API, images) — stream directly
            res.statusCode = proxyRes.statusCode ?? 200
            res.setHeader('Content-Type', ct || 'application/octet-stream')
            proxyRes.pipe(res)
          }
        })

        proxyReq.on('timeout', () => {
          proxyReq.destroy()
          if (!res.headersSent) {
            res.statusCode = 504
            res.end(JSON.stringify({ error: 'Gateway timeout — server did not respond in 30 s' }))
          }
        })

        proxyReq.on('error', (err) => {
          if (!res.headersSent) {
            res.statusCode = 502
            res.end(JSON.stringify({ error: 'Proxy request failed', detail: err.message }))
          }
        })

        proxyReq.end()
      })
    },
  }
}

export default defineConfig({
  plugins: [react(), corsProxyPlugin()],
})
