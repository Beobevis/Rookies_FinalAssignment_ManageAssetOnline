namespace WebApi.Repositories;

using WebApi.Helpers;
using WebApi.Entities;
using WebApi.Models.Assets;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

public interface IAssetRepository
{
    Task CreateAsset(AssetCreateModel asset, string Location, int adminid);
    Task<IEnumerable<Asset>> GetAllAssets();
    Task<Asset> GetAsset(int id);
    Task UpdateAsset(AssetUpdateModel asset, string adminlocation);
    Task DeleteAsset(int id, string adminlocation);
}

public class AssetRepository : IAssetRepository
{
    private DataContext _context;
    private readonly AppSettings _appSettings;

    public AssetRepository(
        DataContext context,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public async Task CreateAsset(AssetCreateModel asset, string location, int adminid)
    {
        if (!CheckCategoryId(asset.CategoryId)) throw new AppException("CategoryId is not valid");
        if (!CheckInstalledDate(asset.InstalledDate)) throw new AppException("Installed Date must not be in the future");
        if (asset.State == null) throw new AppException("State is required");
        if (asset.AssetName != null && asset.AssetName.Trim().Equals("")) throw new AppException("Asset Name is required");
        if (asset.Specification != null && asset.Specification.Trim().Equals("")) throw new AppException("Specification is required");
        if (asset.Specification == null) throw new AppException("Specification is required");
        
        var assetEntity = new Asset()
        {
            AssetCode = GenerateAssetCode(asset.CategoryId),
            AssetName = asset.AssetName?.Trim(),
            CategoryId = asset.CategoryId,
            Specification = asset.Specification.Trim(),
            InstalledDate = asset.InstalledDate,
            Location = location,
            State = asset.State.Trim().ToLower().Equals("available") ? AssetState.Available : AssetState.NotAvailable,
            CreateAt = DateTime.Now,
            CreateBy = adminid,
            UpdateAt = null,
            UpdateBy = null,
        };
        await _context.Assets.AddAsync(assetEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Asset>> GetAllAssets()
    {
        return await _context.Assets.Include(a => a.Category).ToListAsync();
    }

    public async Task<Asset> GetAsset(int id)
    {
        var asset = await _context.Assets
        .Include(a => a.Category)
        .Include(a => a.Assignments.Where(s => (s.State.Equals(AssignmentState.Accepted) || s.State.Equals(AssignmentState.Declined)) && s.IsInProgress == false)).ThenInclude(a => a.AssignTo)
        .Include(a => a.Assignments.Where(s => (s.State.Equals(AssignmentState.Accepted) || s.State.Equals(AssignmentState.Declined)) && s.IsInProgress == false)).ThenInclude(a => a.AssignedBy)
        .SingleOrDefaultAsync(a => a.Id == id);
        if (asset == null) throw new AppException("Asset not found");
        return asset;
    }

    public async Task UpdateAsset(AssetUpdateModel asset, string adminlocation)
    {
        var assetEntity = await _context.Assets.FindAsync(asset.Id);

        if (asset.State == null) throw new AppException("State is required");
        if (assetEntity == null) throw new AppException("Asset not found");
        if (assetEntity.AssetCode == null || assetEntity.Location == null) throw new AppException("Asset do not have asset code or location");
        if (!CheckInstalledDate(asset.InstalledDate)) throw new AppException("Installed Date must not be in the future");
        if (assetEntity.AssetCode.Equals(AssetState.Assigned)) throw new AppException("Asset is assigned");
        if (assetEntity.Location.Equals(adminlocation))
        {
            assetEntity.AssetName = asset.AssetName?.Trim();
            assetEntity.Specification = asset.Specification.Trim();
            assetEntity.InstalledDate = asset.InstalledDate;
            assetEntity.State = SetUpdateAssetState(asset.State);
            assetEntity.UpdateAt = DateTime.Now; // optional
        }
        else
        {
            throw new AppException("Can not update asset at different location");
        }
        _context.Assets.Update(assetEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsset(int id, string adminlocation)
    {
        var asset = await _context.Assets.FindAsync(id);
        //var check = _context.Assignments.Any(a => a.AssetId == id && !a.State.Equals(AssignmentState.Declined));
        var check = _context.Assignments.Any(a => a.AssetId == id);

        if (asset == null) throw new AppException("Asset not found");
        if (asset.AssetCode == null || asset.Location == null) throw new AppException("Asset do not have asset code or location");
        if (asset.AssetCode.Equals(AssetState.Assigned)) throw new AppException("Asset is assigned");
        if (!asset.Location.Equals(adminlocation)) throw new AppException("Can delete only assets in your location");
        if (check) throw new AppException("Cannot delete the asset because it belongs to one or more historical assignments");

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    private string GenerateAssetCode(int categoryid)
    {
        var prefix = _context.Categories.SingleOrDefault(c => c.Id == categoryid)?.Prefix;
        var listSameCatalogue = _context.Assets.Where(a => a.CategoryId == categoryid);
        if (listSameCatalogue.Count() == 0)
        {
            return prefix + "000001";
        }
        else
        {
            var lastAssetCode = listSameCatalogue.OrderByDescending(o => o.Id).FirstOrDefault()?.AssetCode;
            var lastAssetId = Convert.ToInt32(lastAssetCode?.Substring(lastAssetCode.Length - 6)) + 1;
            return prefix + String.Format("{0,0:D6}", lastAssetId++);
        }
    }

    private bool CheckInstalledDate(DateTime date)
    {
        if (DateTime.Compare(DateTime.Now, date) < 0)
        {
            return false;
        }
        return true;
    }

    private AssetState SetUpdateAssetState(string state)
    {
        var st = state.Trim().ToLower();

        if (st.Equals("available")) return AssetState.Available;
        if (st.Equals("notavailable")) return AssetState.NotAvailable;
        if (st.Equals("waitingforrecycling")) return AssetState.WaitingForRecycling;
        if (st.Equals("recycled")) return AssetState.Recycled;

        return AssetState.NotAvailable;
    }

    private bool CheckCategoryId(int categoryid)
    {
        return _context.Categories.Any(c => c.Id == categoryid);
    }

}