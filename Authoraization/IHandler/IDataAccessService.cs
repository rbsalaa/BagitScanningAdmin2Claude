using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Authoraization.IHandler
{
    public interface IDataAccessService
    {
        Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string? ctrl, string? act);
        Task<List<AspNavigationMenuDTO>> GetMenuItemsAsync(ClaimsPrincipal principal);
        Task<List<AspNavigationMenuDTO>> GetPermissionsByRoleIdAsync(string? id);
        Task<List<IGrouping<int, AspNavigationMenuDTO>>> GetPermissionsByRoleIdAsync2(string id);
     
        Task<bool> SetPermissionsByRoleIdAsync(string? id, IEnumerable<Guid> permissionIds);
    }
}
