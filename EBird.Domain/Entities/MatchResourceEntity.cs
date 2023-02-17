using System.ComponentModel.DataAnnotations;
using EBird.Domain.Common;

namespace EBird.Domain.Entities;
public class MatchResourceEntity : BaseEntity
{
    [Required]
    public Guid MatchBirdId { get; set; }
    //1 - M relationship Bird
    public MatchBirdEntity MatchBird { get; set; } = null!;
    [Required]
    public Guid ResourceId { get; set; }
    //1- M relationship Resource
    public ResourceEntity Resource { get; set; } = null!;
}