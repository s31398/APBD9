using DeviceApi.Dtos;
using DeviceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeListDto>>> Get()
    {
        var list = await _db.Employees
            .Include(e => e.Person)
            .Select(e => new EmployeeListDto(e.Id, e.Person.FirstName + " " + e.Person.LastName))
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> Get(int id)
    {
        var e = await _db.Employees
            .Include(emp => emp.Person)
            .Include(emp => emp.Position)
            .FirstOrDefaultAsync(emp => emp.Id == id);

        if (e == null) return NotFound();

        var dto = new EmployeeDetailDto(
            e.Id,
            e.Person.PassportNumber,
            e.Person.FirstName,
            e.Person.MiddleName,
            e.Person.LastName,
            e.Person.PhoneNumber,
            e.Person.Email,
            e.Salary,
            new { e.Position.Id, e.Position.Name },
            e.HireDate
        );

        return Ok(dto);
    }
}
