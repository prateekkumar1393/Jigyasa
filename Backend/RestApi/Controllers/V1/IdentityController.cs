using Application.CQRS.Identities.Commands.RegisterUser;
using Application.CQRS.Identities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Application.CQRS.Identities.Commands.LoginUser;
using Application.CQRS.Identities.Commands.RefreshToken;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers.V1
{
    public class IdentityController : BaseController
    {
        public IdentityController(IMediator mediator) : base(mediator) { }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(AuthenticationVM))]
        public async Task<ActionResult<AuthenticationVM>> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationVM
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            AuthenticationVM authenticationVM = await CommandAsync(command);

            if (!authenticationVM.Success)
            {
                return BadRequest(authenticationVM);
            }

            return Ok(authenticationVM);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(AuthenticationVM))]
        public async Task<ActionResult<AuthenticationVM>> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationVM
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            AuthenticationVM authenticationVM = await CommandAsync(command);

            if (!authenticationVM.Success)
            {
                return BadRequest(authenticationVM);
            }

            return Ok(authenticationVM);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(AuthenticationVM))]
        public async Task<ActionResult<AuthenticationVM>> Refresh([FromBody] RefreshTokenCommand command)
        {
            AuthenticationVM authenticationVM = await CommandAsync(command);

            if (!authenticationVM.Success)
            {
                return BadRequest(authenticationVM);
            }

            return Ok(authenticationVM);
        }
    }
}
