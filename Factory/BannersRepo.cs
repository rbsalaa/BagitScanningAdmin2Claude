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

namespace IdentityCore.Factory.RepositoryBannersDTO
{
   
        public class BannersRepo : IBanner
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public BannersRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }


      
        public async Task<IEnumerable<BannersDTO>> GetBannersList(ClaimsPrincipal User)
            {
                
            IQueryable<BannersDTO> filtered2 = _accessFilter.BannerFilter(User, DBcontext.Banner);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<BannersDTO> GetBanners( int BannersID)
            {
                var Group = await DBcontext.Banner.FirstOrDefaultAsync(x => x.BannerID == BannersID);
                return Group?.ToOneBannersDTO();
            }

            public int SaveBanners( BannersDTO obj)
            {
                try
                {

                    if (DBcontext.Banner.Any(x => x.BannerID == obj.BannerID)) { return (int)SearchStauts.Dublicated; }
              
                var DBObj = new Banners()
                    {


                    BannerTitle = obj.BannerTitle,
                        BannerStartDate = obj.BannerStartDate,
                        BannerEndDate = obj.BannerEndDate,
                        BannerURL = obj.BannerURL,
                        IsActive = true,
                    BranchID = obj.BranchID,
                    BrandID = obj.BrandID,
                    CompID = obj.CompID,
                    FranchID = obj.FranchID,
                    TargetID = obj.TargetID,
                    TargetType = obj.TargetType,
                    TargetUrl = obj.TargetUrl

                };

                    DBcontext.Banner.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                  
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditBanners( BannersDTO obj)
            {
                try
                {

                    var DBObj = DBcontext.Banner.FirstOrDefault(x => x.BannerID == obj.BannerID );
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {




                    DBObj.BannerTitle = obj.BannerTitle;
                    DBObj.BannerStartDate = obj.BannerStartDate;
                    DBObj.BannerEndDate = obj.BannerEndDate;
                    DBObj.BannerURL = obj.BannerURL;
                    DBObj.IsActive = obj.IsActive; 
                    DBObj.BranchID = obj.BranchID;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.CompID = obj.CompID;
                    DBObj.FranchID = obj.FranchID;
                    DBObj.TargetID = obj.TargetID;
                    DBObj.TargetType = obj.TargetType;
                    DBObj.TargetUrl = obj.TargetUrl;
                    DBObj.IsActive = obj.IsActive;


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
            public int DeleteBanners( int id)
            {

                var DBObj = DBcontext.Banner.FirstOrDefault(x => x.BannerID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                        if (DBcontext.KiosikBanner.Any(x => x.Banner == DBObj.BannerID))
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

        public async Task<IEnumerable<BannersDTO>> GetKiosikBannerList(int KiosikID,ClaimsPrincipal User)
        {
            var BannerList = _accessFilter.BannerFilter(User, DBcontext.Banner);
            var tKiosikBannerList = await DBcontext.KiosikBanner.Where(x => x.KiosikID == KiosikID).Select(x => x.Banner).ToListAsync();
            tKiosikBannerList.Except(tKiosikBannerList);
            var usersList2 = await _accessFilter.BannerFilter(User, DBcontext.Banner).Where(x => !tKiosikBannerList.Contains(x.BannerID)).ToListAsync();
            return usersList2;
        }
        public async Task<IEnumerable<BannersDTO>> GetCurrentKiosikBannerList(int KiosikID, ClaimsPrincipal User)
        {
            var BannerList = _accessFilter.BannerFilter(User, DBcontext.Banner);
            var tKiosikBannerList = await DBcontext.KiosikBanner.Where(x => x.KiosikID == KiosikID).Select(x => x.Banner).ToListAsync();
            tKiosikBannerList.Except(tKiosikBannerList);
            var usersList2 = await _accessFilter.BannerFilter(User, DBcontext.Banner).Where(x => tKiosikBannerList.Contains(x.BannerID)).ToListAsync();
            return usersList2;
        }
        public int AssignBannerToKiosik(int BannerID, int KiosikID)
        {
            try
            {

                var DBObj = new KiosikBanners();

                {




                    DBObj.KiosikID = KiosikID;
                    DBObj.Banner = BannerID;


                    DBcontext.KiosikBanner.Add(DBObj);
                    var r = DBcontext.SaveChanges();
                    return r;

                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignBannerToKiosik(int KiosikID, int BannerID)
        {
            try
            {

                var DBObj = DBcontext.KiosikBanner.FirstOrDefault(x => x.Banner == BannerID && x.KiosikID == KiosikID);

                if (DBObj != null)
                {



                    DBcontext.KiosikBanner.Remove(DBObj);
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
