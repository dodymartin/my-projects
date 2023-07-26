namespace MinimalApi.Core;

public class Facility
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Database> Databases { get; } = new();
}
