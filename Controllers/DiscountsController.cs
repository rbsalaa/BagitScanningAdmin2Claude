using IdentityCore.Factory.RepositoryDiscountsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Diagnostics;
using static SelfKOrderWeb.Factory.Enums;
using SelectList = SelfKOrderWeb.Factory.SelectList;

namespace SelfKOrderWeb.Controllers
{
   
    public class  DiscountsController : Controller
    {
        private readonly ILogger< DiscountsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IDiscountsRepo _DiscountsRepo;
        private readonly IAccessFilterService _accessFilter;
        public  DiscountsController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger< DiscountsController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _DiscountsRepo = new  DiscountsRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult>  DiscountsList()
        {

            var  DiscountsList = await _DiscountsRepo.GetDiscountsList(User );
            return View( DiscountsList);

        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues(int id2)
        {
            var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues2(int id3, int id2)
        {
            var PreferenceValues = Factory.SelectList.FranchisieSelectList(DBcontext, id3, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues3(int id3, int id2)
        {
            var PreferenceValues = Factory.SelectList.BranchSelectList(DBcontext, id3, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Discount, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _DiscountsRepo.GetDiscounts(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new  DiscountsDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Discount, "obj.DiscountID", scopeOnly: true)]
        public IActionResult createPost( DiscountsDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _DiscountsRepo.SaveDiscounts(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = " Discounts Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = " Discounts Record Added Successfully";

                }


                return RedirectToAction("DiscountsList", "Discounts" );

            }
            else
            {
                TempData["error"] = " Discounts Record NotAdded An Error Accour";

                
                  return RedirectToAction("create", "Discounts");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Discount, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
              //  var Group =  DBcontext. Discounts.Include(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);
                var Group = DBcontext. Discounts.FirstOrDefault(x => x.DiscountID == id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneTDiscountsDTO();
                  
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    if (GroupViewModel.CompID == null)
                        GroupViewModel.BrandList = new List<SelectListItem>();
                    else
                        GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, GroupViewModel.CompID.Value, User, _accessFilter).ToList();
                    if (GroupViewModel.BrandID == null || GroupViewModel.CompID == null || GroupViewModel.BranchID == null)
                        GroupViewModel.BranchList = new List<SelectListItem>();
                    else
                        GroupViewModel.BranchList = Factory.SelectList.BranchSelectList(DBcontext, GroupViewModel.CompID.Value, GroupViewModel.BrandID.Value, User, _accessFilter).ToList();
                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        [TenantScope(TenantEntity.Discount, "obj.DiscountID")]
        public IActionResult EditPost( DiscountsDTO obj)
        {


            if (ModelState.IsValid)
            {
                var IsSucess = _DiscountsRepo.EditDiscounts( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = " Discounts Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = " Discounts Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = " Discounts Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("DiscountsList", "Discounts" );
            }
            else
            {
               
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.DiscountID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Discount, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _DiscountsRepo.GetDiscounts(  id);
                if (group == null)
                {
                    return NotFound();
                }
                else
                {


                    return View(group);

                }

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("delete")]
        [TenantScope(TenantEntity.Discount, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group =  DBcontext.Discounts.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _DiscountsRepo.DeleteDiscounts(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete  Discounts ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete  Discounts , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The  Discounts , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The  Discounts , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("DiscountsList", "Discounts" );

            }


        }

    }
}
