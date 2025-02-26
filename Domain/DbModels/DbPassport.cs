using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DbModels;

[Table("passports")]
public class DbPassport
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("employees")]
    public int EmployeeId { get; set; } 

    [Required, MaxLength(50)]
    public string Type { get; set; }

    [Required, MaxLength(50)]
    public string Number { get; set; }
}