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
using SelfKOrderWeb.Migrations;
using SelfKOrderWeb.Models;
using System.Threading;
using static SelfKOrderWeb.Factory.Enums;

namespace SelfKOrderWeb.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsersController> _logger;
        private readonly SelfKOrderWebContext db;
        private readonly IUserManageRepo UserRepo;
        private readonly UserManager<SelfKOrderWebUser> userManager;
        private readonly IAccessFilterService _accessFilter;
        public UsersController(IHttpContextAccessor httpContextAccessor, IAccessFilterService accessFilter, ILogger<UsersController> logger, SelfKOrderWebContext _db, UserManager<SelfKOrderWebUser> _userManager)
        {
            _logger = logger;
            db = _db;
            UserRepo = new UserManageRepo(db,accessFilter);
            userManager = _userManager;
            _httpContextAccessor = httpContextAccessor;
            _accessFilter = accessFilter;
        }

        // GET: Users
        #region User 
        [Authorize("Authorization")]
        public IActionResult detail()
        {
            return View();
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> List()
        {
            var userList = await UserRepo.GetUsersList(User);
            var carrier = new CarrierDTO();
            carrier.UsersList = userList;
            //var user = new UserSessionAccessor(_httpContextAccessor);
            // var compId = user.CompID;
            var IsRBSUser =bool.Parse( User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value);
            //carrier.obj = new AddUserDTO();
            //carrier.obj.IsRBSUser = IsRBSUser;
            //carrier.obj.CompList = Factory.SelectList.CompanySelectList(db,User,_accessFilter).ToList();
            //carrier.obj.BrandList = Factory.SelectList.BrandSelectList(db, int.Parse(carrier.obj.CompList.FirstOrDefault().Value), User, _accessFilter).ToList();
            //carrier.obj.FranchList = Factory.SelectList.FranchisieSelectList(db, int.Parse(carrier.obj.CompList.FirstOrDefault().Value), int.Parse(carrier.obj.BrandList.FirstOrDefault().Value), User, _accessFilter).ToList();
            //carrier.obj.BranchList = Factory.SelectList.BranchSelectList(db, int.Parse(carrier.obj.CompList.FirstOrDefault().Value), int.Parse(carrier.obj.BrandList.FirstOrDefault().Value), User, _accessFilter).ToList();


            return View(carrier);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new AddUserDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(db, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            groupDTO.FranchList = new List<SelectListItem>();
            var IsRBSUser = bool.Parse(User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value);
           
            groupDTO.IsCreatorRBSUser = IsRBSUser;
            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> Edit(string id)
        {
            var target = string.IsNullOrEmpty(id) ? null : await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (target == null)
                return NotFound();
            if (!CanManageUser(target))
                return Forbid();

            var GroupViewModel = await UserRepo.GetCrudUser(id);
            GroupViewModel.CompList = Factory.SelectList.CompanySelectList(db, User, _accessFilter).ToList();

            if (GroupViewModel.CompID == null)
                GroupViewModel.BrandList = new List<SelectListItem>();
            else
                GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(db, GroupViewModel.CompID.Value, User, _accessFilter).ToList();
            if (GroupViewModel.BrandID == null || GroupViewModel.CompID == null || GroupViewModel.BranchID == null)
                GroupViewModel.BranchList = new List<SelectListItem>();
            else
                GroupViewModel.BranchList = Factory.SelectList.BranchSelectList(db, GroupViewModel.CompID.Value, GroupViewModel.BrandID.Value, User, _accessFilter).ToList();
            if (GroupViewModel.BrandID == null || GroupViewModel.CompID == null || GroupViewModel.BranchID == null)
                GroupViewModel.FranchList = new List<SelectListItem>();
            else
                GroupViewModel.FranchList = Factory.SelectList.FranchisieSelectList(db, GroupViewModel.CompID.Value, GroupViewModel.BrandID.Value, User, _accessFilter).ToList();


            return View(GroupViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Edit")]
        public async Task<IActionResult> EditPost(AddUserDTO obj)
        {
            var target = string.IsNullOrEmpty(obj?.Id) ? null : await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == obj.Id);
            if (target == null)
            {
                TempData["error"] = "User not found";
                return RedirectToAction("List");
            }
            if (!CanManageUser(target))
                return Forbid();
            RestrictToCreatorScope(obj);

            var result = await UserRepo.EditUser(obj);

            if (result > 0)
            {
                _logger.LogInformation("User Updated Successfully");
                TempData["success"] = "User Updated Successfully";
            }
            else
            {
                TempData["error"] = "User  Record  Not Updated An Error Accour";
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        public async Task<IActionResult> createPost(AddUserDTO carrier)
        {
            RestrictToCreatorScope(carrier);
            var obj = carrier;
            var tempUser = new SelfKOrderWebUser();
            tempUser.Email = obj.Email;
            tempUser.UserName = obj.Email;
            tempUser.FirstName = obj.FirstName;
            tempUser.LastName = obj.LastName;
            tempUser.NormalizedEmail = obj.Email;
            tempUser.NormalizedUserName = obj.FirstName + " " + obj.LastName;
            tempUser.CompID = obj.CompID;
            tempUser.BrandID = obj.BrandID;
            tempUser.FranchID = obj.FranchID;
            tempUser.IsSuperMainAdmin = obj.IsSuperMainAdmin;
            tempUser.IsRBSUser = obj.IsRBSUser;
            tempUser.BranchID=obj.BranchID;
            if (obj.Password == null)
            {
                TempData["error"] = "Password Can not be Null";
                return RedirectToAction("List");
            }
            var result = await userManager.CreateAsync(tempUser, obj.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var createPermission = new UserPermission();
                createPermission.CompID = obj.CompID;
                createPermission.BrandID = obj.BrandID;
                createPermission.FranchID = obj.FranchID;
                createPermission.IsSuperMainAdmin = obj.IsSuperMainAdmin;
                createPermission.IsRBSUser = obj.IsRBSUser;
                createPermission.BranchID = obj.BranchID;
                createPermission.Id = tempUser.Id;
                db.UserPermissions.Add(createPermission);
                db.SaveChanges();
                TempData["success"] = "User Added Successfully";
            }
            else
            {
                TempData["error"] = result.Errors.FirstOrDefault()?.Description ?? "User could not be created";
            }
            return RedirectToAction("List");
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> Delete(string id)
        {
            var target = string.IsNullOrEmpty(id) ? null : await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (target == null)
            {
                TempData["error"] = "User not found";
                return RedirectToAction("List");
            }
            if (!CanManageUser(target))
                return Forbid();

            var result = await UserRepo.DeleteUser(id);

            if (result > 0)
            {
                _logger.LogInformation("User Deleted Successfully");
                TempData["success"] = "User Deleted Successfully";
            }
            else
            {
                TempData["error"] = "User  Record  NotDeleted An Error Accour";
            }
            return RedirectToAction("List");
        }
        // A user may only grant flags they hold themselves: only RBS users can create RBS users,
        // and only RBS users or company super admins can create company super admins.
        // Non-RBS users always stay inside their own company.
        private void RestrictToCreatorScope(AddUserDTO obj)
        {
            if (obj == null) return;
            var isRbs = User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value == "true";
            if (isRbs) return;

            var isSuper = User.Claims.FirstOrDefault(c => c.Type == "IsSuperMainAdmin")?.Value == "true";
            obj.IsRBSUser = false;
            if (!isSuper) obj.IsSuperMainAdmin = false;
            if (int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value, out var myComp))
                obj.CompID = myComp;
        }

        // Roles belong to a company/brand/branch; a user may only manage the roles the role list shows them.
        private async Task<bool> CanManageRoleAsync(string? roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return false;
            if (User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value == "true") return true;
            return await _accessFilter.RoleFilter(User, db.ApplicationRoles.Where(r => r.Id == roleId)).AnyAsync();
        }

        // Non-RBS users create and edit roles only inside their own company.
        private void RestrictRoleScope(RolesDTO obj)
        {
            if (obj == null) return;
            if (User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value == "true") return;
            if (int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value, out var myComp))
                obj.CompID = myComp;
        }

        // Non-RBS users can only edit or delete users of their own company, and never an RBS user.
        private bool CanManageUser(SelfKOrderWebUser target)
        {
            var isRbs = User.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value == "true";
            if (isRbs) return true;
            if (target.IsRBSUser) return false;
            return int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value, out var myComp)
                   && target.CompID == myComp;
        }

        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues(int id2)
        {
            var PreferenceValues = Factory.SelectList.BrandSelectList(db, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues2(int id3, int id2)
        {
            var PreferenceValues = Factory.SelectList.FranchisieSelectList(db, id3, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetPreferenceValues3(int id3, int id2)
        {
            var PreferenceValues = Factory.SelectList.BranchSelectList(db, id3, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        #endregion

        #region Roles 
       [Authorize("Authorization")]
        public async Task<IActionResult> RolesList()
        {
            var userList = await UserRepo.GetRoleList(User);

            return View(userList);
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> RolePermissions(string id)
        {
            if (!await CanManageRoleAsync(id))
                return Forbid();

            var RolesList = await UserRepo.GetPermissionsByRoleIdAsync(id);

            var GropedRole = RolesList.GroupBy(x => x.code, (key, items) => new GroupedAspNavigationMenuDTO
            {
                code = key,
                Items = items.ToList(),
                RoleID = id
            })
        .ToList();

            return View(GropedRole);

        }
        [HttpPost]
       [ValidateAntiForgeryToken]
        [Authorize("RolePermissions")]
        public async Task<IActionResult> EditRolePermission(List<GroupedAspNavigationMenuDTO> viewModel)
        {
            var permissionIds = new List<Guid>();
            string? roleID = viewModel?.FirstOrDefault()?.RoleID;
            if (!await CanManageRoleAsync(roleID))
                return Forbid();
            foreach (var item in viewModel!)
            {
                permissionIds.AddRange((item.Items ?? new List<AspNavigationMenuDTO>()).Where(x => x.Permitted).Select(x => x.Id));

            }

            var RolesList = await UserRepo.SetPermissionsByRoleIdAsync(roleID, permissionIds);
            if (RolesList)
            {
                TempData["success"] = "Permissions had changed Successfully";
                return RedirectToAction("RolesList", "Users");
            }
            else
            {
                TempData["error"] = "Role Permission Record NotChanged An Error Accour";
                return RedirectToAction("RolePermissions", "Users", new { id = roleID }); // there is no "EditRolePermission" view
            }



        }

        [Authorize("Authorization")]
        public async Task<IActionResult> RoleDetail(string id)
        {
            if (!await CanManageRoleAsync(id))
                return Forbid();

            var groupDTO = await UserRepo.GetAspNetRole(id);

            return View(groupDTO);
        }
       [Authorize("Authorization")]
        public IActionResult Rolecreate()
        {

            var groupDTO = new RolesDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(db, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            groupDTO.FranchList = new List<SelectListItem>();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Rolecreate")]
        public IActionResult RolecreatePost(RolesDTO obj)
        {
            RestrictRoleScope(obj);
            if (ModelState.IsValid)
            {

                var IsSucess = UserRepo.SaveAspNetRole(obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "AspNetRole Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "AspNetRole Record Added Successfully";

                }


                return RedirectToAction("RolesList", "Users");

            }
            else
            {


                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("Rolecreate");
            }
        }
       [Authorize("Authorization")]
        public async Task<IActionResult> RoleEdit(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "0" && !await CanManageRoleAsync(id))
                return Forbid();

            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                var Group = db.ApplicationRoles.FirstOrDefault(x => x.Id == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneaspROLEeDTO();
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(db, User, _accessFilter).ToList();

                    if (GroupViewModel.CompID == null)
                        GroupViewModel.BrandList = new List<SelectListItem>();
                    else
                        GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(db, GroupViewModel.CompID.Value, User, _accessFilter).ToList();
                    if (GroupViewModel.BrandID == null || GroupViewModel.CompID == null || GroupViewModel.BranchID == null)
                        GroupViewModel.BranchList = new List<SelectListItem>();
                    else
                        GroupViewModel.BranchList = Factory.SelectList.BranchSelectList(db, GroupViewModel.CompID.Value, GroupViewModel.BrandID.Value, User, _accessFilter).ToList();
                    if (GroupViewModel.BrandID == null || GroupViewModel.CompID == null || GroupViewModel.BranchID == null)
                        GroupViewModel.FranchList = new List<SelectListItem>();
                    else
                        GroupViewModel.FranchList = Factory.SelectList.FranchisieSelectList(db, GroupViewModel.CompID.Value, GroupViewModel.BrandID.Value, User, _accessFilter).ToList();


                    return View(GroupViewModel);

                }

            }

        }
       [Authorize("Authorization")]
        public async Task<IActionResult> AssignUserRole()
        {

            var userList = await UserRepo.GetRoleList(User);

            return View(userList);


        }


        public async Task<IActionResult> RoleCurrentUsersList(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "0" && !await CanManageRoleAsync(id))
                return Forbid();

            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.RoleID = id;
                Group.UsersList = await UserRepo.GetCurrentRolesUsersList(id, User);

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
        public async Task<IActionResult> AssignUserToRoleList(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "0" && !await CanManageRoleAsync(id))
                return Forbid();

            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.RoleID = id;
                Group.UsersList = await UserRepo.GetRolesUsersList(id,User);

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
        public async Task<IActionResult> unAssignUserToRoleListPost(string userid, string roleid)
        {
            var targetUser = string.IsNullOrEmpty(userid) ? null : await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userid);
            if (targetUser == null || !CanManageUser(targetUser) || !await CanManageRoleAsync(roleid))
                return Forbid();


            {
                var IsSucess = UserRepo.unAssignUserToRole(userid, roleid);
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
                    TempData["success"] = " User UnAssigned Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("RoleCurrentUsersList", "Users", new { id = roleid });
            }


        }

        public async Task<IActionResult> AssignUserToRoleListPost(string userid, string roleid)
        {
            var targetUser = string.IsNullOrEmpty(userid) ? null : await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userid);
            if (targetUser == null || !CanManageUser(targetUser) || !await CanManageRoleAsync(roleid))
                return Forbid();


            {
                var IsSucess = UserRepo.AssignUserToRole(userid, roleid);
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

                return RedirectToAction("AssignUserToRoleList", "Users", new { id = roleid });
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("RoleEdit")]
        public async Task<IActionResult> RoleEditPost(RolesDTO obj)
        {
            if (!await CanManageRoleAsync(obj?.Id))
                return Forbid();
            RestrictRoleScope(obj!);


            if (ModelState.IsValid)
            {
                var IsSucess = UserRepo.EditAspNetRole(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "AspNetRole Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "AspNetRole Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "AspNetRole Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("RolesList", "Users");
            }
            else
            {

                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("RoleEdit", new { id = obj.Id });
            }
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> Roledelete(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "0" && !await CanManageRoleAsync(id))
                return Forbid();
            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                var group = await UserRepo.GetAspNetRole(id);
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
        [Authorize("Roledelete")]
        public async Task<IActionResult> RoledeletePOST(string id)
        {
            if (!await CanManageRoleAsync(id))
                return Forbid();
            var Group = db.ApplicationRoles.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = UserRepo.DeleteAspNetRole(id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete AspNetRole ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete AspNetRole , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The AspNetRole , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The AspNetRole , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("RolesList", "Users");

            }


        }

        #endregion


    }
}