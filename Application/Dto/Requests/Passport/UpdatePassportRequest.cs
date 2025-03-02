using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Requests.Passport;

public class UpdatePassportRequest
{
    public string? Type { get; set; }
    
    [RegularExpression(@"^\d{4} \d{6}$", ErrorMessage = "Номер паспорта должен быть в формате 'XXXX XXXXXX'.")]
    public string? Number { get; set; }
}