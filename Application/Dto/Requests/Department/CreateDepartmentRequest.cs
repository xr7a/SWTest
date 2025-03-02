using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Requests.Department;

public class CreateDepartmentRequest
{
    [Required(ErrorMessage = "Название отдела обязателен")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно содержать от 3 до 100 символов.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Телефон отдела обязателен")]
    [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Телефон должен быть в формате +7XXXXXXXXXX.")]
    public string Phone { get; set; }
}