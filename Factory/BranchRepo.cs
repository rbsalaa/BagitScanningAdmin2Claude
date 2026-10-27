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
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{
   
        public class BranchRepo : IBranch
        {

            SelfKOrderWebContext DBcontext;
         IAccessFilterService _accessFilter;
            public BranchRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }

        public async Task<IEnumerable<BranchDTO>> GetStoreList(ClaimsPrincipal User)
        {
           // IEnumerable<BranchDTO> GroupsList = await DBcontext.Branch.Include(x=>x.Brands.Companys).ToStoreDTO().ToListAsync();
            var GroupsList = DBcontext.Branch.Include(x => x.Brands.Companys).AsQueryable();

            IQueryable<BranchDTO> filtered2 = _accessFilter.BranchFilter(User, GroupsList);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<BranchDTO> GetStore(  int StrId, int CompId, int BrndId)
        {
            var Group = await DBcontext.Branch.Include(x => x.Brands.Companys).Include(x=>x.Franchis).FirstOrDefaultAsync(x => x.BranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
            return Group?.ToOneStoreDTO();
        }

        public int SaveStore(  BranchDTO e)
        {
            try
            {
                if(e.FranchID == 0)
                {
                    e.FranchID = null;
                }
              e.CompID=DBcontext.Brand.FirstOrDefault(x=>x.BrandID==e.BrandID).CompID;
                if (DBcontext.Branch.Any(x => (x.BrandID == e.BrandID && e.CompID == x.CompID && x.BranchID == e.BranchID))) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Branch()
                {

                    BrandCode = e.BrandCode,
                   
                    BranchLocation = e.BranchLocation,
                    BranchName = e.BranchName,
                    BrandID = e.BrandID,
                    IsActive = true,
                    CompID = e.CompID,
                FranchID=e.FranchID


                };

                DBcontext.Branch.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditStore(  BranchDTO e)
        {
            try
            {


                {


                    var DBObj = DBcontext.Branch.FirstOrDefault(x => x.BranchID == e.BranchID && x.CompID == e.OldCompId && x.BrandID == e.OldBrndId);

                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {

                        DBObj.BrandCode = e.BrandCode;

                        DBObj.BranchLocation = e.BranchLocation;
                        DBObj.BranchName = e.BranchName;
                        DBObj.BrandID = e.BrandID;
                        DBObj.FranchID = e.FranchID;
                        DBObj.CompID = e.CompID;
                       var r = DBcontext.SaveChanges();
                        return r;

                    }
                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
        public int DeleteStore(  Branch carryData)
        {

            var DBObj = DBcontext.Branch.FirstOrDefault(x => x.BranchID == carryData.BranchID && x.CompID == carryData.CompID && carryData.BranchID == x.BranchID);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.Kiosik.Any(x => x.BranchID == DBObj.BranchID))
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
    }
}
    
