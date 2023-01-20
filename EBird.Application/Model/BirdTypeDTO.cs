using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model
{
    public class BirdTypeDTO
    {
        [Required(ErrorMessage = "Bird type name is required")]
        [StringLength(50, ErrorMessage = "Bird type name cannot be longer than 50 characters")]
        public string TypeCode { get; set; }
        
        [Required(ErrorMessage = "Type name is required")]
        [StringLength(100, ErrorMessage = "Type name cannot be longer than 100 characters")]
        public string TypeName { get; set; }
        
        [Required(ErrorMessage = "Create datetime is required")]
        public DateTime CreatedDatetime { get; set; }
    }
}
