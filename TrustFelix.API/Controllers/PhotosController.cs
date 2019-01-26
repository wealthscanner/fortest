using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrustFelix.API.Data;
using TrustFelix.API.Dtos;
using TrustFelix.API.Helpers;
using TrustFelix.API.Models;

namespace TrustFelix.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IUserRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                this._cloudinaryConfig.Value.CloudName,
                this._cloudinaryConfig.Value.ApiKey,
                this._cloudinaryConfig.Value.ApiSecret
            );

            this._cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{ID}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await this._repo.GetPhoto(id);

            var photo = this._mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
            PhotoForCreationDto photoForCreationDto)
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var userFromRepo = await this._repo.GetUser(userId);

                var file = photoForCreationDto.File;

                var uploadResult = new ImageUploadResult();

                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(200).Crop("fill").Gravity("face").Height(200).Gravity("face")
                        };

                        uploadResult = this._cloudinary.Upload(uploadParams);
                    }
                }

                photoForCreationDto.Url = uploadResult.Uri.ToString();
                photoForCreationDto.PublicId = uploadResult.PublicId;

                var photo = this._mapper.Map<Photo>(photoForCreationDto);

                if (!userFromRepo.Photos.Any(u => u.IsMain))
                    photo.IsMain = true;

                userFromRepo.Photos.Add(photo);

                if (await this._repo.SaveAll())
                {
                    var photoToReturn = this._mapper.Map<PhotoForReturnDto>(photo);
                    return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
                }

                return BadRequest("Could not add the photo");
            }

            [HttpPost("{id}/setMain")]
            public async Task<IActionResult> SetMainPhoto(int userId, int id)
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var user = await this._repo.GetUser(userId);
                if (!user.Photos.Any(p => p.Id == id))
                    return Unauthorized();
                
                var photoFromRepo = await this._repo.GetPhoto(id);

                if (photoFromRepo.IsMain)
                    return BadRequest("This is already the main document");

                var currentMainPhoto = await this._repo.GetMainPhotoForUser(userId);

                currentMainPhoto.IsMain = false;
                photoFromRepo.IsMain = true;

                if (await this._repo.SaveAll())
                    return NoContent();

                return BadRequest("Could not set photo to main");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeletePhoto(int userId, int id)
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var user = await this._repo.GetUser(userId);
                if (!user.Photos.Any(p => p.Id == id))
                    return Unauthorized();
                
                var photoFromRepo = await this._repo.GetPhoto(id);

                if (photoFromRepo.IsMain)
                    return BadRequest("You can not delete the main document");

                if (photoFromRepo.PublicID != null)
                {
                    var deletionParams = new DeletionParams(photoFromRepo.PublicID);
                    var result = this._cloudinary.Destroy(deletionParams);
                    
                    if (result.Result == "ok") {
                        this._repo.Delete(photoFromRepo);
                    }
                }
                else
                    this._repo.Delete(photoFromRepo);

                if (await this._repo.SaveAll())
                    return Ok();

                return BadRequest("Failed to delete the photo");
            }
    }
}