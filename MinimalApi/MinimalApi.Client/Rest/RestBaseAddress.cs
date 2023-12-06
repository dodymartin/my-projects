using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Client;

public record RestBaseAddress(
    [property: Required] string Address,
    [property: Required] bool IsLocal);