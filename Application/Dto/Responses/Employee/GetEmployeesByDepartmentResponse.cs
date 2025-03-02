using Application.Dto.Responses.Department;

namespace Application.Dto.Responses.Employee;

public class GetEmployeesByDepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int CompanyId { get; set; }

    public GetPassportResponse Passport { get; set; }

    public GetDepartmentResponse Department { get; set; }
}