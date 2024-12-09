using AutoMapper;
using CantinaAPI.Dtos;
using CantinaAPI.Models;
using CantinaAPI.Repositories.Interfaces;
using CantinaAPI.Services.Interfaces;
using CantinaAPI.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CantinaAPI.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _itemRepository;
        private readonly ICachingProvider _cacheProvider;
        private readonly TimeSpan _cacheDuration;
        private readonly IMapper _mapper;

        public MenuItemService(
            IMenuItemRepository itemRepository,
            ICachingProvider cacheProvider,
            IOptions<CacheSettings> cacheSettings,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _cacheProvider = cacheProvider;
            _cacheDuration = cacheSettings.Value.GetCacheDuration();
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all menu items.
        /// </summary>
        /// <returns>A response model containing a list of menu items.</returns>
        public async Task<ResponseModel<IEnumerable<MenuItemModel>>> GetAllItemsAsync()
        {
            const string cacheKey = $"{CantinaHelper.CacheKeyAppName}-Items:GetAll";
            return await GetFromCacheAsync(
                cacheKey,
                async () =>
                {
                    var items = await _itemRepository.GetAllAsync();
                    return items?.Any() == true
                        ? CreateResponse(items, "Items retrieved!", HttpStatusCode.OK)
                        : CreateResponse<IEnumerable<MenuItemModel>>(null, "Items not found!", HttpStatusCode.NotFound);
                }
            );
        }

        /// <summary>
        /// Retrieves a menu item by its ID.
        /// </summary>
        /// <param name="id">The ID of the menu item.</param>
        /// <returns>A response model containing the menu item.</returns>
        public async Task<ResponseModel<MenuItemModel>> GetItemByIdAsync(int id)
        {
            string cacheKey = GenerateItemCacheKey(id);
            return await GetFromCacheAsync(
                cacheKey,
                async () =>
                {
                    var item = await _itemRepository.GetByIdAsync(id);
                    return item != null
                        ? CreateResponse(item, "Item retrieved!", HttpStatusCode.OK)
                        : CreateResponse<MenuItemModel>(null, "Item not found!", HttpStatusCode.NotFound);
                }
            );
        }

        /// <summary>
        /// Creates a new menu item.
        /// </summary>
        /// <param name="item">The menu item to create.</param>
        /// <returns>A response model containing the created menu item.</returns>
        public async Task<ResponseModel<MenuItemModel>> CreateItemAsync(MenuItemModel item)
        {
            var createdItem = await _itemRepository.CreateAsync(item);
            await _itemRepository.SaveAsync();
            InvalidateCache();
            return CreateResponse(createdItem, "MenuItem was successfully added", HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates an existing menu item.
        /// </summary>
        /// <param name="id">The ID of the menu item to update.</param>
        /// <param name="itemDto">The updated menu item data.</param>
        /// <returns>A response model containing the updated menu item.</returns>
        public async Task<ResponseModel<MenuItemModel>> UpdateItemAsync(int id, MenuItemRequestDto itemDto)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return CreateResponse<MenuItemModel>(null, "Item update failed, item does not exist", HttpStatusCode.NotFound);
            }

            _mapper.Map(itemDto, existingItem);
            _itemRepository.Update(existingItem);
            await _itemRepository.SaveAsync();
            InvalidateCache(id);
            return CreateResponse(existingItem, "Item successfully updated", HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes a menu item by its ID.
        /// </summary>
        /// <param name="id">The ID of the menu item to delete.</param>
        /// <returns>A response model containing the deleted menu item.</returns>
        public async Task<ResponseModel<MenuItemModel>> RemoveItemAsync(int id)
        {
            var existingItem = await _itemRepository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return CreateResponse<MenuItemModel>(null, "Item delete failed, item does not exist", HttpStatusCode.NotFound);
            }

            _itemRepository.Delete(existingItem);
            await _itemRepository.SaveAsync();
            InvalidateCache(id);
            return CreateResponse(existingItem, "Item successfully deleted", HttpStatusCode.OK);
        }

        /// <summary>
        /// Searches for menu items by text.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        /// <returns>A response model containing a list of matching menu items.</returns>
        public async Task<ResponseModel<IEnumerable<MenuItemModel>>> SearchItemsAsync(string searchText)
        {
            string cacheKey = $"{CantinaHelper.CacheKeyAppName}-Search-{searchText.ToLower()}";
            return await GetFromCacheAsync(
                cacheKey,
                async () =>
                {
                    var searchResults = await _itemRepository.SearchAsync(searchText);
                    return searchResults?.Any() == true
                        ? CreateResponse(searchResults, "Search successful", HttpStatusCode.OK)
                        : CreateResponse<IEnumerable<MenuItemModel>>(null, "No matching items found", HttpStatusCode.NotFound);
                }
            );
        }

        #region Private Helpers

        private async Task<ResponseModel<T>> GetFromCacheAsync<T>(string cacheKey, Func<Task<ResponseModel<T>>> cacheCallback)
        {
            return await _cacheProvider.GetObjectFromCache(cacheKey, cacheCallback, _cacheDuration);
        }

        private void InvalidateCache(int? itemId = null)
        {
            if (itemId.HasValue)
            {
                _cacheProvider.Remove(GenerateItemCacheKey(itemId.Value));
            }

            _cacheProvider.Remove($"{CantinaHelper.CacheKeyAppName}-Items:GetAll");
        }

        private string GenerateItemCacheKey(int id) =>
            $"{CantinaHelper.CacheKeyAppName}-Item-id:{id}";

        private ResponseModel<T> CreateResponse<T>(T data, string message, HttpStatusCode statusCode)
        {
            return new ResponseModel<T>
            {
                Success = data != null,
                Message = message,
                Model = data,
                StatusCode = statusCode
            };
        }

        #endregion
    }
}