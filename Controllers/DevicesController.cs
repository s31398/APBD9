
using DeviceApi.Dtos;
using DeviceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly AppDbContext _db;

    public DevicesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceListDto>>> Get()
    {
        var list = await _db.Devices
            .Select(d => new DeviceListDto(d.Id, d.Name))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDetailDto>> Get(int id)
    {
        var device = await _db.Devices
            .Include(d => d.DeviceType)
            .Include(d => d.DeviceEmployees)
                .ThenInclude(de => de.Employee)
                    .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (device == null)
        {
            return NotFound();
        }

        var currentDe = device.DeviceEmployees
            .FirstOrDefault(de => de.ReturnDate == null);

        object? currentEmployee = null;
        if (currentDe != null)
        {
            currentEmployee = new
            {
                currentDe.Employee.Id,
                FullName = currentDe.Employee.Person.FirstName + " " + currentDe.Employee.Person.LastName
            };
        }

        var dto = new DeviceDetailDto(
            device.Name,
            device.DeviceType?.Name ?? string.Empty,
            device.IsEnabled,
            System.Text.Json.JsonSerializer.Deserialize<object>(device.AdditionalProperties),
            currentEmployee
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<DeviceListDto>> Post(DeviceCreateDto dto)
    {
        var type = await _db.DeviceTypes.FirstOrDefaultAsync(t => t.Name == dto.DeviceTypeName);
        if (type == null)
        {
            type = new DeviceType { Name = dto.DeviceTypeName };
            _db.DeviceTypes.Add(type);
            await _db.SaveChangesAsync();
        }

        var device = new Device
        {
            Name = dto.Name,
            DeviceTypeId = type.Id,
            IsEnabled = dto.IsEnabled,
            AdditionalProperties = System.Text.Json.JsonSerializer.Serialize(dto.AdditionalProperties)
        };

        _db.Devices.Add(device);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = device.Id }, new DeviceListDto(device.Id, device.Name));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, DeviceCreateDto dto)
    {
        var device = await _db.Devices.FindAsync(id);
        if (device == null) return NotFound();

        var type = await _db.DeviceTypes.FirstOrDefaultAsync(t => t.Name == dto.DeviceTypeName);
        if (type == null)
        {
            type = new DeviceType { Name = dto.DeviceTypeName };
            _db.DeviceTypes.Add(type);
            await _db.SaveChangesAsync();
        }

        device.Name = dto.Name;
        device.DeviceTypeId = type.Id;
        device.IsEnabled = dto.IsEnabled;
        device.AdditionalProperties = System.Text.Json.JsonSerializer.Serialize(dto.AdditionalProperties);

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var device = await _db.Devices.FindAsync(id);
        if (device == null) return NotFound();

        _db.Devices.Remove(device);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
