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
   
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IBranch _BranchRepo;
        private readonly IAccessFilterService _accessFilter;
        public BranchController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<BranchController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _BranchRepo = new BranchRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> BranchList()
        {

            var StoreList = await _BranchRepo.GetStoreList(User);
            return View(StoreList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Branch, "StrId")]
        public async Task<IActionResult> Detail(int StrId, int CompId, int BrndId)
        {

            var groupDTO = await _BranchRepo.GetStore( StrId, CompId, BrndId);

            return View(groupDTO);
        }

        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues(int id)
        {
            var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues2(int id, int id2)
        {
            var PreferenceValues = Factory.SelectList.FranchisieSelectList(DBcontext, id, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }

        [Authorize("Authorization")]

        public IActionResult create()
        {

            var groupDTO = new BranchDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            // groupDTO.BrandList = Factory.SelectList.BrandSelectList(DBcontext, int.Parse(groupDTO.CompList.FirstOrDefault().Value), User, _accessFilter).ToList();
            // groupDTO.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, int.Parse(groupDTO.CompList.FirstOrDefault().Value), int.Parse(groupDTO.BrandList.FirstOrDefault().Value), User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.FranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Branch, "obj.BranchID", scopeOnly: true)]
        public IActionResult createPost(BranchDTO obj)
        {
            if (obj.BrandID == 0)
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, obj.CompID,obj.BrandID, User, _accessFilter).ToList();
                TempData["error"] = "Brand Cannot be empty";
                return View("create", obj);
            }


            if (ModelState.IsValid)
            {

                var IsSucess = _BranchRepo.SaveStore( obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Store Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Store Record Added Successfully";

                }


                return RedirectToAction("BranchList", "Branch");

            }
            else
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();

                TempData["error"] = "Store Record NotAdded An Error Accour";
                return View("create", obj);
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Branch, "StrId")]
        public IActionResult edit(int StrId, int CompId, int BrndId)
        {

            if (StrId == null || CompId == 0 || BrndId == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.Branch.Include(r => r.Brands.Companys).Include(r=>r.Franchis).FirstOrDefault(x => x.BranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneStoreDTO();
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, GroupViewModel.CompID, User, _accessFilter).ToList();
                    GroupViewModel.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, GroupViewModel.CompID, GroupViewModel.BrandID, User, _accessFilter).ToList();

                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        [TenantScope(TenantEntity.Branch, "obj.BranchID")]
        public IActionResult EditPost(BranchDTO obj)
        {
            if (obj.BrandID == 0)
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();

                TempData["error"] = "Brand Cannot be empty";
                return View("edit", obj);
            }

            if (ModelState.IsValid)
            {
                var IsSucess = _BranchRepo.EditStore( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Store Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Store Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Store Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("BranchList", "Branch");

            }
            else
            {
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                obj.BrandList = Factory.SelectList.BrandSelectList(DBcontext, obj.CompID, User, _accessFilter).ToList();
                obj.FranchList = Factory.SelectList.FranchisieSelectList(DBcontext, obj.CompID, obj.BrandID, User, _accessFilter).ToList();


                return View("edit", obj);
            }

        }


        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Branch, "StrId")]
        public async Task<IActionResult> delete(int StrId, int CompId, int BrndId)
        {
            if (StrId == null || CompId == 0 || BrndId == 0)
            {
                return NotFound();
            }
            else
            {

                var group = await _BranchRepo.GetStore( StrId, CompId, BrndId);
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
        [TenantScope(TenantEntity.Branch, "StrId")]
        public IActionResult deletePOST(int StrId, int CompId, int BrndId)
        {
            var Group = DBcontext.Branch.FirstOrDefault(x => x.BranchID == StrId && x.CompID == CompId && BrndId == x.BrandID);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _BranchRepo.DeleteStore( Group);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Store ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Store , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Store , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Store , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("BranchList", "Branch");

            }


        }



    }
}
