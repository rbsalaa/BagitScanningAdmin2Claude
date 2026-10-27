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

namespace IdentityCore.Factory.RepositoryDiscountsDTO
{
   
        public class DiscountsRepo : IDiscountsRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public DiscountsRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }


      
        public async Task<IEnumerable<DiscountsDTO>> GetDiscountsList(ClaimsPrincipal User)
            {
             //   IEnumerable<DiscountsDTO> GroupsList = await DBcontext.Discounts.ToDiscountsDTO().ToListAsync();
            IQueryable<DiscountsDTO> filtered2 = _accessFilter.DiscountFilter(User, DBcontext.Discounts);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }

            public async Task<DiscountsDTO> GetDiscounts( int DiscountID)
            {
                var Group = await DBcontext.Discounts.FirstOrDefaultAsync(x => x.DiscountID == DiscountID);
                return Group?.ToOneTDiscountsDTO();
            }

            public int SaveDiscounts( DiscountsDTO e)
            {
                try
                {

                    if (DBcontext.Discounts.Any(x => x.DiscountID == e.DiscountID)) { return (int)SearchStauts.Dublicated; }

                    var DBObj = new Discounts()
                    {


                        DiscountID = e.DiscountID,
                        DiscountTitle = e.DiscountTitle,
                        DiscountPercintage = e.DiscountPercintage,
                        DiscountStartDate = e.DiscountStartDate,
                        DiscountEndDate = e.DiscountEndDate,
                        DiscountCode = e.DiscountCode,
                        IsActive = true,
                        MAXUserNum = e.MAXUserNum,
                        IsPercintage = e.IsPercintage,
                          BranchID = e.BranchID,
                        BrandID = e.BrandID,
                        CompID = e.CompID,
                        FranchID = e.FranchID
                    };

                    DBcontext.Discounts.Add(DBObj);


                    var r = DBcontext.SaveChanges();
                    return r;
                    return 0;
                }
                catch (Exception ex)
                {
                    return (int)SearchStauts.Error;
                }


            }
            public int EditDiscounts( DiscountsDTO e)
            {
                try
                {

                    var DBObj = DBcontext.Discounts.FirstOrDefault(x => x.DiscountID == e.DiscountID );
                    if (DBObj == null)
                    {
                        return (int)SearchStauts.NotFound;
                    }
                    else
                    {

                    DBObj.DiscountID = e.DiscountID;
                 DBObj.DiscountTitle = e.DiscountTitle;
                 DBObj.DiscountPercintage = e.DiscountPercintage;
                 DBObj.DiscountStartDate = e.DiscountStartDate;
                 DBObj.DiscountEndDate = e.DiscountEndDate;
                 DBObj.DiscountCode = e.DiscountCode;
                 DBObj.IsActive = e.IsActive;
                 DBObj.MAXUserNum = e.MAXUserNum;
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

                var DBObj = DBcontext.Discounts.FirstOrDefault(x => x.DiscountID == id);

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
