using System;

namespace MistyDaycare.PublicApi.Endpoints.Authorization
{
    public class RegisterResponse : BaseResponse
    {
        public RegisterResponse()
        {
        }

        public RegisterResponse(Guid correlationId) : base(correlationId)
        {
        }

        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
