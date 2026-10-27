using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IFranshise
    {
        public Task<IEnumerable<FranchisingDTO>> GetFranchiseList(ClaimsPrincipal User);
        public Task<FranchisingDTO> GetFranchise( int StrId, int CompId, int BrndId);
        public int DeleteFranchise(Franchising carryData);
        public int EditFranchise(FranchisingDTO obj);
        public int SaveFranchise(FranchisingDTO obj);
    }
}
