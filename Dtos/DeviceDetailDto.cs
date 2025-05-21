
namespace DeviceApi.Dtos;

public record DeviceDetailDto(
    string Name,
    string DeviceTypeName,
    bool IsEnabled,
    object AdditionalProperties,
    object? CurrentEmployee
);
