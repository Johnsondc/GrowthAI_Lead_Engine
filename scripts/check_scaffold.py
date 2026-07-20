from pathlib import Path

required = [
    'backend/GrowthAI.sln',
    'backend/src/GrowthAI.Api/Program.cs',
    'backend/src/GrowthAI.Api/Controllers/LeadsController.cs',
    'backend/src/GrowthAI.Api/Controllers/LandingController.cs',
    'backend/src/GrowthAI.Api/Controllers/AnalyticsController.cs',
    'backend/src/GrowthAI.Domain/Entities/LeadCustomer.cs',
    'backend/src/GrowthAI.Application/Leads/ILeadService.cs',
    'backend/src/GrowthAI.Infrastructure/Persistence/LeadService.cs',
    'frontend/package.json',
    'frontend/src/App.vue',
    'frontend/src/views/Dashboard.vue',
    'frontend/src/views/LeadPool.vue',
    'frontend/src/views/ContentCenter.vue',
    'frontend/src/views/LandingManager.vue',
    'frontend/src/views/Analytics.vue',
    'frontend/src/api/leads.ts',
    'frontend/src/api/landing.ts',
    'docker-compose.yml',
]
missing = [item for item in required if not Path(item).exists()]
if missing:
    raise SystemExit(f'Missing scaffold files: {missing}')

for project_file in Path('backend/src').glob('GrowthAI.*/*.csproj'):
    text = project_file.read_text(encoding='utf-8')
    if '<TargetFramework>net10.0</TargetFramework>' not in text:
        raise SystemExit(f'{project_file} is not targeting net10.0')

migration = Path('database/migrations/001_init_mvp.sql').read_text(encoding='utf-8')
for token in ['engine=InnoDB', 'json not null', 'create table LeadCustomer']:
    if token not in migration:
        raise SystemExit(f'MySQL migration marker missing: {token}')

print(f'Validated {len(required)} scaffold files, net10.0 projects, and MySQL migration markers')
