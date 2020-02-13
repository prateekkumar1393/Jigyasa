using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Persistense.Identity
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<User>> CreateUserAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return Result<User>.Failure(new string[] { "User with this email address already exists" });
            }

            var newUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return Result<User>.Failure(createdUser.Errors.Select(x => x.Description));
            }

            User user = await getClaims(newUser);
            return Result<User>.Success(user);
        }

        public async Task<Result<User>> LoginUserAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                return Result<User>.Failure(new[] { "User does not exist" });
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(existingUser, password);

            if (!userHasValidPassword)
            {
                return Result<User>.Failure(new[] { "User/password combination is wrong" });
            }

            User user = await getClaims(existingUser);
            return Result<User>.Success(user);
        }

        public async Task<Result<User>> FindUserByIdAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                return Result<User>.Failure(new[] { "User does not exist" });
            }

            User user = await getClaims(existingUser);
            return Result<User>.Success(user);
        }

        private async Task<User> getClaims(IdentityUser iUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, iUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, iUser.Email),
                new Claim("id", iUser.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(iUser);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(iUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            return new User()
            {
                Id = iUser.Id,
                Email = iUser.Email,
                Claims = claims
            };
        }
    }
}
