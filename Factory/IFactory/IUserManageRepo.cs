using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IUserManageRepo
    {
        public Task<IEnumerable<SelfKOrderWebUser>> GetUsersList(ClaimsPrincipal User);
        public Task<int> DeleteUser(string id);
        public Task<AddUserDTO> GetCrudUser(string id);

        public Task<SelfKOrderWebUser> GetUser(string id);
        public Task<int> EditUser(AddUserDTO obj);
        public Task<IEnumerable<RolesDTO>> GetRoleList(ClaimsPrincipal User);
        public int DeleteAspNetRole(string Id);
        public int EditAspNetRole(RolesDTO obj);
        public int SaveAspNetRole(RolesDTO obj);
        public Task<RolesDTO> GetAspNetRole(string Id);
        public Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<Guid> permissionIds);
        public Task<List<AspNavigationMenuDTO>> GetPermissionsByRoleIdAsync(string id);
    
        public  Task<IEnumerable<SelfKOrderWebUser>> GetRolesUsersList(string roleid, ClaimsPrincipal User);
        public  Task<IEnumerable<SelfKOrderWebUser>> GetCurrentRolesUsersList(string roleid, ClaimsPrincipal User);
        public int unAssignUserToRole(string userid, string roleid);
        public int AssignUserToRole(string userid, string roleid);
    }
}
