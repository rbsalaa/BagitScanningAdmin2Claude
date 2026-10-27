using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace SelfKOrderWeb.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; } // Primary key
        public int OrderId { get; set; } // Foreign key
        public string JsonString { get; set; } // Virtual for lazy loading
        public int ItemId { get; set; } // Foreign key for Item
        public string  ItemName { get; set; } // Virtual for lazy loading
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price per unit
        public decimal TotalPrice => Quantity * Price; // Computed property
        public virtual ICollection<ModifierDTO> Modifiers { get; set; } = new List<ModifierDTO>(); // Virtual for lazy loading
    }
    public class OrderDetailsViewModel
    {
        public IEnumerable<OrderDetailDTO> OrderDetails { get; set; } 
        public OrderResponse? OrderJsonParsed { get; set; }
    }
    public class PaymentMethodDTO
    {
      
        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodNameAr { get; set; }

        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }

    public class TAXGroupDTO
    {
       
        public int TAXID { get; set; }
        public string TAXPercentage { get; set; }
        public string TAXName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }

    public class DiscountsDTO
    {
       
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
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }

    public class StoreMenuDiscountsDTO
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
        [AllowNull]
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }

        public int? FranchID { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }
}

