using AutoMapper;
using AutoMapper.QueryableExtensions;
using HrApi.Domain;
using HrApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Controllers;

public class DepartmentsController : ControllerBase
{

    private readonly HrDataContext _context;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _config;

    public DepartmentsController(HrDataContext context, IMapper mapper, MapperConfiguration config)
    {
        _context = context;
        _mapper = mapper;
        _config = config;
    }

    [HttpDelete("/departments/{id:int}")]
    public async Task<ActionResult> RemoveDepartment(int id)
    {
        var department = await _context.GetActiveDepartments()
            .SingleOrDefaultAsync(d => d.Id == id);
        if (department != null)
        {
            department.Removed = true;
            //_context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
        return NoContent(); // 204 - success, but no content (body)
    }

    [HttpPost("/departments")]
    [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult> AddADepartment([FromBody] DepartmentCreateRequest request)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 400

        }

        var departmentToAdd = _mapper.Map<DepartmentEntity>(request);

        _context.Departments.Add(departmentToAdd);
        try
        {
            await _context.SaveChangesAsync();
            var response = _mapper.Map<DepartmentSummaryItem>(departmentToAdd);
            return CreatedAtRoute("get-department-by-id", new { id = response.Id }, response);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest("That Department Exists");
        }
    }


    // GET /departments
    [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
    [HttpGet("/departments")]
    public async Task<ActionResult<DepartmentsResponse>> GetDepartments()
    {
        var response = new DepartmentsResponse
        {
            Data = await _context.GetActiveDepartments()
                .ProjectTo<DepartmentSummaryItem>(_config)
                .ToListAsync()
        };
        return Ok(response);
    }
    // GET /departments/8

    [HttpGet("/departments/{id:int}", Name = "get-department-by-id")]
    public async Task<ActionResult> GetDepartmentById(int id)
    {
        var response = await _context.GetActiveDepartments()
             .Where(dept => dept.Id == id)
             .ProjectTo<DepartmentSummaryItem>(_config)
             .SingleOrDefaultAsync();
        if (response is null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }
}
