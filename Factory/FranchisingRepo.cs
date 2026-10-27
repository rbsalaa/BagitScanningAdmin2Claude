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
   
        public class FranchisingRepo : IFranshise
    {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public FranchisingRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }

        public async Task<IEnumerable<FranchisingDTO>> GetFranchiseList(ClaimsPrincipal User)
        {
           // IEnumerable<FranchisingDTO> GroupsList = await DBcontext.Franchising.Include(x=>x.Brands.Companys).ToFranshisDTO().ToListAsync();
            var GroupsList = DBcontext.Franchising.Include(x => x.Brands.Companys).AsQueryable();

            IQueryable<FranchisingDTO> filtered2 = _accessFilter.FranchFilter(User, GroupsList);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<FranchisingDTO> GetFranchise(  int StrId, int CompId, int BrndId)
        {
            var Group = await DBcontext.Franchising.Include(x => x.Brands.Companys).FirstOrDefaultAsync(x => x.FranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
            return Group?.ToOneFranshisDTO();
        }

        public int SaveFranchise(  FranchisingDTO e)
        {
            try
            {
              e.CompID=DBcontext.Brand.FirstOrDefault(x=>x.BrandID==e.BrandID).CompID;
                if (DBcontext.Franchising.Any(x => (x.FranchID == e.FranchID && e.CompID == x.CompID && x.FranchID == e.FranchID))) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Franchising()
                {


                    FranchID = e.FranchID,
                    FranchCode = e.FranchCode,
                    FranchLocation = e.FranchLocation,
                    FranchName = e.FranchName,
                    BrandID = e.BrandID,
                    IsActive = true,

                    CompID = e.CompID




                };

                DBcontext.Franchising.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditFranchise(  FranchisingDTO e)
        {
            try
            {


                {


                    var DBObj = DBcontext.Franchising.FirstOrDefault(x => x.FranchID == e.FranchID && x.CompID == e.OldCompId && x.BrandID == e.OldBrndId);

                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {

                        DBObj.FranchCode = e.FranchCode;

                        DBObj.FranchLocation = e.FranchLocation;
                        DBObj.FranchName = e.FranchName;
                        DBObj.CompID = e.CompID;

                        DBObj.BrandID = e.BrandID;
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
        public int DeleteFranchise(  Franchising carryData)
        {

            var DBObj = DBcontext.Franchising.FirstOrDefault(x => x.FranchID == carryData.FranchID && x.CompID == carryData.CompID && carryData.FranchID == x.FranchID);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.Branch.Any(x => x.CompID == DBObj.CompID && x.BrandID==DBObj.BrandID&&x.IsActive==true&& x.FranchID==DBObj.FranchID))
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
    
