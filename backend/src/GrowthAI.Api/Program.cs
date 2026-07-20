using GrowthAI.Application.Ai;
using GrowthAI.Application.Analytics;
using GrowthAI.Application.Leads;
using GrowthAI.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<InMemoryGrowthAiStore>();
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IAiContentService, AiContentService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "GrowthAI Lead Engine" }));
app.MapControllers();

app.Run();
