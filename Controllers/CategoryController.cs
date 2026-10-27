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
  
    public class CategoryController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        CategoryRepo _CategoryRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAccessFilterService _accessFilter;

        public CategoryController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService AccessFilter, ILogger<ItemsController> logger, SelfKOrderWebContext _DBcontext, IWebHostEnvironment _webHostEnvironment)
        {
            _signInManager = signInManager;
            _logger = logger;
            _CategoryRepo = new CategoryRepo(_DBcontext, AccessFilter);
            DBcontext = _DBcontext;
            webHostEnvironment = _webHostEnvironment;
            _accessFilter = AccessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {

            var ItemList = await _CategoryRepo.GetItems(User);
            return View(ItemList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Category, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _CategoryRepo.GetItem(id);

            return View(groupDTO);
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
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new CategoryDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Category, "obj.CategoryID", scopeOnly: true)]
        public async Task<IActionResult> createPost(CategoryDTO obj)
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
                    obj.CategoryPic = "\\Category" + "\\\\" + ImagePath;
                    await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Category", ImagePath);
                }


                var IsSucess = _CategoryRepo.SaveItem(obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Category Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Category Record Added Successfully";

                }


                return RedirectToAction("List", "Category");

            }
            else
            {
                TempData["error"] = "Category Record NotAdded An Error Accour";

                return RedirectToAction("create", "Category");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Category, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.Category.FirstOrDefault(x => x.CategoryID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneCategoryDTO();
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
        [TenantScope(TenantEntity.Category, "obj.CategoryID")]
        public IActionResult editPost(CategoryDTO obj)
        {


            if (ModelState.IsValid)
            {
                int IsSucess = _CategoryRepo.EditItem(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Category Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Category Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Category Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("List", "Category");
            }
            else
            {
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.CategoryID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Category, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _CategoryRepo.GetItem(id);
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
        [TenantScope(TenantEntity.Category, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = DBcontext.Category.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _CategoryRepo.DeleteItem(id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Category ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Category , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Category , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Category , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("List", "Category");

            }


        }
        public async Task<IActionResult> assignCategory()
        {

            var userList = await _CategoryRepo.GetItems(User);

            return View(userList);


        }

        [TenantScope(TenantEntity.Category, "id")]
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
                Group.unAssignedItem = await _CategoryRepo.GetAssigned(id,User);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TenantScope(TenantEntity.Category, "CategoryID")]
        public IActionResult UpdateSortOrder(int ItemID, int CategoryID, int SortOrder)
        {
            var itemCategory = DBcontext.ItemsCategory
                .FirstOrDefault(x => x.ItemID == ItemID && x.CategoryID == CategoryID);

            if (itemCategory != null)
            {
                itemCategory.SortOrder = SortOrder;
                DBcontext.SaveChanges();
            }

            TempData["Success"] = "Sort order updated successfully!";
            return RedirectToAction("UnassignItemToCategory", new { id = CategoryID });
        }
        [TenantScope(TenantEntity.Category, "id")]
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
                Group.unAssignedItem = await _CategoryRepo.GetUnAssigned(id,User);

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
        [TenantScope(TenantEntity.Category, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> unAssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _CategoryRepo.unAssignItem(userid, roleid);
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

                return RedirectToAction("assignCategory", "category", new { id = roleid });
            }


        }

        [TenantScope(TenantEntity.Category, "CategoryID")]
        [TenantScope(TenantEntity.Item, "ItemID")]
        public async Task<IActionResult> AssignUserToRoleListPost(int ItemID, int CategoryID, int CategoryOrder)
        {


            {
                var IsSucess = _CategoryRepo.AssignItem(ItemID, CategoryID, CategoryOrder);
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

                return RedirectToAction("assignCategory", "category", new { id = CategoryID });
            }


        }
    }
}
