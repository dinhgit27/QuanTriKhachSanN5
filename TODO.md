# Fix 401 Authorization Error - Authorization API Plan

Status: In Progress

## Steps (approved plan)

### 1. [DONE] Understand & Log Issue - Port confirmed, appsettings Issuer/Audience fixed
- Read launchSettings.json (port)
- Add JWT auth logging in Program.cs

### 2. [DONE] Fix Issuer Mismatch - appsettings Issuer https://localhost:7110 + Audience added
- Make issuer dynamic in Program.cs & JwtService.cs
- Update appsettings.json

### 3. [DONE] Fix Token Claims - Multi-role + Audience in JwtService
- JwtService: Support multiple Role claims (loop over roles)
- Add Audience claim

### 4. [PENDING] Verify DB/Seed
- Run dotnet ef database update
- Restart app, check console seeds

### 5. [PENDING] Test
- Login admin@test.com/123456
- Decode token at jwt.io - verify Permission claims
- Test failing endpoint (user specify)

### 6. [DONE] Cleanup
- Remove debug logging

**Next**: User specify exact endpoint + confirm port/console logs.

