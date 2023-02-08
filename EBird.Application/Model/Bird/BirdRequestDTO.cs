using Duende.IdentityServer.Models;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Bird
{
    public class BirdRequestDTO : IMapTo<BirdEntity>
    {
        [Required(ErrorMessage = "Bird name is required")]
        [StringLength(50, ErrorMessage = "Bird name cannot be longer than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bird age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Bird weight is required")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Bird elo is required")]
        public int Elo { get; set; } = 1500;

        [StringLength(50, ErrorMessage = "Bird status cannot be longer than 50 characters")]
        public string? Status { get; set; }

        [StringLength(1000, ErrorMessage = "Bird description cannot be longer than 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Bird color is required")]
        [StringLength(50, ErrorMessage = "Bird color cannot be longer than 50 characters")]
        public string Color { get; set; }

        //forgeinkey
        [Required(ErrorMessage = "Bird type is required")]
        public Guid BirdTypeId { get; set; }

    }
}
