using IdentityCore.Factory.RepositoryBrandDTO;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using SelfKOrderWeb.Authoraization;

namespace SelfKOrderWeb.Controllers
{
  
    public class BrandController : Controller
    {
      
        private readonly ILogger<BrandController> _logger;
		private readonly SelfKOrderWebContext DBcontext;
		private readonly SignInManager<SelfKOrderWebUser> _signInManager;
		IBrandRepo _BrandRepo;
		private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAccessFilterService _accessFilter;
        //  RlsContextService _rlsContextService;

        public BrandController( SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService accessFilter, ILogger<BrandController> logger, SelfKOrderWebContext _DBcontext, IWebHostEnvironment _webHostEnvironment)
		{
			_signInManager = signInManager;
			_logger = logger;
			_BrandRepo = new BrandRepo(_DBcontext, accessFilter);
			DBcontext = _DBcontext;
			webHostEnvironment = _webHostEnvironment;
			_accessFilter = accessFilter;
           // _rlsContextService = rlsContextService;
        }
        //[Authorize(Policy = "BrandList")]
        [Authorize("Authorization")]
        public async Task<IActionResult> BrandList()
		{
          //  await _rlsContextService.SetRlsContextAsync();
            var BrandList = await _BrandRepo.GetBrandList(User);
			return View(BrandList);

		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Brand, "id")]
        public async Task<IActionResult> Detail(int id)
		{

			var groupDTO = await _BrandRepo.GetBrand(id);

			return View(groupDTO);
		}
        [Authorize("Authorization")]
        public IActionResult create()
		{

			var groupDTO = new BrandDTO();
			groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
			return View(groupDTO);
		}
		[HttpPost]
		public async Task<List<SelectListItem>> GetPreferenceValues(int id)
		{
			var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id ,User, _accessFilter).ToList();
			return PreferenceValues;
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize("create")]
		[TenantScope(TenantEntity.Brand, "obj.BrandID", scopeOnly: true)]
		public async Task<IActionResult> createPost(BrandDTO obj)
		{
			if (obj.File == null)
			{
                TempData["error"] = "Brand Record NotAdded An Error Accour";
                obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                
                return View("Create",obj);
            }

			if (!UploadHelper.IsAllowedImage(obj.File))
			{
				TempData["error"] = "Please upload an image file (jpg, png, gif, webp, svg) up to 10 MB.";
				obj.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
				return View("Create", obj);
			}
			string ImagePath = UploadHelper.UniqueName(obj.File);
			obj.BrandPic = "\\Brand" + "\\\\" + ImagePath;
			await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Brand", ImagePath);


			var IsSucess = _BrandRepo.SaveBrand(obj);
			if (IsSucess == (int)SearchStauts.Error)
			{

				TempData["error"] = "Brand Record NotAdded An Error Accour";

			}
			else if (IsSucess == (int)SearchStauts.Dublicated)
			{

				TempData["error"] = "ID Is Taken ,please Choose Anther One";

			}
			else
			{
				TempData["success"] = "Brand Record Added Successfully";

			}


			return RedirectToAction("BrandList", "Brand");

			//}
			//else
			//{
			//    TempData["error"] = "Brand Record NotAdded An Error Accour";

			//    obj.CompList = SelectList.CompanySelectList(DBcontext).ToList();
			//    return RedirectToAction("create", "Brand");
			//}
		}
       // [Authorize("Authorization")]
        [TenantScope(TenantEntity.Brand, "id")]
        public IActionResult edit(int id)
		{

			if (id == 0)
			{
				return NotFound();
			}
			else
			{
				var Group = DBcontext.Brand.Include(x => x.Companys).FirstOrDefault(x => x.BrandID == id);
				if (Group == null)
				{
					return NotFound();
				}
				else
				{
					var GroupViewModel = Group.ToOneBrandDTO();
					GroupViewModel.CompList = SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();

					return View(GroupViewModel);

				}

			}

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[TenantScope(TenantEntity.Brand, "obj.BrandID")]
		public async Task<IActionResult> EditPost(BrandDTO obj)
		{


			if (obj.File != null)
			{

				if (!UploadHelper.IsAllowedImage(obj.File))
				{
					TempData["error"] = "Please upload an image file (jpg, png, gif, webp, svg) up to 10 MB.";
					return RedirectToAction("edit", new { id = obj.BrandID });
				}
				string ImagePath = UploadHelper.UniqueName(obj.File);
				obj.BrandPic = "\\Brand" + "\\\\" + ImagePath;
				await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Brand", ImagePath);
			}


			var IsSucess = _BrandRepo.EditBrand(obj);
			if (IsSucess == (int)SearchStauts.NotFound)
			{
				TempData["error"] = "Brand Record  failed to edit NOTFOUND";

			}
			else if (IsSucess == (int)SearchStauts.Error)
			{
				TempData["error"] = "Brand Record  failed to edit An Error Accour";

			}
			else if (IsSucess == (int)SearchStauts.NoChanges)
			{
				TempData["error"] = "No Changes Added In Object";

			}
			else
			{
				TempData["success"] = "Brand Record Edited Successfully";
			}
			// ViewBag.UserRegion = UserFactory.RegionList(_db);

			return RedirectToAction("BrandList", "Brand");

		}
        [Authorize("Authorization")]

        [TenantScope(TenantEntity.Brand, "id")]
        public async Task<IActionResult> delete(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}
			else
			{
				var group = await _BrandRepo.GetBrand(id);
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
		[TenantScope(TenantEntity.Brand, "id")]
		public IActionResult deletePOST(int id)
		{
			
			{

				var IsDeleted = _BrandRepo.DeleteBrand(id);
				if (IsDeleted == (int)SearchStauts.Error)
				{
					TempData["error"] = "Fail to delete Brand ,An Error Accour";
				}
				else if (IsDeleted == (int)SearchStauts.NotFound)
				{
					TempData["error"] = "Fail to delete Brand , Not Found";
				}
				else if (IsDeleted == (int)SearchStauts.IlligalOperation)
				{
					TempData["error"] = "Can not Delete The Brand , There is a Related Active Region ";
				}
				else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
				{
					TempData["error"] = "Cant Delete The Brand , There Is Related Records";
				}
				else
				{

					TempData["success"] = "Record Deleted Successfully";
				}
				return RedirectToAction("BrandList", "Brand");

			}


		}

	}
}
