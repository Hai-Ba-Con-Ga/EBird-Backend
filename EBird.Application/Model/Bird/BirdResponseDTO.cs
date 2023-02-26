using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Resource;
using EBird.Domain.Entities;

namespace EBird.Application.Model.Bird
{
    public class BirdResponseDTO : IMapFrom<BirdEntity>
    {
        public int Number { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public int Elo { get; set; } = 1500;
        public string Status { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public BirdRatioDTO Ratio { get; set; }
        public Guid BirdTypeId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public ICollection<ResourceResponse>? ResourceList { get; set; }
    }
}
