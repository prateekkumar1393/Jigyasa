using MediatR;

namespace Application.CQRS.Identities.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<AuthenticationVM>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
