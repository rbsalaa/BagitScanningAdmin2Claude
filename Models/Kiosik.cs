using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Kiosik
    { 


        [System.ComponentModel.DataAnnotations.Key]
        public int KiosikID { get; set; }
        public string KiosikName { get; set; }
        public string KiosikRegion { get; set; }
        public int BranchID { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public int? MenuID { get; set; }
        public int CustomerID { get; set; }
        public bool IsCurrentSupscriped { get; set; }
        public bool IsActive { get; set; } 
        public bool NFC { get; set; } 
        public bool Printer { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // Virtual for lazy loading


        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("CompID")]
        public virtual Company Company { get; set; }
        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; }
        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("MenuID")]
        public virtual Menu? Menu { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
}
