using Xunit;
using Moq;
using System.Collections.Generic;
using WebApi.Entities;
using System;
using WebApi.Repositories;
using System.Threading.Tasks;
using WebApi.Services;
using System.Linq;
using AutoMapper;
using WebApi.Models.Assets;
using WebApi.Helpers;

namespace WebApi.UnitTests;

public class AssetServiceTests
{
    private readonly Mock<IAssetRepository> _assetRepositoryMock = new Mock<IAssetRepository>();
    private readonly IMapper _mapper;

    public AssetServiceTests()
    {
        _assetRepositoryMock = new Mock<IAssetRepository>();
        if(_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    public Task GetAll_GetListOfAssets_ReturnListOfAssets()
    {
        // Arrange
        var assets = GetSampleAssets();
        _assetRepositoryMock.Setup(x => x.GetAllAssets()).ReturnsAsync(assets);

        var service = new AssetService(_assetRepositoryMock.Object, _mapper);
        // Act
        var actionResult = service.GetAllAssets();
        var actual = actionResult.Result as IEnumerable<AssetModel>;

        // Assert
        Assert.IsType<List<AssetModel>>(actual);
        Assert.Equal(GetSampleAssets().Count(), actual.Count());
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAll_EmptyList_ReturnListOfNone()
    {
        // Arrange
        var assets = new List<Asset>();
        _assetRepositoryMock.Setup(x => x.GetAllAssets()).ReturnsAsync(assets);
        var service = new AssetService(_assetRepositoryMock.Object, _mapper);

        // Act
        var actionResult = service.GetAllAssets();
        var actual = actionResult.Result as IEnumerable<AssetModel>;

        // Assert
        Assert.IsType<List<AssetModel>>(actual);
        Assert.IsNotType<AssetModel>(actual);
        Assert.IsNotType<List<Asset>>(actual);
        Assert.Equal(0, actual.Count());
        Assert.Empty(actual);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    [Fact]
    public Task GetAsset_GetAssetById_ReturnAsset()
    {
        // Arrange
        var assets = GetSampleAssets();
        //var asset = assets.FirstOrDefault(x => x.Id == 1);
        var asset = assets.First(x => x.Id == 1);
        _assetRepositoryMock.Setup(x => x.GetAsset(1)).ReturnsAsync(asset);
        var service = new AssetService(_assetRepositoryMock.Object, _mapper);

        // Act
        var actionResult = service.GetAsset(1);
        var actual = actionResult.Result as AssetModel;

        // Assert
        Assert.IsType<AssetModel>(actual);
        Assert.Equal(1, actual.Id);
        Assert.NotEqual(2, actual.Id);
        Assert.NotNull(actual);

        return Task.CompletedTask;
    }

    public IEnumerable<Asset> GetSampleAssets()
    {
        return new List<Asset>{
            new Asset{
                Id = 1,
                AssetCode = "LA000001",
                AssetName = "asset test1",
                CategoryId = 1,
                Specification = "test",
                InstalledDate = new DateTime(2022,03,15),
                Location = "Hochiminh",
                State = AssetState.Available,
            },
             new Asset{
                Id = 2,
                AssetCode = "LA000002",
                AssetName = "asset test2",
                CategoryId = 1,
                Specification = "test",
                InstalledDate = new DateTime(2022,03,20),
                Location = "Hochiminh",
                State = AssetState.Available,
            },
             new Asset{
                Id = 3,
                AssetCode = "DE000001",
                AssetName = "asset test3",
                CategoryId = 2,
                Specification = "test",
                InstalledDate = new DateTime(2022,03,25),
                Location = "Hochiminh",
                State = AssetState.Available,
            },
        };
    }
}