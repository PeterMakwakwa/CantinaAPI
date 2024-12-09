using CantinaAPI.Models;

namespace CantinaAPI.Repositories.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItemModel>
    {
        Task<IEnumerable<MenuItemModel>> SearchAsync(string searchText);
        Task<bool> ItemExistsAsync(string itemName);
    }
}
