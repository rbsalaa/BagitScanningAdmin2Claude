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

namespace IdentityCore.Factory.RepositoryStoreMenuDiscountsDTO
{
   
        public class ShopDiscountsRepo : IShopDiscountsRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public ShopDiscountsRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }


      
        public async Task<IEnumerable<StoreMenuDiscountsDTO>> GetDiscountsList(ClaimsPrincipal User)
            {
              //  IEnumerable<StoreMenuDiscountsDTO> GroupsList = await DBcontext.StoreMenuDiscounts.ToStoreMenuDiscountsDTO().ToListAsync();
            IQueryable<StoreMenuDiscountsDTO> filtered2 = _accessFilter.ShopDiscountFilter(User, DBcontext.StoreMenuDiscounts);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<StoreMenuDiscountsDTO> GetDiscounts( int DiscountID)
            {
                var Group = await DBcontext.StoreMenuDiscounts.FirstOrDefaultAsync(x => x.DiscountMenuID == DiscountID);
                return Group?.ToOneStoreMenuDiscountssDTO();
            }

            public int SaveDiscounts( StoreMenuDiscountsDTO e)
            {
                try
                {

                    if (DBcontext.StoreMenuDiscounts.Any(x => x.DiscountMenuID == e.DiscountMenuID)) { return (int)SearchStauts.Dublicated; }

                    var DBObj = new StoreMenuDiscounts()
                    {


                        DiscountMenuID = e.DiscountMenuID,
                        DiscountTitle = e.DiscountTitle,
                        DiscountPercintage = e.DiscountPercintage,
                        DiscountStartDate = e.DiscountStartDate,
                        DiscountEndDate = e.DiscountEndDate,
                        DiscountCode = e.DiscountCode,
                        IsActive = true,
                        IsAllMenuDiscount = e.IsAllMenuDiscount,
                        IsPercintage = e.IsPercintage,
                        BranchID = e.BranchID,
                        BrandID = e.BrandID,
                        CompID = e.CompID,
                        FranchID = e.FranchID

                    };

                    DBcontext.StoreMenuDiscounts.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditDiscounts( StoreMenuDiscountsDTO e)
            {
                try
                {

                    var DBObj = DBcontext.StoreMenuDiscounts.FirstOrDefault(x => x.DiscountMenuID == e.DiscountMenuID);
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {

                    DBObj.DiscountMenuID = e.DiscountMenuID;
                 DBObj.DiscountTitle = e.DiscountTitle;
                 DBObj.DiscountPercintage = e.DiscountPercintage;
                 DBObj.DiscountStartDate = e.DiscountStartDate;
                 DBObj.DiscountEndDate = e.DiscountEndDate;
                 DBObj.DiscountCode = e.DiscountCode;
                 DBObj.IsActive = e.IsActive;
                 DBObj.IsAllMenuDiscount = e.IsAllMenuDiscount;
                 DBObj.IsPercintage = e.IsPercintage;
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
            public int DeleteDiscounts( int id)
            {

                var DBObj = DBcontext.StoreMenuDiscounts.FirstOrDefault(x => x.DiscountMenuID == id);

                try
                {
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound; ;
                    }
                    else
                    {
                       
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
