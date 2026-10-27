using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IMenuRepo
    {
        public Task<IEnumerable<MenuDTO>> GetItems(ClaimsPrincipal User );
        public Task<MenuDTO> GetItem( int itemID, ClaimsPrincipal User);
        public int DeleteItem(  int  carryData);
        public int EditItem (MenuDTO obj);
        public int SaveItem(MenuDTO obj);





        public Task<IEnumerable<ItemDTO>> GetUnAssigned(int roleid, ClaimsPrincipal User);
        public Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User);
        public int unAssignItem(int userid, int roleid);
        public int AssignItem(int userid, int roleid);
    }

}
