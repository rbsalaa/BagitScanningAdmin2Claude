using IdentityCore.Factory.RepositoryBannersDTO;
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
  
    public class BannersController : Controller
    {
        private readonly ILogger<BannersController> _logger;
        private readonly SelfKOrderWebContext DBcontext;
        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        IBanner _BannersRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        IKiosik _KiosikRepo;
        private readonly IAccessFilterService _accessFilter;

        public BannersController(  IWebHostEnvironment _webHostEnvironment, IAccessFilterService accessFilter, SignInManager<SelfKOrderWebUser> signInManager, ILogger<BannersController> logger, SelfKOrderWebContext _DBcontext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _BannersRepo = new BannersRepo(_DBcontext, accessFilter);
            _KiosikRepo=new KiosikRepo(_DBcontext, accessFilter);
            DBcontext = _DBcontext;
            webHostEnvironment = _webHostEnvironment;
            _accessFilter= accessFilter;
        }
        [Authorize("Authorization")]
        public async Task<IActionResult> BannersList()
        {

            var BannersList = await _BannersRepo.GetBannersList( User);
            return View(BannersList);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Banner, "id")]
        public async Task<IActionResult> Detail(int id)
        {

            var groupDTO = await _BannersRepo.GetBanners(  id);

            return View(groupDTO);
        }
        [Authorize("Authorization")]
        public IActionResult create()
        {

            var groupDTO = new BannersDTO();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();

            return View(groupDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("create")]
        [TenantScope(TenantEntity.Banner, "obj.BannerID", scopeOnly: true)]
        public async Task<IActionResult> createPost(BannersDTO obj)
        {

            if (ModelState.IsValid)
            {




				

				if (!UploadHelper.IsAllowedMedia(obj.FileAds))
				{
				    TempData["error"] = "Please upload an image or video file (jpg, png, gif, webp, mp4, webm, mov).";
				    return RedirectToAction("create", "Banners");
				}
				string ImagePath = UploadHelper.UniqueName(obj.FileAds);
				obj.BannerURL = "\\Banners" + "\\" + ImagePath;
				await UploadHelper.SaveAsync(obj.FileAds, webHostEnvironment.WebRootPath, "Banners", ImagePath);
                
				//var path3 = "\\Ads";
    //            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, webHostEnvironment.ContentRootPath, path3);
    //            string ImagePath = Guid.NewGuid().ToString() + "_" + obj.FileAds.FileName;
    //            obj.AdsURL = Path.Combine(uploadsFolder, ImagePath);
    //            using (FileStream fs = new FileStream(obj.AdsURL, FileMode.Create))
    //            {
    //                await obj.FileAds.CopyToAsync(fs);
    //            }

                var IsSucess = _BannersRepo.SaveBanners(  obj);
                if (IsSucess == (int)SearchStauts.Error)
                {

                    TempData["error"] = "Banners Record NotAdded An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.Dublicated)
                {

                    TempData["error"] = "ID Is Taken ,please Choose Anther One";

                }
                else
                {
                    TempData["success"] = "Banners Record Added Successfully";

                }


                return RedirectToAction("BannersList", "Banners" );

            }
            else
            {
                TempData["error"] = "Banners Record NotAdded An Error Accour";

               
                  return RedirectToAction("create", "Banners");
            }
        }
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Banner, "id")]
        public IActionResult edit(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var Group =  DBcontext.Banner.FirstOrDefault(x => x.BannerID == id);
                if (Group == null)
                {
                    return NotFound();
                }
                else
                {
                    var GroupViewModel = Group.ToOneBannersDTO();

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
        [TenantScope(TenantEntity.Banner, "obj.BannerID")]
        public async Task<IActionResult> EditPost(BannersDTO obj)
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
                        return RedirectToAction("edit", "Banners", new { id = obj.BannerID });
                    }
                    var path3 = "Banner";
                    string imagePath = UploadHelper.UniqueName(obj.FileAds);
                    obj.BannerURL = Path.Combine(path3, imagePath); // Relative path to store in the DB
                    await UploadHelper.SaveAsync(obj.FileAds, webHostEnvironment.WebRootPath, path3, imagePath);
                }

                var IsSucess = _BannersRepo.EditBanners( obj);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Banners Record  failed to edit NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Banners Record  failed to edit An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Banners Record Edited Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("BannersList", "Banners" );
            }
            else
            {
                TempData["error"] = "upload your image";

                return RedirectToAction("edit", "Banners", new { id = obj.BannerID });
            }
        }

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Banner, "id")]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var group = await _BannersRepo.GetBanners(  id);
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
        [TenantScope(TenantEntity.Banner, "id")]
        public IActionResult deletePOST(int id)
        {
            var Group =  DBcontext.Advertisment.Find(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {

                var IsDeleted = _BannersRepo.DeleteBanners(  id);
                if (IsDeleted == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Fail to delete Banners ,An Error Accour";
                }
                else if (IsDeleted == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Fail to delete Banners , Not Found";
                }
                else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                {
                    TempData["error"] = "Can not Delete The Banners , There is a Related Active Region ";
                }
                else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
                {
                    TempData["error"] = "Cant Delete The Banners , There Is Related Records";
                }
                else
                {

                    TempData["success"] = "Record Deleted Successfully";
                }
                return RedirectToAction("BannersList", "Banners" );

            }


        }



      
        public async Task<IActionResult> AssignBannerKiosik()
        {

            var userList = await _KiosikRepo.GetKiosikList(User);

            return View(userList);


        }


        [TenantScope(TenantEntity.Kiosik, "id")]
        public async Task<IActionResult> KiosikCurrentBannerList(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.KiosikID = id;
                Group.BannersList = await _BannersRepo.GetCurrentKiosikBannerList(id,User);

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
        public async Task<IActionResult> AssignBannerKiosikList(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                CarrierDTO Group = new CarrierDTO();
                Group.KiosikID = id;
                Group.BannersList = await _BannersRepo.GetKiosikBannerList(id, User);

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
        [TenantScope(TenantEntity.Banner, "userid")]
        public async Task<IActionResult> unAssignBannerKiosikListPost(int userid, int roleid)
        {


            {
                var IsSucess = _BannersRepo.unAssignBannerToKiosik(roleid, userid);
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

                return RedirectToAction("KiosikCurrentBannerList", "Banners", new { id = roleid });
            }


        }
        
       
        [TenantScope(TenantEntity.Kiosik, "roleid")]
        [TenantScope(TenantEntity.Banner, "userid")]
        public async Task<IActionResult> AssignBannerKiosikListPost(int userid, int roleid)
        {


            {
                var IsSucess = _BannersRepo.AssignBannerToKiosik(userid, roleid);
                if (IsSucess == (int)SearchStauts.NotFound)
                {
                    TempData["error"] = "Banner Record  failed to Assign NOTFOUND";

                }
                else if (IsSucess == (int)SearchStauts.Error)
                {
                    TempData["error"] = "Banner Record  failed to Assign An Error Accour";

                }
                else if (IsSucess == (int)SearchStauts.NoChanges)
                {
                    TempData["error"] = "No Changes Added In Object";

                }
                else
                {
                    TempData["success"] = "Banner Record Assign Successfully";
                }
                // ViewBag.UserRegion = UserFactory.RegionList(_db);

                return RedirectToAction("AssignBannerKiosikList", "Banners", new { id = roleid });
            }


        }



        [HttpGet]
        public IActionResult GetTargetsByType(string type)
        {
            if (string.Equals(type, "Item", StringComparison.OrdinalIgnoreCase))
            {
                var items = Factory.SelectList.ItemsSelectList(DBcontext,User,_accessFilter);
                return Json(items);
            }
            else if (string.Equals(type, "Category", StringComparison.OrdinalIgnoreCase))
            {
                var categories = Factory.SelectList.CategorySelectList(DBcontext, User, _accessFilter);
                return Json(categories);
            }

            return Json(new List<SelectListItem>());
        }

    }
}
