using Application.Dto.Requests.Passport;

namespace Application.Dto.Requests.Employee;

public class UpdateEmployeeRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public int? CompanyId { get; set; }
    public int? DepartmentId { get; set; }
    public UpdatePassportRequest? Passport { get; set; }
}