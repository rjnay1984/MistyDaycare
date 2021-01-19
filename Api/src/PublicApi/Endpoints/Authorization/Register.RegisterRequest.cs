using System.ComponentModel.DataAnnotations;

namespace MistyDaycare.PublicApi.Endpoints.Authorization
{
    public class RegisterRequest : BaseRequest
    {
        [Required]
        public string UserName { get; set; }
        // TODO: Add length check here.
        [Required]
        public string Password { get; set; }
    }
}
