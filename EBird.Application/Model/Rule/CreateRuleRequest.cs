using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Rule;
public class CreateRuleRequest{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
    
}