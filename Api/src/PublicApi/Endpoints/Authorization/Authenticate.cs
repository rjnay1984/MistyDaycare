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
    public class Authenticate : BaseAsyncEndpoint<AuthenticateRequest, AuthenticateResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenClaimsService _tokenClaimsService;

        public Authenticate(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, IMapper mapper, ITokenClaimsService tokenClaimsService)
        {
            _tokenClaimsService = tokenClaimsService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("api/authenticate")]
        [SwaggerOperation(
            Summary = "Authenticates a user.",
            OperationId = "account.authenticate",
            Tags = new[] { "AuthorizationEndpoint" }
        )]
        public override async Task<ActionResult<AuthenticateResponse>> HandleAsync(AuthenticateRequest request, CancellationToken cancellationToken = default)
        {
            var response = new AuthenticateResponse();
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

            if (user == null) return BadRequest("Invalid Username.");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

            if (!result.Succeeded) return Unauthorized();

            response.UserName = request.UserName;
            response.Token = await _tokenClaimsService.GetTokenAsync(request.UserName);

            return response;
        }
    }
}