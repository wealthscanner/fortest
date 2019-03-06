using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using technical.API.Data;
using technical.API.Dtos;
using technical.API.Helpers;
using technical.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace technical.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class YodleeController : ControllerBase
    {
        private readonly IYodleeRepository _repo;
        private readonly IMapper _mapper;

        private string _token;

        public YodleeController(IYodleeRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;

        }

        [HttpPost("{login}")]
        public async Task<IActionResult> Login(string username)
        {
            var log = await this._repo.YodleeLogin("todo: username");

            return Ok(log);
        }

        [HttpGet("providers")]
        public async Task<IActionResult> Providers()
        {
            var prov = await this._repo.SaveProviderData();

            return Ok(prov);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> Accounts()
        {
            var acc = await this._repo.SaveAccountData_User("todo: username");

            return Ok(acc);
        }

        [HttpGet("{testing}")]
        public async Task<IActionResult> Testing()
        {

            Int32 unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var claims = new[]
            {
                new Claim("iss", "c82579bd-7217-43c7-a6c5-fa9a75746b9a"),
                new Claim("iat", unixTimeStamp.ToString(), ClaimValueTypes.Integer32),
                new Claim("exp", (unixTimeStamp + 900).ToString(), ClaimValueTypes.Integer32),
                new Claim("sub", "sbMem5c73e905ecbda1")
            };

            string privateKey = @"
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCP4HZL3j1uJ9NIuriNpCdLwLfI
cJJCgQ5gMbuXLhQgVt7ShNw20ig3OknI5X9V73WLoq8epXfhxLe1/mx/HYATta84jbG9UCKKWg5h
YHB2kJuOXm97ossV2ydqyFmIHB6aESWvjPZ5O4d4+ieSUPuhujtCI1Ca3vINEptfGHs8wxLhu5V0
ROxwbC2/14CJQwFPROKSudkdKq3cllGb8j0HzkKLED65pvqhqS6WjsD0tif5RsFo7fUyLV2gTvwx
ecUNYepoix6XfoO741TU9FLghFjJuUiFxJpLDZ7ROzA31K0DPNfVeUmCq3JIADTAC6nHU8Lhl3F1
eVAK7gy9LJFhAgMBAAECggEAR4f6aHfipMyqEAO9U7ZkTxZOiRnmrafKROsU7/HZ0cY+2/1wGtVb
ZzHUk0v+hfQaxcSpOhxcQOPzzkjjGkPJdIMkzYIs9wlLUuu5MJKo91mn7R1oYdirDX+61i0rzi7k
zAZwbRoxRE7jaLM+T1Fx+ZNEePpjzcvAHd29X4clZix7CYOsMf/DTxbdf3tN6P/bdbtVCGGrkfIB
pUF4FD09qQlUZ4DDbEnklT6VULjv0tm80YAyzCzOfDrPQgb6cFHsXMp6Zi50/wNrnp15Ir0onoIb
P+krWQKXtBFqQpyEqKVU0w0R+Qjtkh3dhvtXcn78btgaFkrQV4oAOElt4kWM0QKBgQDNJo7cDSu3
bZmEWd/ayHNKmKL+lG7lHr9uyqCFd5slEN2kvjqNSuBzm8UcONnm97AOcnCOZ2jr6lH5pzMdx34V
7upw/XY1ANGvfk8XZ59qGG742ecBvDlRnUWsO+PxOwbiEbuNTyqZSttdPdJFbhEW12SgAzUKRhXd
+ZEE2EEdRQKBgQCzieQS+zlg5IqL/sbmg84BU0q6fOCAO+Dr91MSSExsvuurxVyOgC8OWzcPMIED
+iLNrLrvAHxVYcZCdP5y9hVy/iMRCugym0SiYXHkSBB1IZlNGI5QCo9fKPO7NUDQBhk/Do9/AfOX
V8VmT/MJVedhVpZuzVf41zNM9yndmmTfbQKBgQC0nYTbIdFiTeEB0CqwsRgoDdg68K3tlLOPtF+9
Af+ak42/9CcSrGCOCA9y+G6H1XuwTHriNRL/2S3Q8a6kQjW008KeNdizc4Qo0LiLb5S/UMGq4BVs
xq1kOGXV7GiTwpcdw+Tu1Us46NnW7o3IyM3M4VfbNNemsuufoZBWxpoVTQKBgAZm92QZ40zqOWqO
lkcoEhOIBdUqmNLZz8Z6VlMDkv4ZvMuuSQOn3IW1iPwYrbEXnWAaNbxKFyTwTKKYC27MCa3FFDkK
W4dadBMaSHZsf3G91fqi0ohWKBCrpC1b08jXPtU5zrInvqj570cbuL3ve0XEUa730ZhFDEZFVg/Z
7pjtAoGBAJRIYSkPPhbnlHUILwU1FtjebbuNnqUhcclVn0vAhupmQgPpWJFVULBThGGY95RC+NMs
MTlWhKmCFrIimNEBqp+rA5GFu8957IaC7XtxzUc84zbLaH0aEs9Qit/k3fZBrwdH3NcleQfmU0t4
XA7YlQ8eX6+F5YtxemVkU/8GHLJ9";

            RSA rsa = new RSACryptoServiceProvider(2048);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            
            var creds = new SigningCredentials(
                new RsaSecurityKey(rsa), 
                SecurityAlgorithms.RsaSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds
            );

            var ret_token = new JwtSecurityTokenHandler().WriteToken(token);

            YodleeInfoDto yid = new YodleeInfoDto();
            yid.Info = ret_token;
            yid.Message = "token created";

            return Ok(yid);
        }
    }
}

