namespace GrowthAI.Domain.Entities;

public sealed class AppUser : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = "Sales";
    public bool IsActive { get; set; } = true;
}
