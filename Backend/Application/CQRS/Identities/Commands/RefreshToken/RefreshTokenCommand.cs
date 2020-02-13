using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Identities.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthenticationVM>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
