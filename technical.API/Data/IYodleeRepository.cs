using System.Threading.Tasks;
using technical.API.Models;
using technical.API.Models.Yodlee;

namespace technical.API.Data
{
    public interface IYodleeRepository
    {         
        Task<Log> YodleeLogin(string username);

        Task<Providers> SaveProviderData();

        Task<Accounts> SaveAccountData_User(string username);
    }
}
