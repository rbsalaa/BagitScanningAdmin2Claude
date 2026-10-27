using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization.IHandler;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using SelfKOrderWeb.Data;

namespace SelfKOrderWeb.Authoraization
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IMemoryCache _cache;
        private readonly SelfKOrderWebContext _context;

        public DataAccessService(SelfKOrderWebContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<List<AspNavigationMenuDTO>> GetMenuItemsAsync(ClaimsPrincipal principal)
        {
            var isAuthenticated = principal.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                return new List<AspNavigationMenuDTO>();
            }

            var roleIds = await GetUserRoleIds(principal);

            var permissions = await _cache.GetOrCreateAsync("Permissions",
                 async x => await (from menu in _context.NavigationMenu select menu).ToListAsync());

            var rolePermissions = await _cache.GetOrCreateAsync("RolePermissions",
                 async x => await (from menu in _context.RoleMenuPermission select menu).Include(x => x.NavigationMenu).ToListAsync());

            var data = (from menu in rolePermissions
                        join p in permissions on menu.NavigationMenuId equals p.Id
                        where roleIds.Contains(menu.RoleId)
                        select p)
                               .Select(m => new AspNavigationMenuDTO()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   Visible = m.Visible,
                                   IsExternal = m.IsExternal,
                                   ActionName = m.ActionName,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   ControllerName = m.ControllerName,
                               }).Distinct().ToList();

            return data;
        }

       

        public async Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string? ctrl, string? act)
        {
            // Runs on every permission-protected request. One query that only returns the matching
            // permission rows, instead of loading the user's role ids and then every menu entry of those roles.
            var userId = GetUserId(ctx);
            if (string.IsNullOrEmpty(userId) || ctrl == null || act == null)
                return false;

            var matches = await (from ur in _context.ApplicationUserRoles
                                 join rm in _context.RoleMenuPermission on ur.RoleId equals rm.RoleId
                                 where ur.UserId == userId
                                       && rm.NavigationMenu.ControllerName == ctrl
                                       && rm.NavigationMenu.ActionName == act
                                 select new { rm.NavigationMenu.ControllerName, rm.NavigationMenu.ActionName })
                                .AsNoTracking()
                                .ToListAsync();

            // Case-insensitive, like MVC routing itself: the permission rows mix "Edit" and "edit" while the
            // action methods are named edit/delete/create, so an exact match denied pages the role was given.
            return matches.Any(m => string.Equals(m.ControllerName, ctrl, StringComparison.OrdinalIgnoreCase)
                                 && string.Equals(m.ActionName, act, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<List<AspNavigationMenuDTO>> GetPermissionsByRoleIdAsync( string id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenuPermission
                               on new { X1 = m.Id, X2 = id } equals
                                  new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                               into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new AspNavigationMenuDTO()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id,
                                   FatherRoleID = id,
                                   code=m.Code,
                                   
                               }).OrderBy(x=>x.DisplayOrder)
                            .AsNoTracking()
                            .ToListAsync();


            return items;
        }
        public async Task<List<IGrouping<int,AspNavigationMenuDTO>>> GetPermissionsByRoleIdAsync2(string id)
        {
            var items = await (from m in _context.NavigationMenu
                               join rm in _context.RoleMenuPermission
                               on new { X1 = m.Id, X2 = id } equals
                                  new { X1 = rm.NavigationMenuId, X2 = rm.RoleId }
                               into rmp
                               from rm in rmp.DefaultIfEmpty()
                               select new AspNavigationMenuDTO()
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                                   Area = m.Area,
                                   ActionName = m.ActionName,
                                   ControllerName = m.ControllerName,
                                   IsExternal = m.IsExternal,
                                   ExternalUrl = m.ExternalUrl,
                                   DisplayOrder = m.DisplayOrder,
                                   ParentMenuId = m.ParentMenuId,
                                   Visible = m.Visible,
                                   Permitted = rm.RoleId == id,
                                   FatherRoleID = id
                               }).GroupBy(x => x.DisplayOrder)
                            .AsNoTracking()
                            .ToListAsync();

            return items;
        }

        public async Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<Guid> permissionIds)
        {
            var existing = await _context.RoleMenuPermission.Where(x => x.RoleId == id).ToListAsync();
            _context.RemoveRange(existing);

            foreach (var item in permissionIds)
            {
                await _context.RoleMenuPermission.AddAsync(new RoleMenuPermission()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            var result = await _context.SaveChangesAsync();

            // Remove existing permissions to roles from Cache so it can re evaluate and take effect
             _cache.Remove("RolePermissions");
           

            return result > 0;
        }
       
        private async Task<List<string>> GetUserRoleIds(ClaimsPrincipal ctx)
        {
            var userId = GetUserId(ctx);
            var data = await (from role in _context.ApplicationUserRoles
                              where role.UserId == userId
                              select role.RoleId).ToListAsync();

            return data;
        }

        private static string? GetUserId(ClaimsPrincipal user) =>
            (user.Identity) == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
