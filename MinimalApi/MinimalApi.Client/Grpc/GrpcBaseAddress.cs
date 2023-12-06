using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Client;

public record GrpcBaseAddress(
    [property: Required] string Address,
    [property: Required] bool IsLocal);