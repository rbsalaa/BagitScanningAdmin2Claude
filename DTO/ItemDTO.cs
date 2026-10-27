using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{

    public class ItemDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int? SortOrder { get; set; }
        
        public string ItemName_AR { get; set; }
        public string ItemDescription { get; set; }
        public string ItemDescription_AR { get; set; }
        public string? AlergiesAr { get; set; }
        public decimal ItemPrice { get; set; }
        public bool IsActive { get; set; }
        public string? Calories { get; set; }
        public string? Steps { get; set; }
        public string? Alergies { get; set; }
        public string? AlohaProductID { get; set; }
        public string? GroupName { get; set; }
        public List<SelectListItem>? GrpList { get; set; } = null;
        public int? GroupID { get; set; }

        // Nested structure
        public List<ModifierCategoryDTO> ModifierCategories { get; set; } = new();
        //[Required(ErrorMessage = "The {0} field is required")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public bool IsEnabled { get; set; }
        // Add list of modifier categories
      //  public List<string> ModifierCategories { get; set; } = new();
        public bool IsGroupItem { get; set; }
        public string? ItemPic { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }


    }

    public class InventoryDTO
    {
        [Key]
        public int InvID { get; set; }

        [Required, MaxLength(100)]
        public string InvName { get; set; }

        [MaxLength(100)]
        public string InvName_AR { get; set; }

        [MaxLength(255)]
        public string InvLocation { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }

        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ItemInventory> ItemInventories { get; set; }
    }
    public class InvBranchDTO
    {
        public int invID { get; set; }
        public string invName { get; set; }
        public IEnumerable<Branch> unAssignedBranch { get; set; }
        public IEnumerable<Branch> assignedBranch { get; set; }
    }

    public class StockDTO
    {
        public int invID { get; set; }
        public string invName { get; set; }
        public List<ItemInventory> stockItems { get; set; }
        public List<SelectListItem> itemList { get; set; }
        public List<SelectListItem> unitList { get; set; }
        public List<SelectListItem> branchList { get; set; }   // branches assigned to this warehouse (for transfer)
    }

    public class BranchStockDTO
    {
        public int branchID { get; set; }
        public string branchName { get; set; }
        public List<BranchStock> stockItems { get; set; }
    }

}
