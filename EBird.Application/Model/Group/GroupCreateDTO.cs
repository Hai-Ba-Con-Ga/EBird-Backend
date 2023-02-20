using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Group
{
    public class GroupCreateDTO : GroupRequestDTO
    {
        public Guid? CreatedById { get; set; }
    }
}
