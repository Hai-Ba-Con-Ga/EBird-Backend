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
    public class PostResponseDTO : IMapFrom<PostEntity>
    {

        public Guid Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public DateTime CreateDateTime { get; set; }

        public Guid CreateById { get; set; }

        public ResourceResponse? Thumbnail { get; set; }
    }
}
