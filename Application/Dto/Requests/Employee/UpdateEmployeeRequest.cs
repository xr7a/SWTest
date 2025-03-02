using System.ComponentModel.DataAnnotations;
using Application.Dto.Requests.Passport;

namespace Application.Dto.Requests.Employee;

public class UpdateEmployeeRequest
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 50 символов")]
    public string? Name { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 50 символов")]
    public string? Surname { get; set; }

    [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Телефон должен быть в формате +7XXXXXXXXXX")]
    public string? Phone { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CompanyId должен быть больше 0")]
    public int? CompanyId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "DepartmentId должен быть больше 0")]
    public int? DepartmentId { get; set; }

    public UpdatePassportRequest? Passport { get; set; }
}