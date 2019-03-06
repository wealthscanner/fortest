using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using technical.API.Helpers;
using technical.API.Models;
using technical.API.Models.Yodlee;

namespace technical.API.Data
{
    public class YodleeRepository : IYodleeRepository
    {
        private readonly DataContext _context;

        private static string nodejs_createtoken = "node /home/martin/Dev/fortest/finopt/token.js";


        public YodleeRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<Log> YodleeLogin(string username)
        {
            // execute Node.js file to get an encrypted token
            var token = ShellHelper.Bash(YodleeRepository.nodejs_createtoken)
                .Replace("\n", string.Empty);

            Log log = new Log();
            log.Source = "YodleeLogin." + username;
            log.Text = token;
            log.CId = 1250;   // 1250..Yodlee token (todo: create class for Yodlee)
            log.Stamp = DateTime.Now;
            await this._context.Logs.AddAsync(log);
            await this._context.SaveChangesAsync();

            return log;
        }

        public async Task<Providers> SaveProviderData()
        {
            var log = await this._context.Logs.LastOrDefaultAsync(l => l.CId == 1250);
            string token = log.Text;

            string url_providers = "https://sandbox.api.yodlee.com/ysl/providers";

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url_providers),
                Headers = {
                    { "Authorization", "Bearer " + token }
                },
                Content = new StringContent(@"{}")
            };
            httpRequestMessage.Content.Headers.Add("Api-Version", "1.1");
            var response = client.SendAsync(httpRequestMessage).Result;

            string json = await response.Content.ReadAsStringAsync();
            var providerData = JsonConvert.DeserializeObject<Providers>(json);

            return (providerData);
        }

        public async Task<Accounts> SaveAccountData_User(string username)
        {
            Accounts acc = new Accounts();

            var log = await this._context.Logs.LastOrDefaultAsync(l => l.CId == 1250);
            string token = log.Text;

            string url_providers = "https://sandbox.api.yodlee.com/ysl/accounts";

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url_providers),
                Headers = {
                    { "Authorization", "Bearer " + token }
                },
                Content = new StringContent(@"{}")
            };
            httpRequestMessage.Content.Headers.Add("Api-Version", "1.1");
            var response = client.SendAsync(httpRequestMessage).Result;

            string json = await response.Content.ReadAsStringAsync();
            var accountData = JsonConvert.DeserializeObject<Accounts>(json);

            return (accountData);
        }
        
    }
}

