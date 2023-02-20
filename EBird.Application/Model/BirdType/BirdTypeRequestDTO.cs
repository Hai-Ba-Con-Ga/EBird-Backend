using System.ComponentModel.DataAnnotations;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.BirdType
{
    public class BirdTypeRequestDTO : IMapTo<BirdTypeEntity>
    {
        [Required(ErrorMessage = "Bird type name is required")]
        [StringLength(50, ErrorMessage = "Bird type name cannot be longer than 50 characters")]
        public string TypeCode { get; set; }

        [Required(ErrorMessage = "Type name is required")]
        [StringLength(100, ErrorMessage = "Type name cannot be longer than 100 characters")]
        public string TypeName { get; set; }
    }
}
