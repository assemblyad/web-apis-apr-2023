namespace HrApi.Domain;

public class EmployeeEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DepartmentEntity? Department { get; set; }
    public decimal Salary { get; set; }
    public DateTime HiredOn { get; set; }
}
