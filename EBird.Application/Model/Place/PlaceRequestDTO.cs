using System;
using System.Collections.Generic;
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


        [StringLength(100)]
        [Required]
        public string Longitude { get; set; }


        [StringLength(100)]
        [Required]
        public string Latitude { get; set; }
    }
}