using EBird.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model
{
    public class GroupDTO
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
       
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Max ELO must be greater than 0")]
        public int MaxELO { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Min ELO must be greater than 0")]
        public int MinELO { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        public DateTime? CreateDatetime { get; set; }

        //forgein key

        public Guid CreatedById { get; set; }

    }
}
