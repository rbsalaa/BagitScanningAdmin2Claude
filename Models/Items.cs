using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Items
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int ItemID { get; set; }

        public string ItemName { get; set; }
        public string ItemName_AR { get; set; }
        public string ItemDescription { get; set; }
        public string ItemDescription_AR { get; set; }
        public string? Calories { get; set; }
        public string? Steps { get; set; }
        public string? Alergies { get; set; }


        public string? AlergiesAr { get; set; }
        public string? AlohaProductID { get; set; }
        public decimal ItemPrice { get; set; }
        public bool IsGroupItem { get; set; }
        public int? GroupID { get; set; }
        public string? ItemPic { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Groups Groups { get; set; }

        public virtual ICollection<ItemModifier> ItemModifiers { get; set; } = new List<ItemModifier>();
        public virtual ICollection<ItemsCategory> ItemCategories { get; set; } = new List<ItemsCategory>();
        public ICollection<ItemInventory> ItemInventories { get; set; }
        public ICollection<ItemUnitConversion> UnitConversions { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }

    public class Groups
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupName_AR { get; set; }
        public decimal ItemPrice { get; set; }

        public int? GroupItemsCount { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class GroupsItem
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int GroupsItemID { get; set; }
        public int GroupID { get; set; }
        public int ItemID { get; set; }
        public decimal ItemPrice { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

        public bool IsActive { get; set; }

        [ForeignKey("GroupID")]
        public virtual Groups Groups { get; set; }
        [ForeignKey("ItemID")]
        public virtual Items items { get; set; }
    }








    [Table("Units")]
    public class Unit
    {
        [Key]
        public int UnitID { get; set; }

        [Required, MaxLength(50)]
        public string UnitName { get; set; }

        [MaxLength(50)]
        public string UnitName_AR { get; set; }

        [Required, MaxLength(20)]
        public string UnitType { get; set; } = "Count"; // Count, Weight, Volume, Service

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ItemInventory> ItemInventories { get; set; }
        public ICollection<ItemUnitConversion> FromConversions { get; set; }
        public ICollection<ItemUnitConversion> ToConversions { get; set; }
    }

    [Table("InventoryWarehouse")]
    public class InventoryWarehouse
    {
        [Key]
        public int InvID { get; set; }

        [Required, MaxLength(100)]
        public string InvName { get; set; }

        [MaxLength(100)]
        public string InvName_AR { get; set; }

        [MaxLength(255)]
        public string InvLocation { get; set; }

      
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ItemInventory> ItemInventories { get; set; }
        public ICollection<InventoryBranch> InventoryBranches { get; set; }
    }

    [Table("ItemInventory")]
    public class ItemInventory
    {
        [Key]
        public int InventoryID { get; set; }

        [Required]
        public int ItemID { get; set; }
        [ForeignKey(nameof(ItemID))]
        public Items Item { get; set; }

        public int? InvID { get; set; }
        [ForeignKey(nameof(InvID))]
        public InventoryWarehouse Warehouse { get; set; }

        public int? UnitID { get; set; }
        [ForeignKey(nameof(UnitID))]
        public Unit Unit { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Count { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinThreshold { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? MaxThreshold { get; set; }

        [Column(TypeName = "bit")]
        public bool IsNearToFinish { get; set; }

        public DateTime? LastRestockDate { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
    [Table("InventoryBranch")]
    public class InventoryBranch
    {
        public int InvID { get; set; }
        public int BranchID { get; set; }

        public InventoryWarehouse Inventory { get; set; }
        public Branch Branch { get; set; }
    }
    [Table("ItemUnitConversion")]
    public class ItemUnitConversion
    {
        [Key]
        public int ConversionID { get; set; }

        [Required]
        public int ItemID { get; set; }
        [ForeignKey(nameof(ItemID))]
        public Items Item { get; set; }

        [Required]
        public int FromUnitID { get; set; }
        [ForeignKey(nameof(FromUnitID))]
        public Unit FromUnit { get; set; }

        [Required]
        public int ToUnitID { get; set; }
        [ForeignKey(nameof(ToUnitID))]
        public Unit ToUnit { get; set; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal ConversionFactor { get; set; }
    }
    [Table("BranchStock")]
    public class BranchStock
    {
        [Key]
        public int BranchStockID { get; set; }

        [Required]
        public int BranchID { get; set; }

        [Required]
        public int ItemID { get; set; }
        [ForeignKey(nameof(ItemID))]
        public Items Item { get; set; }

        public int? UnitID { get; set; }
        [ForeignKey(nameof(UnitID))]
        public Unit Unit { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Count { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinThreshold { get; set; }

        public DateTime? LastTransferDate { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }

}