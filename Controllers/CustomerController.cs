using IdentityCore.Factory.RepositoryCustomerDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
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
   
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        ICustomerRepo _CustomerRepo;

        public CustomerController(SignInManager<SelfKOrderWebUser> signInManager, ILogger<CustomerController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _CustomerRepo = new CustomerRepo(_DBcontext);
            DBcontext = _DBcontext;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> CustomerList()
        {

            var CustomerList = await _CustomerRepo.GetCustomerList( );
            return View(CustomerList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _CustomerRepo.GetCustomer(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new CustomerDTO();
          // groupDTO.SupscriptionList= Factory.SelectList.SubscriptionSelectList(DBcontext).ToList();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        public IActionResult createPost(CustomerDTO obj)
        {

            if (ModelState.IsValid)
            {

                var IsSucess = _CustomerRepo.SaveCustomer(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Customer Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Customer Record Added Successfully";

                }


                return RedirectToAction("CustomerList", "Customer" );

            }
            else
            {
                TempData["error"] = "Customer Record NotAdded An Error Accour";

                
                  return RedirectToAction("create", "Customer");
            }
        }
        [Authorize("Authorization")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
              //  var Group =  DBcontext.Customer.Include(x => x.SubscrubtionInfo).FirstOrDefault(x => x.CustomerID == id);
                var Group = DBcontext.Customer.FirstOrDefault(x => x.CustomerID == id);


                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneCustomerDTO();
                  //  GroupViewModel.SupscriptionList = Factory.SelectList.SubscriptionSelectList(DBcontext).ToList();
                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        public IActionResult EditPost(CustomerDTO obj)
        {


            if (ModelState.IsValid)
            {
                var IsSucess = _CustomerRepo.EditCustomer( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Customer Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Customer Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Customer Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("CustomerList", "Customer" );
            }
            else
            {
               
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.CustomerID });
            }
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _CustomerRepo.GetCustomer(  id);
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
        public IActionResult deletePOST(int id)
        {
         
            {

                var IsDeleted = _CustomerRepo.DeleteCustomer(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Customer ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Customer , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Customer , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Customer , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("CustomerList", "Customer" );

            }


        }

    }
}
