
namespace DeviceApi.Models;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public string AdditionalProperties { get; set; } = string.Empty;
    public int? DeviceTypeId { get; set; }

    public DeviceType? DeviceType { get; set; }
    public ICollection<DeviceEmployee> DeviceEmployees { get; set; } = new List<DeviceEmployee>();
}
