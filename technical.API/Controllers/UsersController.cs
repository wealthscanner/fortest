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