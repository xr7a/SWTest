using System.ComponentModel.DataAnnotations;
using Application.Dto.Requests.Passport;

namespace Application.Dto.Requests;

public class CreateEmployeeRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public int CompanyId { get; set; }
    [Required]
    public int DepartmentId { get; set; }
    [Required]
    public CreatePassportRequest Passport { get; set; }
}