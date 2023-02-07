using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Auth
{
    public class SendForgotPasswordModel
    {
        public string ResetPasswordLink { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Code { get; set; }
        public string Email { get; set; }
    }
}
