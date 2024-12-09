using CantinaAPI.Data;
using CantinaAPI.Models;
using CantinaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CantinaAPI.Repositories
{
    /// <summary>
    /// Repository for managing menu items.
    /// </summary>
    public class MenuItemRepository : Repository<MenuItemModel>, IMenuItemRepository
    {
        public MenuItemRepository(CantinaDbContext context) : base(context) { }

        /// <summary>
        /// Checks if an item with the specified name exists.
        /// </summary>
        /// <param name="itemName">The name of the item.</param>
        /// <returns>True if the item exists, otherwise false.</returns>
        public async Task<bool> ItemExistsAsync(string itemName)
        {
            return await _context.Items.AnyAsync(i => i.Name == itemName);
        }

        /// <summary>
        /// Searches for items by name or description.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        /// <returns>A list of items that match the search text.</returns>
        public async Task<IEnumerable<MenuItemModel>> SearchAsync(string searchText)
        {
            return await _context.Items
                .Where(i => i.Name.Contains(searchText) || i.Description.Contains(searchText))
                .ToListAsync();
        }
    }
}