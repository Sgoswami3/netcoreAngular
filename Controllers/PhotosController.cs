using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatingApp.API.Controllers
{
    [Route("api/users/{userID}/photos")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly Cloudinary _Cloudinary;
        private IDatingRepository _Repo { get; }
        private IMapper _Mapper { get; }
        private IOptions<CloudinarySettings> _CloudinaryConfig { get; }

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig )
        {
            _Repo = repo;
            _Mapper = mapper;
            _CloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _CloudinaryConfig.Value.CloudName,
                _CloudinaryConfig.Value.ApiKey,
                _CloudinaryConfig.Value.ApiSecret
                );

            _Cloudinary = new Cloudinary(account);

        }


        [HttpGet("{id}",Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _Repo.GetPhoto(id);
            var photo = _Mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }




        [HttpPost]
        public async Task<IActionResult> addPhotoForUser(int userID, [FromForm]PhotoForCreationDto photoForCreationDto )
        {
            if (userID != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userFromRepo = await _Repo.Getuser(userID);
            var file = photoForCreationDto.file;
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                uploadResult = _Cloudinary.Upload(uploadParams);

                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;
            var photo = _Mapper.Map<Photo>(photoForCreationDto);

            if(!userFromRepo.photos.Any(u =>  u.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.photos.Add(photo);

        

            if(await _Repo.SaveAll())
            {
                var photoToReturn = _Mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id}, photoToReturn);
            }

            return BadRequest("Cloud not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> setMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _Repo.Getuser(userId);
            if(!user.photos.Any(p=> p.Id == id))
            {
                return Unauthorized();
            }

            var photofromRepo = await _Repo.GetPhoto(id);
            if (photofromRepo.IsMain)
                return BadRequest("this is already the main Photo");

            var currentMainphoto = await _Repo.GetMainPhotoForUser(userId);
            currentMainphoto.IsMain = false;

            photofromRepo.IsMain = true;

            if(await _Repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("couldnot set photo to main");

        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _Repo.Getuser(userId);
            if (!user.photos.Any(p => p.Id == photoId))
            {
                return Unauthorized();
            }

            var photofromRepo = await _Repo.GetPhoto(photoId);
            if (photofromRepo.IsMain)
                return BadRequest("Cannot Delete Main Photo, change the status of the photo first");


            if (photofromRepo.PublicID != null)
            {
                var deletionParams = new DeletionParams(photofromRepo.PublicID);

                var result = _Cloudinary.Destroy(deletionParams);

                if (result.Result == "ok")
                {
                    _Repo.Delete(photofromRepo);
                }
            }

            if(photofromRepo.PublicID == null)
                _Repo.Delete(photofromRepo);

            if (await _Repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("failed to Delete Photo");
        
        }



    }
}
