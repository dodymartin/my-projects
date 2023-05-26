using MinimalApi.Dal;

namespace MinimalApi.Repositories;

public class ApplicationDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    #region Cast Operators

    public static explicit operator ApplicationDto(Application from)
    {
        return new ApplicationDto
        {
            Id = from.Id,
            Name = from.Name
        };
    }

    #endregion
}
