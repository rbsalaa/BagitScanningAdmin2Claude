using IdentityCore.Factory.RepositoryBrandDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
using System.Dynamic;
using static SelfKOrderWeb.Factory.Enums;
using SelectList = SelfKOrderWeb.Factory.SelectList;

namespace SelfKOrderWeb.Controllers
{
   // [Authorize("Authorization")]
    public class ModifierCategoryController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        ModifierCategoryRepo _ModifierCategoryRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        IAccessFilterService _accessFilter;
        public ModifierCategoryController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<ItemsController> logger, SelfKOrderWebContext _DBcontext, IWebHostEnvironment _webHostEnvironment)
        {
            _signInManager = signInManager;
            _logger = logger;
            _ModifierCategoryRepo = new ModifierCategoryRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            webHostEnvironment = _webHostEnvironment;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {

            var ItemList = await _ModifierCategoryRepo.GetItems(User);
            return View(ItemList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.ModifierCategory, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _ModifierCategoryRepo.GetItem(id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new ModifierCategoryDTO();
          //  groupDTO.GrpList = Factory.SelectList.GroupSelectList(DBcontext).ToList();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        public async Task<IActionResult > createPost(ModifierCategoryDTO obj)
        {

            
               

                var IsSucess = _ModifierCategoryRepo.SaveItem(obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "modifier Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "modifier Record Added Successfully";

                }


                return RedirectToAction("List", "modifierCategory");

            
            
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.ModifierCategory, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.ModifierCategory.FirstOrDefault(x => x.ModifierCategoryID == id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneModifierCategoryDTO();
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
        [TenantScope(TenantEntity.ModifierCategory, "obj.ModifierCategoryID")]
        public IActionResult editPost(ModifierCategoryDTO obj)
        {


           
                int IsSucess = _ModifierCategoryRepo.EditItem(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "modifier Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "modifier Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "modifier Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("List", "modifierCategory");
           
        }
        [Authorize("Authorization")]

        [TenantScope(TenantEntity.ModifierCategory, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _ModifierCategoryRepo.GetItem(id);
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
        [TenantScope(TenantEntity.ModifierCategory, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = DBcontext.ModifierCategory.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _ModifierCategoryRepo.DeleteItem(id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete modifier ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete modifier , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The modifier , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The modifier , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("List", "modifierCategory");

            }


        }
        public async Task<IActionResult> assignCategory()
        {

            var userList = await _ModifierCategoryRepo.GetItems(User);

            return View(userList);


        }

        [TenantScope(TenantEntity.ModifierCategory, "id")]
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
                Group.unAssignedItem = await _ModifierCategoryRepo.GetAssigned(id,User);

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
        [TenantScope(TenantEntity.ModifierCategory, "id")]
        public async Task<IActionResult> AssignItemToCategory(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                dynamic Group = new ExpandoObject();
                Group.categoryID = id;
                Group.unAssignedItem = await _ModifierCategoryRepo.GetUnAssigned(id,User);
              

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
        [TenantScope(TenantEntity.ModifierCategory, "roleid")]
        [TenantScope(TenantEntity.Item, "ItemID")]
        public async Task<IActionResult> unAssignUserToRoleListPost(int ItemID, int roleid)
        {


            {
                var IsSucess = _ModifierCategoryRepo.unAssignItem(ItemID, roleid);
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
                    TempData["success"] = " Category UnAssigned Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("assignCategory", "modifier", new { id = roleid });
            }


        }

        [TenantScope(TenantEntity.ModifierCategory, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> AssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _ModifierCategoryRepo.AssignItem(userid, roleid);
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

                return RedirectToAction("assignCategory", "modifier", new { id = roleid });
            }


        }
    }
}
