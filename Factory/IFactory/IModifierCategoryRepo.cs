using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IModifierCategoryRepo
    {
        public Task<IEnumerable<ModifierCategoryDTO>> GetItems(ClaimsPrincipal User);
        public Task<ModifierCategoryDTO> GetItem( int itemID);
        public int DeleteItem(  int  carryData);
        public int EditItem (ModifierCategoryDTO obj);
        public int SaveItem(ModifierCategoryDTO obj);





        public Task<IEnumerable<ModifierDTO>> GetUnAssigned(int roleid,ClaimsPrincipal User);
        public Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User);
        public int unAssignItem(int userid, int roleid);
        public int AssignItem(int userid, int roleid);
    }

}
