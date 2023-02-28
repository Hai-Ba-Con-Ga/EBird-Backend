using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class MatchDetailUpdateDTO
    {
        [Required]
        public Guid MatchBirdId { get; set; }
        [Required]
        public Guid MatchId { get; set; }
        [Required]
        public Guid OldBirdId { get; set; }
        [Required]
        public Guid NewBirdId { get; set; }
    }
}