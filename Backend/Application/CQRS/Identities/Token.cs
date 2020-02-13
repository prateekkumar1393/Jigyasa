using Application.Interfaces;
using Application.Models;
using Application.Options;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Identities
{
    public class Token
    {
        public async Task<AuthenticationVM> GenerateAuthenticationResultForUserAsync(JwtSettings jwtSettings, User user, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(user.Claims),
                Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = token.Id,
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync(cancellationToken);

            return new AuthenticationVM
            {
                Success = true,
                Data = new AuthenticationResult
                {
                    Token = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken.Token
                }
            };
        }
    }
}
