using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Migrations;
using SelfKOrderWeb.Models;
using System.Diagnostics;
using System.Security.Claims;
using static SelfKOrderWeb.Factory.Enums;

namespace SelfKOrderWeb.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly SelfKOrderWebContext db;
        private readonly ICompanyRepo _CompanyRepo;
        private readonly UserManager<SelfKOrderWebUser> userManager;
    
        public CompanyController(ILogger<CompanyController> logger, IAccessFilterService accessFilter, SelfKOrderWebContext _db, UserManager<SelfKOrderWebUser> _userManager)
        {
            _logger = logger;
            db = _db;
          
            //_accessFilter = accessFilter;
            userManager = _userManager;
        
          
            _CompanyRepo = new CompanyRepo(db, accessFilter);

        }

        [Authorize("Authorization")]
        public async Task<IActionResult> CompanyList()
        {
            var compId = User.Claims;
            var CompanyList = await _CompanyRepo.GetCompanyList( User);
         
           
           var data =  CompanyList.ToList();
            return View(CompanyList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [TenantScope(TenantEntity.Company, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _CompanyRepo.GetCompany(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new CompanyDTO();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Company, "obj.CompID", scopeOnly: true)]
        public IActionResult createPost(CompanyDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _CompanyRepo.SaveCompany(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Company Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Company Record Added Successfully";

                }


                return RedirectToAction("CompanyList", "Company");

            }
            else
            {


                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("create");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Company, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group = db.Company.Find(id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToAssignOneCompanyDTO();

                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        [TenantScope(TenantEntity.Company, "obj.CompID")]
        public IActionResult EditPost(CompanyDTO obj)
        {


            if (ModelState.IsValid)
            {
                var IsSucess = _CompanyRepo.EditCompany(  obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Company Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Company Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Company Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("CompanyList", "Company");
            }
            else
            {

                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.CompID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Company, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _CompanyRepo.GetCompany(  id);
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
        [TenantScope(TenantEntity.Company, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group = db.Company.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _CompanyRepo.DeleteCompany(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Company ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Company , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Company , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Company , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("CompanyList", "Company");

            }


        }
    }
}
