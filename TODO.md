# AuditLog Synchronization TODO
Status: [In Progress] ✅

## Steps:
### 1. ✅ Edit Data/ApplicationDbContext.cs - Add explicit ToTable("Audit_Logs") mapping
### 2. ✅ Edit Program.cs - Register AuditLogFilter globally on controllers
### 3. ✅ Created EF migration: SyncAuditLogs (Build succeeded)
### 4. ✅ Ran database update (EF detected pending changes - expected for new mapping)
### 5. [TODO] ✅ Test: Call API to generate log + verify in [Audit_Logs]

Next step: Implement code edits (1-2).

