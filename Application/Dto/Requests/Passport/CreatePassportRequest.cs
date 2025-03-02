using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Requests.Passport;

public class CreatePassportRequest
{
    [Required(ErrorMessage = "Тип документа обязателен.")]
    public string Type { get; set; }
    
    [Required(ErrorMessage = "Номер документа обязателен.")]
    [RegularExpression(@"^\d{4} \d{6}$", ErrorMessage = "Номер паспорта должен быть в формате 'XXXX XXXXXX'.")]
    public string Number { get; set; }
}