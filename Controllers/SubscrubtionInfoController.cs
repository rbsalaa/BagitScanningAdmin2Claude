using IdentityCore.Factory.RepositorySubscrubtionInfoDTO;
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
 //   [Authorize("Authorization")]
    public class SubscrubtionInfoController : Controller
    {
        private readonly ILogger<SubscrubtionInfoController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        ISubscrubtionInfoRepo _SubscrubtionInfoRepo;
        private readonly IAccessFilterService _accessFilter;
        public SubscrubtionInfoController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<SubscrubtionInfoController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _SubscrubtionInfoRepo = new SubscrubtionInfoRepo(_DBcontext);
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> SubscrubtionInfoList()
        {
            TempData["carrierString"] = null;
            var SubscrubtionInfoList = await _SubscrubtionInfoRepo.GetSubscrubtionInfoList( );
            return View(SubscrubtionInfoList);

        }
        public async Task<IActionResult> SubscrubtionHistory()
        {
            //TempData["carrierString"] = null;
            var SubscrubtionInfoList = await _SubscrubtionInfoRepo.GetHistorySubscrubtionInfoList();
            return View(SubscrubtionInfoList);

        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> Detail(string id)
        {

            var groupDTO = await _SubscrubtionInfoRepo.GetSubscrubtionInfo(  id);

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
            var PreferenceValues = Factory.SelectList.BranchSelectList(DBcontext, id, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }


        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new SubscrubtionInfoDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList =new List<SelectListItem>();
            groupDTO.CustomerList = Factory.SelectList.CustomerSelectList(DBcontext, User, _accessFilter).ToList();
            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        public IActionResult createPost(SubscrubtionInfoDTO obj)
        {

           // if (ModelState.IsValid)
            {

                var IsSucess = _SubscrubtionInfoRepo.SaveSubscrubtionInfo(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "SubscrubtionInfo Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "SubscrubtionInfo Record Added Successfully";

                }


                return RedirectToAction("SubscrubtionInfoList", "SubscrubtionInfo" );

            }
            //else
            //{
            //    TempData["error"] = "SubscrubtionInfo Record NotAdded An Error Accour";

                
            //      return RedirectToAction("create", "SubscrubtionInfo");
            //}
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetBrand(int id)
        {
            var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]
        public async Task<List<SelectListItem>> GetBranch(int id, int id2)
        {
            var PreferenceValues = Factory.SelectList.BranchSelectList(DBcontext, id, id2, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        [HttpPost]//   data: { compID: $("#compID").val(), BrandID: $("#BrandID").val(), BranchID: $("#BranchID").val() },
        public async Task<List<SelectListItem>> GetKiosik(int id, int id2, int id3)
        {
            var PreferenceValues = Factory.SelectList.KisoikSelectList(DBcontext, id, id2, id3, User, _accessFilter).ToList();
            return PreferenceValues;
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> LicenseList(string id)
        {
            TempData["carrierString"] = null;
            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                //  var Group =  DBcontext.SubscrubtionInfo.Includef(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);

                var GroupViewModel = new SubscrubtionInfoDetailsDTO();
                GroupViewModel.SupscrubtionID = id.Split('-').First();
                GroupViewModel.BrandID = int.Parse(id.Split('-')[1]);
                GroupViewModel.CompID = int.Parse(id.Split('-').Last());
                GroupViewModel.carrierString =id;
                TempData["carrierString"] = id;
               
                GroupViewModel.LisecsesList =await _SubscrubtionInfoRepo.GetSubscrubtionDetailInfo(GroupViewModel.SupscrubtionID); 
                
                return View(GroupViewModel);



            }

        }
        public async Task<IActionResult> DLicenseList(string id)
        {
            TempData["carrierString"] = null;
            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                //  var Group =  DBcontext.SubscrubtionInfo.Includef(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);

                var GroupViewModel = new SubscrubtionInfoDetailsDTO();
                GroupViewModel.SupscrubtionID = id.Split('-').First();
                GroupViewModel.BrandID = int.Parse(id.Split('-')[1]);
                GroupViewModel.CompID = int.Parse(id.Split('-').Last());
                GroupViewModel.carrierString = id;
                TempData["carrierString"] = id;

                GroupViewModel.LisecsesList = await _SubscrubtionInfoRepo.GetHestorySubscrubtionDetailInfo(GroupViewModel.SupscrubtionID);

                return RedirectToAction("LicenseList", "SubscrubtionInfo", new
                {
                    id = GroupViewModel.carrierString

                });



            }

        }
        [Authorize("Authorization")]
        public IActionResult AddLicense()
        {
            string id = TempData["carrierString"] as string;

            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                //  var Group =  DBcontext.SubscrubtionInfo.Include(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);
              
             
                var GroupViewModel =new SubscrubtionInfoDetailsDTO();
                GroupViewModel.BranchList = new List<SelectListItem>();
                GroupViewModel.BrandList = new List<SelectListItem>();
                GroupViewModel.SupscrubtionID = id.Split('-').First();
                GroupViewModel.carrierString = id;
                GroupViewModel.BrandID = int.Parse(id.Split('-')[1]);
                GroupViewModel.CompID = int.Parse(id.Split('-').Last());
                GroupViewModel.CompList = Factory.SelectList.CompanyoneSelectList(DBcontext, GroupViewModel.CompID, User, _accessFilter).ToList();
                GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, GroupViewModel.CompID, User, _accessFilter).ToList();
                if (GroupViewModel.BrandList.Count()>0)
                    GroupViewModel.BranchList = Factory.SelectList.BranchSelectList(DBcontext, GroupViewModel.CompID, GroupViewModel.BrandID, User, _accessFilter).ToList();
                if (GroupViewModel.BranchList.Count() > 0)
                    GroupViewModel.KiosikList = Factory.SelectList.KisoikSelectList(DBcontext, GroupViewModel.CompID, GroupViewModel.BrandID, int.Parse(GroupViewModel.BranchList.FirstOrDefault().Value), User, _accessFilter).ToList();

                return View(GroupViewModel);

               

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("AddLicense")]
        public IActionResult AddLicensePost(SubscrubtionInfoDetailsDTO obj)
        {

            TempData["carrierString"] = null;


                var IsSucess = _SubscrubtionInfoRepo.AddLicensesDetail(obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "License Record  failed to Add NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "License Record  failed to Add An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "License Record Add Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("LicenseList", "SubscrubtionInfo", new
                {
                    id= obj.carrierString
                  
                });
            
           
        }

        [Authorize("Authorization")]
        public IActionResult edit(string id)
        {

            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
              //  var Group =  DBcontext.SubscrubtionInfo.Include(x => x.Company).Include(x=>x.Brands).FirstOrDefault(x => x.SupscrubtionID == id);
                var Group = DBcontext.SubscrubtionInfo.FirstOrDefault(x => x.SupscrubtionID == id);

                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneSubscrubtionInfoDTO();
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    GroupViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, GroupViewModel.CompID.Value, User, _accessFilter).ToList();
                    GroupViewModel.CustomerList = Factory.SelectList.CustomerSelectList(DBcontext, User, _accessFilter).ToList();
                    return View(GroupViewModel);

                }

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("edit")]
        public IActionResult EditPost(SubscrubtionInfoDTO obj)
        {


            if (ModelState.IsValid)
            {
                var IsSucess = _SubscrubtionInfoRepo.EditSubscrubtionInfo( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "SubscrubtionInfo Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "SubscrubtionInfo Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "SubscrubtionInfo Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("SubscrubtionInfoList", "SubscrubtionInfo" );
            }
            else
            {
               
                // There is no view named after this POST action: go back to the form instead of failing.
                TempData["error"] ??= "Please check the form values and try again.";
                return RedirectToAction("edit", new { id = obj.SupscrubtionID });
            }
        }

        [Authorize("Authorization")]
        public async Task<IActionResult> delete(string id)
        {
            if (id == null || id == "0")
            {
                return NotFound();
            }
            else
            {
                var group = await _SubscrubtionInfoRepo.GetSubscrubtionInfo(  id);
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

       
        public IActionResult DeleteLicense(string id,string subID, string carrier)
        {
            var Group = DBcontext.SubscrubtionInfoDetails.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _SubscrubtionInfoRepo.DeleteSubscrubtionDetailInfo(id,subID);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete SubscrubtionInfoDetails ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete SubscrubtionInfoDetails , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The SubscrubtionInfoDetails , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The SubscrubtionInfoDetails , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("LicenseList", "SubscrubtionInfo", new
                {
                    id = carrier
                }) ;

            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("delete")]
        public IActionResult deletePOST(string id)
        {
            var Group =  DBcontext.SubscrubtionInfo.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _SubscrubtionInfoRepo.DeleteSubscrubtionInfo(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete SubscrubtionInfo ,An Error Accour";
                }
               else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete SubscrubtionInfo , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The SubscrubtionInfo , There is a Related Active License ";
                }
               else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The SubscrubtionInfo , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("SubscrubtionInfoList", "SubscrubtionInfo" );

            }


        }

    }
}
