using IdentityCore.Factory.RepositoryAdvertismentsDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using static SelfKOrderWeb.Factory.Enums;
using System.Diagnostics;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Factory;
using SelectList = SelfKOrderWeb.Factory.SelectList;
using Microsoft.AspNetCore.Authorization;
using IdentityCore.Factory.RepositoryBrandDTO;
using SelfKOrderWeb.Authoraization;

namespace SelfKOrderWeb.Controllers
{
   
    public class AdvertismentsController : Controller
    {
        private readonly ILogger<AdvertismentsController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IAdvertismentsRepo _AdvertismentsRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        IKiosik _KiosikRepo;
        private readonly IAccessFilterService _accessFilter;

        public AdvertismentsController(  IWebHostEnvironment _webHostEnvironment, IAccessFilterService accessFilter, SignInManager<SelfKOrderWebUser> signInManager, ILogger<AdvertismentsController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _AdvertismentsRepo = new AdvertismentsRepo(_DBcontext, accessFilter);
            _KiosikRepo=new KiosikRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            webHostEnvironment = _webHostEnvironment;
            _accessFilter= accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> AdvertismentsList()
        {

            var AdvertismentsList = await _AdvertismentsRepo.GetAdvertismentsList( User);
            return View(AdvertismentsList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Advertisement, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _AdvertismentsRepo.GetAdvertisments(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new AdvertismentsDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Advertisement, "obj.AdsID", scopeOnly: true)]
        public async Task<IActionResult> createPost(AdvertismentsDTO obj)
        {

            if (ModelState.IsValid)
            {




				

				if (!UploadHelper.IsAllowedMedia(obj.FileAds))
				{
				    TempData["error"] = "Please upload an image or video file (jpg, png, gif, webp, mp4, webm, mov).";
				    return RedirectToAction("create", "Advertisments");
				}
				string ImagePath = UploadHelper.UniqueName(obj.FileAds);
				obj.AdsURL = "\\Ads" + "\\" + ImagePath;
				await UploadHelper.SaveAsync(obj.FileAds, webHostEnvironment.WebRootPath, "Ads", ImagePath);
                
				//var path3 = "\\Ads";
    //            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, webHostEnvironment.ContentRootPath, path3);
    //            string ImagePath = Guid.NewGuid().ToString() + "_" + obj.FileAds.FileName;
    //            obj.AdsURL = Path.Combine(uploadsFolder, ImagePath);
    //            using (FileStream fs = new FileStream(obj.AdsURL, FileMode.Create))
    //            {
    //                await obj.FileAds.CopyToAsync(fs);
    //            }

                var IsSucess = _AdvertismentsRepo.SaveAdvertisments(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Advertisments Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Advertisments Record Added Successfully";

                }


                return RedirectToAction("AdvertismentsList", "Advertisments" );

            }
            else
            {
                TempData["error"] = "Advertisments Record NotAdded An Error Accour";

               
                  return RedirectToAction("create", "Advertisments");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Advertisement, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group =  DBcontext.Advertisment.FirstOrDefault(x => x.AdsID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneAdvertismentsDTO();

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
        [TenantScope(TenantEntity.Advertisement, "obj.AdsID")]
        public async Task<IActionResult> EditPost(AdvertismentsDTO obj)
        {


            // Uploading a new file is optional when editing: without one the current file is kept.
            ModelState.Remove(nameof(obj.FileAds));

            if (ModelState.IsValid)
            {
                if (obj.FileAds != null)
                {
                    if (!UploadHelper.IsAllowedMedia(obj.FileAds))
                    {
                        TempData["error"] = "Please upload an image or video file (jpg, png, gif, webp, mp4, webm, mov).";
                        return RedirectToAction("edit", "Advertisments", new { id = obj.AdsID });
                    }
                    var path3 = "Ads";
                    string imagePath = UploadHelper.UniqueName(obj.FileAds);
                    obj.AdsURL = Path.Combine(path3, imagePath); // Relative path to store in the DB
                    await UploadHelper.SaveAsync(obj.FileAds, webHostEnvironment.WebRootPath, path3, imagePath);
                }

                var IsSucess = _AdvertismentsRepo.EditAdvertisments( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Advertisments Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Advertisments Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Advertisments Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("AdvertismentsList", "Advertisments" );
            }
            else
            {
               
                return RedirectToAction("edit", "Advertisments", new { id = obj.AdsID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Advertisement, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _AdvertismentsRepo.GetAdvertisments(  id);
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
        [TenantScope(TenantEntity.Advertisement, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group =  DBcontext.Advertisment.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _AdvertismentsRepo.DeleteAdvertisments(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Advertisments ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Advertisments , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Advertisments , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Advertisments , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("AdvertismentsList", "Advertisments" );

            }


        }



      
        public async Task<IActionResult> AssignAdsKiosik()
        {

            var userList = await _KiosikRepo.GetKiosikList(User);

            return View(userList);


        }


        [TenantScope(TenantEntity.Kiosik, "id")]
        public async Task<IActionResult> KiosikCurrentAdsList(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.KiosikID = id;
                Group.AdsList = await _AdvertismentsRepo.GetCurrentKiosikAdsList(id,User);

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
        [TenantScope(TenantEntity.Kiosik, "id")]
        public async Task<IActionResult> AssignAdsKiosikList(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.KiosikID = id;
                Group.AdsList = await _AdvertismentsRepo.GetKiosikAdsList(id, User);

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
        [TenantScope(TenantEntity.Kiosik, "roleid")]
        [TenantScope(TenantEntity.Advertisement, "userid")]
        public async Task<IActionResult> unAssignAdsKiosikListPost(int userid, int roleid)
        {


            {
                // The repository takes (KiosikID, AdsID); the arguments were passed the other way round,
                // so un-assigning an ad never found the row.
                var IsSucess = _AdvertismentsRepo.unAssignAdsToKiosik(roleid, userid);
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

                return RedirectToAction("KiosikCurrentAdsList", "Advertisments", new { id = roleid });
            }


        }
        
       
        [TenantScope(TenantEntity.Kiosik, "roleid")]
        [TenantScope(TenantEntity.Advertisement, "userid")]
        public async Task<IActionResult> AssignAdsKiosikListPost(int userid, int roleid)
        {


            {
                var IsSucess = _AdvertismentsRepo.AssignAdsToKiosik(userid, roleid);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Ads Record  failed to Assign NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Ads Record  failed to Assign An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Ads Record Assign Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("AssignAdsKiosikList", "Advertisments", new { id = roleid });
            }


        }

    }
}
