# TODO: Fix Auth & Clean Project

## Plan Steps (Approved)

### 1. Delete unnecessary files
- [ ] Controllers/ExampleRBACController.cs
- [ ] Controllers/HRRBACController.cs  
- [ ] Services/HRRBACService.cs
- [ ] Interfaces/IHRRBACService.cs

### 2. Edit Program.cs
- [ ] Change "User" → "Guest" in seeding
- [ ] Add Role seeding
- [ ] Add User_Role seeding
- [ ] Remove HRRBACService registration

### 3. Enhance Data/SeedData.cs
- [ ] Add Permissions seed
- [ ] Add Role_Permission links

### 4. Standardize auth attributes
- [ ] Replace [Permission()] → [Authorize(Roles=)] in all controllers (e.g. AttractionsController.cs)
- [ ] Delete Attributes/PermissionAttribute.cs

### 5. Database migrations
- Run `dotnet ef migrations add FixAuthRoles`
- Run `dotnet ef database update`

### 6. Test
- [ ] dotnet run (seeds data)
- [ ] Test logins: admin@test.com/123456 etc.

**Current progress: ✅ Steps 1-4 complete. Step 5: Migrations & Test**

