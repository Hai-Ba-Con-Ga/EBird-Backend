﻿using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Group
{
    public class GroupRequestDTO : IMapTo<GroupEntity>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Max ELO must be greater than 0")]
        public int MaxELO { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Min ELO must be greater than 0")]
        public int MinELO { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }
    }
}
