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

namespace EBird.Application.Model.BirdType
{
    public class BirdTypeResponseDTO : IMapFrom<BirdTypeEntity>
    {
        public Guid Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public DateTime CreatedDatetime { get; set; }
    }
}
