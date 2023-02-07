using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Auth
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set;}
        public string NewPassword { get; set;}
        public string ConfirmPassword { get; set;}
    }
}
