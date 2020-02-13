using Application.Models;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserManager
    {
        Task<Result<User>> CreateUserAsync(string email, string password);
        Task<Result<User>> LoginUserAsync(string email, string password);
        Task<Result<User>> FindUserByIdAsync(string id);
    }
}
