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
   
  
        public class InventoryController : Controller
        {
            private readonly SelfKOrderWebContext DBcontext;
            private readonly IItIInventoryemRepo _InventoryRepo;
            private readonly IAccessFilterService _accessFilter;

            public InventoryController(SelfKOrderWebContext _DBcontext, IItIInventoryemRepo InventoryRepo, IAccessFilterService accessFilter)
            {
                DBcontext = _DBcontext;
                _InventoryRepo = InventoryRepo;
                _accessFilter = accessFilter;
            }

            // Cascading dropdowns (same pattern as Items controller)
            [HttpPost]
            public async Task<List<SelectListItem>> GetPreferenceValues(int id2)
            {
                var PreferenceValues = Factory.SelectList.BrandSelectList(DBcontext, id2, User, _accessFilter).ToList();
                return PreferenceValues;
            }

            [HttpPost]
            public async Task<List<SelectListItem>> GetPreferenceValues3(int id3, int id2)
            {
                var PreferenceValues = Factory.SelectList.BranchSelectList(DBcontext, id3, id2, User, _accessFilter).ToList();
                return PreferenceValues;
            }

            [Authorize("Authorization")]
            public async Task<IActionResult> InventoryList()
            {
                var InventoryList = await _InventoryRepo.GetInventory(User);
                return View(InventoryList);
            }

          //  [Authorize("Authorization")]
            [TenantScope(TenantEntity.Inventory, "id")]
            public async Task<IActionResult> Detail(int id)
            {
                var invDTO = await _InventoryRepo.GetInventory(id);
                if (invDTO == null)
                {
                    return NotFound();
                }
                return View(invDTO);
            }

           // [Authorize("Authorization")]
            public IActionResult create()
            {
                var invDTO = new InventoryDTO();
                invDTO.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();
                invDTO.BrandList = new List<SelectListItem>();
                invDTO.BranchList = new List<SelectListItem>();
                return View(invDTO);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [TenantScope(TenantEntity.Inventory, "obj.InvID", scopeOnly: true)]
            public async Task<IActionResult> createPost(InventoryDTO obj)
            {
                if (true || ModelState.IsValid)
                {
                    var IsSucess = _InventoryRepo.SaveInventory(obj);
                    if (IsSucess == (int)SearchStauts.Error)
                    {
                        TempData["error"] = "Inventory Record NotAdded An Error Accour";
                    }
                    else if (IsSucess == (int)SearchStauts.Dublicated)
                    {
                        TempData["error"] = "ID Is Taken ,please Choose Anther One";
                    }
                    else
                    {
                        TempData["success"] = "Inventory Record Added Successfully";
                    }

                    return RedirectToAction("InventoryList", "Inventory");
                }
                else
                {
                    TempData["error"] = "Inventory Record NotAdded An Error Accour";
                    return RedirectToAction("create", "Inventory");
                }
            }

         //   [Authorize("Authorization")]
            [TenantScope(TenantEntity.Inventory, "id")]
            public IActionResult edit(int id)
            {
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var DBObj = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == id);
                    if (DBObj == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        var InvViewModel = new InventoryDTO
                        {
                            InvID = DBObj.InvID,
                            InvName = DBObj.InvName,
                            InvName_AR = DBObj.InvName_AR,
                            InvLocation = DBObj.InvLocation,
                           // BranchID = DBObj.BranchID,
                            CompID = DBObj.CompID,
                            BrandID = DBObj.BrandID,
                            IsActive = DBObj.IsActive,
                            IsDeleted = DBObj.IsDeleted,
                            LastModified = DBObj.LastModified
                        };

                        InvViewModel.CompList = Factory.SelectList.CompanySelectList(DBcontext, User, _accessFilter).ToList();

                        if (InvViewModel.CompID == null)
                            InvViewModel.BrandList = new List<SelectListItem>();
                        else
                            InvViewModel.BrandList = Factory.SelectList.BrandSelectList(DBcontext, InvViewModel.CompID.Value, User, _accessFilter).ToList();

                        if (InvViewModel.BrandID == null || InvViewModel.CompID == null || InvViewModel.BranchID == null)
                            InvViewModel.BranchList = new List<SelectListItem>();
                        else
                            InvViewModel.BranchList = Factory.SelectList.BranchSelectList(DBcontext, InvViewModel.CompID.Value, InvViewModel.BrandID.Value, User, _accessFilter).ToList();

                        return View(InvViewModel);
                    }
                }
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [TenantScope(TenantEntity.Inventory, "obj.InvID")]
            public async Task<IActionResult> editPost(InventoryDTO obj)
            {
                if (true || ModelState.IsValid)
                {
                    var IsSucess = _InventoryRepo.EditInventory(obj);
                    if (IsSucess == (int)SearchStauts.Error)
                    {
                        TempData["error"] = "Inventory Record NotUpdated An Error Accour";
                    }
                    else if (IsSucess == (int)SearchStauts.NotFound)
                    {
                        TempData["error"] = "Inventory Record Not Found";
                    }
                    else
                    {
                        TempData["success"] = "Inventory Record Updated Successfully";
                    }

                    return RedirectToAction("InventoryList", "Inventory");
                }
                else
                {
                    TempData["error"] = "Inventory Record NotUpdated An Error Accour";
                    return RedirectToAction("edit", "Inventory");
                }
            }

          //  [Authorize("Authorization")]
            [TenantScope(TenantEntity.Inventory, "id")]
            public async Task<IActionResult> delete(int id)
            {
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var inv = await _InventoryRepo.GetInventory(id);
                    if (inv == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return View(inv);
                    }
                }
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            [TenantScope(TenantEntity.Inventory, "id")]
            public IActionResult deletePOST(int id)
            {
                var Inv = DBcontext.InventoryWarehouses.Find(id);
                if (Inv == null)
                {
                    return NotFound();
                }
                else
                {
                    var IsDeleted = _InventoryRepo.DeleteInventory(id);
                    if (IsDeleted == (int)SearchStauts.Error)
                    {
                        TempData["error"] = "Fail to delete Inventory ,An Error Accour";
                    }
                    else if (IsDeleted == (int)SearchStauts.NotFound)
                    {
                        TempData["error"] = "Fail to delete Inventory , Not Found";
                    }
                    else if (IsDeleted == (int)SearchStauts.IlligalOperation)
                    {
                        TempData["error"] = "Can not Delete The Inventory , There Are Related Item Records";
                    }
                    else
                    {
                        TempData["success"] = "Record Deleted Successfully";
                    }
                    return RedirectToAction("InventoryList", "Inventory");
                }
            }
       // [Authorize("Authorization")]
        [TenantScope(TenantEntity.Inventory, "id")]
        public async Task<IActionResult> AssignBranchToInventory(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var inv = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == id);
                if (inv == null)
                {
                    return NotFound();
                }

                InvBranchDTO Group = new InvBranchDTO();
                Group.invID = id;
                Group.invName = inv.InvName;
                Group.unAssignedBranch = await _InventoryRepo.GetUnAssignedBranches(id, User);
                Group.assignedBranch = await _InventoryRepo.GetAssignedBranches(id);

                return View(Group);
            }
        }

        [TenantScope(TenantEntity.Inventory, "InvID")]
        [TenantScope(TenantEntity.Branch, "BranchID")]
        public async Task<IActionResult> AssignBranchPost(int BranchID, int InvID)
        {
            var IsSucess = _InventoryRepo.AssignBranch(BranchID, InvID);
            if (IsSucess == (int)SearchStauts.NotFound)
            {
                TempData["error"] = "Branch Record failed to Assign NOTFOUND";
            }
            else if (IsSucess == (int)SearchStauts.Dublicated)
            {
                TempData["error"] = "Branch Already Assigned To This Inventory";
            }
            else if (IsSucess == (int)SearchStauts.Error)
            {
                TempData["error"] = "Branch Record failed to Assign An Error Accour";
            }
            else
            {
                TempData["success"] = "Branch Assigned Successfully";
            }
            return RedirectToAction("AssignBranchToInventory", "Inventory", new { id = InvID });
        }
      //  [Authorize("Authorization")]
        [TenantScope(TenantEntity.Inventory, "id")]
        public async Task<IActionResult> UnAssignBranchList(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var inv = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == id);
            if (inv == null)
            {
                return NotFound();
            }

            InvBranchDTO Group = new InvBranchDTO();
            Group.invID = id;
            Group.invName = inv.InvName;
            Group.assignedBranch = await _InventoryRepo.GetAssignedBranches(id);

            return View(Group);
        }
        [TenantScope(TenantEntity.Inventory, "InvID")]
        [TenantScope(TenantEntity.Branch, "BranchID")]
        public async Task<IActionResult> UnAssignBranchPost(int BranchID, int InvID)
        {
            var IsSucess = _InventoryRepo.unAssignBranch(BranchID, InvID);
            if (IsSucess == (int)SearchStauts.NotFound)
            {
                TempData["error"] = "Record failed to UnAssign NOTFOUND";
            }
            else if (IsSucess == (int)SearchStauts.Error)
            {
                TempData["error"] = "Failed to UnAssign An Error Accour";
            }
            else
            {
                TempData["success"] = "Branch UnAssigned Successfully";
            }
            return RedirectToAction("AssignBranchToInventory", "Inventory", new { id = InvID });
        }

        // ===================== WAREHOUSE STOCK PAGE =====================

      //  [Authorize("Authorization")]
        [TenantScope(TenantEntity.Inventory, "id")]
        public async Task<IActionResult> Stock(int id)
        {
            if (id == 0) return NotFound();

            var inv = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == id);
            if (inv == null) return NotFound();

            var model = new StockDTO();
            model.invID = id;
            model.invName = inv.InvName;
            model.stockItems = await _InventoryRepo.GetStock(id);

            var existingItemIds = model.stockItems.Select(x => x.ItemID).ToList();
            model.itemList = DBcontext.Items
                .Where(x => !existingItemIds.Contains(x.ItemID) && x.IsActive)
                .Select(x => new SelectListItem { Value = x.ItemID.ToString(), Text = x.ItemName })
                .ToList();

            model.unitList = DBcontext.Units
                .Select(x => new SelectListItem { Value = x.UnitID.ToString(), Text = x.UnitName })
                .ToList();

            // only branches assigned to this warehouse (transfer targets)
            model.branchList = DBcontext.InventoryBranch
                .Where(x => x.InvID == id)
                .Select(x => new SelectListItem { Value = x.BranchID.ToString(), Text = x.Branch.BranchName })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [TenantScope(TenantEntity.Inventory, "InvID")]
        [TenantScope(TenantEntity.Item, "ItemID")]
        public IActionResult AddItemToStockPost(int InvID, int ItemID, int? UnitID, decimal Count, decimal MinThreshold, decimal? MaxThreshold)
        {
            var IsSucess = _InventoryRepo.AddItemToStock(InvID, ItemID, UnitID, Count, MinThreshold, MaxThreshold);
            if (IsSucess == (int)SearchStauts.Dublicated)
                TempData["error"] = "Item Already Exists In This Warehouse";
            else if (IsSucess == (int)SearchStauts.Error)
                TempData["error"] = "Failed To Add Item An Error Accour";
            else
                TempData["success"] = "Item Added To Stock Successfully";

            return RedirectToAction("Stock", "Inventory", new { id = InvID });
        }

        [HttpPost]
        [TenantScope(TenantEntity.Inventory, "InvID")]
        public IActionResult UpdateStockPost(int InventoryID, int InvID, decimal Count, decimal MinThreshold, decimal? MaxThreshold)
        {
            // The stock row must belong to the warehouse that was checked above.
            if (!DBcontext.ItemInventories.Any(x => x.InventoryID == InventoryID && x.InvID == InvID))
                return NotFound();
            var IsSucess = _InventoryRepo.UpdateStock(InventoryID, Count, MinThreshold, MaxThreshold);
            if (IsSucess == (int)SearchStauts.NotFound)
                TempData["error"] = "Stock Record Not Found";
            else if (IsSucess == (int)SearchStauts.Error)
                TempData["error"] = "Failed To Update Stock An Error Accour";
            else
                TempData["success"] = "Stock Updated Successfully";

            return RedirectToAction("Stock", "Inventory", new { id = InvID });
        }

        [HttpPost]
        [TenantScope(TenantEntity.Inventory, "InvID")]
        public IActionResult ReceiveStockPost(int InventoryID, int InvID, decimal Qty)
        {
            if (!DBcontext.ItemInventories.Any(x => x.InventoryID == InventoryID && x.InvID == InvID))
                return NotFound();
            var IsSucess = _InventoryRepo.ReceiveStock(InventoryID, Qty);
            if (IsSucess == (int)SearchStauts.NotFound)
                TempData["error"] = "Stock Record Not Found";
            else if (IsSucess == (int)SearchStauts.IlligalOperation)
                TempData["error"] = "Quantity Must Be Greater Than Zero";
            else if (IsSucess == (int)SearchStauts.Error)
                TempData["error"] = "Failed To Receive Stock An Error Accour";
            else
                TempData["success"] = "Stock Received Successfully";

            return RedirectToAction("Stock", "Inventory", new { id = InvID });
        }

        [HttpPost]
        [TenantScope(TenantEntity.Inventory, "InvID")]
        [TenantScope(TenantEntity.Branch, "BranchID")]
        [TenantScope(TenantEntity.Item, "ItemID")]
        public IActionResult TransferStockPost(int InvID, int BranchID, int ItemID, decimal Qty)
        {
            var IsSucess = _InventoryRepo.TransferStock(InvID, BranchID, ItemID, Qty);
            if (IsSucess == (int)SearchStauts.NotFound)
                TempData["error"] = "Item Not Found In Warehouse";
            else if (IsSucess == (int)SearchStauts.IlligalOperation)
                TempData["error"] = "Transfer Failed : Not Enough Stock Or Branch Not Assigned To This Warehouse";
            else if (IsSucess == (int)SearchStauts.Error)
                TempData["error"] = "Failed To Transfer An Error Accour";
            else
                TempData["success"] = "Stock Transferred Successfully";

            return RedirectToAction("Stock", "Inventory", new { id = InvID });
        }

        // ===================== BRANCH STOCK PAGE =====================

       // [Authorize("Authorization")]
        [TenantScope(TenantEntity.Branch, "id")]
        public async Task<IActionResult> BranchStock(int id)
        {
            if (id == 0) return NotFound();

            var branch = DBcontext.Branch.FirstOrDefault(x => x.BranchID == id);
            if (branch == null) return NotFound();

            var model = new BranchStockDTO();
            model.branchID = id;
            model.branchName = branch.BranchName;
            model.stockItems = await _InventoryRepo.GetBranchStock(id);

            return View(model);
        }

        [HttpPost]
        [TenantScope(TenantEntity.Branch, "BranchID")]
        public IActionResult UpdateBranchStockPost(int BranchStockID, int BranchID, decimal Count, decimal MinThreshold)
        {
            if (!DBcontext.BranchStocks.Any(x => x.BranchStockID == BranchStockID && x.BranchID == BranchID))
                return NotFound();
            var IsSucess = _InventoryRepo.UpdateBranchStock(BranchStockID, Count, MinThreshold);
            if (IsSucess == (int)SearchStauts.NotFound)
                TempData["error"] = "Branch Stock Record Not Found";
            else if (IsSucess == (int)SearchStauts.Error)
                TempData["error"] = "Failed To Update An Error Accour";
            else
                TempData["success"] = "Branch Stock Updated Successfully";

            return RedirectToAction("BranchStock", "Inventory", new { id = BranchID });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
