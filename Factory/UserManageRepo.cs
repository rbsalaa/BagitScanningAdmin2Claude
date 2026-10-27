using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using static SelfKOrderWeb.Factory.Enums;

namespace SelfKOrderWeb.Factory
{
    public class UserManageRepo : IUserManageRepo
    {
        SelfKOrderWebContext db;
        IAccessFilterService _accessFilter;
        public UserManageRepo(SelfKOrderWebContext _db, IAccessFilterService accessFilter)
        {
            db = _db;
            _accessFilter = accessFilter;
        }
        public async Task<IEnumerable<SelfKOrderWebUser>> GetUsersList(ClaimsPrincipal User)
        {
           IQueryable <SelfKOrderWebUser> filtered2 = _accessFilter.UsersFilter(User, db.Users);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }
        public async Task<IEnumerable<SelfKOrderWebUser>> GetRolesUsersList(string roleid, ClaimsPrincipal User)
        {
          //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList=await db.ApplicationUserRoles.Where(x=>x.RoleId==roleid).Select(x=>x.UserId).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = await _accessFilter.UsersFilter(User, db.Users.Where(x => !RolesUserList.Contains(x.Id))).ToListAsync();
            return usersList2;
        }

        public async Task<IEnumerable<SelfKOrderWebUser>> GetCurrentRolesUsersList(string roleid,ClaimsPrincipal User)
        {//Lastversion
           // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await db.ApplicationUserRoles.Where(x => x.RoleId == roleid).Select(x => x.UserId).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = await _accessFilter.UsersFilter(User, db.Users.Where(x => RolesUserList.Contains(x.Id))).ToListAsync();
            return usersList2;
        }
        public async Task<IEnumerable<RolesDTO>> GetRoleList(ClaimsPrincipal User)
        {
            IQueryable<RolesDTO> filtered2 = _accessFilter.RoleFilter(User, db.ApplicationRoles);
          //  IEnumerable<RolesDTO> GroupsList = await db.ApplicationRoles.ToaspROLEDTO().ToListAsync();
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<SelfKOrderWebUser> GetUser(string id)
        {
            return await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<AddUserDTO> GetCrudUser(string id)
        {
            var obj = await db.Users.Where(x => x.Id == id).ToUserDTO().FirstOrDefaultAsync();

            return  obj;
        }
        public async Task<int> DeleteUser(string id)
        {
            var userobj = await GetUser(id);
            if (userobj == null) return (int)Enums.SearchStauts.NotFound;
            db.User.Remove(userobj);
            var result = db.SaveChanges();
            return result;
        }

        public async Task<int> EditUser(AddUserDTO obj)
        {
            if (obj.BranchID == 0) obj.BranchID = null;
            if (obj.CompID == 0) obj.CompID = null;
            if (obj.BrandID == 0) obj.BrandID = null;
            if (obj.FranchID == 0) obj.FranchID = null;
            var userobj = await GetUser(obj.Id);
            if (userobj == null) return (int)Enums.SearchStauts.NotFound;
            userobj.Email = obj.Email;
            userobj.NormalizedEmail = obj.Email?.ToUpperInvariant(); // Identity looks emails up by the upper-cased value
            userobj.FirstName = obj.FirstName;
            userobj.LastName = obj.LastName;
            userobj.BrandID = obj.BrandID;
            userobj.CompID = obj.CompID;
            userobj.FranchID = obj.FranchID;
            userobj.IsRBSUser = obj.IsRBSUser;
            userobj.IsSuperMainAdmin = obj.IsSuperMainAdmin;
            userobj.BranchID = obj.BranchID;

            var currentPermission=db.UserPermissions.FirstOrDefault(x=>x.Id== userobj.Id);
            if(currentPermission != null) { 
            currentPermission.BrandID = obj.BrandID;
            currentPermission.CompID = obj.CompID;
            currentPermission.FranchID = obj.FranchID;
            currentPermission.IsRBSUser = obj.IsRBSUser;
            currentPermission.IsSuperMainAdmin = obj.IsSuperMainAdmin;
            currentPermission.BranchID = obj.BranchID;
            }
            else
            {
                currentPermission=new UserPermission();
                currentPermission.Id = obj.Id;
                currentPermission.BrandID = obj.BrandID;
                currentPermission.CompID = obj.CompID;
                currentPermission.FranchID = obj.FranchID;
                currentPermission.IsRBSUser = obj.IsRBSUser;
                currentPermission.IsSuperMainAdmin = obj.IsSuperMainAdmin;
                currentPermission.BranchID = obj.BranchID;
                db.UserPermissions.Add(currentPermission);
            }
            var result = db.SaveChanges();
            return result;
        }




        public async Task<RolesDTO> GetAspNetRole(string Id)
        {
            var Group = await db.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == Id);
            return Group?.ToOneaspROLEeDTO();
        }

        public int SaveAspNetRole(RolesDTO obj)
        {
            try
            {

                if (db.ApplicationRoles.Any(x => x.Id == obj.Id)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new ApplicationRole()
                {

                    Id = obj.Id,
                    Name = obj.Name,
                    ConcurrencyStamp = obj.ConcurrencyStamp,
                    NormalizedName = obj.NormalizedName,
                    BrandID = obj.BrandID,
                CompID = obj.CompID,
                BranchID = obj.BranchID,

            };

                db.ApplicationRoles.Add(DBObj);


                var r = db.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditAspNetRole(RolesDTO obj)
        {
            try
            {

                var DBObj = db.ApplicationRoles.FirstOrDefault(x => x.Id == obj.Id);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {




                    DBObj.Name = obj.Name;
                    DBObj.ConcurrencyStamp = obj.ConcurrencyStamp;
                    DBObj.NormalizedName = obj.NormalizedName;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.CompID = obj.CompID;
                    DBObj.BranchID = obj.BranchID;
                    // db.AppLogs.Add(new AppLog() { UId = "", ActionDesc = "editing  User with Id " + obj.UId, ActionDateTime = DateTime.Now, UIdNavigation = null });
                    var r = db.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int AssignUserToRole(string userid, string roleid )
        {
            try
            {

                var DBObj = new ApplicationUserRole();
              
                {


                  

                    DBObj.UserId = userid;
                    DBObj.RoleId = roleid;
                  

                    db.ApplicationUserRoles.Add(DBObj);
                     var r = db.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignUserToRole(string userid, string roleid)
        {
            try
            {

                var DBObj =db.ApplicationUserRoles.FirstOrDefault(x=>x.UserId==userid&&x.RoleId==roleid);

                if(DBObj!=null) {



                    db.ApplicationUserRoles.Remove(DBObj);
                    var r = db.SaveChanges();
                    return r;

                }
                else
                {
                    return (int)SearchStauts.Error;
                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
        public int DeleteAspNetRole(string Id)
        {

            var DBObj = db.ApplicationRoles.FirstOrDefault(x => x.Id == Id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    //if (db.CrStores.Any(x => x.Id == DBObj.Id))
                    //{
                    //    return (int)SearchStauts.IlligalOperation;
                    //}
                    //else
                    {
                        var x = db.Remove(DBObj);
                        //db.AppLogs.Add(new AppLog() { UId = "", ActionDesc = "UnActive/Delete New User with Id " + Id, ActionDateTime = DateTime.Now, UIdNavigation = null });

                        return db.SaveChanges();
                    }

                    //return -1;
                }

            }
            catch (Exception ex)
            {
                return (int)SearchStauts.NonDeletedUser;
            }
        }

        public async Task<List<AspNavigationMenuDTO>> GetPermissionsByRoleIdAsync(string id)
        {
            var items = await (from m in db.NavigationMenu
                               join rm in db.RoleMenuPermission
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
                                   FatherRoleID = m.ParentMenuId.ToString(),
                                   code = m.Code,
                               }).OrderBy(x=>x.DisplayOrder)
                            .AsNoTracking()
                            .ToListAsync();

            return items;
        }

        public async Task<bool> SetPermissionsByRoleIdAsync(string id, IEnumerable<Guid> permissionIds)
        {
            var existing = await db.RoleMenuPermission.Where(x => x.RoleId == id).ToListAsync();
            db.RemoveRange(existing);

            foreach (var item in permissionIds)
            {
                await db.RoleMenuPermission.AddAsync(new RoleMenuPermission()
                {
                    RoleId = id,
                    NavigationMenuId = item,
                });
            }

            var result = await db.SaveChangesAsync();

            // Remove existing permissions to roles from Cache so it can re evaluate and take effect
            // _cache.Remove("RolePermissions");

            return result > 0;
        }


    }
}

