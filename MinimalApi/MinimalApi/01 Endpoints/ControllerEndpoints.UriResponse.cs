using MinimalApi.Dal;

namespace MinimalApi.Endpoints;

public class ControllerUriResponse
{
    public string Address { get; set; }
    public int Order { get; set; }
    public int Port { get; set; }
    public string UriName { get; set; }
    public bool UseHttps { get; set; }
    public string Version { get; set; }

    #region Cast Operators

    public static explicit operator ControllerUriResponse(ControllerUriInfoByApplication from)
    {
        return new ControllerUriResponse
        {
            Address = from.Address,
            Order = from.Order,
            Port = from.Port,
            UriName = from.UriName,
            UseHttps = from.UseHttps,
            Version = from.Version
        };
    }

    public static explicit operator ControllerUriResponse(ControllerUriFacilityInfoByApplication from)
    {
        return new ControllerUriResponse
        {
            Address = from.Address,
            Order = from.Order,
            Port = from.Port,
            UriName = from.UriName,
            UseHttps = from.UseHttps,
            Version = from.Version
        };
    }

    public static explicit operator ControllerUriResponse(ControllerUriInfo from)
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
