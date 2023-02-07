﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Bird
{
    public class BirdResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public int Elo { get; set; } = 1500;
        public string Status { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public Guid BirdTypeId { get; set; }
        public Guid OwnerId { get; set; }
    }
}