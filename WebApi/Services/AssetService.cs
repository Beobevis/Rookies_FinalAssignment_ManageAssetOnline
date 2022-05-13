using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Assets;
using WebApi.Repositories;

namespace WebApi.Services;

public interface IAssetService
{
    Task CreateAsset(AssetCreateModel asset, string location, int adminid);
    Task<IEnumerable<AssetModel>> GetAllAssets();
    Task<AssetModel> GetAsset(int id);
    Task UpdateAsset(AssetUpdateModel asset, string adminlocation);
    Task DeleteAsset(int id, string adminlocation);
    //Task<IEnumerable<Asset>> GetAllAssets();
}

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetsRepository;
    private readonly IMapper _mapper;
    public AssetService(IAssetRepository assetRepository, IMapper mapper)
    {
        _assetsRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task CreateAsset(AssetCreateModel asset, string location, int adminid)
    {
        await _assetsRepository.CreateAsset(asset, location, adminid);
    }

    public async Task<IEnumerable<AssetModel>> GetAllAssets()
    {
        return _mapper.Map<IEnumerable<AssetModel>>(await _assetsRepository.GetAllAssets());
    }

    public async Task<AssetModel> GetAsset(int id)
    {
        return _mapper.Map<AssetModel>(await _assetsRepository.GetAsset(id));
    }

    public async Task UpdateAsset(AssetUpdateModel asset, string adminlocation)
    {
        await _assetsRepository.UpdateAsset(asset, adminlocation);

    }
    public async Task DeleteAsset(int id, string adminlocation)
    {
        await _assetsRepository.DeleteAsset(id, adminlocation);
    }

    //public async Task<IEnumerable<Asset>> GetAllAssets()
    //{
    //    return await _assetsRepository.GetAllAssets();
    //}
}
