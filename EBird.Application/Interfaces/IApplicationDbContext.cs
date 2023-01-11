﻿using EBird.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<PlaceEntity> Places { get; set; }
    }
}
