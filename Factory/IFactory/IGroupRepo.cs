using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IGroupRepo
    {
        public Task<IEnumerable<GroupDTO>> GetItems(ClaimsPrincipal User);
        public Task<GroupDTO> GetItem( int itemID);
        public int DeleteItem(  int  carryData);
        public int EditItem (GroupDTO obj);
        public int SaveItem(GroupDTO obj);

        public Task<IEnumerable<ItemDTO>> GetUnAssigned(int roleid);
        public Task<IEnumerable<ItemDTO>> GetAssigned(int roleid);
        public int unAssignItem(int userid, int roleid);
        public int AssignItem(int userid, int roleid);
    }
}
