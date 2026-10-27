using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Authoraization
{
    public interface IAccessFilterService
    {
        //  IEnumerable<CompanyDTO> CompFilterOld(ClaimsPrincipal user, IEnumerable<CompanyDTO> data);
        IQueryable<BrandDTO> BrandFilter(ClaimsPrincipal user, IQueryable<Brand> data);
        IQueryable<SelfKOrderWebUser> UsersFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWebUser> data);
        IQueryable<RolesDTO> RoleFilter(ClaimsPrincipal user, IQueryable<ApplicationRole> data);
        IQueryable<CompanyDTO> CompFilter(ClaimsPrincipal user, IQueryable<Company> data);
        IQueryable<FranchisingDTO> FranchFilter(ClaimsPrincipal user, IQueryable<Franchising> data);
         IQueryable<BranchDTO> BranchFilter(ClaimsPrincipal user, IQueryable<Branch> data);
         IQueryable<KiosikDTO> KiosikFilter(ClaimsPrincipal user, IQueryable<Kiosik> data);
        IQueryable<PaymentMethodDTO> PaymentMethodFilter(ClaimsPrincipal user, IQueryable<PaymentMethod> data);
        DashBoardDTO CountKiosikFilter(ClaimsPrincipal user, IQueryable<Kiosik> data);
        IQueryable<TAXGroupDTO> TAXFilter(ClaimsPrincipal user, IQueryable<TAXGroup> data);
        IQueryable<StoreMenuDiscountsDTO> ShopDiscountFilter(ClaimsPrincipal user, IQueryable<StoreMenuDiscounts> data);
        IQueryable<DiscountsDTO> DiscountFilter(ClaimsPrincipal user, IQueryable<Discounts> data);
        IQueryable<AdvertismentsDTO> AdverFilter(ClaimsPrincipal user, IQueryable<Advertisments> data);
        IQueryable<CategoryDTO> catFilter(ClaimsPrincipal user, IQueryable<Category> data);
        IQueryable<GroupDTO> GroupFilter(ClaimsPrincipal user, IQueryable<Models.Groups> data);
        IQueryable<ItemDTO> ItemFilter(ClaimsPrincipal user, IQueryable<Models.Items> data);
        IQueryable<MenuDTO> MenuFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Menu> data);
        IQueryable<ModifierDTO> ModifierFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Modifier> data);
        IQueryable<BannersDTO> BannerFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Banners> data);
        IQueryable<OrderSummaryDTO> OrderFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Order> data);
        
             IQueryable<KiosikHardwareLog> HardwareFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.KiosikHardwareLog> data);
        IQueryable<OrderDetail> OrderDetailsFilter(ClaimsPrincipal user, IQueryable<OrderDetail> data);
        IQueryable<ModifierDTO> ModifierFilter2(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Modifier> data,int id);
        IQueryable<MenuDTO> Menufilter(ClaimsPrincipal user, IQueryable<Menu> data);
        IQueryable<ModifierCategoryDTO> ModifierCategoryFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.ModifierCategory> data);
        IQueryable<InventoryDTO> InventoryFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.InventoryWarehouse> data);
        int OrderCountFilter(ClaimsPrincipal user, IQueryable<Order> data, int kiosikID);
    }

    public class AccessFilterService : IAccessFilterService
    {
        public IQueryable<InventoryDTO> InventoryFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.InventoryWarehouse> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToInventoryDTO();

            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToInventoryDTO();

            // see all branches related to your Comp
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToInventoryDTO();

            // franchise user
            if (FranchID != "")
                return data.Where(d =>
                    d.CompID == int.Parse(CompID) &&
                    d.BrandID == int.Parse(BrandID) //&&
                 //   (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
                    ).ToInventoryDTO();

            // see MyBranch
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) //&&
             //   (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
                ).ToInventoryDTO();
        }

        public IQueryable<CompanyDTO> CompFilter(ClaimsPrincipal user, IQueryable<Company> data)
        {
          
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;

            if (IsRBSUser=="true")
                return data.ToCompanyDTO();

            if (IsSuperMainAdmin=="false")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToCompanyDTO();

            return data.Where(d =>
                d.CompID == int.Parse(CompID)).ToCompanyDTO();
        }
        public IQueryable<BrandDTO> BrandFilter(ClaimsPrincipal user, IQueryable<Brand> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;

            if (IsRBSUser == "true")
                return data.ToBrandDTO();
            //see all brands in db

            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToBrandDTO();
            //see all brands of spesififc comp(MAC&&KFC)
            if(BrandID=="")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) 
              
              ).ToBrandDTO();
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID)
               ).ToBrandDTO();
            //see my brand only
        }
        public IQueryable<FranchisingDTO> FranchFilter(ClaimsPrincipal user, IQueryable<Franchising> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;

            if (IsRBSUser == "true")
                return data.ToFranshisDTO();
            //see all FRANCHISE IN db

            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToFranshisDTO();
            //see all FRANCHISE related to Your Comp
            if (FranchID == ""&&(BrandID!=""))
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToFranshisDTO();
            //see all FRANCHISE related to Your Brand 
            if (FranchID == "" && (BrandID == ""))
                return data.Where(d => d.CompID == int.Parse(CompID) ).ToFranshisDTO();
            //see all FRANCHISE related to Your COMP 
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.FranchID == int.Parse(FranchID)
               ).ToFranshisDTO();
            //see ME
        }

        public IQueryable<BranchDTO> BranchFilter(ClaimsPrincipal user, IQueryable<Branch> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToStoreDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID) ).ToStoreDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToStoreDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if(FranchID!="")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) &&
               d.FranchID == int.Parse(FranchID)
              ).ToStoreDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToStoreDTO();
            //see MyBranch  
        }

        public IQueryable<KiosikDTO> KiosikFilter(ClaimsPrincipal user, IQueryable<Kiosik> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToKiosikDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToKiosikDTO();
            //see all Kiosik  related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToKiosikDTO();
            //see all Kiosik related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) 
              
              ).ToKiosikDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToKiosikDTO();
            //all Kiosik related to Your Branch  
        }
        public IQueryable<RolesDTO> RoleFilter(ClaimsPrincipal user, IQueryable<ApplicationRole> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToaspROLEDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToaspROLEDTO();
            //see all Kiosik  related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToaspROLEDTO();
            //see all Kiosik related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)

              ).ToaspROLEDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToaspROLEDTO();
            //all Kiosik related to Your Branch  
        }

        public DashBoardDTO CountKiosikFilter(ClaimsPrincipal user, IQueryable<Kiosik> data)
        {
            var dashboearObj = new DashBoardDTO()
; var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
            {
              
                dashboearObj.BraKioskConNum = data.Where(x=>x.Branch!=null).Count(x => x.Branch.FranchID == null);
                dashboearObj.FraKioskConNum = data.Where(x => x.Branch != null).Count(x => x.Branch.FranchID != null);
                return dashboearObj;
            }



            if (IsSuperMainAdmin == "true")
            {
                dashboearObj.BraKioskConNum = data.Where(x => x.Branch != null).Count(d => d.CompID == int.Parse(CompID) && d.Branch.FranchID == null);
                dashboearObj.FraKioskConNum =data.Where(x => x.Branch != null).Count(d => d.CompID == int.Parse(CompID) && d.Branch.FranchID != null);
                return dashboearObj;
            }
               
            //see all Kiosik  related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
            {
                dashboearObj.BraKioskConNum = data.Where(x => x.Branch != null).Count(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID) && d.Branch.FranchID == null);
                dashboearObj.FraKioskConNum = data.Where(x => x.Branch != null).Count(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID) && d.Branch.FranchID != null);
                return dashboearObj;
            }
              
            //see all Kiosik related to Your Brand ( MAC) Branche
            if (FranchID != "")
            {
                dashboearObj.BraKioskConNum = data.Where(x => x.Branch != null).Count(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)
                && d.Branch.FranchID == null
              );
                dashboearObj.FraKioskConNum = data.Where(x => x.Branch != null).Count(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)
                && d.Branch.FranchID != null
              ); return dashboearObj;
            }
            else
            {
                dashboearObj.BraKioskConNum = data.Where(x => x.Branch != null).Count(d =>
                            d.CompID == int.Parse(CompID) &&
                            d.BrandID == int.Parse(BrandID) &&
                            d.BranchID == int.Parse(BranchID)
                             && d.Branch.FranchID == null
                           );
                dashboearObj.FraKioskConNum = data.Where(x => x.Branch != null).Count(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)
                && d.Branch.FranchID != null
              ); return dashboearObj;
            }
           
            //all Kiosik related to Your Branch  
        }

        public IQueryable<SelfKOrderWebUser> UsersFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWebUser> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data;
            //see every thing

            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID));
            //see all Users  related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID));
            //see all Users related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)

              );
            //see YourBranch Users  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               );
            //all Kiosik Users related to Your Branch  
        }
        public IQueryable<PaymentMethodDTO> PaymentMethodFilter(ClaimsPrincipal user, IQueryable<PaymentMethod> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToPaymentMethodDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToPaymentMethodDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToPaymentMethodDTO();
            //see all branches related to Your Brand ( MAC) Branche
            //if (FranchID != "")
            //    return data.Where(d =>
            //   d.CompID == int.Parse(CompID) &&
            //   d.BrandID == int.Parse(BrandID) &&
            //   d.BranchID == int.Parse(BranchID) &&
            //   d.FranchID == int.Parse(FranchID)
            //  ).ToPaymentMethodDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToPaymentMethodDTO();
            //see MyBranch  
        }
        public IQueryable<TAXGroupDTO> TAXFilter(ClaimsPrincipal user, IQueryable<TAXGroup> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToTAXGroupDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToTAXGroupDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToTAXGroupDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) &&
               d.FranchID == int.Parse(FranchID)
              ).ToTAXGroupDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToTAXGroupDTO();
            //see MyBranch  
        }

        public IQueryable<OrderSummaryDTO> OrderFilter(ClaimsPrincipal user, IQueryable<Order> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToOrderDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToOrderDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToOrderDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) 
             
              ).ToOrderDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToOrderDTO();
            //see MyBranch  
        }
        public IQueryable<OrderDetail> OrderDetailsFilter(ClaimsPrincipal user, IQueryable<OrderDetail> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data;


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.Order.CompID == int.Parse(CompID));
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.Order.CompID == int.Parse(CompID) && d.Order.BrandID == int.Parse(BrandID));
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.Order.CompID == int.Parse(CompID) &&
               d.Order.BrandID == int.Parse(BrandID) &&
               d.Order.BranchID == int.Parse(BranchID)

              );
            //see MyBranch  
            return data.Where(d =>
                d.Order.CompID == int.Parse(CompID) &&
                d.Order.BrandID == int.Parse(BrandID) &&
                d.Order.BranchID == int.Parse(BranchID)
               );
            //see MyBranch  
        }

        public int OrderCountFilter(ClaimsPrincipal user, IQueryable<Order> data,int kiosikID)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.Count(order => order.KiosikID == kiosikID);


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).Count(order => order.KiosikID == kiosikID);
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).Count(order => order.KiosikID == kiosikID);
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)

              ).Count(order => order.KiosikID == kiosikID);
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).Count(order => order.KiosikID == kiosikID);
            //see MyBranch  
        }
        public IQueryable<KiosikHardwareLog> HardwareFilter(ClaimsPrincipal user, IQueryable<KiosikHardwareLog> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data;


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID));
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID));
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID)

              );
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               );
            //see MyBranch  
        }
        public IQueryable<DiscountsDTO> DiscountFilter(ClaimsPrincipal user, IQueryable<Discounts> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToDiscountsDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToDiscountsDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToDiscountsDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) &&
               d.FranchID == int.Parse(FranchID)
              ).ToDiscountsDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToDiscountsDTO();
            //see MyBranch  
        }
        public IQueryable<StoreMenuDiscountsDTO> ShopDiscountFilter(ClaimsPrincipal user, IQueryable<StoreMenuDiscounts> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToStoreMenuDiscountsDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToStoreMenuDiscountsDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToStoreMenuDiscountsDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               d.BranchID == int.Parse(BranchID) &&
               d.FranchID == int.Parse(FranchID)
              ).ToStoreMenuDiscountsDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToStoreMenuDiscountsDTO();
            //see MyBranch  
        }
        public IQueryable<AdvertismentsDTO> AdverFilter(ClaimsPrincipal user, IQueryable<Advertisments> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToAdvertismentsDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToAdvertismentsDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToAdvertismentsDTO();
            //see all branches related to Your Brand ( MAC) Branche
            //if (FranchID != "")
            //    return data.Where(d =>
            //   d.CompID == int.Parse(CompID) &&
            //   d.BrandID == int.Parse(BrandID) &&
            //   d.BranchID == int.Parse(BranchID) &&
            //   d.FranchID == int.Parse(FranchID)
            //  ).ToAdvertismentsDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToAdvertismentsDTO();
            //see MyBranch  
        }



        public IQueryable<BannersDTO> BannerFilter(ClaimsPrincipal user, IQueryable<Banners> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToBannersDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToBannersDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToBannersDTO();
            //see all branches related to Your Brand ( MAC) Branche
            //if (FranchID != "")
            //    return data.Where(d =>
            //   d.CompID == int.Parse(CompID) &&
            //   d.BrandID == int.Parse(BrandID) &&
            //   d.BranchID == int.Parse(BranchID) &&
            //   d.FranchID == int.Parse(FranchID)
            //  ).ToAdvertismentsDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) &&
                d.BranchID == int.Parse(BranchID)
               ).ToBannersDTO();
            //see MyBranch  
        }
        //--------------------------------------------------- comment branch for all belows to make all branches users see same items menu 
        public IQueryable<ItemDTO> ItemFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Items> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToItemsDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToItemsDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToItemsDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
              (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              // d.FranchID == int.Parse(FranchID)
              ).ToItemsDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) 
               && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               ).ToItemsDTO();
            //see MyBranch  
        }
        public IQueryable<GroupDTO> GroupFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Groups> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToGroupDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToGroupDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToGroupDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID)&&(d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              //   d.FranchID == int.Parse(FranchID)
              ).ToGroupDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               //  d.BranchID == int.Parse(BranchID)
               ).ToGroupDTO();
            //see MyBranch  
        }
        public IQueryable<CategoryDTO> catFilter(ClaimsPrincipal user, IQueryable<Category> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToCategoryDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToCategoryDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToCategoryDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              //  d.BranchID == int.Parse(BranchID) &&
              //  d.FranchID == int.Parse(FranchID)
              ).ToCategoryDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               // d.BranchID == int.Parse(BranchID)
               ).ToCategoryDTO();
            //see MyBranch  
        }
        public IQueryable<MenuDTO> MenuFilter(ClaimsPrincipal user, IQueryable<SelfKOrderWeb.Models.Menu> data)
        {
            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;
            
            if (IsRBSUser == "true")
                return data.ToMenuDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToMenuDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToMenuDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID)&&
                (d.BranchID == int.Parse(BranchID) || d.BranchID == null)).ToMenuDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID)&&
               (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               ).ToMenuDTO();
            //see MyBranch  
        }

        //public MenuDTO OneMenuFilter(ClaimsPrincipal user, SelfKOrderWeb.Models.Menu data)
        //{
        //    var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
        //    var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
        //    var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
        //    var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
        //    var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
        //    var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

        //    if (IsRBSUser == "true")
        //        return data.ToMenuDTO2();


        //    if (IsSuperMainAdmin == "true")
        //        return data.Where(d => d.CompID == int.Parse(CompID)).ToMenuDTO2();
        //    //see all branches related to Your Comp (KFC && MAC) Branche
        //    if (BranchID == "")
        //        return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToMenuDTO2();
        //    //see all branches related to Your Brand ( MAC) Branche
        //    if (FranchID != "")
        //        return data.Where(d =>
        //       d.CompID == int.Parse(CompID) &&
        //       d.BrandID == int.Parse(BrandID) &&
        //        (d.BranchID == int.Parse(BranchID) || d.BranchID == null)).ToMenuDTO2();
        //    //see MyBranch  
        //    return data.Where(d =>
        //        d.CompID == int.Parse(CompID) &&
        //        d.BrandID == int.Parse(BrandID) &&
        //       (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
        //       ).ToMenuDTO2();
        //    //see MyBranch  
        //}

        public IQueryable<ModifierDTO> ModifierFilter(ClaimsPrincipal user, IQueryable<Modifier> data)
        {

            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToModifierDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToModifierDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToModifierDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              //   d.BranchID == int.Parse(BranchID) &&
              //  d.FranchID == int.Parse(FranchID)
              ).ToModifierDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               //&& d.BranchID == int.Parse(BranchID)
               ).ToModifierDTO();
        }

      
        public IQueryable<ModifierDTO> ModifierFilter2(ClaimsPrincipal user, IQueryable<Modifier> data,int id)
        {

            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.Where(x => x.ModifierCategory.ModifierCategoryID == id).ToModifierDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID) &&d.ModifierCategory.ModifierCategoryID == id).ToModifierDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID) && d.ModifierCategory.ModifierCategoryID == id).ToModifierDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) &&
               (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               //   d.FranchID == int.Parse(FranchID)
               &&
               d.ModifierCategory.ModifierCategoryID == id
              ).ToModifierDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID) && d.ModifierCategory.ModifierCategoryID == id
               && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               ).ToModifierDTO();
        }

        public IQueryable<ModifierCategoryDTO> ModifierCategoryFilter(ClaimsPrincipal user, IQueryable<ModifierCategory> data)
        {

            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToModifierCategoryDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToModifierCategoryDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToModifierCategoryDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              // d.BranchID == int.Parse(BranchID) //&&
              //  d.FranchID == int.Parse(FranchID)
              ).ToModifierCategoryDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID)
               && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               ).ToModifierCategoryDTO();
        }
        public IQueryable<MenuDTO> Menufilter(ClaimsPrincipal user, IQueryable<Menu> data)
        {

            var CompID = user.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value;
            var BrandID = user.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value;
            var FranchID = user.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value;
            var IsRBSUser = user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value;
            var IsSuperMainAdmin = user.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value;
            var BranchID = user.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value;

            if (IsRBSUser == "true")
                return data.ToMenuDTO();


            if (IsSuperMainAdmin == "true")
                return data.Where(d => d.CompID == int.Parse(CompID)).ToMenuDTO();
            //see all branches related to Your Comp (KFC && MAC) Branche
            if (BranchID == "")
                return data.Where(d => d.CompID == int.Parse(CompID) && d.BrandID == int.Parse(BrandID)).ToMenuDTO();
            //see all branches related to Your Brand ( MAC) Branche
            if (FranchID != "")
                return data.Where(d =>
               d.CompID == int.Parse(CompID) &&
               d.BrandID == int.Parse(BrandID) && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
              // d.BranchID == int.Parse(BranchID) //&&
              //  d.FranchID == int.Parse(FranchID)
              ).ToMenuDTO();
            //see MyBranch  
            return data.Where(d =>
                d.CompID == int.Parse(CompID) &&
                d.BrandID == int.Parse(BrandID)
               && (d.BranchID == int.Parse(BranchID) || d.BranchID == null)
               ).ToMenuDTO();
        }
    }
}
