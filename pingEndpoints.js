const https = require('https');

function post(url, body) {
  return new Promise((resolve) => {
    const u = new URL(url);
    const req = https.request({
      hostname: u.hostname,
      port: u.port,
      path: u.pathname,
      method: 'POST',
      rejectUnauthorized: false,
      headers: {
        'Content-Type': 'application/json',
        'Content-Length': Buffer.byteLength(body)
      }
    }, (res) => {
      res.on('data', () => {});
      res.on('end', () => {
        resolve({ ok: true, name: body, status: res.statusCode });
      });
    });
    req.on('error', (e) => resolve({ ok: false, status: null, error: e.message }));
    req.write(body);
    req.end();
  });
}

(async () => {
  const auth = await post('https://localhost:5070/api/Auth/login', '{}');
  console.log('Auth/login:', auth);
  const audit = await post('https://localhost:5070/api/AuditLogs', JSON.stringify({ totalEvents: 0, events: [] }));
  console.log('AuditLogs:', audit);
})();

