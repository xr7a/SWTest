using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DbModels;

[Table("departments")]
public class DbDepartment
{
    [Key] public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string Name { get; set; }

    [Required, MaxLength(50)]
    public string Number { get; set; }
}