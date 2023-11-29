using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Client;

public class AppSettings
{
    public const string ConfigurationSection = "MinimalApi.Client.AppSettings";

    [Required]
    public required IDictionary<int, GrpcBaseAddress> GrpcBaseAddresses { get; set; }
    [Required]
    public required IDictionary<int, RestBaseAddress> RestBaseAddresses { get; set; }

}