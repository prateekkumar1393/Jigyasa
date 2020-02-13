using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Identities.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<AuthenticationVM>
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
