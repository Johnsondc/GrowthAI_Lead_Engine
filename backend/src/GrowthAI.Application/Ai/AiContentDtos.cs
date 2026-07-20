namespace GrowthAI.Application.Ai;

public sealed record GenerateContentRequest(
    string Industry,
    string ProductName,
    string City,
    string TargetAudience,
    string SellingPoints,
    string Platform);

public sealed record GeneratedContentDto(
    string Platform,
    string Title,
    string Body,
    string Script,
    string Tags,
    string CallToAction);
