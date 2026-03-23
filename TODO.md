# TODO: Hoàn thiện hệ thống Phân quyền RBAC

Status: [🚀 Started - Checking & Fixing]

## Breakdown Steps (RBAC Review & Fix Plan):

### 1. ✅ **Seed Permissions & Role_Permission data** (Program.cs)
   - Add Permissions (MANAGE_ROOMS, VIEW_ROOMS, MANAGE_ROOMTYPES...)
   - Link Role → Permissions (Admin full, Receptionist limited...)
   
### 2. ✅ **Fix PermissionAttribute.cs**
   - ✅ Claim-based permission check (userPermissions.Any(_permissions))
   - Multiple OR permissions supported
   
### 3. ✅ **Enable HRRBACController.cs**
   - ✅ Namespace fixed + using Authorization
   - ✅ [Authorize(Roles="Admin")]
   - ✅ Build success
   
### 4. 🔄 **Enhance HRRBACService.cs**
   - Add AssignRoleToUserAsync, AssignPermissionToRoleAsync
   - Add controller endpoints
   
### 5. 🔄 **Protect Business Controllers**
   - RoomTypesController.cs: Apply [Authorize(Policy=...)]
   - BookingsController.cs, etc. (scan & fix)
   
### 6. 🧪 **Test Full Flow**
   - dotnet ef migrations add SeedRBAC
   - dotnet ef database update
   - Login test users → jwt.io verify claims
   - Call protected APIs
   
### 7. ✅ **Complete! Run dotnet run & demo Swagger**

## Old Tasks (RoomType):
- [x] Create DTOs/Services/Controllers
- Next: Test endpoints

**Current Step: 1/7 - Seeding data**
