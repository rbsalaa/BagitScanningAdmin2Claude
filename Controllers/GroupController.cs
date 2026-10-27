using IdentityCore.Factory.RepositoryBrandDTO;
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
   
    public class GroupController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        GroupRepo _GroupRepo;
        private readonly IAccessFilterService _accessFilter;
        public GroupController(SignInManager<SelfKOrderWebUser> signInManager, ILogger<ItemsController> logger,    IAccessFilterService AccessFilter, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _GroupRepo = new GroupRepo(_DBcontext, AccessFilter);
            DBcontext = _DBcontext;
            _accessFilter = AccessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {

            var ItemList = await _GroupRepo.GetItems(User);
            return View(ItemList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Group, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _GroupRepo.GetItem(id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new GroupDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Group, "obj.GroupID", scopeOnly: true)]
        public IActionResult createPost(GroupDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _GroupRepo.SaveItem(obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Group Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Group Record Added Successfully";

                }


                return RedirectToAction("List", "Group");

            }
            else
            {
                TempData["error"] = "Group Record NotAdded An Error Accour";

                return RedirectToAction("create", "Group");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Group, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.Groups.FirstOrDefault(x => x.GroupID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneGroupDTO();
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
        [TenantScope(TenantEntity.Group, "obj.GroupID")]
        public IActionResult editPost(GroupDTO obj)
        {


            if (ModelState.IsValid)
            {
                int IsSucess = _GroupRepo.EditItem(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Group Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Group Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Group Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("List", "Group");
            }
            else
            {
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.GroupID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Group, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _GroupRepo.GetItem(id);
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
        [TenantScope(TenantEntity.Group, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = DBcontext.Groups.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _GroupRepo.DeleteItem(id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Group ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Group , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Group , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Group , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("List", "Group");

            }

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


        public async Task<IActionResult> assignCategory()
        {

            var userList = await _GroupRepo.GetItems(User);

            return View(userList);


        }

        [TenantScope(TenantEntity.Group, "id")]
        public async Task<IActionResult> UnassignItemToCategory(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CatItemDTO Group = new CatItemDTO();
                Group.categoryID = id;
                Group.unAssignedItem = await _GroupRepo.GetAssigned(id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group;

                    return View(GroupViewModel);

                }

            }

        }
        [TenantScope(TenantEntity.Group, "id")]
        public async Task<IActionResult> AssignItemToCategory(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CatItemDTO Group = new CatItemDTO();
                Group.categoryID = id;
                Group.unAssignedItem = await _GroupRepo.GetUnAssigned(id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group;

                    return View(GroupViewModel);

                }

            }

        }
        [TenantScope(TenantEntity.Group, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> unAssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _GroupRepo.unAssignItem(userid, roleid);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = " Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "   failed to User UnAssigned An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = " Group UnAssigned Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("assignCategory", "group", new { id = roleid });
            }


        }

        [TenantScope(TenantEntity.Group, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> AssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _GroupRepo.AssignItem(userid, roleid);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "AspNetRole Record  failed to Assign NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "AspNetRole Record  failed to Assign An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "AspNetRole Record Assign Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("assignCategory", "group", new { id = roleid });
            }



        }

    }
}
