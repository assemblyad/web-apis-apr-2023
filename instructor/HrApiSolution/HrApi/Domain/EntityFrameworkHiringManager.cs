using AutoMapper;
using AutoMapper.QueryableExtensions;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Domain;

public class EntityFrameworkHiringManager : IManageHiringRequests
{
    private readonly HrDataContext _context;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _config;

    public EntityFrameworkHiringManager(HrDataContext context, IMapper mapper, MapperConfiguration config)
    {
        _context = context;
        _mapper = mapper;
        _config = config;
    }

    public async Task<bool> AssignSalaryAsync(int id, decimal salary)
    {
        var hiringRequest = await _context.HiringRequests.SingleOrDefaultAsync(h => 
            h.Id == id && h.Status == HiringRequestStatus.AwaitingSalary); 
        if(hiringRequest != null)
        {
            hiringRequest.Salary = salary;
            hiringRequest.Status = HiringRequestStatus.AwaitingDepartment;
            await _context.SaveChangesAsync();
            return true;
        } else
        {
            return false;
        }
    }

    public async Task<(bool WasFound, int Id)> AssignToDeparment(int departmentId, HiringRequestResponseModel request)
    {
        // make sure that department exists.
        var department = await _context.Departments.SingleOrDefaultAsync(d => d.Id == departmentId);
        if(department is null) { return (false, 0); }

        // make sure the hiring request exists and is in the state of waiting for a department.
        var hiringRequest = await _context.HiringRequests.SingleOrDefaultAsync(r => 
        r.Id == int.Parse(request.Id) && r.Status == HiringRequestStatus.AwaitingDepartment);
        if(hiringRequest is null) { return (false, 0); }
        // Make the hiring request approved.
        hiringRequest.Status = HiringRequestStatus.Hired;
        // create an employee out of it.
        var employee = new EmployeeEntity
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Department = department,
            HiredOn = DateTime.Now,
            Salary = hiringRequest.Salary
        };
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return (true, employee.Id);
    }

    public async Task<HiringRequestResponseModel> CreateHiringRequestAsync(HiringRequestCreateModel request)
    {
        var entity = _mapper.Map<HiringRequestEntity>(request);
        _context.HiringRequests.Add(entity);
        await _context.SaveChangesAsync();
        var response = _mapper.Map<HiringRequestResponseModel>(entity);
        //// HiringRequestModel => HiringRequestEntity
        return response;
    }

    public async Task<HiringRequestResponseModel?> GetHiringRequestByIdAsync(int id)
    {
        var response = await _context.HiringRequests
               .Where(req => req.Id == id)
              .ProjectTo<HiringRequestResponseModel>(_config)
              .SingleOrDefaultAsync();
        return response;
    }

    public async Task<HiringRequestSalaryModel?> GetSalaryForAsync(int id)
    {
       var salary = await _context.HiringRequests
            .Where(r => r.Id == id && r.Status != HiringRequestStatus.AwaitingSalary)
            .Select(r => r.Salary).SingleOrDefaultAsync();
        if (salary != 0)
        {
            return new HiringRequestSalaryModel { Salary = salary };
        } else
        {
            return null;
        }
    }
}
