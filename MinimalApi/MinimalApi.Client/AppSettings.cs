using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Client;

public class AppSettings
{
    public const string ConfigurationSection = "MinimalApi.Client.AppSettings";

    [Required]
    public required string GrpcBaseAddress { get; set; }
    [Required]
    public required IDictionary<int, RestBaseAddress> RestBaseAddresses { get; set; }

}