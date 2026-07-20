from pathlib import Path

required = [
    'backend/GrowthAI.sln',
    'backend/src/GrowthAI.Api/Program.cs',
    'backend/src/GrowthAI.Api/Controllers/LeadsController.cs',
    'backend/src/GrowthAI.Domain/Entities/LeadCustomer.cs',
    'backend/src/GrowthAI.Application/Leads/ILeadService.cs',
    'backend/src/GrowthAI.Infrastructure/Persistence/LeadService.cs',
    'frontend/package.json',
    'frontend/src/App.vue',
    'frontend/src/views/Dashboard.vue',
    'frontend/src/views/LeadPool.vue',
    'frontend/src/views/ContentCenter.vue',
    'frontend/src/api/leads.ts',
]
missing = [item for item in required if not Path(item).exists()]
if missing:
    raise SystemExit(f'Missing scaffold files: {missing}')
print(f'Validated {len(required)} scaffold files')
