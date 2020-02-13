using Application.Interfaces;
using Application.Options;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identities.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthenticationVM>
    {
        private readonly IUserManager _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IApplicationDbContext _context;

        public LoginUserCommandHandler(IUserManager userManager, JwtSettings jwtSettings, IApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<AuthenticationVM> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManager.LoginUserAsync(request.Email, request.Password);

            if (!result.Succeeded)
            {
                return new AuthenticationVM
                {
                    Errors = result.Errors
                };
            }

            return await new Token().GenerateAuthenticationResultForUserAsync(_jwtSettings, result.Data, _context, cancellationToken);
        }
    }
}
