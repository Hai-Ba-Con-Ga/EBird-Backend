using System.ComponentModel.DataAnnotations;
using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class MatchResourceEntity : BaseEntity
{
    [Required]
    public Guid MatchDetailId { get; set; }
    //1 - M relationship Bird
    public MatchDetailEntity MatchDetail { get; set; } = null!;
    
    [Required]
    public Guid ResourceId { get; set; }
    //1- M relationship Resource
    public ResourceEntity Resource { get; set; } = null!;
}