// using EBird.Application.Interfaces.IMapper;
// using EBird.Domain.Entities;
// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace EBird.Application.Model.Request
// {
//     public class RequestUpdateDTO : IMapTo<RequestEntity>
//     { 
//         [Required]
//         public DateTime RequestDatetime { get; set; }

//         [StringLength(50)]
//         [Required]
//         public string Status { get; set; }
        
//         [Required]
//         public Guid BirdId { get; set; }

//         [Required]
//         public Guid PlaceId { get; set; }

//     }
// }
