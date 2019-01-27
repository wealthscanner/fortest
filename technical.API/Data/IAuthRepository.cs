using System.Threading.Tasks;
using technical.API.Models;

namespace technical.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
         
        Task<User> Login(string username, string password);

        Task<bool> UserExistsAsync(string username);
    }
}