using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IKiosik
    {
        public Task<IEnumerable<KiosikDTO>> GetKiosikList(ClaimsPrincipal User );
        public Task<KiosikDTO> GetKiosik( int KiosikId,int StrId, int CompId, int BrndId);
        public int DeleteKiosik(Kiosik carryData);
        public int EditKiosik(KiosikDTO obj);
        public int SaveKiosik(KiosikDTO obj);
        public IEnumerable<IGrouping<string, KiosikDTO>> GetKiosikListbyCustomer(ClaimsPrincipal User);
    }
}
