using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IModifierRepo
    {
        public Task<IEnumerable<ModifierDTO>> GetItems(ClaimsPrincipal User);
        public Task<ModifierDTO> GetItem( int itemID);
        public int DeleteItem(  int  carryData);
        public int EditItem (ModifierDTO obj);
        public int SaveItem(ModifierDTO obj);
        public Task<IEnumerable<ModifierCategoryDTO>> GetItemsModifires(ClaimsPrincipal User);
        public  Task<IEnumerable<ModifierDTO>> GetUnAssignedMod(int roleid, ClaimsPrincipal User);

        public  Task<IEnumerable<ModifierDTO>> GetAssignedMod(int roleid, ClaimsPrincipal User);
        public int AssignItemMod(int userid, int roleid);
        public int unAssignItemMod(int itemID, int roleid);
        public Task<IEnumerable<ModifierCategoryDTO>> GetUnAssigned(int roleid, ClaimsPrincipal User);
        public Task<IEnumerable<ModifierCategoryDTO>> GetAssigned(int roleid, ClaimsPrincipal User);
        public int unAssignItem(int userid, int roleid);
        public int AssignItem(int userid, int roleid);
    }

}
