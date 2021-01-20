using System.ComponentModel.DataAnnotations;

namespace MistyDaycare.PublicApi.Endpoints.Authorization
{
    public class AuthenticateRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}