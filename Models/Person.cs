namespace apiBackend.Models;

public partial class Person
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public int IdentificationNumber { get; set; }

    public string? IdentificationType { get; set; }

    public string? ConcatenatedNameLastName { get; set; }

    public string? ConcatenatedTypeDocument { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }
}
