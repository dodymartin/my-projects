using MinimalApi.Dom.Enumerations;
using MinimalApi.Shared;
using Stratos.Core;

namespace MinimalApi.App;

public class ControllerUriFacilityInfo : EntityBase<ControllerUriFacilityInfo, Guid>
{
    public override Guid Id { get; set; }

    public string Address { get; set; }
    public EnvironmentTypes EnvironmentType { get; set; }
    public int FacilityId { get; set; }
    public int Order { get; set; }
    public int Port { get; set; }
    public string? RedirectVersion { get; set; }
    public string UriName { get; set; }
    public bool UseHttps { get; set; }
    public bool UseProxy { get; set; }
    public string Version { get; set; }
    public int WebApiVersionId { get; set; }

    #region Cast Operators

    public static explicit operator ControllerUriResponse(ControllerUriFacilityInfo from)
    {
        return new ControllerUriResponse
        {
            Address = from.Address,
            Order = from.Order,
            Port = from.Port,
            UriName = from.UriName,
            UseHttps = from.UseHttps,
            Version = from.RedirectVersion
        };
    }

    #endregion
}