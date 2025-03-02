using System.ComponentModel.DataAnnotations;
using Application.Dto.Requests.Passport;

namespace Application.Dto.Requests.Employee;

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 50 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от 2 до 50 символов")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Телефон обязателен.")]
    [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Телефон должен быть в формате +7XXXXXXXXXX")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Айди компании обязателен")]
    [Range(1, int.MaxValue - 1, ErrorMessage = "CompanyId должен быть больше 0")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "Айди отдела обязателен")]
    [Range(1, int.MaxValue - 1, ErrorMessage = "DepartmentId должен быть больше 0")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Паспортные данные обязательны")]
    public CreatePassportRequest Passport { get; set; }
}