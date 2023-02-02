using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model
{
    public class ResetPasswordModel
    {
        public string Password { get; set; }
        public string Confirm { get; set; }
        public string Code { get; set; }
        public Guid AccountId { get; set; }

    }
}
