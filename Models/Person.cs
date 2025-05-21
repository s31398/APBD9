
namespace DeviceApi.Models;

public class Person
{
    public int Id { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Employee? Employee { get; set; }
}
