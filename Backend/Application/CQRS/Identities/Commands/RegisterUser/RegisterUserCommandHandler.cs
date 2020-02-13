using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Options;
using MediatR;

namespace Application.CQRS.Identities.Commands.RegisterUser
{
    class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthenticationVM>
    {
        private readonly IUserManager _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IApplicationDbContext _context;
        public RegisterUserCommandHandler(IUserManager userManager, JwtSettings jwtSettings, IApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<AuthenticationVM> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userManager.CreateUserAsync(request.Email, request.Password);

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
