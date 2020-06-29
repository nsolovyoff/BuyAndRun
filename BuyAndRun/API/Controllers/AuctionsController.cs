using System.Collections.Generic;
using System.Threading.Tasks;
using API.Errors;
using API.Models.Auction;
using API.Models.Category;
using API.Models.User;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Auction;
using BLL.Resources.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public AuctionsController(IMapper mapper,
                                IAuctionService auctionService,
                                ICategoryService categoryService,
                                IUserService userService,
                                IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _auctionService = auctionService;
            _categoryService = categoryService;
            _userService = userService;
            _authorizationService = authorizationService;
        }

        // GET: api/auctions
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<AuctionModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var auctions = await _auctionService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<AuctionModel>(auctions)
            {
                Data = _mapper.Map<IEnumerable<AuctionModel>>(auctions.Data)
            };
            return Ok(result);
        }

        // GET: api/auctions/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuctionModel>> Get(int id)
        {
            var auction = await _auctionService.GetAuctionById(id);
            var result = _mapper.Map<AuctionModel>(auction);
            return Ok(result);
        }

        // GET: api/auctions/5/categories
        [HttpGet("{id}/Categories")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<CategoryModel>>> GetCategories(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var categories = await _categoryService.GetCategoriesByAuctionIdPaged(id, page, pageSize);
            var result = new PagedResultResource<CategoryModel>(categories)
            {
                Data = _mapper.Map<IEnumerable<CategoryModel>>(categories.Data)
            };
            return Ok(result);
        }

        // GET: api/auctions/5/categories-count
        [HttpGet("{id}/Categories-Count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCategoriesCount(int id)
        {
            var result = await _categoryService.GetCategoriesCountByAuctionId(id);
            return Ok(result);
        }

        // GET: api/auctions/5/moderators
        [HttpGet("{id}/Moderators")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<UserModel>>> GetModerators(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });

            var users = await _userService.GetUsersAsAuctionModeratorsPaged(id, page, pageSize);
            var result = new PagedResultResource<UserModel>(users)
            {
                Data = _mapper.Map<IEnumerable<UserModel>>(users.Data)
            };
            return Ok(result);
        }

        // POST: api/auctions
        [HttpPost]
        [Authorize(Roles = "GlobalModerators, Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuctionModel>> Post([FromBody] CreateAuctionResource auctionBody)
        {
            var auction = await _auctionService.CreateAuction(auctionBody);
            var result = _mapper.Map<AuctionModel>(auction);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/auctions/5
        [HttpPut("{id}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateAuctionResource auction)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "ModeratorOfAuction")).Succeeded)
            {
                return Forbid();
            }

            await _auctionService.UpdateAuction(id, auction);
            return NoContent();
        }

        // DELETE: api/auctions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "GlobalModerators, Admins")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            await _auctionService.DeleteAuction(id);
            return NoContent();
        }
    }
}