using System.ComponentModel.DataAnnotations;

namespace EBird.Api.UserFeatures.Requests
{
    public class CheckEmailRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
