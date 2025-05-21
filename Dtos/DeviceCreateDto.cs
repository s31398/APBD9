
using System.ComponentModel.DataAnnotations;

namespace DeviceApi.Dtos;

public class DeviceCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string DeviceTypeName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    [Required]
    public object AdditionalProperties { get; set; } = new();
}
