using EBird.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Group
{
    public class GroupResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxELO { get; set; }
        public int MinELO { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public Guid CreatedById { get; set; }

    }
}
