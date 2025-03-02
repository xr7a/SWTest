namespace Application.Dto.Responses.Employee;

public class UpdateEmployeeResponse
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string Phone { get; set; }

    public int CompanyId { get; set; }

    public int DepartmentId { get; set; }

    public GetPassportResponse Passport { get; set; }
}