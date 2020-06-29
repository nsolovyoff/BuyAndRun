using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Errors;
using API.Models.Category;
using API.Models.Lot;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Category;
using BLL.Resources.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILotService _lotService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CategoriesController(IMapper _mapper,
                                 ICategoryService categoryService,
                                 ILotService lotService,
                                 IAuthorizationService authorizationService)
        {
            this._mapper = _mapper;
            _categoryService = categoryService;
            _lotService = lotService;
            _authorizationService = authorizationService;
        }

        // GET: api/categories
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResultResource<CategoryModel>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            
            var categories = await _categoryService.GetAllPaged(page, pageSize);
            var result = new PagedResultResource<CategoryModel>(categories)
            {
                Data = _mapper.Map<IEnumerable<CategoryModel>>(categories.Data)
            };
            return Ok(result);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryModel>> Get(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            var result = _mapper.Map<CategoryModel>(category);
            return Ok(result);
        }

        // GET: api/categories/5/lots
        [HttpGet("{id}/Lots")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResultResource<LotModel>>> GetLots(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page <= 0 || pageSize <= 0) return BadRequest(new ErrorModel { Error = "Can not get result with negative page or pageSize" });
            var lots = await _lotService.GetLotsByCategoryIdPaged(id, page, pageSize);
            var result = new PagedResultResource<LotModel>(lots)
            {
                Data = _mapper.Map<IEnumerable<LotModel>>(lots.Data)
            };
            return Ok(result);
        }

        // GET: api/categories/5/lots-count
        [HttpGet("{id}/Lots-count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetLotsCount(int id)
        {
            var result = await _lotService.GetLotsCountByCategoryId(id);
            return Ok(result);
        }

        // POST: api/categories
        [Authorize]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryModel>> Lot([FromBody] CreateCategoryModel categoryBody)
        {
            categoryBody.StartedBy = (string.IsNullOrEmpty(categoryBody.StartedBy)) ? User.FindFirstValue(ClaimTypes.Name) : categoryBody.StartedBy;

            var categoryBodyResource = _mapper.Map<CreateCategoryResource>(categoryBody);
            var category = await _categoryService.CreateCategory(categoryBodyResource);
            var result = _mapper.Map<CategoryModel>(category);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/categories/5
        [Authorize]
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoryModel categoryBody)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id, "EditCategory")).Succeeded)
            {
                return Forbid();
            }

            var categoryResource = _mapper.Map<UpdateCategoryResource>(categoryBody);
            await _categoryService.UpdateCategory(id, categoryResource);
            return NoContent();
        }

        // DELETE: api/categories/5
        [Authorize]
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var t = await _categoryService.GetCategoryById(id);
            var auctionId = t.ParentAuction.Id;
            if (!(await _authorizationService.AuthorizeAsync(User, auctionId, "ModeratorOfauction")).Succeeded)
            {
                return Forbid();
            }

            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}