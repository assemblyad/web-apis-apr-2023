using HrApi.Models;

namespace HrApi.Domain;

public interface IManageHiringRequests
{
    Task<bool> AssignSalaryAsync(int id, decimal salary);
    Task<(bool WasFound, int Id)> AssignToDeparment(int departmentId, HiringRequestResponseModel request);
    Task<HiringRequestResponseModel> CreateHiringRequestAsync(HiringRequestCreateModel request);
    Task<HiringRequestResponseModel?> GetHiringRequestByIdAsync(int id);
    Task<HiringRequestSalaryModel?> GetSalaryForAsync(int id);
}
