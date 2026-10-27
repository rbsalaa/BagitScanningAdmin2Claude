using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Category
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryName_AR { get; set; }
        public int CategoryOrder { get; set; }
        public string? CategoryPic { get; set; }
        public bool IsSuggestionCat { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
    public class ItemsCategory
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int ItemsCategoryID { get; set; }
        public int CategoryID { get; set; }
        public int ItemID { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        [ForeignKey("ItemID")]
        public virtual Items items { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
}
