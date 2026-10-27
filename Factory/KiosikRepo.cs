using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Collections;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{
   
        public class KiosikRepo : IKiosik
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
            public KiosikRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }

        public async Task<IEnumerable<KiosikDTO>> GetKiosikList(ClaimsPrincipal User)
        {
           var GroupsList =  DBcontext.Kiosik.Include(x=>x.Company).Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Brand).AsQueryable();
            
               IQueryable<KiosikDTO> filtered2 = _accessFilter.KiosikFilter(User, GroupsList);
             //  return filtered2;*/
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }
        public  IEnumerable<IGrouping<string, KiosikDTO>> GetKiosikListbyCustomer(ClaimsPrincipal User)
        {

            // var GroupsList1 =  DBcontext.Kiosik.Include(x => x.Company).Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Brand).ToKiosikDTO().AsEnumerable();
            // IEnumerable<IGrouping<string, KiosikDTO>> GroupsList =  GroupsList1.GroupBy(x => x.CustomerName).AsEnumerable();
            var GroupsList = DBcontext.Kiosik.Include(x => x.Company).Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Brand).AsQueryable();

            IEnumerable<KiosikDTO> filtered2 = _accessFilter.KiosikFilter(User, GroupsList).AsEnumerable();
            IEnumerable<IGrouping<string, KiosikDTO>> GroupsList2 = filtered2.GroupBy(x => x.CustomerName).AsEnumerable();
            return GroupsList2;
        }


        public async Task<KiosikDTO> GetKiosik( int Kiosikid, int StrId, int CompId, int BrndId)
        {
            var Group = await DBcontext.Kiosik.Include(x => x.Company).Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Brand).FirstOrDefaultAsync(x => x.KiosikID == Kiosikid && x.BranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
            return Group?.ToOneKiosikDTO();
        }

        public int SaveKiosik(  KiosikDTO e)
        {
            try
            {
             // e.CompID=DBcontext.Brand.FirstOrDefault(x=>x.BrandID==e.BrandID).CompID;
                if (DBcontext.Kiosik.Any(x => (x.BrandID == e.BrandID && e.CompID == x.CompID && x.BranchID == e.BranchID && x.KiosikID == e.KiosikID))) { return (int)SearchStauts.Dublicated; }

                var DBObj = new Kiosik()
                {

                  
                    KiosikName = e.KiosikName,
                    KiosikRegion = e.KiosikRegion,
                    BranchID = e.BranchID,
                    BrandID = e.BrandID,
                    CompID = e.CompID,
                    CustomerID = e.CustomerID,
                    IsActive = true,
                    IsCurrentSupscriped=false,
                    MenuID=e.MenuID
                  



                };

                DBcontext.Kiosik.Add(DBObj);


                var r = DBcontext.SaveChanges();
                return r;
                return 0;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }


        }
        public int EditKiosik(  KiosikDTO e)
        {
            try
            {


                {


                    var DBObj = DBcontext.Kiosik.FirstOrDefault(x => x.BranchID == e.OldBranchID && x.CompID == e.OldCompID && x.BrandID == e.OldBrandID && x.KiosikID==e.KiosikID);

                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {


                        DBObj.KiosikName = e.KiosikName;
                        DBObj.KiosikRegion = e.KiosikRegion;
                     DBObj.BranchID = e.BranchID;
                     DBObj.BrandID = e.BrandID;
                     DBObj.CompID = e.CompID;
                     DBObj.CustomerID = e.CustomerID;
                     DBObj.IsActive = true;
                        DBObj.MenuID = e.MenuID;
                        DBObj.IsCurrentSupscriped = DBObj.IsCurrentSupscriped;
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
        public int DeleteKiosik(  Kiosik carryData)
        {

            var DBObj = DBcontext.Kiosik.FirstOrDefault(x => x.BranchID == carryData.BranchID && x.CompID == carryData.CompID && carryData.BrandID == x.BrandID &&x.KiosikID==carryData.KiosikID);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                  
                        var x = DBcontext.Remove(DBObj);
                        //db.AppLogs.Add(new AppLog() { Uid = "", ActionDesc = "UnActive/Delete New User with ID " + id, ActionDateTime = DateTime.Now, UidNavigation = null });

                        return DBcontext.SaveChanges();
                  

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
    
