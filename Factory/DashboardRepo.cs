using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{

    public class DashboardRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public DashboardRepo(SelfKOrderWebContext _DBcontext,IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter=accessFilter;
        }

        public async Task<DashBoardDTO> GetCountKiosiksberFranch(ClaimsPrincipal User)
        {
            var GroupsList = DBcontext.Kiosik.Include(x => x.Company).Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Brand).AsQueryable();

            DashBoardDTO filtered2 = _accessFilter.CountKiosikFilter(User, GroupsList);


            //var KiskInBranch = await DBcontext.Kiosik.CountAsync();

            return filtered2;
        }

        public async Task<dynamic> GetTopItemsList(ClaimsPrincipal User)
        {
            //IEnumerable<OrderDetail> GroupsList = await DBcontext.OrderDetail.Include(x => x.Items).ToListAsync();


            var top10OrderedIstems = await _accessFilter.OrderDetailsFilter(User, DBcontext.OrderDetail)
    .Join(
       _accessFilter.ItemFilter(User, DBcontext.Items),
        od => od.ItemId,
        item => item.ItemID,
        (od, item) => new { od.Quantity, item.ItemName }
    )
    .GroupBy(x => x.ItemName)
    .Select(g => new
    {
        ItemName = g.Key,
        TotalQuantity = g.Sum(x => x.Quantity)
    })
    .OrderByDescending(x => x.TotalQuantity)
    .Take(6)
    .ToListAsync();

            return top10OrderedIstems;
        }
        public async Task<dynamic> GetKioskOrder( ClaimsPrincipal User)
        {
         
            // 1️⃣ Fetch kiosiks (only data EF can translate)
            var kiosiks = await _accessFilter.KiosikFilter(User, DBcontext.Kiosik)
                .Select(k => new { k.KiosikID, k.KiosikName })
                .ToListAsync();

            // 2️⃣ One grouped COUNT for all kiosks (same company/brand/branch rules as OrderCountFilter)
            //    instead of one COUNT query per kiosk.
            var counts = await _accessFilter.OrderFilter(User, DBcontext.Order)
                .GroupBy(o => o.KiosikID)
                .Select(g => new { KiosikID = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.KiosikID, x => x.Count);

            var kioskOrderCounts = kiosiks.Select(k => new
            {
                k.KiosikName,
                OrderCount = counts.TryGetValue(k.KiosikID, out var c) ? c : 0
            }).ToList();

            return kioskOrderCounts;
        }


          

        public async Task<dynamic> GetOrderCount(ClaimsPrincipal User)
        {
            var orderCountByStatus = await _accessFilter.OrderFilter(User, DBcontext.Order)
    .GroupBy(order => order.Status)
    .Select(g => new
    {
        Status = g.Key,
        Count = g.Count()
    })
    .ToListAsync();
            //var Group = await DBcontext.Brand.Include(x => x.Companys).FirstOrDefaultAsync(x => x.BrandID == BrandID);
            return orderCountByStatus;
        }

        public async Task<dynamic> TotalAmount(ClaimsPrincipal User)
        {
            var totalAmountByStatus = await _accessFilter.OrderFilter(User, DBcontext.Order).GroupBy(order => order.Status)
    .Select(g => new
    {
        Status = g.Key,
        TotalAmount = g.Sum(order => order.TotalAmount)
    })
    .ToListAsync();
            return totalAmountByStatus;

        }


        public async Task<List<KiosikHardwareLogDTO>> KiosikHardwareLog(ClaimsPrincipal User)
        {
            var kiosikHardwareStatus = await _accessFilter.HardwareFilter(User, DBcontext.KiosikHardwareLog)
      .GroupBy(x => new { x.KiosikID, x.KiosikName })
      .Select(g => new KiosikHardwareLogDTO
      {
         KiosikID= g.Key.KiosikID,
         KiosikName= g.Key.KiosikName,
          POS  = g.Where(x => x.HardWareName == "POS").Select(x => x.Stauts).FirstOrDefault(),
          Payment = g.Where(x => x.HardWareName == "Payment Machiene").Select(x => x.Stauts).FirstOrDefault(),
          Printer = g.Where(x => x.HardWareName == "Printer Machiene").Select(x => x.Stauts).FirstOrDefault(),
          LastUpateDate = g.Max(x => x.LastUpateDate)
      })
      .OrderBy(x => x.KiosikName)
      .ToListAsync();

            return kiosikHardwareStatus;

        }






        public async Task<dynamic> KioskCount()
        {
            var totalAmountByStatus = await DBcontext.Kiosik
    .GroupBy(order => order.KiosikID)
    .Select(g => new
    {
        Status = g.Key,
        count = g.Count()
    })
    .ToListAsync();
            return totalAmountByStatus;

        }





        public int EditBrand(BrandDTO obj)
        {
            try
            {

                var DBObj = DBcontext.Brand.FirstOrDefault(x => x.BrandID == obj.BrandID && x.CompID == obj.CompID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {




                    DBObj.BrandName = obj.BrandName;
                    DBObj.BrandCode = obj.BrandCode;
                    if (obj.BrandPic != null)
                        DBObj.BrandPic = obj.BrandPic;
                    DBObj.CompID = obj.CompID;

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
        public int DeleteBrand(int id)
        {

            var DBObj = DBcontext.Brand.FirstOrDefault(x => x.BrandID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound; ;
                }
                else
                {
                    if (DBcontext.Branch.Any(x => x.BrandID == DBObj.BrandID))
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
