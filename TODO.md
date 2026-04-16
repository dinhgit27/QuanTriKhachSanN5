# Fix Audit_Logs Schema Mismatch - Step-by-Step Plan

## Status: [IN PROGRESS] 

## Steps:

### 1. ✅ [COMPLETE] Understand problem via file analysis
- Analyzed DB schema vs EF model mismatch
- Identified root cause: extra columns in Audit_Log model

### 2. ✅ [COMPLETE] Create this TODO.md

### 3. ✅ [COMPLETE] Update Models/HRRBACModels.cs
- Simplified Audit_Log class to match DB: id, user_id, log_date, role_name, log_data
- Removed Action, TargetTable, Status, EventId properties

### 4. ✅ [COMPLETE] Update Controllers/AuditLogFilter.cs
- Modified Audit_Log creation: only use supported properties (UserId, RoleName, Timestamp, LogData)
- Store action details (eventId, actionType, targetTable, status) in LogData JSON

### 5. ✅ [COMPLETE] Update Controllers/AuditLogsController.cs
- Updated AuditLogDto: removed Action, TargetTable, Status, EventId fields (via create_file)
- Adjusted query/projection to match new model

### 6. ✅ [COMPLETE] Minor check Data/ApplicationDbContext.cs
- Verified ToTable("Audit_Logs") configuration is correct - no changes needed

### 7. ✅ [COMPLETE] Generate & Apply EF Migration
```
dotnet ef migrations add FixAuditLogsModel --context ApplicationDbContext ✓
dotnet ef database update (running...)
```

### 8. [PENDING] Test Fix
- Run `dotnet run`
- Login with admin@hotel.com
- Perform POST/PUT/DELETE action (e.g., create booking)
- Verify:
  - No SQL "Invalid column" error
  - Check Audit_Logs table has new records
  - AuditLogFilter logs to console if issues

### 9. [PENDING] Update this TODO.md as steps complete
- Mark [COMPLETE] when done

## Commands to run:
```
dotnet ef migrations add FixAuditLogsModel
dotnet ef database update
dotnet run
```

