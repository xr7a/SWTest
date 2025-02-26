using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DbModels;

[Table("employees")]
public class DbEmployee
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required, MaxLength(100)]
    public string Surname { get; set; }

    [Required, MaxLength(20)]
    public string Phone { get; set; }

    [Required]
    public int CompanyId { get; set; }
    
    [ForeignKey("departments")]
    public int DepartmentId { get; set; }
}