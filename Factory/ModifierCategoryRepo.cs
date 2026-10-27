using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{

    public class ModifierCategoryRepo : IModifierCategoryRepo
    {


        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public ModifierCategoryRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }


        public async Task<IEnumerable<ModifierCategoryDTO>> GetItems(ClaimsPrincipal User)
        {
          //  IEnumerable<ModifierCategoryDTO> GroupsList = await DBcontext.ModifierCategory.ToModifierCategoryDTO().ToListAsync();
            IQueryable<ModifierCategoryDTO> filtered2 = _accessFilter.ModifierCategoryFilter(User, DBcontext.ModifierCategory);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<ModifierCategoryDTO> GetItem(int ItemID)
        {
            var Group = await DBcontext.ModifierCategory.FirstOrDefaultAsync(x => x.ModifierCategoryID == ItemID);
            return Group?.ToOneModifierCategoryDTO();
        }

        public int SaveItem(ModifierCategoryDTO obj)
        {
            try
            {
                Debug.WriteLine("Hello 1");
                if (DBcontext.ModifierCategory.Any(x => x.ModifierCategoryID == obj.ModifierCategoryID)) { return (int)SearchStauts.Dublicated; }
                Debug.WriteLine("Hello 2");
                var DBObj = new ModifierCategory()
                {


                    ModifierCategoryID = obj.ModifierCategoryID,
                    ModifierCategoryName = obj.ModifierCategoryName,
                    ModifierCategoryName_AR = obj.ModifierCategoryName_AR,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID

                };

                DBcontext.ModifierCategory.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("eror"+ex.Message);

                return (int)SearchStauts.Error;
            }


        }
        public int EditItem(ModifierCategoryDTO obj)
        {
            try
            {

                var DBObj = DBcontext.ModifierCategory.FirstOrDefault(x => x.ModifierCategoryID == obj.ModifierCategoryID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {

                    DBObj.ModifierCategoryID = obj.ModifierCategoryID;
                    DBObj.ModifierCategoryName = obj.ModifierCategoryName;
                    DBObj.ModifierCategoryName = obj.ModifierCategoryName;
                    DBObj.ModifierCategoryName_AR = obj.ModifierCategoryName_AR;
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

            var DBObj = DBcontext.ModifierCategory.FirstOrDefault(x => x.ModifierCategoryID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.Modifiers.Any(x => x.ModifierCategory.ModifierCategoryID == DBObj.ModifierCategoryID))
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

        public async Task<IEnumerable<ModifierDTO>> GetUnAssigned(int roleid,ClaimsPrincipal User)
        {
            //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList = _accessFilter.ModifierFilter2(User, DBcontext.Modifiers, roleid);
            //RolesUserList.Except(RolesUserList);
            //var usersList2 = await DBcontext.Modifiers.Where(x => !RolesUserList.Contains(x.ItemID)).ToItemsDTO().ToListAsync();
            return RolesUserList;
            /* // Step 1: Get the modifiers assigned to this category
            var assignedModifiers = await DBcontext.mo
                .Where(x => x.ModifierCategoryID == roleid)
                .Select(x => x.ModifierID)
                .ToListAsync();

            // Step 2: Apply access filter for user (optional security filtering)
            var rolesUserList = _accessFilter.ModifierFilter(User, DBcontext.Modifiers);

            // Step 3: Return modifiers that are not already assigned
            var unassigned = await rolesUserList
                .Where(x => !assignedModifiers.Contains(x.ModifierID))
                .ToItemsDTO()
                .ToListAsync();

            return unassigned;*/
        }

        public async Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User)
        {
            // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await DBcontext.ItemModifier.Where(x => x.ModifierCategoryID == roleid).Select(x => x.ItemId).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = _accessFilter.ItemFilter(User, DBcontext.Items).Where(x => RolesUserList.Contains(x.ItemID));
            return usersList2;
        }
        public int AssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = new ItemModifier();

                {




                    DBObj.ItemId = userid;
                    DBObj.ModifierCategoryID = roleid;


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

        public int unAssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = DBcontext.ItemModifier.FirstOrDefault(x => x.ItemId == userid && x.ModifierCategoryID == roleid);

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






    }
}

