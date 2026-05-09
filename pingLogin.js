const https = require('https');

function post(url, body, headers = {}) {
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
        'Content-Length': Buffer.byteLength(body),
        ...headers,
      }
    }, (res) => {
      let data = '';
      res.on('data', (c) => data += c);
      res.on('end', () => resolve({ status: res.statusCode, data }));
    });
    req.on('error', (e) => resolve({ status: null, error: e.message }));
    req.write(body);
    req.end();
  });
}

(async () => {
  // thử login với tài khoản seed (admin@hotel.com / 123456)
  const login = await post('https://localhost:5070/api/Auth/login', JSON.stringify({ email: 'admin@hotel.com', password: '123456' }));
  console.log('login:', login.status, login.data?.slice(0, 200));

  let token;
  try {
    token = JSON.parse(login.data).token;
  } catch {}

  const auditBody = JSON.stringify({ totalEvents: 1, events: [{ eventId: 'e1', actionType: 'CREATE', timestamp: new Date().toISOString(), module: 'Test', objectName: 'X', description: 'd' }] });
  const audit = await post('https://localhost:5070/api/AuditLogs', auditBody, token ? { Authorization: `Bearer ${token}` } : {});
  console.log('audit:', audit.status, audit.data?.slice(0, 200));
})();

