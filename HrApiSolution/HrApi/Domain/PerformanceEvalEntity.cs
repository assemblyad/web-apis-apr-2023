namespace HrApi.Domain;

public class PerformanceEvalEntity
{
    public int Id { get; set; }
    public EmployeeEntity Employee { get; set; } = new();
    public int Rating { get; set; }
}
