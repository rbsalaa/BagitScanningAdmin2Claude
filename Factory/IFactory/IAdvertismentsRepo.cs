using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IAdvertismentsRepo
    {
        public Task<IEnumerable<AdvertismentsDTO>> GetAdvertismentsList(ClaimsPrincipal User);
        public Task<AdvertismentsDTO> GetAdvertisments(  int id);
        public int DeleteAdvertisments(  int id);
        public int EditAdvertisments(  AdvertismentsDTO obj);
        public int SaveAdvertisments(  AdvertismentsDTO obj);
        public Task<IEnumerable<AdvertismentsDTO>> GetKiosikAdsList(int KiosikID, ClaimsPrincipal User);
        public  Task<IEnumerable<AdvertismentsDTO>> GetCurrentKiosikAdsList(int KiosikID, ClaimsPrincipal User);
        public int AssignAdsToKiosik(int KiosikID, int AdsID);
        public int unAssignAdsToKiosik(int KiosikID, int AdsID);
    }
}
