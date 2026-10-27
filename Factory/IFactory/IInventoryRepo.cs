using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IItIInventoryemRepo
    {
        public Task<IEnumerable<InventoryDTO>> GetInventory(ClaimsPrincipal User);
        public Task<InventoryDTO> GetInventory( int itemID);
        public int DeleteInventory(  int  carryData);
        public int EditInventory (  InventoryDTO obj);
        public int SaveInventory( InventoryDTO obj);
        public Task<IEnumerable<Branch>> GetUnAssignedBranches(int invID, ClaimsPrincipal User);
        public Task<IEnumerable<Branch>> GetAssignedBranches(int invID);
        public int AssignBranch(int BranchID, int InvID);
        public int unAssignBranch(int BranchID, int InvID);
        // Warehouse stock (buyer / PO side)
        public Task<List<ItemInventory>> GetStock(int invID);
        public int AddItemToStock(int InvID, int ItemID, int? UnitID, decimal Count, decimal MinThreshold, decimal? MaxThreshold);
        public int UpdateStock(int InventoryID, decimal Count, decimal MinThreshold, decimal? MaxThreshold);
        public int ReceiveStock(int InventoryID, decimal Qty);                 // PO received from supplier (+)

        // Transfer warehouse -> branch
        public int TransferStock(int InvID, int BranchID, int ItemID, decimal Qty);

        // Branch stock (kiosk side)
        public Task<List<BranchStock>> GetBranchStock(int branchID);
        public int UpdateBranchStock(int BranchStockID, decimal Count, decimal MinThreshold);
        public int DeductBranchStock(int BranchID, int ItemID, decimal Qty);   // kiosk sale (-)
    }
}
