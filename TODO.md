# RBAC 4-ROLES IMPLEMENTATION (Admin/Guest/Receptionist/Housekeeping)
✅ **Step 1 Complete:** Created TODO.md tracking

## PROGRESS: 1/12 ✅

**Phase 1: Core Fixes (4 steps)**
- [x] 1. Create TODO.md 
- [ ] 2. Models/HRRBACModels.cs: Remove redundant User.Role string
- [ ] 3. Services/JwtService.cs: Fix role claim from junction table
- [ ] 4. Data/AuthSeedData.cs: Update seeding (null string Role)
- [ ] 5. Program.cs: Add role-based policies

**Phase 2: Controllers Standardization (6 steps)**
- [ ] 6. Controllers/AuthController.cs: Pass roles to JWT
- [ ] 7. Fix Attractions/Rooms/RoomInventory (existing mixed Roles+Policy)
- [ ] 8. Fix Bookings/Reception/Payment/OrderServices/LossAndDamages
- [ ] 9. Guest-only protections (read-only)
- [ ] 10. Add missing [Authorize] to unprotected controllers

**Phase 3: Role Management (1 step)**
- [ ] 11. Create UserRolesController.cs (Admin CRUD)

**Phase 4: Test & Docs (1 step)**
- [ ] 12. Migrations + Tests + Docs

**Test Users (all roles seeded):**
- Admin: admin@test.com / 123456
- Guest: guest@test.com / 123456  
- Receptionist: receptionist@test.com / 123456
- Housekeeping: housekeeping@test.com / 123456

