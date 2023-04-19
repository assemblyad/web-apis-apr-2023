using HrApi.Models;

namespace HrApi.Domain;

public interface IManageHiringRequests
{
    Task<bool> AssignSalaryAsync(int id, decimal salary);
    Task<HiringRequestResponseModel> CreateHiringRequestAsync(HiringRequestCreateModel request);
    Task<HiringRequestResponseModel?> GetHiringRequestByIdAsync(int id);
}
