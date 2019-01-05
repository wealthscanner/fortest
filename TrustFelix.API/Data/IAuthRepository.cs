using System.Threading.Tasks;
using TrustFelix.API.Models;

namespace TrustFelix.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
         
        Task<User> Login(string username, string password);

        Task<bool> UserExistsAsync(string username);
    }
}