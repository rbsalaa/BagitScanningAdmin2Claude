using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using System.Security.Claims;
using static SelfKOrderWeb.Factory.Enums;

namespace SelfKOrderWeb.Factory
{
    public static class SelectList
    {
        public static IEnumerable<SelectListItem> KiosikMenuSelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.Menufilter(User, db.Menu);
            var list = x.Select(item => new SelectListItem()
            { Text = item.MenuName, Value = item.MenuID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> CompanySelectList(SelfKOrderWebContext db,ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.CompFilter(User, db.Company);
           var list = x.Select(item => new SelectListItem()
            { Text = item.CompName, Value = item.CompID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> ModifierCategorySelectList(SelfKOrderWebContext db)
        {

            var list = db.ModifierCategory.Select(item => new SelectListItem()
            { Text = item.ModifierCategoryName, Value = item.ModifierCategoryID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> CompanyoneSelectList(SelfKOrderWebContext db,int id, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {

            var x = accessFilter.CompFilter(User, db.Company);
            var list = x.Where(x=>x.CompID==id).Select(item => new SelectListItem()
            { Text = item.CompName, Value = item.CompID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> GroupSelectList(SelfKOrderWebContext db)
        {

            var list = db.Groups.Select(item => new SelectListItem()
            { Text = item.GroupName, Value = item.GroupID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> BrandSelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {

            var x = accessFilter.BrandFilter(User, db.Brand.Where(x=>x.IsActive==true));
            var list =x.Select(item => new SelectListItem()
            { Text = item.BrandName, Value = item.BrandID.ToString() });

            return list;

        }

        public static IEnumerable<SelectListItem> CustomerSelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {

            //var x = accessFilter.BrandFilter(User, db.Brand);
            var list = db.Customer.Where(x=>x.IsActive).Select(item => new SelectListItem()
            { Text = item.CustomerName, Value = item.CustomerID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> BrandSelectList(SelfKOrderWebContext db, int id, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.BrandFilter(User, db.Brand.Where(x => x.CompID == id));
            var list = x.Select(item => new SelectListItem()
            { Text = item.BrandName, Value = item.BrandID.ToString() });

            return list;

        }

        public static IEnumerable<SelectListItem> BranchSelectList(SelfKOrderWebContext db, int id, int BrandID, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.BranchFilter(User, db.Branch.Where(x => x.CompID == id && x.BrandID == BrandID));
            var list = x.Select(item => new SelectListItem()
            { Text = item.BranchName, Value = item.BranchID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> FranchisieSelectList(SelfKOrderWebContext db, int id, int BrandID, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.FranchFilter(User, db.Franchising.Where(x => x.CompID == id && x.BrandID == BrandID ));
            var list =x.Select(item => new SelectListItem()
            { Text = item.FranchName, Value = item.FranchID.ToString() });

            return list;

        }

        public static IEnumerable<SelectListItem> ItemsSelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var items = accessFilter.ItemFilter(User, db.Items);
            var list=items.Select(i => new SelectListItem { Value = i.ItemID.ToString(), Text = i.ItemName })
                   .ToList();

                  
            return list;

        }
        public static IEnumerable<SelectListItem> CategorySelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var items = accessFilter.catFilter(User, db.Category);
            var list = items.Select(i => new SelectListItem { Value = i.CategoryID.ToString(), Text = i.CategoryName })
                   .ToList();


            return list;

        }
        public static IEnumerable<SelectListItem> KisoikSelectList(SelfKOrderWebContext db, int id, int BrandID,int BranchID, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.KiosikFilter(User, db.Kiosik.Where(x => x.CompID == id && x.BrandID == BrandID && x.BranchID == BranchID));
            var list = x.Select(item => new SelectListItem()
            { Text = item.KiosikName, Value = item.KiosikID.ToString() });

            return list;

        }
        public static IEnumerable<SelectListItem> AllKisoikSelectList(SelfKOrderWebContext db, ClaimsPrincipal User, IAccessFilterService accessFilter)
        {
            var x = accessFilter.KiosikFilter(User, db.Kiosik);
            var list = x.Select(item => new SelectListItem()
            
            { Text = item.KiosikName, Value = item.CompID+"*" + item.BrandID + "*" + item.BranchID  + "*" + item.KiosikID });

            return list;

        }

        public static IEnumerable<SelectListItem> CustomersSelectList(SelfKOrderWebContext db)
        {

            var list = db.Customer.Where(x => x.IsActive == true ).Select(item => new SelectListItem()
            { Text = item.CustomerName, Value = item.CustomerID.ToString() });

            return list;

        }

        public static IEnumerable<SelectListItem> SubscriptionSelectList(SelfKOrderWebContext db)
        {

            var list = db.SubscrubtionInfo.Where(x => x.IsActive == true).Select(item => new SelectListItem()
            { Text = item.SubscriptionTitle, Value = item.SupscrubtionID.ToString() });


            return list;

        }

    }
}
