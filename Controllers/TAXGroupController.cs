using IdentityCore.Factory.RepositoryTAXGroupDTO;
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
    // [Authorize("Authorization")]
    public class  TAXGroupController : Controller
    {
        private readonly ILogger< TAXGroupController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        ITAXGroupRepo _TAXGroupRepo;
        private readonly IAccessFilterService _accessFilter;

        public  TAXGroupController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger< TAXGroupController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _TAXGroupRepo = new  TAXGroupRepo(_DBcontext,accessFilter);
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult>  TAXGroupList()
        {

            var  TAXGroupList = await _TAXGroupRepo.GetTAXGroupList(User);
            return View( TAXGroupList);

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
        [TenantScope(TenantEntity.Tax, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _TAXGroupRepo.GetTAXGroup(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new  TAXGroupDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Tax, "obj.TAXID", scopeOnly: true)]
        public IActionResult createPost( TAXGroupDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _TAXGroupRepo.SaveTAXGroup(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = " TAXGroup Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = " TAXGroup Record Added Successfully";

                }


                return RedirectToAction("TAXGroupList", "TAXGroup" );

            }
            else
            {
                TempData["error"] = " TAXGroup Record NotAdded An Error Accour";

                
                  return RedirectToAction("create", "TAXGroup");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Tax, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
              //  var Group =  DBcontext. TAXGroup.Include(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);
                var Group = DBcontext. TAXGroup.FirstOrDefault(x => x.TAXID == id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneTAXGroupDTO();
                  
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
        [TenantScope(TenantEntity.Tax, "obj.TAXID")]
        public IActionResult EditPost( TAXGroupDTO obj)
        {


            if (ModelState.IsValid)
            {
                var IsSucess = _TAXGroupRepo.EditTAXGroup( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = " TAXGroup Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = " TAXGroup Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = " TAXGroup Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("TAXGroupList", "TAXGroup" );
            }
            else
            {
               
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.TAXID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Tax, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _TAXGroupRepo.GetTAXGroup(  id);
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
        [TenantScope(TenantEntity.Tax, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group =  DBcontext.TAXGroup.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _TAXGroupRepo.DeleteTAXGroup(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete  TAXGroup ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete  TAXGroup , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The  TAXGroup , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The  TAXGroup , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("TAXGroupList", "TAXGroup" );

            }


        }

    }
}
