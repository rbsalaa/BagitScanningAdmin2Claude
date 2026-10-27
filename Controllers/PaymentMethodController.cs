using IdentityCore.Factory.RepositoryPaymentMethodDTO;
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
using System.Threading;
using static SelfKOrderWeb.Factory.Enums;
using SelectList = SelfKOrderWeb.Factory.SelectList;

namespace SelfKOrderWeb.Controllers
{
 
    public class  PaymentMethodController : Controller
    {
        private readonly ILogger< PaymentMethodController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IPaymentMethodRepo _PaymentMethodRepo;
        private readonly IAccessFilterService _accessFilter;
        public  PaymentMethodController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter,ILogger< PaymentMethodController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _PaymentMethodRepo = new  PaymentMethodRepo(_DBcontext,accessFilter);
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult>  PaymentMethodList()
        {

            var  PaymentMethodList = await _PaymentMethodRepo.GetPaymentMethodList( User);
            return View( PaymentMethodList);

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
        [TenantScope(TenantEntity.PaymentMethod, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _PaymentMethodRepo.GetPaymentMethod(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new  PaymentMethodDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
                groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.PaymentMethod, "obj.PaymentMethodID", scopeOnly: true)]
        public IActionResult createPost( PaymentMethodDTO obj)
        {
            if (ModelState.IsValid)
            {
                var IsSucess = _PaymentMethodRepo.SavePaymentMethod(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = " PaymentMethod Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "PaymentMethod Record Added Successfully";

                }


                return RedirectToAction("PaymentMethodList", "PaymentMethod" );

            }
            else
            {
                TempData["error"] = " PaymentMethod Record NotAdded An Error Accour";

                
                  return RedirectToAction("create", "PaymentMethod");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.PaymentMethod, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
              //  var Group =  DBcontext. PaymentMethod.Include(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);
                var Group = DBcontext. PaymentMethod.FirstOrDefault(x => x.PaymentMethodID == id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOnePaymentMethodDTO();
                   GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    if(GroupViewModel.CompID==null)
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
      //  [ValidateAntiForgeryToken]
        [Authorize("edit")]
        [TenantScope(TenantEntity.PaymentMethod, "obj.PaymentMethodID")]
        public IActionResult EditPost( PaymentMethodDTO obj)
        {
           

            if (ModelState.IsValid)
            {
                var IsSucess = _PaymentMethodRepo.EditPaymentMethod( obj);

                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = " PaymentMethod Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = " PaymentMethod Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = " PaymentMethod Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("PaymentMethodList", "PaymentMethod" );
            }
            else
            {

                return RedirectToAction("PaymentMethodList", "PaymentMethod");
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.PaymentMethod, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _PaymentMethodRepo.GetPaymentMethod(  id);
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
        [TenantScope(TenantEntity.PaymentMethod, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group =  DBcontext.PaymentMethod.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _PaymentMethodRepo.DeletePaymentMethod(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete  PaymentMethod ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete  PaymentMethod , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The  PaymentMethod , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The  PaymentMethod , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("PaymentMethodList", "PaymentMethod" );

            }


        }

    }
}
