using System.Collections.Generic;
using System.Security.Claims;

namespace Application.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
