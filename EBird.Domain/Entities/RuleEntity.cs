using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EBird.Domain.Entities;

[Table("Rule")]
public class RuleEntity : BaseEntity{
    
    public Guid CreateById { get; set; }
    public AccountEntity Account { get; set; } = null!;
    [Column(TypeName = "text")]
    public string? Content { get; set; }
    [Column(TypeName = "nvarchar")]
    [MaxLength(100)]
    public string? Title { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime CreateDateTime { get; set; } = DateTime.Now;


}