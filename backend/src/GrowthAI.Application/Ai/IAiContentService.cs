namespace GrowthAI.Application.Ai;

public interface IAiContentService
{
    GeneratedContentDto Generate(GenerateContentRequest request);
}
