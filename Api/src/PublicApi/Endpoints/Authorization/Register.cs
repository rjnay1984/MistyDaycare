using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistyDaycare.ApplicationCore.Interfaces;
using MistyDaycare.Infrastructure.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace MistyDaycare.PublicApi.Endpoints.Authorization
{
    public class Register : BaseAsyncEndpoint<RegisterRequest, RegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        public Register(UserManager<ApplicationUser> userManager, ITokenClaimsService tokenClaimsService, IMapper mapper)
        {
            _tokenClaimsService = tokenClaimsService;
            _userManager = userManager;
        }

        [HttpPost("api/register")]
        [SwaggerOperation(
            Summary = "Registers a user.",
            OperationId = "Authorization.Register",
            Tags = new[] { "AuthorizationEndpoint" }
        )]
        public override async Task<ActionResult<RegisterResponse>> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var response = new RegisterResponse(request.CorrelationId());

            if (await UserExists(request.UserName)) return BadRequest("Username is taken.");

            var user = new ApplicationUser();

            user.UserName = request.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            response.UserName = request.UserName;
            response.Token = await _tokenClaimsService.GetTokenAsync(request.UserName);

            return response;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
