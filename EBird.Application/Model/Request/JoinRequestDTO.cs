using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Request
{
    public class JoinRequestDTO
    {
        [Required]
        public Guid ChallengerBirdId { get; set; }
    }
}