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

    public class GroupRepo : IGroupRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public GroupRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<GroupDTO>> GetItems(ClaimsPrincipal User)
        {
          //  IEnumerable<GroupDTO> GroupsList = await DBcontext.DBcontext.Groups.ToGroupDTO().ToListAsync();
            IQueryable<GroupDTO> filtered2 = _accessFilter.GroupFilter(User, DBcontext.Groups);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<GroupDTO> GetItem(int ItemID)
        {
            var Group = await DBcontext.Groups.FirstOrDefaultAsync(x => x.GroupID == ItemID);
            return Group?.ToOneGroupDTO();
        }

        public int SaveItem(GroupDTO e)
        {
            try
            {

                if (DBcontext.Groups.Any(x => x.GroupID == e.GroupID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Groups()
                {

                    GroupID = e.GroupID,
                    GroupName = e.GroupName,
                    GroupName_AR = e.GroupName_AR,
                    GroupItemsCount = e.GroupItemsCount,
                    ItemPrice = e.ItemPrice,
                    IsActive = true,
                    BranchID = e.BranchID,
                    BrandID = e.BrandID,
                    CompID = e.CompID,
                    FranchID = e.FranchID

                };

                DBcontext.Groups.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditItem(GroupDTO e)
        {
            try
            {

                var DBObj = DBcontext.Groups.FirstOrDefault(x => x.GroupID == e.GroupID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {
                    DBObj.GroupID = e.GroupID;
                    DBObj.GroupName = e.GroupName;
                    DBObj.GroupName_AR = e.GroupName_AR;
                    DBObj.GroupItemsCount = e.GroupItemsCount;
                    DBObj.ItemPrice = e.ItemPrice;
                    DBObj.IsActive = true;
                    DBObj.BranchID = e.BranchID;
                    DBObj.BrandID = e.BrandID;
                    DBObj.CompID = e.CompID;
                    DBObj.FranchID = e.FranchID;



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

            var DBObj = DBcontext.Groups.FirstOrDefault(x => x.GroupID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.GroupsItem.Any(x => x.GroupID == DBObj.GroupID))
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

        public async Task<IEnumerable<ItemDTO>> GetUnAssigned(int roleid)
        {
            //  var usersList=await db.Users.Select(x=>x.Id).ToListAsync();
            var RolesUserList = await DBcontext.GroupsItem.Where(x => x.GroupID == roleid).Select(x => x.ItemID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = await DBcontext.Items.Where(x => !RolesUserList.Contains(x.ItemID)).ToItemsDTO().ToListAsync();
            return usersList2;
        }

        public async Task<IEnumerable<ItemDTO>> GetAssigned(int roleid)
        {
            // var usersList = await db.Users.Select(x => x.Id).ToListAsync();
            var RolesUserList = await DBcontext.GroupsItem.Where(x => x.GroupID == roleid).Select(x => x.ItemID).ToListAsync();
            RolesUserList.Except(RolesUserList);
            var usersList2 = await DBcontext.Items.Where(x => RolesUserList.Contains(x.ItemID)).ToItemsDTO().ToListAsync();
            return usersList2;
        }
        public int AssignItem(int userid, int roleid)
        {
            try
            {

                var DBObj = new GroupsItem();

                {




                    DBObj.ItemID = userid;
                    DBObj.GroupID = roleid;


                    DBcontext.GroupsItem.Add(DBObj);
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

                var DBObj = DBcontext.GroupsItem.FirstOrDefault(x => x.ItemID == userid && x.GroupID == roleid);

                if (DBObj != null)
                {



                    DBcontext.GroupsItem.Remove(DBObj);
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

