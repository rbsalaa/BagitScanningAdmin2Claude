using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{

    public class MenuRepo : IMenuRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public MenuRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<MenuDTO>> GetItems(ClaimsPrincipal User)
        {
           // IEnumerable<MenuDTO> GroupsList = await DBcontext.Menu.ToMenuDTO().ToListAsync();
            IQueryable<MenuDTO> filtered2 = _accessFilter.MenuFilter(User, DBcontext.Menu);
            

            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<MenuDTO> GetItem(int ItemID, ClaimsPrincipal User)
        {
            //IQueryable<MenuDTO> filtered2 = _accessFilter.MenuFilter(User, DBcontext.Menu.where(x => x.MenuID == ItemID));


            var group = await DBcontext.Menu
     .Include(x => x.MenuItems)
         .ThenInclude(x => x.item)
             .ThenInclude(i => i.ItemCategories)
                 .ThenInclude(ic => ic.Category)
     .Include(x => x.MenuItems)
         .ThenInclude(x => x.item)
             .ThenInclude(i => i.ItemModifiers)
                 .ThenInclude(imc => imc.ModifierCat)
                     .ThenInclude(mc => mc.Modifiers)
     .AsSplitQuery()   // two collection paths in one JOIN multiply rows (items x categories x modifiers)
     .AsNoTracking()
     .FirstOrDefaultAsync(x => x.MenuID == ItemID);

            return group?.ToMenuDTO2();
        }

        public int SaveItem(MenuDTO obj)
        {
            try
            {

                if (DBcontext.Menu.Any(x => x.MenuID == obj.MenuID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Menu()
                {

                    MenuID = obj.MenuID,
                    MenuName = obj.MenuName,
                    IsCurrentMenu = obj.IsCurrentMenu,
                    IsActive = obj.IsActive,
                    IsDefault = obj.IsDefault,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID


                };

                DBcontext.Menu.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditItem(MenuDTO obj)
        {
            try
            {

                var DBObj = DBcontext.Menu.FirstOrDefault(x => x.MenuID == obj.MenuID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {

                    DBObj.MenuID = obj.MenuID;
                    DBObj.MenuName = obj.MenuName;
                    DBObj.IsCurrentMenu = obj.IsCurrentMenu;
                    DBObj.IsActive = obj.IsActive;
                    DBObj.IsDefault = obj.IsDefault;
                    DBObj.BranchID = obj.BranchID;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.CompID = obj.CompID;
                    DBObj.FranchID = obj.FranchID;



                    // db.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "editing  User with ID " + obj.Uid, ActionDateTime = DateTime.Now, UidNavigation = null });
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
        public int DeleteItem(int id)
        {

            var DBObj = DBcontext.Menu.FirstOrDefault(x => x.MenuID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.MenuItems.Any(x => x.ItemID == DBObj.MenuID) || DBcontext.MenuItems.Any(x => x.ItemID == DBObj.MenuID))
                    {
                        return (int)SearchStauts.IlligalOperation;
                    }
                    else
                    {
                        var x = DBcontext.Remove(DBObj);
                        //db.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "UnActive/Delete New User with ID " + id, ActionDateTime = DateTime.Now, UidNavigation = null });

                        return DBcontext.SaveChanges();
                    }

                    //return -1;
                }

            }
            catch (Exception ex)
            {
                return (int)SearchStauts.NonDeletedUser;
            }
        }

        public async Task<IEnumerable<ItemDTO>> GetUnAssigned(int roleid,ClaimsPrincipal User)
        {
            //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList = await DBcontext.MenuItems.Where(x => x.MenuID == roleid).Select(x => x.ItemID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 =  _accessFilter.ItemFilter(User, DBcontext.Items).Where(x => !RolesUserList.Contains(x.ItemID));
            return usersList2;
        }

        public async Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User)
        {
            // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await DBcontext.MenuItems.Where(x => x.MenuID == roleid).Select(x => x.ItemID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 =  _accessFilter.ItemFilter(User, DBcontext.Items).Where(x => RolesUserList.Contains(x.ItemID));
            return usersList2;
        }
        public int AssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = new MenuItems();

                {




                    DBObj.ItemID = userid;
                    DBObj.MenuID = roleid;
                    DBObj.IsNearFinish = false;
                    DBObj.IsCurrentAvalible = true;
                    DBObj.IsCurrentEnabled = true;
                    DBcontext.MenuItems.Add(DBObj);
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = DBcontext.MenuItems.FirstOrDefault(x => x.ItemID == userid && x.MenuID == roleid);

                if (DBObj != null)
                {



                    DBcontext.MenuItems.Remove(DBObj);
                    var r = DBcontext.SaveChanges();
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

       

       

       
    }
}

