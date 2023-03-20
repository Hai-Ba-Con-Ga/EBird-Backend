using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Place
{
    public class PlaceRequestDTO : IMapTo<PlaceEntity>
    {

        [StringLength(150)]
        [Required]
        public string Address { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [DefaultValue(false)]
        public bool FavoritePlace { get; set; }
    }
}