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

namespace technical.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    // IUserRepository = IDatingRepository

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await this._repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;

            // if not set, then use gender of logged in user
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender;
            }

            var users = await this._repo.GetUsers(userParams);
            var usersToReturn = this._mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount,
                users.TotalPages);
            return Ok(usersToReturn);
        }
        

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this._repo.GetUser(id);
            var userToReturn = this._mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

/*
        [HttpGet("{getyodlee}")]
        public async Task<IActionResult> GetYodlee()
        {
            var user = await this._repo.GetUser(33);

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

            Int32 unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var header = "{\"alg\":\"RS512\",\"typ\":\"JWT\"}";
            var claims = "{\"iss\":\"c82579bd-7217-43c7-a6c5-fa9a75746b9a\","
                + "\"iat\":" + unixTimeStamp + ",\"exp\":" + (unixTimeStamp + 900)
                + ",\"sub\":\"sbMem5c73e905ecbda1\"}";

            var b64header = Convert.ToBase64String(Encoding.UTF8.GetBytes(header))
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
            var b64claims = Convert.ToBase64String(Encoding.UTF8.GetBytes(claims))
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            var payload = b64header + "." + b64claims;
            Console.WriteLine("JWT without sig:    " + payload);

            // privateKey
            byte[] key = Convert.FromBase64String(privateKey);
            byte[] message = Encoding.UTF8.GetBytes(payload);

            string sig = Convert.ToBase64String(HashHMAC(key, message))
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            Console.WriteLine("JWT with signature: " + payload + "." + sig);

            YodleeInfoDto yid = new YodleeInfoDto();
            yid.Info = payload + "." + sig;
            yid.Message = "test message";

            return Ok(yid);
        }

    private static byte[] HashHMAC(byte[] key, byte[] message)
    {
        var hash = new HMACSHA512(key);
        return hash.ComputeHash(message);
    }
*/

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await this._repo.GetUser(id);

            this._mapper.Map(userForUpdateDto, userFromRepo);

            Log lg = new Log();
            lg.Text = "Introduction: " + userForUpdateDto.Introduction + ", LookingFor: " + userForUpdateDto.LookingFor
                + ", City: " + userForUpdateDto.City + ", Country: " + userForUpdateDto.Country;
            this._repo.Add(lg);

            if (await this._repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/sell/{assetId}")]
        public async Task<IActionResult> SellAsset(int id, int assetId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var sell = await this._repo.GetSell(id, assetId);

            if (sell != null)
                return BadRequest("Asset already added to SalesCloud");

            if (await this._repo.GetUser(assetId) == null)
                return NotFound();

            sell = new Sell
            {
                SellerId = id,
                AssetId = assetId
            };

            this._repo.Add<Sell>(sell);
            if (await this._repo.SaveAll())
                return Ok();

            return BadRequest("Asset sell FAILED");
        }

    }
}