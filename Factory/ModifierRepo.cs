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

    public class ModifierRepo : IModifierRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public ModifierRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<ModifierDTO>> GetItems(ClaimsPrincipal User)
        {
           // IEnumerable<ModifierDTO> GroupsList = await DBcontext.Modifiers.ToModifierDTO().ToListAsync();
            IQueryable<ModifierDTO> filtered2 = _accessFilter.ModifierFilter(User, DBcontext.Modifiers);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<IEnumerable<ModifierCategoryDTO>> GetItemsModifires(ClaimsPrincipal User)
        {
            // IEnumerable<ModifierDTO> GroupsList = await DBcontext.Modifiers.ToModifierDTO().ToListAsync();
            IQueryable<ModifierCategoryDTO> filtered2 = _accessFilter.ModifierCategoryFilter(User, DBcontext.ModifierCategory);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }
      

        public async Task<ModifierDTO> GetItem(int ItemID)
        {
            var Group = await DBcontext.Modifiers.FirstOrDefaultAsync(x => x.ModifierID == ItemID);
            return Group?.ToOneModifierDTO();
        }

        public int SaveItem(ModifierDTO obj)
        {
            try
            {

                if (DBcontext.Modifiers.Any(x => x.ModifierID== obj.ModifierID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Modifier()
                {


                    ModifierID = obj.ModifierID,
                    ModifierName = obj.ModifierName,
                    ModifierName_AR = obj.ModifierName_AR,
                    ModifierPrice = obj.ModifierPrice,
                    ModifierPic = obj.ModifierPic,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID

                };

                DBcontext.Modifiers.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditItem(ModifierDTO obj)
        {
            try
            {

                var DBObj = DBcontext.Modifiers.FirstOrDefault(x => x.ModifierID== obj.ModifierID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {

                    DBObj.ModifierID= obj.ModifierID;
                    DBObj.ModifierName= obj.ModifierName;
                    DBObj.ModifierName_AR= obj.ModifierName_AR;
                    DBObj.ModifierPrice= obj.ModifierPrice;
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

            var DBObj = DBcontext.Modifiers.FirstOrDefault(x => x.ModifierID== id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.ItemModifier.Any(x => x.ModifierCategoryID== DBObj.ModifierID) )
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

        public async Task<IEnumerable<ModifierCategoryDTO>> GetUnAssigned(int roleid, ClaimsPrincipal User)
        {
            //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList = await DBcontext.ItemModifier.Where(x => x.ItemId== roleid).Select(x => x.ModifierCategoryID).ToListAsync();
           
            var usersList2 = _accessFilter.ModifierCategoryFilter(User, DBcontext.ModifierCategory).Where(x => !RolesUserList.Contains(x.ModifierCategoryID));
            return usersList2;
        }

        public async Task<IEnumerable<ModifierCategoryDTO>> GetAssigned(int roleid,ClaimsPrincipal User)
        {
            // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await DBcontext.ItemModifier.Where(x => x.ItemId == roleid).Select(x => x.ModifierCategoryID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 =  _accessFilter.ModifierCategoryFilter(User, DBcontext.ModifierCategory).Where(x => RolesUserList.Contains(x.ModifierCategoryID));
            return usersList2;
        }
        public int AssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = new ItemModifier();

                {




                    DBObj.ItemId = userid;
                    DBObj.ModifierCategoryID= roleid;
                    DBObj.AlohaProductID = "0";


                    DBcontext.ItemModifier.Add(DBObj);
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignItem(int itemID, int roleid)
        {
            try
            {

                var DBObj = DBcontext.ItemModifier.FirstOrDefault(x => x.ItemId == itemID && x.ModifierCategoryID== roleid);

                if (DBObj != null)
                {



                    DBcontext.ItemModifier.Remove(DBObj);
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


        //--------------------------------------- mod cat assign 
        public async Task<IEnumerable<ModifierDTO>> GetUnAssignedMod(int roleid, ClaimsPrincipal User)
        {
            //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList = await DBcontext.ModifierModifierCategory.Where(x => x.ModifierCategoryID == roleid).Select(x => x.ModifierID).ToListAsync();

            var usersList2 = _accessFilter.ModifierFilter(User, DBcontext.Modifiers).Where(x => !RolesUserList.Contains(x.ModifierID));
            return usersList2;
        }
        public async Task<IEnumerable<ModifierDTO>> GetAssignedMod(int roleid, ClaimsPrincipal User)
        {
            // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await DBcontext.ModifierModifierCategory.Where(x => x.ModifierCategoryID == roleid).Select(x => x.ModifierID).ToListAsync();
          //  RolesUserList.Except(RolesUserList);
            var usersList2 = _accessFilter.ModifierFilter(User, DBcontext.Modifiers).Where(x => RolesUserList.Contains(x.ModifierID));
            return usersList2;
        }
        public int AssignItemMod(int userid, int roleid)
        {
            try
            {

                var DBObj = new ModifierModifierCategory();

                {




                    DBObj.ModifierCategoryID = userid;
                    DBObj.ModifierID = roleid;
                 


                    DBcontext.ModifierModifierCategory.Add(DBObj);
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignItemMod(int itemID, int roleid)
        {
            try
            {

                var DBObj = DBcontext.ModifierModifierCategory.FirstOrDefault(x => x.ModifierID == roleid && x.ModifierCategoryID == itemID);

                if (DBObj != null)
                {



                    DBcontext.ModifierModifierCategory.Remove(DBObj);
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

