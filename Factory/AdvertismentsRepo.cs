using Microsoft.AspNetCore.Hosting;
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

namespace IdentityCore.Factory.RepositoryAdvertismentsDTO
{
   
        public class AdvertismentsRepo : IAdvertismentsRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public AdvertismentsRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }


      
        public async Task<IEnumerable<AdvertismentsDTO>> GetAdvertismentsList(ClaimsPrincipal User)
            {
                
            IQueryable<AdvertismentsDTO> filtered2 = _accessFilter.AdverFilter(User, DBcontext.Advertisment);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<AdvertismentsDTO> GetAdvertisments( int AdvertismentsID)
            {
                var Group = await DBcontext.Advertisment.FirstOrDefaultAsync(x => x.AdsID == AdvertismentsID);
                return Group?.ToOneAdvertismentsDTO();
            }

            public int SaveAdvertisments( AdvertismentsDTO obj)
            {
                try
                {

                    if (DBcontext.Advertisment.Any(x => x.AdsID == obj.AdsID)) { return (int)SearchStauts.Dublicated; }
              
                var DBObj = new Advertisments()
                    {

                       
                        AdsTitle = obj.AdsTitle,
                        AdsStartDate = obj.AdsStartDate,
                        AdsEndDate = obj.AdsEndDate,
                        AdsURL = obj.AdsURL,
                        IsActive = true,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID

                };

                    DBcontext.Advertisment.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditAdvertisments( AdvertismentsDTO obj)
            {
                try
                {

                    var DBObj = DBcontext.Advertisment.FirstOrDefault(x => x.AdsID == obj.AdsID );
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {




                    DBObj.AdsTitle = obj.AdsTitle;
                    DBObj.AdsStartDate = obj.AdsStartDate;
                    DBObj.AdsEndDate = obj.AdsEndDate;
                    DBObj.AdsURL = obj.AdsURL;
                    DBObj.IsActive = obj.IsActive; 
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
            public int DeleteAdvertisments( int id)
            {

                var DBObj = DBcontext.Advertisment.FirstOrDefault(x => x.AdsID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                        if (DBcontext.KiosikAds.Any(x => x.AdsID == DBObj.AdsID))
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

        public async Task<IEnumerable<AdvertismentsDTO>> GetKiosikAdsList(int KiosikID,ClaimsPrincipal User)
        {
            var AdsList = _accessFilter.AdverFilter(User, DBcontext.Advertisment);
            var tKiosikAdsList = await DBcontext.KiosikAds.Where(x => x.KiosikID == KiosikID).Select(x => x.AdsID).ToListAsync();
            tKiosikAdsList.Except(tKiosikAdsList);
            var usersList2 = await _accessFilter.AdverFilter(User, DBcontext.Advertisment).Where(x => !tKiosikAdsList.Contains(x.AdsID)).ToListAsync();
            return usersList2;
        }
        public async Task<IEnumerable<AdvertismentsDTO>> GetCurrentKiosikAdsList(int KiosikID, ClaimsPrincipal User)
        {
            var AdsList = _accessFilter.AdverFilter(User, DBcontext.Advertisment);
            var tKiosikAdsList = await DBcontext.KiosikAds.Where(x => x.KiosikID == KiosikID).Select(x => x.AdsID).ToListAsync();
            tKiosikAdsList.Except(tKiosikAdsList);
            var usersList2 = await _accessFilter.AdverFilter(User, DBcontext.Advertisment).Where(x => tKiosikAdsList.Contains(x.AdsID)).ToListAsync();
            return usersList2;
        }
        public int AssignAdsToKiosik(int AdsID, int KiosikID)
        {
            try
            {

                var DBObj = new KiosikAds();

                {




                    DBObj.KiosikID = KiosikID;
                    DBObj.AdsID = AdsID;


                    DBcontext.KiosikAds.Add(DBObj);
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignAdsToKiosik(int KiosikID, int AdsID)
        {
            try
            {

                var DBObj = DBcontext.KiosikAds.FirstOrDefault(x => x.AdsID == AdsID && x.KiosikID == KiosikID);

                if (DBObj != null)
                {



                    DBcontext.KiosikAds.Remove(DBObj);
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
