using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Errors;
using API.Models.User;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Identity.User;
using BLL.Resources.Paging;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly Cloudinary _cloudinary;


        public UsersController(IMapper mapper, IUserService userService, IConfiguration configuration)
        {
            _mapper = mapper;
            _userService = userService;
            
            var cloudinaryConfiguration = configuration.GetSection("Cloudinary");

            var account = new Account(
                cloudinaryConfiguration.GetSection("cloud_name").Value,
                cloudinaryConfiguration.GetSection("api_key").Value,
                cloudinaryConfiguration.GetSection("api_secret").Value
            );
            
            _cloudinary = new Cloudinary(account);
        }

        // GET: api/users
        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = "Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<UserModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var users = await _userService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<UserModel>(users)
            {
                Data = _mapper.Map<IEnumerable<UserModel>>(users.Data)
            };
            return Ok(result);
        }

        // GET: api/users/bob
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserModel>> Get(string username)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            var currentUserName = currentUser.UserName;

            if (!User.IsInRole("Admins") && currentUserName != username)
            {
                return Forbid();
            }
            
            var user = await _userService.GetUserByUserName(username);
            var result = _mapper.Map<UserModel>(user);
            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> Post([FromBody] CreateUserModel userBody)
        {
            var uploadParams = new ImageUploadParams(){
                File = new FileDescription(@"data:image/png;base64," + userBody.ImageBase64)};
            var uploadResult = _cloudinary.Upload(uploadParams);

            var userBodyResource = _mapper.Map<CreateUserResource>(userBody);
            userBodyResource.ImageUrl = uploadResult.Url.ToString();

            var user = await _userService.CreateUser(userBodyResource);
            var result = _mapper.Map<UserModel>(user);

            return CreatedAtAction(nameof(Get), new { userName = result.UserName }, result);
        }

        // PUT: api/users/bob
        [HttpPut("{username}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(string username, [FromBody] UpdateUserResource user)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            if (currentUser.UserName != username) return Forbid();
            if (!User.IsInRole("Admins")) return Forbid();

            await _userService.UpdateUser(username, user);
            return NoContent();
        }

        // DELETE: api/users/bob
        [HttpDelete("{username}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string username)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            var currentUserName = currentUser.UserName;

            if (!User.IsInRole("Admins") && currentUserName != username)
            {
                return Forbid();
            }
            
            Regex regex = new Regex(@"[^\/]+(?=\.)");
            MatchCollection matches = regex.Matches(currentUser.ImageUrl);
            var publicId = matches[1].ToString();

            var deletionParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
            
            await _userService.DeleteUser(username);
            return NoContent();
        }
    }
}