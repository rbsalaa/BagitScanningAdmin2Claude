using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class TransactionSummery
    { 


        [System.ComponentModel.DataAnnotations.Key]
        public int TransactionSummeryID { get; set; }
        public int KiosikID { get; set; }
        public DateTime TransactionDate { get; set; }
       
        public decimal TotalPrice { get; set; }
        public int? DiscountID { get; set; }
        public int TAXID { get; set; }
        public int StautsID { get; set; }
        public bool IsDelivered { get; set; }

        public bool IsUpdated{ get; set; }
        [ForeignKey("KiosikID")]
        public virtual Kiosik Kiosik { get; set; }
      
        [ForeignKey("StautsID")]
        public virtual TransactionStauts Stauts { get; set; }
        public virtual Collection<TransactionDetails> TransactionDetails { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
    public class TransactionDetails
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int TransactionDetailsID { get; set; }
        public int TransactionSummeryID { get; set; }
        public int? DiscountID { get; set; }
        public int TAXID { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal TotalBeforeTaxAndDiscount { get; set; }
        public decimal TotalAfterTaxAndDiscount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public int ItemID { get; set; }
   
        public int PaymentMethodID { get; set; }
        [ForeignKey("DiscountID")]
        public virtual Discounts Discounts { get; set; }

        [ForeignKey("PaymentMethodID")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("TransactionSummeryID")]
        public virtual TransactionSummery TransactionSummery { get; set; }
       
        [ForeignKey("TAXID")]
        public virtual TAXGroup TAX { get; set; }

       
        [ForeignKey("ItemID")]
        public virtual Items Item { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class TransactionStauts
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int StautsID { get; set; }
        public string Stautsname { get; set; }
         public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional


    }

    public class PaymentMethod
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodNameAr { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class TAXGroup
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int TAXID { get; set; }
        public string TAXName { get; set; }
        public string TAXPercentage { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class Discounts
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int DiscountID { get; set; }
        public string DiscountTitle { get; set; }
        public decimal DiscountPercintage { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public string DiscountCode { get; set; }
        public int? MAXUserNum { get; set; }
        public bool IsPercintage { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class StoreMenuDiscounts
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int DiscountMenuID { get; set; }
        public string DiscountTitle { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountPercintage { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public bool IsAllMenuDiscount { get; set; }
        public bool IsPercintage { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }
}
