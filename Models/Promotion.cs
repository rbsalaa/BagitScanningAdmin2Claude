using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Promotion
    { 


        [System.ComponentModel.DataAnnotations.Key]
        public int PromotionID { get; set; }
        public string PromotionHeader { get; set; }
        public decimal SubtractAmount { get; set; }
        public DateTime PromotionStartDate { get; set; }
        public DateTime PromotionEndDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Collection<ItemsPromotion> ItemsPromotion { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional


    }
    public class ItemsPromotion
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int ItemsPromotionID { get; set; }
        public int PromotionID { get; set; }
        public int ItemID { get; set; }
     
        public bool IsActive { get; set; }

        [ForeignKey("PromotionID")]
        public virtual Promotion Promotion { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
}
