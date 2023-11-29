using System.ComponentModel.DataAnnotations;

public record RestBaseAddress(
    [property: Required] string Address,
    [property: Required] bool IsLocal);
