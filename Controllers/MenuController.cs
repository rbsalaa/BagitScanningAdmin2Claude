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
  
    public class MenuController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        MenuRepo _MenuRepo;
        private readonly IAccessFilterService _accessFilter;

        public MenuController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilte, ILogger<ItemsController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _MenuRepo = new MenuRepo(_DBcontext, accessFilte);
            DBcontext = _DBcontext;
            _accessFilter = accessFilte;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {

            var ItemList = await _MenuRepo.GetItems(User);
            return View(ItemList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        [TenantScope(TenantEntity.Menu, "id")]
        public async Task<IActionResult> MenuItemsShow(int id)
        {

            var groupDTO = await _MenuRepo.GetItem(id, User);
            if (groupDTO == null)
                return NotFound();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TenantScope(TenantEntity.Menu, "model.MenuID")]
        public async Task<IActionResult> UpdateMenuItems(MenuDTO model)
        {
            // Load this menu's items once instead of one query per posted item.
            var menuItems = await DBcontext.MenuItems
                .Where(mi => mi.MenuID == model.MenuID)
                .ToListAsync();

            foreach (var category in model.categories ?? new List<CategoryDTO>())
            {
                foreach (var item in category.Items ?? new List<ItemDTO>())
                {
                    foreach (var menuItem in menuItems.Where(mi => mi.ItemID == item.ItemID))
                    {
                        menuItem.IsCurrentEnabled = item.IsEnabled;
                    }
                }
            }
            //foreach (var itemDto in model.menuItems)
            //{
            //    var menuItem = await DBcontext.MenuItems
            //        .FirstOrDefaultAsync(mi => mi.MenuID == model.MenuID && mi.ItemID == itemDto.ItemID);

            //    if (menuItem != null)
            //    {
            //        menuItem.IsCurrentEnabled = itemDto.IsEnabled;
            //    }
            //}

            await DBcontext.SaveChangesAsync();

            TempData["Success"] = "Menu items updated successfully!";
            return RedirectToAction("MenuItemsShow", new { id = model.MenuID });
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Menu, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _MenuRepo.GetItem(id, User);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new MenuDTO();
           
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Menu, "obj.MenuID", scopeOnly: true)]
        public IActionResult createPost(MenuDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _MenuRepo.SaveItem(obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Menu Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Menu Record Added Successfully";

                }


                return RedirectToAction("List", "menu");

            }
            else
            {
                TempData["error"] = "Menu Record NotAdded An Error Accour";

                return RedirectToAction("create", "menu");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Menu, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = DBcontext.Menu.FirstOrDefault(x => x.MenuID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneMenuDTO();
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
        [TenantScope(TenantEntity.Menu, "obj.MenuID")]
        public IActionResult editPost(MenuDTO obj)
        {


            if (ModelState.IsValid)
            {
                int IsSucess = _MenuRepo.EditItem(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Menu Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Menu Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Menu Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("List", "menu");
            }
            else
            {
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.MenuID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Menu, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _MenuRepo.GetItem(id, User);
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
        [TenantScope(TenantEntity.Menu, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = DBcontext.Menu.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _MenuRepo.DeleteItem(id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Menu ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Menu , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Menu , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Menu , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("List", "menu");

            }


        }
        public async Task<IActionResult> assignCategory()
        {

            var userList = await _MenuRepo.GetItems(User);

            return View(userList);


        }

        [TenantScope(TenantEntity.Menu, "id")]
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
                Group.unAssignedItem = await _MenuRepo.GetAssigned(id, User);

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
        [TenantScope(TenantEntity.Menu, "id")]
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
                Group.unAssignedItem = await _MenuRepo.GetUnAssigned(id,User);

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
        [TenantScope(TenantEntity.Menu, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> unAssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _MenuRepo.unAssignItem(userid, roleid);
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

                return RedirectToAction("assignCategory", "menu", new { id = roleid });
            }


        }

        [TenantScope(TenantEntity.Menu, "roleid")]
        [TenantScope(TenantEntity.Item, "userid")]
        public async Task<IActionResult> AssignUserToRoleListPost(int userid, int roleid)
        {


            {
                var IsSucess = _MenuRepo.AssignItem(userid, roleid);
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

                return RedirectToAction("AssignItemToCategory", "menu", new { id = roleid });
            }


        }
    }
}
