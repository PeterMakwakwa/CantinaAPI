using Moq;
using AutoMapper;
using CantinaAPI.Dtos;
using CantinaAPI.Models;
using CantinaAPI.Repositories.Interfaces;
using CantinaAPI.Services;
using CantinaAPI.Shared;
using Microsoft.Extensions.Options;
using System.Net;

public class MenuItemServiceTests
{
    private readonly Mock<IMenuItemRepository> _mockItemRepository;
    private readonly Mock<ICachingProvider> _mockCacheProvider;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IOptions<CacheSettings> _mockCacheSettings;
    private readonly MenuItemService _menuItemService;

    public MenuItemServiceTests()
    {
        _mockItemRepository = new Mock<IMenuItemRepository>();
        _mockCacheProvider = new Mock<ICachingProvider>();
        _mockMapper = new Mock<IMapper>();
        _mockCacheSettings = Options.Create(new CacheSettings { Cachetime = "5"  });

        _menuItemService = new MenuItemService(
            _mockItemRepository.Object,
            _mockCacheProvider.Object,
            _mockCacheSettings,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllItemsAsync_ReturnsItems()
    {
        // Arrange
        var items = new List<MenuItemModel> { new MenuItemModel { Id = 1, Name = "Item1" } };
        _mockItemRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);
        _mockCacheProvider.Setup(cache => cache.GetObjectFromCache(It.IsAny<string>(), It.IsAny<Func<Task<ResponseModel<IEnumerable<MenuItemModel>>>>>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(new ResponseModel<IEnumerable<MenuItemModel>> { Model = items, Success = true, StatusCode = HttpStatusCode.OK });

        // Act
        var result = await _menuItemService.GetAllItemsAsync();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Model);
        Assert.Single(result.Model);
    }

    [Fact]
    public async Task GetItemByIdAsync_ItemExists_ReturnsItem()
    {
        // Arrange
        var item = new MenuItemModel { Id = 1, Name = "Item1" };
        _mockItemRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);
        _mockCacheProvider.Setup(cache => cache.GetObjectFromCache(It.IsAny<string>(), It.IsAny<Func<Task<ResponseModel<MenuItemModel>>>>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(new ResponseModel<MenuItemModel> { Model = item, Success = true, StatusCode = HttpStatusCode.OK });

        // Act
        var result = await _menuItemService.GetItemByIdAsync(1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model.Id);
    }

    [Fact]
    public async Task CreateItemAsync_ReturnsCreatedItem()
    {
        // Arrange
        var item = new MenuItemModel { Id = 1, Name = "NewItem" };
        _mockItemRepository.Setup(repo => repo.CreateAsync(item)).ReturnsAsync(item);
        _mockItemRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _menuItemService.CreateItemAsync(item);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Model);
        Assert.Equal("NewItem", result.Model.Name);
    }

    [Fact]
    public async Task UpdateItemAsync_ItemExists_ReturnsUpdatedItem()
    {
        // Arrange
        var item = new MenuItemModel { Id = 1, Name = "UpdatedItem" };
        var itemDto = new MenuItemRequestDto { Name = "UpdatedItem" };
        _mockItemRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);
        _mockMapper.Setup(mapper => mapper.Map(itemDto, item)).Returns(item);
        _mockItemRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _menuItemService.UpdateItemAsync(1, itemDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Model);
        Assert.Equal("UpdatedItem", result.Model.Name);
    }

    [Fact]
    public async Task RemoveItemAsync_ItemExists_ReturnsDeletedItem()
    {
        // Arrange
        var item = new MenuItemModel { Id = 1, Name = "ItemToDelete" };
        _mockItemRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);
        _mockItemRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _menuItemService.RemoveItemAsync(1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(result.Model);
        Assert.Equal("ItemToDelete", result.Model.Name);
    }
}