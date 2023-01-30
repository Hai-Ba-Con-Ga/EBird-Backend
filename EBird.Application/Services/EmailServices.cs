using EBird.Application.AppConfig;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace EBird.Application.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly MailSetting _mailSetting;
        public EmailServices(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        private string CreateForgoPasswordEmailBody(SendForgotPasswordModel request)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(".\\Templates\\Fotgot_Password_Template.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{fullName}", request.FirstName);

            body = body.Replace("{code}", request.Code);

            body = body.Replace("{username}", request.UserName);

            body = body.Replace("{resetPasswordLink}", request.ResetPasswordLink);

            return body;

        }
        public async Task SendForgotPassword(SendForgotPasswordModel request)
        {
            var mail =  new MailRequest()
            {
                ToEmail = request.Email,
                Body= CreateForgoPasswordEmailBody(request),
                Subject = "Đổi lại mật khẩu"

            };
            await SendEmailAsync(mail);
        }
    }
}
