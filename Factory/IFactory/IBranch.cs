using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IBranch
    {
        public Task<IEnumerable<BranchDTO>> GetStoreList(ClaimsPrincipal User);
        public Task<BranchDTO> GetStore( int StrId, int CompId, int BrndId);
        public int DeleteStore(  Branch carryData);
        public int EditStore(  BranchDTO obj);
        public int SaveStore( BranchDTO obj);
    }
}
