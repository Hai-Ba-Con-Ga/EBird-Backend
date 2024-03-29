﻿using AutoMapper;
using EBird.Application.Interfaces.IMapper;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class RequestUpdateDTO : IMapTo<RequestEntity>
    {
        [Required]
        public DateTime RequestDatetime { get; set; }

        [Required]
        public Guid HostBirdId { get; set; }

        [Required]
        public Guid PlaceId { get; set; }
    }
}
