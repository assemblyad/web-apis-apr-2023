using HrApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Controllers;

public class EmployeesController : ControllerBase
{
    private readonly HrDataContext _context;

    public EmployeesController(HrDataContext context)
    {
        _context = context;
    }

    [HttpGet("/employees", Name ="employees-get-all")]
    public async Task<ActionResult> GetAllEmployees()
    {
        var results = await _context.Employees
            .Include(e => e.Department)
            .Select(e => new EmployeeSummaryItem
            {
                Id = e.Id.ToString(),
                Name = $"{e.FirstName} {e.LastName}",
                Department = e.Department!.Name
            })
            .ToListAsync(); // TODO: Don't send domain objects to the client!
        return Ok(new {Data = results});
    }
}

public record EmployeeSummaryItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
