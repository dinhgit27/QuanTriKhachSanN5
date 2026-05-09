$ErrorActionPreference = 'Stop'

function Test-Post($name, $uri, $body){
  try {
    $handler = new-object System.Net.Http.HttpClientHandler
    $handler.ServerCertificateCustomValidationCallback = { $true }
    $client = new-object System.Net.Http.HttpClient($handler)
    $content = new-object System.Net.Http.StringContent($body, [System.Text.Encoding]::UTF8, 'application/json')
    $resp = $client.PostAsync($uri, $content).GetAwaiter().GetResult()
    $code = [int]$resp.StatusCode
    Write-Output "$name HTTP $code"
  } catch {
    Write-Output "$name ERR: $($_.Exception.Message)"
  }
}

Test-Post 'Auth/login'  'https://localhost:5070/api/Auth/login'  '{}'
Test-Post 'AuditLogs'  'https://localhost:5070/api/AuditLogs'  '{"totalEvents":0,"events":[]}'

