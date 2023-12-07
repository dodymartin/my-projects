namespace MinimalApi.Api.Common.Interfaces;

public interface IAuditableEntity
{
    DateTime? CreatedOn { get; set; }
    string? CreatedBy { get; set; }
}
