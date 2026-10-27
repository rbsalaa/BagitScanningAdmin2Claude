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
using static SelfKOrderWeb.Factory.Enums;
using SelectList = SelfKOrderWeb.Factory.SelectList;

namespace SelfKOrderWeb.Controllers
{
    //[Authorize("Authorization")]
    public class ModifierController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        ModifierRepo _ModifierRepo;
        ItemRepo _ItemRepo;
        ModifierCategoryRepo _ModifierCategoryRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        IAccessFilterService _accessFilter;

        public ModifierController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<ItemsController> logger, SelfKOrderWebContext _DBcontext, IWebHostEnvironment _webHostEnvironment)
        {
            _signInManager = signInManager;
            _logger = logger;
            _ModifierCategoryRepo = new ModifierCategoryRepo(_DBcontext, accessFilter);
            _ModifierRepo = new ModifierRepo(_DBcontext, accessFilter);
                   _ItemRepo = new ItemRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            webHostEnvironment = _webHostEnvironment;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {

            var ItemList = await _ModifierRepo.GetItems(User);
            return View(ItemList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Modifier, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _ModifierRepo.GetItem(id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new ModifierDTO();
         //   groupDTO.modifierCategory = Factory.SelectList.ModifierCategorySelectList(DBcontext).ToList();
           
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        public async Task<IActionResult > createPost(ModifierDTO obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.File != null)
                {
                    if (!UploadHelper.IsAllowedImage(obj.File))
                    {
                        TempData["error"] = "Please upload an image file (jpg, png, gif, webp, svg) up to 10 MB.";
                        return RedirectToAction("create");
                    }
                    string ImagePath = UploadHelper.UniqueName(obj.File);
                    obj.ModifierPic = "\\Modifier" + "\\\\" + ImagePath;
                    await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Modifier", ImagePath);
                }

                var IsSucess = _ModifierRepo.SaveItem(obj);
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


                return RedirectToAction("List", "modifier");

            }
            else
            {
                TempData["error"] = "modifier Record NotAdded An Error Accour";

                return RedirectToAction("create", "modifier");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Modifier, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.Modifiers.FirstOrDefault(x => x.ModifierID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneModifierDTO();
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
        [TenantScope(TenantEntity.Modifier, "obj.ModifierID")]
        public IActionResult editPost(ModifierDTO obj)
        {


            if (ModelState.IsValid)
            {
                int IsSucess = _ModifierRepo.EditItem(obj);
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

                return RedirectToAction("List", "modifier");
            }
            else
            {
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.ModifierID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Modifier, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _ModifierRepo.GetItem(id);
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
        [TenantScope(TenantEntity.Modifier, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = DBcontext.Modifiers.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _ModifierRepo.DeleteItem(id);
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
                return RedirectToAction("List", "modifier");

            }


        }
        public async Task<IActionResult> assignModifierCategory()
        {

            var userList = await _ModifierCategoryRepo.GetItems(User);

            return View(userList);


        }
        public async Task<IActionResult> UnassignModToCategory(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CatItemDTO Group = new CatItemDTO();
                Group.categoryID = id;
                Group.unAssignedItem3 = await _ModifierRepo.GetAssignedMod(id, User);

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
        public async Task<IActionResult> AssignModToCategory(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CatItemDTO Group = new CatItemDTO();
                Group.categoryID = id;
                Group.unAssignedItem3 = await _ModifierRepo.GetUnAssignedMod(id, User);

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
        public async Task<IActionResult> unAssignUserToRoleListPost2(int itemid, int roleid)
        {


            {
                var IsSucess = _ModifierRepo.unAssignItemMod(itemid, roleid);
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

                return RedirectToAction("UnassignModToCategory", "modifier", new { id = itemid });
            }


        }

        public async Task<IActionResult> AssignUserToRoleListPost2(int itemId, int roleid)
        {


            {
                var IsSucess = _ModifierRepo.AssignItemMod(itemId, roleid);
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

                return RedirectToAction("AssignModToCategory", "modifier", new { id = itemId });
            }


        } //----------------------------------------
        public async Task<IActionResult> assignCategory()
        {

            var userList = await _ItemRepo.GetItems(User);

            return View(userList);


        }
      
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
                Group.unAssignedItem2 = await _ModifierRepo.GetAssigned(id, User);

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
                Group.unAssignedItem2 = await _ModifierRepo.GetUnAssigned(id, User);

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
        public async Task<IActionResult> unAssignUserToRoleListPost(int itemid, int roleid)
        {


            {
                var IsSucess = _ModifierRepo.unAssignItem(itemid, roleid);
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

        public async Task<IActionResult> AssignUserToRoleListPost(int itemId, int roleid)
        {


            {
                var IsSucess = _ModifierRepo.AssignItem(itemId, roleid);
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
