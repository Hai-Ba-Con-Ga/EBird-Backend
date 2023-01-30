using EBird.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IEmailServices
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendForgotPassword(SendForgotPasswordModel request);
    }
}
