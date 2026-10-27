using IdentityCore.Factory.RepositoryBrandDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using static SelfKOrderWeb.Factory.Enums;
using System.Diagnostics;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Factory;
using SelectList = SelfKOrderWeb.Factory.SelectList;
using Microsoft.AspNetCore.Authorization;
using SelfKOrderWeb.Authoraization;

namespace SelfKOrderWeb.Controllers
{
  //  [Authorize("Authorization")]
    public class KiosikController : Controller
    {
        private readonly ILogger<KiosikController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IKiosik _KiosikRepo;
        private readonly IAccessFilterService _accessFilter;
        public KiosikController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<KiosikController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _KiosikRepo = new KiosikRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            _accessFilter= accessFilter;
        }

        [Authorize("Authorization")]
        public IActionResult KiosikList()
        {

            //    var StoreList = await _KiosikRepo.GetKiosikList();
            IEnumerable<IGrouping<string, KiosikDTO>> StoreList =  _KiosikRepo.GetKiosikListbyCustomer(User);
            return View(StoreList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       [Authorize("Authorization")]
        [TenantScope(TenantEntity.Kiosik, "KiosikID")]
        public async Task<IActionResult> Detail(int KiosikID,int StrId, int CompId, int BrndId)
        {

            var groupDTO = await _KiosikRepo.GetKiosik(KiosikID,StrId, CompId, BrndId);

            return View(groupDTO);
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues(int id)
        {
            var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues2(int id,int  id2)
        {
            var PreferenceValues = Factory.SelectList.BranchSelectList(DBcontext, id,id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }

         [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new KiosikDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
         //   groupDTO.BrandList = Factory.SelectList.BrandSelectList(DBcontext, int.Parse(groupDTO.CompList.FirstOrDefault().Value), User, _accessFilter).ToList();
          //  groupDTO.BranchList = Factory.SelectList.BranchSelectList(DBcontext, int.Parse(groupDTO.CompList.FirstOrDefault().Value), int.Parse(groupDTO.BrandList.FirstOrDefault().Value), User, _accessFilter).ToList();
            groupDTO.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
            groupDTO.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
        //    groupDTO.CustomerList = new List<SelectListItem>();
          
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Kiosik, "obj.KiosikID", scopeOnly: true)]
        public IActionResult createPost(KiosikDTO obj)
        {
            if (obj.BrandID == 0)
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                TempData["error"] = "Brand Cannot be empty";
                return View("create", obj);
            }


            if (ModelState.IsValid)
            {

                var IsSucess = _KiosikRepo.SaveKiosik( obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Kiosik Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Kiosik Record Added Successfully";

                }


                return RedirectToAction("KiosikList", "Kiosik");

            }
            else
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.BranchList = Factory.SelectList.BranchSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();
                obj.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
                obj.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

                TempData["error"] = "Kiosik Record NotAdded An Error Accour";
                return View("create", obj);
            }
        }
       [Authorize("Authorization")]
        [TenantScope(TenantEntity.Kiosik, "KiosikId")]
        public async Task<IActionResult> edit(int KiosikId,int StrId, int CompId, int BrndId)
        {

            if (StrId == null || CompId == 0 || BrndId == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = await _KiosikRepo.GetKiosik( KiosikId,  StrId,  CompId,  BrndId);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group;
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, GroupViewModel.CompID, User, _accessFilter).ToList();
                    GroupViewModel.BranchList = Factory.SelectList.BranchSelectList(DBcontext, GroupViewModel.CompID, GroupViewModel.BrandID, User, _accessFilter).ToList();
                    GroupViewModel.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
                    GroupViewModel.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        [TenantScope(TenantEntity.Kiosik, "obj.KiosikID")]
        public IActionResult EditPost(KiosikDTO obj)
        {
            if (obj.BrandID == 0)
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.BranchList = Factory.SelectList.BranchSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();
                obj.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
                obj.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

                TempData["error"] = "Kiosik Cannot be empty";
                return View("edit", obj);
            }

            if (ModelState.IsValid)
            {
                var IsSucess = _KiosikRepo.EditKiosik( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Kiosik Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Kiosik Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Kiosik Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.BranchList = Factory.SelectList.BranchSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();
                obj.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
                obj.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

                return View("edit", obj);

            }
            else
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.BranchList = Factory.SelectList.BranchSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();
                obj.CustomerList = Factory.SelectList.CustomersSelectList(DBcontext).ToList();
                obj.MenuList = Factory.SelectList.KiosikMenuSelectList(DBcontext, User, _accessFilter).ToList();

                return View("edit", obj);
            }

        }


        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Kiosik, "KiosikId")]
        public async Task<IActionResult> delete(int KiosikId,int StrId, int CompId, int BrndId)
        {
            if (StrId == null || CompId == 0 || BrndId == 0)
            {
                return NotFound();
            }
            else
            {

                var group = await _KiosikRepo.GetKiosik(KiosikId, StrId, CompId, BrndId);
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
        [TenantScope(TenantEntity.Kiosik, "KiosikId")]
        public IActionResult deletePOST(int KiosikId,int StrId, int CompId, int BrndId)
        {
            var Group = DBcontext.Kiosik.FirstOrDefault(x => KiosikId ==x.KiosikID&& x.BranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _KiosikRepo.DeleteKiosik( Group);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Kiosik ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Kiosik , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Kiosik , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Kiosik The Store , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("KiosikList", "Kiosik");

            }


        }



    }
}
