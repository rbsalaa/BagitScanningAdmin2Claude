using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Menu
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int MenuID { get; set; }
        public string MenuName { get; set; }

        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCurrentMenu { get; set; }

        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public virtual ICollection<MenuItems> MenuItems { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
    
    public class MenuSuggestions
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int MenuSugID { get; set; }
        public int MenuID { get; set; }
        public int ItemID { get; set; }
        public bool IsCurrentEnabled { get; set; }
        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("ItemID")]
        public virtual Items item { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
        public class MenuItems
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int MenuItemsID { get; set; }
        public int MenuID { get; set; }
        public int ItemID { get; set; }
        public bool IsCurrentEnabled { get; set; }
        public decimal Price { get; set; }
        public bool IsNearFinish { get; set; }
        public int? LastXItems { get; set; }
        public bool IsCurrentAvalible { get; set; }

        [ForeignKey("MenuID")]
        public virtual Menu Menu { get; set; }
        [ForeignKey("ItemID")]
        public virtual Items item { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
}
