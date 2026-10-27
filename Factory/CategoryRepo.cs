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

    public class CategoryRepo : ICategoryRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public CategoryRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<CategoryDTO>> GetItems(ClaimsPrincipal User)
        {
          //  IEnumerable<CategoryDTO> GroupsList = await DBcontext.Category.ToCategoryDTO().ToListAsync();
            IQueryable<CategoryDTO> filtered2 = _accessFilter.catFilter(User, DBcontext.Category);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<CategoryDTO> GetItem(int ItemID)
        {
            var Group = await DBcontext.Category.FirstOrDefaultAsync(x => x.CategoryID == ItemID);
            return Group?.ToOneCategoryDTO();
        }

        public int SaveItem(CategoryDTO obj)
        {
            try
            {

                if (DBcontext.Category.Any(x => x.CategoryID == obj.CategoryID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Category()
                {


                    CategoryID = obj.CategoryID,
                    CategoryName = obj.CategoryName,
                    CategoryName_AR = obj.CategoryName_AR,
                    CategoryOrder = obj.CategoryOrder,
                    CategoryPic=obj.CategoryPic,
                    IsActive = true,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID,
                    IsSuggestionCat = obj.IsSuggestionCat,

                };

                DBcontext.Category.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditItem(CategoryDTO obj)
        {
            try
            {

                var DBObj = DBcontext.Category.FirstOrDefault(x => x.CategoryID == obj.CategoryID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {

                    DBObj.CategoryID = obj.CategoryID;
                    DBObj.CategoryName = obj.CategoryName;
                    DBObj.CategoryName_AR = obj.CategoryName_AR;
                    DBObj.CategoryOrder = obj.CategoryOrder;
                    DBObj.CategoryPic = obj.CategoryPic;
                    DBObj.IsActive = true;
                    DBObj.BranchID = obj.BranchID;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.CompID = obj.CompID;
                    DBObj.FranchID = obj.FranchID;
                    DBObj.IsSuggestionCat = obj.IsSuggestionCat;


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

            var DBObj = DBcontext.Category.FirstOrDefault(x => x.CategoryID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.MenuItems.Any(x => x.ItemID == DBObj.CategoryID) || DBcontext.ItemsCategory.Any(x => x.ItemID == DBObj.CategoryID))
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
            var RolesUserList = await DBcontext.ItemsCategory.Where(x => x.CategoryID == roleid).Select(x => x.ItemID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = await _accessFilter.ItemFilter(User, DBcontext.Items.Where(x => !RolesUserList.Contains(x.ItemID))).ToListAsync();
            return usersList2;
        }

        //public async Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User)
        //{

        //    var RolesUserList = await DBcontext.ItemsCategory.Where(x => x.CategoryID == roleid).Select(x => x.ItemID).ToListAsync();

        //    var usersList2 = await _accessFilter.ItemFilter(User, DBcontext.Items.Where(x => RolesUserList.Contains(x.ItemID))).ToListAsync();
        //    return usersList2;
        //}
        public async Task<IEnumerable<ItemDTO>> GetAssigned(int roleid, ClaimsPrincipal User)
        {
            // Step 1: Join ItemsCategory with Items to get SortOrder
            var query = from ic in DBcontext.ItemsCategory
                        join i in DBcontext.Items on ic.ItemID equals i.ItemID
                        where ic.CategoryID == roleid
                        select new { Item = i, ic.SortOrder };

            // Step 2: Apply access filter on Items
            var filteredItems = await _accessFilter.ItemFilter(User, query.Select(x => x.Item))
                                                   .ToListAsync();

            // Step 3: Attach SortOrder from ItemsCategory
            var result = from f in filteredItems
                         join ic in DBcontext.ItemsCategory
                             on f.ItemID equals ic.ItemID
                         where ic.CategoryID == roleid
                         select new ItemDTO
                         {
                             ItemID = f.ItemID,
                             ItemName = f.ItemName,
                             ItemPrice = f.ItemPrice,
                             GroupName = f.GroupName,
                             SortOrder = ic.SortOrder
                         };

            return result;
        }
        public int AssignItem(int userid, int roleid,int? CategoryOrder)
        {
            try
            {

                var DBObj = new ItemsCategory();

                {



                    DBObj.SortOrder = CategoryOrder;
                    DBObj.IsActive = true;
                    DBObj.ItemID = userid;
                    DBObj.CategoryID = roleid;


                    DBcontext.ItemsCategory.Add(DBObj);
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

                var DBObj = DBcontext.ItemsCategory.FirstOrDefault(x => x.ItemID == userid && x.CategoryID == roleid);

                if (DBObj != null)
                {



                    DBcontext.ItemsCategory.Remove(DBObj);
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

