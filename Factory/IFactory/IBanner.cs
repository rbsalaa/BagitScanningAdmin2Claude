using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IBanner
    {
       
            public Task<IEnumerable<BannersDTO>> GetBannersList(ClaimsPrincipal User);
            public Task<BannersDTO> GetBanners(int id);
            public int DeleteBanners(int id);
            public int EditBanners(BannersDTO obj);
            public int SaveBanners(BannersDTO obj);
            public Task<IEnumerable<BannersDTO>> GetKiosikBannerList(int KiosikID, ClaimsPrincipal User);
            public Task<IEnumerable<BannersDTO>> GetCurrentKiosikBannerList(int KiosikID, ClaimsPrincipal User);
            public int AssignBannerToKiosik(int KiosikID, int AdsID);
            public int unAssignBannerToKiosik(int KiosikID, int AdsID);
        
    }
}
