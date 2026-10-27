using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using static SelfKOrderWeb.Factory.Enums;

namespace IdentityCore.Factory.RepositoryBrandDTO
{

    public class InventoryRepo : IItIInventoryemRepo
    {

        SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public InventoryRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
        {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
        }



        public async Task<IEnumerable<InventoryDTO>> GetInventory(ClaimsPrincipal User)
        {
            IQueryable<InventoryDTO> filtered = _accessFilter.InventoryFilter(User, DBcontext.InventoryWarehouses);
            return await filtered.ToListAsync();
        }

        public async Task<InventoryDTO> GetInventory(int invID)
        {
            var DBObj = await DBcontext.InventoryWarehouses
                .Include(i => i.ItemInventories)
                .FirstOrDefaultAsync(x => x.InvID == invID);

            if (DBObj == null) return null;

            return new InventoryDTO
            {
                InvID = DBObj.InvID,
                InvName = DBObj.InvName,
                InvName_AR = DBObj.InvName_AR,
                InvLocation = DBObj.InvLocation,
              //  BranchID = DBObj.BranchID,
                CompID = DBObj.CompID,
                BrandID = DBObj.BrandID,
                IsActive = DBObj.IsActive,
                IsDeleted = DBObj.IsDeleted,
                LastModified = DBObj.LastModified,
                ItemInventories = DBObj.ItemInventories
            };
        }

        public int SaveInventory(InventoryDTO obj)
        {
            try
            {
                if (DBcontext.InventoryWarehouses.Any(x => x.InvID == obj.InvID)) { return (int)SearchStauts.Dublicated; }

                var DBObj = new InventoryWarehouse()
                {
                    InvID = obj.InvID,
                    InvName = obj.InvName,
                    InvName_AR = obj.InvName_AR,
                    InvLocation = obj.InvLocation,
                 //   BranchID = obj.BranchID,
                    CompID = obj.CompID,
                    BrandID = obj.BrandID,
                    IsActive = true,
                    IsDeleted = false,
                    LastModified = DateTime.Now
                };

                DBcontext.InventoryWarehouses.Add(DBObj);

                var r = DBcontext.SaveChanges();
                return r;
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int EditInventory(InventoryDTO obj)
        {
            try
            {
                var DBObj = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == obj.InvID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {
                    DBObj.InvName = obj.InvName;
                    DBObj.InvName_AR = obj.InvName_AR;
                    DBObj.InvLocation = obj.InvLocation;
                 //   DBObj.BranchID = obj.BranchID;
                    DBObj.CompID = obj.CompID;
                    DBObj.BrandID = obj.BrandID;
                    DBObj.IsActive = true;
                    DBObj.LastModified = DateTime.Now;

                    var r = DBcontext.SaveChanges();
                    return r;
                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int DeleteInventory(int id)
        {
            var DBObj = DBcontext.InventoryWarehouses.FirstOrDefault(x => x.InvID == id);

            try
            {
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }
                else
                {
                    // block delete if inventory has linked items (same pattern as Items delete check)
                    if (DBcontext.ItemInventories.Any(x => x.InvID == DBObj.InvID))
                    {
                        return (int)SearchStauts.IlligalOperation;
                    }
                    else
                    {
                        DBcontext.Remove(DBObj);
                        return DBcontext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }




        public async Task<IEnumerable<Branch>> GetUnAssignedBranches(int invID, ClaimsPrincipal User)
        {
            var inv = await DBcontext.InventoryWarehouses.FirstOrDefaultAsync(x => x.InvID == invID);
            if (inv == null) return new List<Branch>();

            var assignedIds = DBcontext.InventoryBranch
                .Where(x => x.InvID == invID)
                .Select(x => x.BranchID);

            // only branches of the same Comp/Brand as the inventory, not already assigned
            return await DBcontext.Branch
                .Where(b => b.CompID == inv.CompID
                         && b.BrandID == inv.BrandID
                         && !assignedIds.Contains(b.BranchID))
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> GetAssignedBranches(int invID)
        {
            return await DBcontext.InventoryBranch
                .Where(x => x.InvID == invID)
                .Select(x => x.Branch)
                .ToListAsync();
        }

        public int AssignBranch(int BranchID, int InvID)
        {
            try
            {
                if (DBcontext.InventoryBranch.Any(x => x.InvID == InvID && x.BranchID == BranchID))
                {
                    return (int)SearchStauts.Dublicated;
                }

                DBcontext.InventoryBranch.Add(new InventoryBranch
                {
                    InvID = InvID,
                    BranchID = BranchID
                });

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int unAssignBranch(int BranchID, int InvID)
        {
            try
            {
                var DBObj = DBcontext.InventoryBranch
                    .FirstOrDefault(x => x.InvID == InvID && x.BranchID == BranchID);

                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                DBcontext.InventoryBranch.Remove(DBObj);
                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }




        // ===================== WAREHOUSE STOCK =====================

        public async Task<List<ItemInventory>> GetStock(int invID)
        {
            return await DBcontext.ItemInventories
                .Include(x => x.Item)
                .Include(x => x.Unit)
                .Where(x => x.InvID == invID)
                .ToListAsync();
        }

        public int AddItemToStock(int InvID, int ItemID, int? UnitID, decimal Count, decimal MinThreshold, decimal? MaxThreshold)
        {
            try
            {
                if (DBcontext.ItemInventories.Any(x => x.InvID == InvID && x.ItemID == ItemID))
                {
                    return (int)SearchStauts.Dublicated;
                }

                DBcontext.ItemInventories.Add(new ItemInventory
                {
                    InvID = InvID,
                    ItemID = ItemID,
                    UnitID = UnitID,
                    Count = Count,
                    MinThreshold = MinThreshold,
                    MaxThreshold = MaxThreshold,
                    LastRestockDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsNearToFinish=false
                });

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int UpdateStock(int InventoryID, decimal Count, decimal MinThreshold, decimal? MaxThreshold)
        {
            try
            {
                var DBObj = DBcontext.ItemInventories.FirstOrDefault(x => x.InventoryID == InventoryID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                DBObj.Count = Count;
                DBObj.MinThreshold = MinThreshold;
                DBObj.MaxThreshold = MaxThreshold;
                DBObj.LastModified = DateTime.Now;

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int ReceiveStock(int InventoryID, decimal Qty)
        {
            try
            {
                if (Qty <= 0) return (int)SearchStauts.IlligalOperation;

                var DBObj = DBcontext.ItemInventories.FirstOrDefault(x => x.InventoryID == InventoryID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                DBObj.Count += Qty;
                DBObj.LastRestockDate = DateTime.Now;
                DBObj.LastModified = DateTime.Now;

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        // ===================== TRANSFER (warehouse -> branch) =====================

        public int TransferStock(int InvID, int BranchID, int ItemID, decimal Qty)
        {
            using var transaction = DBcontext.Database.BeginTransaction();
            try
            {
                if (Qty <= 0) return (int)SearchStauts.IlligalOperation;

                // branch must be assigned to this warehouse
                if (!DBcontext.InventoryBranch.Any(x => x.InvID == InvID && x.BranchID == BranchID))
                {
                    return (int)SearchStauts.IlligalOperation;
                }

                var warehouseStock = DBcontext.ItemInventories
                    .FirstOrDefault(x => x.InvID == InvID && x.ItemID == ItemID);

                if (warehouseStock == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                if (warehouseStock.Count < Qty)
                {
                    return (int)SearchStauts.IlligalOperation; // not enough in warehouse
                }

                warehouseStock.Count -= Qty;
                warehouseStock.LastModified = DateTime.Now;

                var branchStock = DBcontext.BranchStocks
                    .FirstOrDefault(x => x.BranchID == BranchID && x.ItemID == ItemID);

                if (branchStock == null)
                {
                    DBcontext.BranchStocks.Add(new BranchStock
                    {
                        BranchID = BranchID,
                        ItemID = ItemID,
                        UnitID = warehouseStock.UnitID,
                        Count = Qty,
                        MinThreshold = 0,
                        LastTransferDate = DateTime.Now,
                        LastModified = DateTime.Now
                    });
                }
                else
                {
                    branchStock.Count += Qty;
                    branchStock.LastTransferDate = DateTime.Now;
                    branchStock.LastModified = DateTime.Now;
                }

                DBcontext.SaveChanges();
                transaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (int)SearchStauts.Error;
            }
        }

        // ===================== BRANCH STOCK (kiosk side) =====================

        public async Task<List<BranchStock>> GetBranchStock(int branchID)
        {
            return await DBcontext.BranchStocks
                .Include(x => x.Item)
                .Include(x => x.Unit)
                .Where(x => x.BranchID == branchID)
                .ToListAsync();
        }

        public int UpdateBranchStock(int BranchStockID, decimal Count, decimal MinThreshold)
        {
            try
            {
                var DBObj = DBcontext.BranchStocks.FirstOrDefault(x => x.BranchStockID == BranchStockID);
                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                DBObj.Count = Count;
                DBObj.MinThreshold = MinThreshold;
                DBObj.LastModified = DateTime.Now;

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }

        public int DeductBranchStock(int BranchID, int ItemID, decimal Qty)
        {
            try
            {
                if (Qty <= 0) return (int)SearchStauts.IlligalOperation;

                var DBObj = DBcontext.BranchStocks
                    .FirstOrDefault(x => x.BranchID == BranchID && x.ItemID == ItemID);

                if (DBObj == null)
                {
                    return (int)SearchStauts.NotFound;
                }

                if (DBObj.Count < Qty)
                {
                    return (int)SearchStauts.IlligalOperation; // not enough on shelf
                }

                DBObj.Count -= Qty;
                DBObj.LastModified = DateTime.Now;

                return DBcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                return (int)SearchStauts.Error;
            }
        }
    }
}
