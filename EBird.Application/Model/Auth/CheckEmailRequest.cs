using System.ComponentModel.DataAnnotations;

namespace EBird.Application.Model.Auth
{
    public class CheckEmailRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
