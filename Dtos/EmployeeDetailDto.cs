
namespace DeviceApi.Dtos;

public record EmployeeDetailDto(
    int Id,
    string PassportNumber,
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber,
    string Email,
    decimal Salary,
    object Position,
    DateTime HireDate
);
