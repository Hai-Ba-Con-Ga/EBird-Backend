using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EBird.Domain.Entities
{
    [Table("Match_Resource")]
    public class MatchResourceEntity : BaseEntity
    {
        //PK
        [ForeignKey("MatchId")]
        public Guid MatchId { get; set; }
        public MatchEntity Match { get; set; } = null!;

        //PK
        [ForeignKey("ResourceId")]
        public Guid ResourceId { get; set; }
        public ResourceEntity Resource { get; set; } = null!;
    }
}
