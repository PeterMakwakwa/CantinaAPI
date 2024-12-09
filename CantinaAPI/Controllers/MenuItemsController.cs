using AutoMapper;
using CantinaAPI.CustomActionFilters;
using CantinaAPI.Dtos;
using CantinaAPI.Models;
using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CantinaAPI.Controllers
{
    /// <summary>
    /// Controller for managing menu items.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(RateLimitFilter))]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService _MenuItemService;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(IMenuItemService MenuItemService, IMapper mapper, ILogger<MenuItemsController> logger)
        {
            _MenuItemService = MenuItemService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all menu items.
        /// </summary>
        /// <returns>List of menu items.</returns>
        [HttpGet("getallMenuItems")]
        public async Task<ActionResult<IEnumerable<MenuItemModel>>> GetAllMenuItems()
        {
            var getItemsResponse = await _MenuItemService.GetAllItemsAsync();
            if (!getItemsResponse.Success)
            {
                return NotFound(getItemsResponse);
            }
            return Ok(getItemsResponse);
        }

        /// <summary>
        /// Gets a menu item by ID.
        /// </summary>
        /// <param name="id">Menu item ID.</param>
        /// <returns>Menu item.</returns>
        [HttpGet("getMenuItemById/{id:int}")]
        public async Task<ActionResult<MenuItemModel>> GetMenuItem(int id)
        {
            var item = await _MenuItemService.GetItemByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Adds a new menu item.
        /// </summary>
        /// <param name="itemRequest">Menu item request DTO.</param>
        /// <returns>Created menu item.</returns>
        [HttpPost("addMenuItem")]
        [ValidateModel]
        public async Task<IActionResult> AddMenuItem(MenuItemRequestDto itemRequest)
        {
            var maper = _mapper.Map<MenuItemModel>(itemRequest);
            var creadtedItem = await _MenuItemService.CreateItemAsync(maper);
            return Ok(creadtedItem);
        }

        /// <summary>
        /// Updates an existing menu item.
        /// </summary>
        /// <param name="id">Menu item ID.</param>
        /// <param name="item">Menu item request DTO.</param>
        /// <returns>Updated menu item.</returns>
        [HttpPut("updateMenuItem/{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemRequestDto item)
        {
            var updatedItem = await _MenuItemService.UpdateItemAsync(id, item);
            if (!updatedItem.Success)
                return NotFound(updatedItem);
            return Ok(updatedItem);
        }

        /// <summary>
        /// Deletes a menu item.
        /// </summary>
        /// <param name="id">Menu item ID.</param>
        /// <returns>Deleted menu item.</returns>
        [HttpDelete("deleteMenuItem/{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var itemDeleted = await _MenuItemService.RemoveItemAsync(id);
            if (!itemDeleted.Success)
            {
                return NotFound(itemDeleted);
            }
            return Ok(itemDeleted);
        }

        /// <summary>
        /// Searches for menu items.
        /// </summary>
        /// <param name="searchText">Search text.</param>
        /// <returns>List of menu items.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MenuItemModel>>> SearchMenuItem([FromQuery] string searchText)
        {
            return Ok(await _MenuItemService.SearchItemsAsync(searchText));
        }
    }
}