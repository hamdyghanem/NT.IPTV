/**
 * Production proxy server for NileFusion IPTV
 * Runs on Azure App Service via iisnode.
 * Serves the built SPA static files AND handles /proxy?url=... requests
 * using Node.js built-ins only — no npm install required.
 * ES5 compatible.
 */
var http = require('http');
var https = require('https');
var fs = require('fs');
var path = require('path');
var url = require('url');

var PORT = process.env.PORT || 8080;
var STATIC_DIR = __dirname;

function isDirectory(p) {
  try {
    return fs.statSync(p).isDirectory();
  } catch (e) {
    return false;
  }
}

var MIME_TYPES = {
  '.html': 'text/html; charset=utf-8',
  '.js':   'application/javascript',
  '.mjs':  'application/javascript',
  '.css':  'text/css',
  '.json': 'application/json',
  '.png':  'image/png',
  '.jpg':  'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.svg':  'image/svg+xml',
  '.ico':  'image/x-icon',
  '.woff': 'font/woff',
  '.woff2':'font/woff2',
  '.ttf':  'font/ttf',
  '.webp': 'image/webp',
};

// ── Static file helper ────────────────────────────────────────────────────────
function serveFile(res, filePath) {
  var ext = path.extname(filePath).toLowerCase();
  var mime = MIME_TYPES[ext] || 'application/octet-stream';
  fs.stat(filePath, function (err, stat) {
    if (err || !stat.isFile()) {
      // SPA fallback
      var index = path.join(STATIC_DIR, 'index.html');
      res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
      fs.createReadStream(index).pipe(res);
      return;
    }
    res.writeHead(200, { 'Content-Type': mime, 'Content-Length': stat.size });
    fs.createReadStream(filePath).pipe(res);
  });
}

// ── Outbound proxy ────────────────────────────────────────────────────────────
function proxyRequest(targetUrl, res, redirectCount) {
  if (redirectCount > 5) {
    res.writeHead(502, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ error: 'Too many redirects' }));
    return;
  }

  var parsed;
  try {
    parsed = url.parse(targetUrl);
  } catch (e) {
    res.writeHead(400, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ error: 'Invalid target URL' }));
    return;
  }

  var isHttps = parsed.protocol === 'https:';
  var transport = isHttps ? https : http;

  var options = {
    hostname: parsed.hostname,
    port:     parsed.port || (isHttps ? 443 : 80),
    path:     (parsed.pathname || '') + (parsed.search || ''),
    method:   'GET',
    timeout:  30000,
    headers:  { 'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)' }
  };

  if (isHttps) {
    options.rejectUnauthorized = false;
  }

  var proxyReq = transport.request(options, function (proxyRes) {
    var status = proxyRes.statusCode || 200;

    // Follow redirects server-side
    if ((status === 301 || status === 302 || status === 303 || status === 307 || status === 308) && proxyRes.headers.location) {
      try {
        var resolved = url.resolve(targetUrl, proxyRes.headers.location);
        proxyRequest(resolved, res, redirectCount + 1);
      } catch (e) {
        res.writeHead(502); res.end();
      }
      return;
    }

    var ct = (proxyRes.headers['content-type'] || '').toLowerCase();
    var isM3u8 = ct.indexOf('mpegurl') !== -1 || targetUrl.indexOf('.m3u8') !== -1;

    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, OPTIONS');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type');

    if (isM3u8) {
      // Rewrite segment URLs so HLS.js fetches them through this proxy too
      var chunks = [];
      proxyRes.on('data', function (c) { chunks.push(c); });
      proxyRes.on('end', function () {
        var playlist = Buffer.concat(chunks).toString('utf8');
        var rewritten = playlist.split('\n').map(function (line) {
          var t = line.trim();
          if (!t || t.indexOf('#') === 0) return line;
          try {
            var abs = url.resolve(targetUrl, t);
            return '/proxy?url=' + encodeURIComponent(abs);
          } catch (e) { return line; }
        }).join('\n');
        if (!res.headersSent) {
          res.writeHead(status, { 'Content-Type': 'application/vnd.apple.mpegurl' });
          res.end(rewritten);
        }
      });
    } else {
      // Stream everything else directly
      res.writeHead(status, { 'Content-Type': ct || 'application/octet-stream' });
      proxyRes.pipe(res);
    }
  });

  proxyReq.on('timeout', function () {
    proxyReq.destroy();
    if (!res.headersSent) {
      res.writeHead(504, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: 'Gateway timeout — IPTV server did not respond in 30s' }));
    }
  });

  proxyReq.on('error', function (err) {
    if (!res.headersSent) {
      res.writeHead(502, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: err.message }));
    }
  });

  proxyReq.end();
}

// ── HTTP server ───────────────────────────────────────────────────────────────
var server = http.createServer(function (req, res) {
  // CORS preflight
  if (req.method === 'OPTIONS') {
    res.writeHead(204, {
      'Access-Control-Allow-Origin':  '*',
      'Access-Control-Allow-Methods': 'GET, OPTIONS',
      'Access-Control-Allow-Headers': 'Content-Type',
    });
    res.end();
    return;
  }

  var reqUrl;
  try {
    reqUrl = url.parse(req.url, true);
  } catch (e) {
    res.writeHead(400); res.end(); return;
  }

  // ── /proxy?url=<target> ──
  if (reqUrl.pathname === '/proxy') {
    var target = reqUrl.query.url;
    if (!target) {
      res.writeHead(400, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ error: 'Missing ?url= parameter' }));
      return;
    }
    proxyRequest(target, res, 0);
    return;
  }

  // ── Static files ──
  var filePath = path.join(STATIC_DIR, reqUrl.pathname);

  // Prevent path traversal
  if (filePath.indexOf(STATIC_DIR) !== 0) {
    res.writeHead(403); res.end(); return;
  }

  // If directory, try index.html inside it
  if (isDirectory(filePath)) {
    filePath = path.join(filePath, 'index.html');
  }

  serveFile(res, filePath);
});

server.listen(PORT, function () {
  console.log('NileFusion proxy server listening on port ' + PORT);
});
