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
}
