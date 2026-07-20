namespace GrowthAI.Application.Leads;

public interface ILeadService
{
    IReadOnlyList<LeadCustomerDto> List(Guid tenantId, string? status, string? platform);
    LeadCustomerDto Create(Guid tenantId, CreateLeadCustomerRequest request);
    LeadCustomerDto? UpdateStatus(Guid tenantId, Guid leadId, UpdateLeadStatusRequest request);
}
