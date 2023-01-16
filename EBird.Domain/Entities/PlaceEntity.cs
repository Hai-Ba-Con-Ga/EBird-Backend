using EBird.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Domain.Entities
{
    [Table("Place")]
    public class PlaceEntity : BaseEntity
    {
        [Required]
        public string Address { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
