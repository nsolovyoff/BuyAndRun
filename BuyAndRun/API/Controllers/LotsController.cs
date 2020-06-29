using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Errors;
using API.Models.Lot;
using API.Models.User;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Lot;
using BLL.Resources.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LotsController : ControllerBase
    {
        private readonly ILotService _lotService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        private readonly Cloudinary _cloudinary;
        
        public LotsController(IMapper mapper,
                               ILotService lotService,
                               ICategoryService categoryService,
                               IAuthorizationService authorizationService,
                               IUserService userService,
                               IConfiguration configuration)
        {
            _mapper = mapper;
            _lotService = lotService;
            _categoryService = categoryService;
            _authorizationService = authorizationService;
            _userService = userService;

            var cloudinaryConfiguration = configuration.GetSection("Cloudinary");

            var account = new Account(
            cloudinaryConfiguration.GetSection("cloud_name").Value,
            cloudinaryConfiguration.GetSection("api_key").Value,
            cloudinaryConfiguration.GetSection("api_secret").Value
            );
            
            _cloudinary = new Cloudinary(account);
        }

        // GET: api/lots
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<LotModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var lots = await _lotService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<LotModel>(lots)
            {
                Data = _mapper.Map<IEnumerable<LotModel>>(lots.Data)
            };
            return Ok(result);
        }
        
        // GET: api/lots/search/searchQuery
        [HttpGet("search/{searchQuery}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LotModel>> Search(string searchQuery)
        {
            var lots = await _lotService.GetAll();
            var filteredLots = lots.Where(l => l.Title.Contains(searchQuery));
            
            var result = _mapper.Map<IEnumerable<LotModel>>(filteredLots);
            return Ok(result);
        }

        // GET: api/lots/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LotModel>> Get(int id)
        {
            var lot = await _lotService.GetLotById(id);
            var result = _mapper.Map<LotModel>(lot);
            return Ok(result);
        }
        
        // GET: api/lots/bob/owner
        [HttpGet("{lotId}/owner")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WinnerUserModel>> GetOwner(int lotId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            var currentUserName = currentUser.UserName;

            var lot = await _lotService.GetLotById(lotId);
            
            if (!User.IsInRole("Admins") && currentUserName != lot.BidUser && DateTime.Now < lot.Expiring)
            {
                return Forbid();
            }
            
            var user = await _userService.GetUserByUserName(lot.User);
            var result = _mapper.Map<WinnerUserModel>(user);
            return Ok(result);
        }

        // POST: api/lots
        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LotModel>> Lot([FromBody] CreateLotModel lotBody)
        {
            
            var uploadParams = new ImageUploadParams(){
                File = new FileDescription(@"data:image/png;base64," + lotBody.ImageBase64)};
            var uploadResult = _cloudinary.Upload(uploadParams);
            
            var lotBodyResource = _mapper.Map<CreateLotResource>(lotBody);
            lotBodyResource.ImageUrl = uploadResult.Url.ToString();
            
            var lot = await _lotService.CreateLot(lotBodyResource);
            var result = _mapper.Map<LotModel>(lot);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/lots/5
        [HttpPut("{id}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLotResource lot)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "EditLot")).Succeeded)
            {
                return Forbid();
            }

            await _lotService.UpdateLot(id, lot);
            return NoContent();
        }
        
        // PUT: api/lots/5/buy-now
        [HttpPut("{id}/Buy-now")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> BuyNow(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            var currentUserName = currentUser.UserName;
            
            await _lotService.BuyNow(id, currentUserName);
            return NoContent();
        }

        // PUT: api/lots/5/make-bid
        [HttpPut("{id}/Make-bid")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> MakeBid(int id, [FromBody] MakeBidResource bid)
        {
            var lotToUpdate = await _lotService.GetLotById(id);
            if (lotToUpdate.Bid >= bid.Bid)
                return BadRequest("new bid must be bigger then old");
            LotResource lot = new LotResource();
            lot.Bid = bid.Bid;
            lot.BidUser = bid.UserId;

            await _lotService.MakeBid(id, lot);
            return NoContent();
        }

        // DELETE: api/lots/5
        [HttpDelete("{id}")]
        [Authorize]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userService.GetUserById(currentUserId);
            var currentUserName = currentUser.UserName;

            var lotToDelete = await _lotService.GetLotById(id);
            if (!User.IsInRole("Admins") && currentUserName != lotToDelete.User)
            {
                return Forbid();
            }

            Regex regex = new Regex(@"[^\/]+(?=\.)");
            MatchCollection matches = regex.Matches(lotToDelete.ImageUrl);
            var publicId = matches[1].ToString();

            var deletionParams = new DeletionParams(publicId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            await _lotService.DeleteLot(id);
            return NoContent();
        }
    }
}