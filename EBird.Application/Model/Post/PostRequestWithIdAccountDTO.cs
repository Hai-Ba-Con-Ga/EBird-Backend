using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using EBird.Application.Model.Resource;

namespace EBird.Application.Model.Post
{
    public class PostRequestWithIdAccountDTO : IMapTo<PostEntity>
    {
        [Required(ErrorMessage = "Content is required")]
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;

        public Guid CreateById { get; set; }

        public ResourceCreateDTO? Thumbnail { get; set; }
    }
}
