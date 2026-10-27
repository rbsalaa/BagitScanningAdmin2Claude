using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IItemRepo
    {
        public Task<IEnumerable<ItemDTO>> GetItems(ClaimsPrincipal User);
        public Task<ItemDTO> GetItem( int itemID);
        public int DeleteItem(  int  carryData);
        public int EditItem (  ItemDTO obj);
        public int SaveItem( ItemDTO obj);
    }
}
