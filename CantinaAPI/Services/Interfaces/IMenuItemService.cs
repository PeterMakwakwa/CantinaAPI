using CantinaAPI.Dtos;
using CantinaAPI.Models;

namespace CantinaAPI.Services.Interfaces
{

    public interface IMenuItemService
    {
        Task<ResponseModel<IEnumerable<MenuItemModel>>> GetAllItemsAsync();
        Task<ResponseModel<MenuItemModel> >GetItemByIdAsync(int id);
        Task<ResponseModel<IEnumerable<MenuItemModel>>> SearchItemsAsync(string searchText);
        Task<ResponseModel<MenuItemModel>> CreateItemAsync(MenuItemModel item);
        Task<ResponseModel<MenuItemModel>> UpdateItemAsync(int id, MenuItemRequestDto item);
        Task<ResponseModel<MenuItemModel>> RemoveItemAsync(int id);

    }
}
