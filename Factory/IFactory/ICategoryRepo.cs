using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface ICategoryRepo
    {
        public Task<IEnumerable<CategoryDTO>> GetItems( ClaimsPrincipal User);
        public Task<CategoryDTO> GetItem( int itemID);
        public int DeleteItem(  int  carryData);
        public int EditItem (CategoryDTO obj);
        public int SaveItem(CategoryDTO obj);





        public Task<IEnumerable<ItemDTO>> GetUnAssigned(int roleid, ClaimsPrincipal User);
        public Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User);
        public int unAssignItem(int userid, int roleid);
        public int AssignItem(int userid, int roleid,int? CategoryOrder);
    }

}
