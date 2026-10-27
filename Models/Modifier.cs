using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public partial class Modifier
    {
        public Modifier()
        {
         
        }

        public int ModifierID { get; set; }
        public string ModifierName { get; set; } = null!;
        public string ModifierName_AR { get; set; } = null!;
        public decimal ModifierPrice { get; set; }
        public string? ModifierPic { get; set; }
     
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Virtual for lazy loading
        [ForeignKey("ModifierCategoryID")]
        public virtual ModifierCategory? ModifierCategory { get; set; } = null!;
       // public int ModifierCategoryID { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
        public string? AlohaProductID { get; set; }

        public virtual ICollection<ModifierModifierCategory> ModifierModifierCategories { get; set; }

    }
    public partial class ModifierCategory
    {

        public ModifierCategory()
        {
            Modifiers = new HashSet<Modifier>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        public int ModifierCategoryID { get; set; }

        public string ModifierCategoryName { get; set; } = null!;
        public string ModifierCategoryName_AR { get; set; } = null!;
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool IsMandetory { get; set; } = false!;
        public int Min { get; set; } = 0!;
        public int Max { get; set; } = 0!;

        public string? AlohaProductID { get; set; }
        // تعريف العلاقة العكسية
        public virtual ICollection<Modifier> Modifiers { get; set; }
        public virtual ICollection<ItemModifier> ItemModifiers { get; set; } = new List<ItemModifier>();
        // 👇 New many-to-many navigation
        public virtual ICollection<ModifierModifierCategory> ModifierModifierCategories { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class ModifierModifierCategory
    {
        public int ModifierID { get; set; }
        public Modifier Modifier { get; set; } = null!;

        public int ModifierCategoryID { get; set; }
        public ModifierCategory ModifierCategory { get; set; } = null!;
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
}
