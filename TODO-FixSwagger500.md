# Fix Swagger 500 Error - QuanTriKhachSanN5
Status: 🔄 In Progress

## Approved Plan
- ✅ Information Gathered: DB ok, migrations ok, seed order wrong (Users before Roles → crash)
- ✅ Plan: Fix Program.cs seed order + policies + exception handler; fix UserRolesController policy
- Dependent Files: Program.cs, Controllers/UserRolesController.cs
- Followup: dotnet build → dotnet run → test Swagger → attempt_completion

## Steps
- [ ] 1. Create this TODO ✅
- [✅] 2. Edit Program.cs (seed order, policies, exception middleware)
- [✅] 3. Edit Controllers/UserRolesController.cs (Policy="AdminOnly" → "ManageRoles")
- [✅] 4. `dotnet build` ✓ Succeeded (warnings only)
- [✅] 5. `dotnet run` ✓ Success: DB connect, Users:5, no seed crash, server running http://localhost:5070, Swagger ready
- [✅] 6. Test Swagger /auth/login (admin@test.com/123456) → Success (no 500)

**Status:** ✅ COMPLETE - Swagger 500 fixed!

Run `http://localhost:5070/swagger` to test.
Login: admin@test.com / 123456

