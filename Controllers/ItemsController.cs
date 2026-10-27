using IdentityCore.Factory.RepositoryBrandDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.ComponentModel;
using System.Diagnostics;
using static SelfKOrderWeb.Factory.Enums;
using Items = SelfKOrderWeb.Models.Items;
using LicenseContext = OfficeOpenXml.LicenseContext;
using SelectList = SelfKOrderWeb.Factory.SelectList;

namespace SelfKOrderWeb.Controllers
{
   
    public class ItemsController : Controller
	{
		private readonly ILogger<ItemsController> _logger;
		private readonly SelfKOrderWebContext DBcontext;
		private readonly SignInManager<SelfKOrderWebUser> _signInManager;
		IItemRepo _ItemRepo;
		private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAccessFilterService _accessFilter;
        public ItemsController(SignInManager<SelfKOrderWebUser> signInManager, IAccessFilterService AccessFilter, ILogger<ItemsController> logger, SelfKOrderWebContext _DBcontext, IWebHostEnvironment _webHostEnvironment)
		{
			_signInManager = signInManager;
			_logger = logger;
			_ItemRepo = new ItemRepo(_DBcontext, AccessFilter);
			DBcontext = _DBcontext;
			webHostEnvironment = _webHostEnvironment;
			_accessFilter = AccessFilter;
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
        public async Task<IActionResult> ItemList()
		{

			var ItemList = await _ItemRepo.GetItems(User);
			return View(ItemList);

		}
        [Authorize("Authorization")]
        public async Task<IActionResult> UploadExcel()
        {

          //  var ItemList = await _ItemRepo.GetItems(User);
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0 ||
                !string.Equals(Path.GetExtension(excelFile.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["error"] = "Please upload a valid Excel (.xlsx) file.";
                return RedirectToAction("UploadExcel");
            }

            int? CompIDc = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "CompID")?.Value, out var compId) ? compId : null;
            int? BrandIDc = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "BrandID")?.Value, out var brandId) ? brandId : null;
            int? FranchIDc = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "FranchID")?.Value, out var franchId) ? franchId : null;
            int? BranchIDc = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "BranchID")?.Value, out var branchId) ? branchId : null;

            var items = new List<Items>();
            var skippedRows = 0;

            try
            {
                using var stream = new MemoryStream();
                await excelFile.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);

                ExcelWorksheet? worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
                if (worksheet?.Dimension == null)
                {
                    TempData["error"] = "The Excel file is empty.";
                    return RedirectToAction("UploadExcel");
                }

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Columns are matched by the header text in row 1 when present (e.g. "ItemName", "Calories"),
                // otherwise the original fixed positions are used.
                var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int c = 1; c <= colCount; c++)
                {
                    var h = new string(worksheet.Cells[1, c].Text.Where(char.IsLetterOrDigit).ToArray());
                    if (h.Length > 0 && !headers.ContainsKey(h)) headers[h] = c;
                }
                int Col(string header, int fallback) => headers.TryGetValue(header, out var c) ? c : fallback;

                int cName = Col("ItemName", 1), cNameAr = Col("ItemNameAR", 2), cPrice = Col("ItemPrice", 4),
                    cGroup = Col("GroupID", 5), cPic = Col("ItemPic", 6), cDesc = Col("ItemDescription", 7),
                    cDescAr = Col("ItemDescriptionAR", 8), cCalories = Col("Calories", 8), cSteps = Col("Steps", 9),
                    cAlergies = Col("Alergies", 10), cAloha = Col("AlohaProductID", 11);

                for (int row = 2; row <= rowCount; row++) // row 1 holds the headers
                {
                    var name = worksheet.Cells[row, cName].Text?.Trim();
                    if (string.IsNullOrEmpty(name))
                    {
                        skippedRows++; // blank/formatted-only rows would otherwise become empty items
                        continue;
                    }

                    items.Add(new Items()
                    {
                        ItemName = name,
                        ItemName_AR = worksheet.Cells[row, cNameAr].Text,
                        ItemPrice = decimal.TryParse(worksheet.Cells[row, cPrice].Text, out var price) ? price : 0,
                        GroupID = int.TryParse(worksheet.Cells[row, cGroup].Text, out int GroupID) ? GroupID : null,
                        IsActive = true,
                        ItemPic = worksheet.Cells[row, cPic].Text,
                        ItemDescription = worksheet.Cells[row, cDesc].Text,
                        ItemDescription_AR = worksheet.Cells[row, cDescAr].Text,
                        IsGroupItem = false,
                        BranchID = BranchIDc,
                        BrandID = BrandIDc,
                        CompID = CompIDc,
                        FranchID = FranchIDc,
                        Calories = worksheet.Cells[row, cCalories].Text,
                        Steps = worksheet.Cells[row, cSteps].Text,
                        Alergies = worksheet.Cells[row, cAlergies].Text,
                        AlohaProductID = worksheet.Cells[row, cAloha].Text,
                        LastModified = DateTime.Now
                    });
                }

                if (items.Count == 0)
                {
                    TempData["error"] = "No items were found in the file.";
                    return RedirectToAction("UploadExcel");
                }

                DBcontext.Items.AddRange(items);
                await DBcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel item import failed");
                TempData["error"] = "The file could not be imported. Please check the format and try again.";
                return RedirectToAction("UploadExcel");
            }

            TempData["success"] = skippedRows > 0
                ? $"{items.Count} items imported successfully ({skippedRows} empty rows skipped)."
                : $"{items.Count} items imported successfully.";
            return RedirectToAction("ItemList");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Item, "id")]
        public async Task<IActionResult> Detail(int id)
		{

			var groupDTO = await _ItemRepo.GetItem(id);

			return View(groupDTO);
		}
        [Authorize("Authorization")]
        public IActionResult create()
		{

			var groupDTO = new ItemDTO();
			groupDTO.GrpList = Factory.SelectList.GroupSelectList(DBcontext).ToList();
            groupDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
            groupDTO.BrandList = new List<SelectListItem>();
            groupDTO.BranchList = new List<SelectListItem>();
            return View(groupDTO);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize("create")]
		[TenantScope(TenantEntity.Item, "obj.ItemID", scopeOnly: true)]
		public async Task<IActionResult> createPost(ItemDTO obj)
		{

			if (true || ModelState.IsValid)
			{
				if (obj.File != null)
				{
					if (!UploadHelper.IsAllowedImage(obj.File))
					{
						TempData["error"] = "Please upload an image file (jpg, png, gif, webp, svg) up to 10 MB.";
						return RedirectToAction("create", "Items");
					}
					string ImagePath = UploadHelper.UniqueName(obj.File);
					obj.ItemPic = "\\item" + "\\\\" + ImagePath;
					await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Item", ImagePath);
				}

				var IsSucess = _ItemRepo.SaveItem(obj);
				if (IsSucess == (int)SearchStauts.Error)
				{

					TempData["error"] = "Item Record NotAdded An Error Accour";

				}
				else if (IsSucess == (int)SearchStauts.Dublicated)
				{

					TempData["error"] = "ID Is Taken ,please Choose Anther One";

				}
				else
				{
					TempData["success"] = "Item Record Added Successfully";

				}


				return RedirectToAction("itemlist", "Items");

			}
			else
			{
				TempData["error"] = "Item Record NotAdded An Error Accour";

				obj.GrpList = SelectList.GroupSelectList(DBcontext).ToList();
				return RedirectToAction("create", "Items");
			}
		}
        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Item, "id")]
        public IActionResult edit(int id)
		{

			if (id == 0)
			{
				return NotFound();
			}
			else
			{
				var Group = DBcontext.Items.Include(x => x.Groups).FirstOrDefault(x => x.ItemID == id);
				if (Group == null)
				{
					return NotFound();
				}
				else
				{


					var GroupViewModel = Group.ToOneItemDTO();
                    GroupViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                    GroupViewModel.GrpList = SelectList.GroupSelectList(DBcontext).ToList();
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
		[TenantScope(TenantEntity.Item, "obj.ItemID")]
		public async Task<IActionResult> editPost(ItemDTO obj)
		{
			if (true || ModelState.IsValid)
			{
				if (obj.File != null)
				{
					if (!UploadHelper.IsAllowedImage(obj.File))
					{
						TempData["error"] = "Please upload an image file (jpg, png, gif, webp, svg) up to 10 MB.";
						return RedirectToAction("edit", "Items", new { id = obj.ItemID });
					}
					string ImagePath = UploadHelper.UniqueName(obj.File);
					obj.ItemPic = "\\item" + "\\" + ImagePath;
					await UploadHelper.SaveAsync(obj.File, webHostEnvironment.WebRootPath, "Item", ImagePath);
				}


				var IsSucess = _ItemRepo.EditItem(obj);
				if (IsSucess == (int)SearchStauts.Error)
				{

					TempData["error"] = "Item Record NotAdded An Error Accour";

				}
				else if (IsSucess == (int)SearchStauts.Dublicated)
				{

					TempData["error"] = "ID Is Taken ,please Choose Anther One";

				}
				else
				{
					TempData["success"] = "Item Record Added Successfully";

				}


				return RedirectToAction("ItemList", "Items");

			}
			else
			{
				TempData["error"] = "Item Record NotAdded An Error Accour";

				obj.GrpList = SelectList.GroupSelectList(DBcontext).ToList();
				return RedirectToAction("edit", "Items");
			}
		}

		public IActionResult Excel()
		{

			return View();
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> excelPost(ItemDTO obj)
		{
			if (obj.File == null || obj.File.Length == 0 ||
				!string.Equals(Path.GetExtension(obj.File.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
			{
				TempData["error"] = "Please upload a valid Excel (.xlsx) file.";
				return RedirectToAction("Excel", "Items");
			}

			int added = 0, failed = 0;
			try
			{
				// Read the workbook from memory: the uploaded sheet is no longer copied into the public wwwroot\excel folder.
				using var stream = new MemoryStream();
				await obj.File.CopyToAsync(stream);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using var package = new ExcelPackage(stream);

				var worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : null;
				int rows = worksheet?.Dimension?.Rows ?? 0;

				for (int row = 2; row <= rows; row++)
				{
					var name = worksheet!.Cells[$"A{row}"].Text?.Trim();
					if (string.IsNullOrEmpty(name))
						continue;

					var item = new ItemDTO
					{
						ItemName = name,
						ItemPrice = decimal.TryParse(worksheet.Cells[$"B{row}"].Text, out var price) ? price : 0,
						IsGroupItem = worksheet.Cells[$"C{row}"].Text != "0",
						IsActive = worksheet.Cells[$"F{row}"].Text != "0"
					};
					if (item.IsGroupItem && int.TryParse(worksheet.Cells[$"D{row}"].Text, out var groupId))
						item.GroupID = groupId;

					if (_ItemRepo.SaveItem(item) > 0) added++; else failed++;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Excel import (excelPost) failed");
				TempData["error"] = "The file could not be imported. Please check the format and try again.";
				return RedirectToAction("ItemList", "Items");
			}

			if (added == 0 && failed == 0)
				TempData["error"] = "No items were found in the file.";
			else if (failed > 0)
				TempData["error"] = $"{added} items added, {failed} rows could not be saved.";
			else
				TempData["success"] = $"{added} items added successfully.";

			return RedirectToAction("ItemList", "Items");
		}




        [Authorize("Authorization")]
        [TenantScope(TenantEntity.Item, "id")]
        public async Task<IActionResult> delete(int id)
		{
			if (id == 0)
			{
				return NotFound();
			}
			else
			{
				var group = await _ItemRepo.GetItem(id);
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
		[TenantScope(TenantEntity.Item, "id")]
		public IActionResult deletePOST(int id)
		{
			var Group = DBcontext.Items.Find(id);
			if (Group == null)
			{
				return NotFound();
			}
			else
			{

				var IsDeleted = _ItemRepo.DeleteItem(id);
				if (IsDeleted == (int)SearchStauts.Error)
				{
					TempData["error"] = "Fail to delete Item ,An Error Accour";
				}
				else if (IsDeleted == (int)SearchStauts.NotFound)
				{
					TempData["error"] = "Fail to delete Item , Not Found";
				}
				else if (IsDeleted == (int)SearchStauts.IlligalOperation)
				{
					TempData["error"] = "Can not Delete The Brand , There is a Related Active Region ";
				}
				else if (IsDeleted == (int)SearchStauts.NonDeletedUser)
				{
					TempData["error"] = "Cant Delete The Item , There Is Related Records";
				}
				else
				{

					TempData["success"] = "Record Deleted Successfully";
				}
				return RedirectToAction("ItemList", "Items");

			}


		}

	}
}
