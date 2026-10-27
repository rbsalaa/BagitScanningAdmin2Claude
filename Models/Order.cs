using System;
using System.Collections.Generic;

namespace SelfKOrderWeb.Models;




public partial class ApiLog

{
    public string RefrenceID { get; set; } // Primary key
    public string OrderName { get; set; } 
 
    public string JsonString { get; set; } // Optional
    public bool IsSuccess { get; set; } // e.g., Pending, Completed, Cancelled
    public decimal TotalAmount { get; set; } // Calculated based on OrderDetails
    public int KiosikID { get; set; } // Foreign key
    public virtual Kiosik Kiosik { get; set; } // Virtual for lazy loading
    public int BranchID { get; set; }
    public int CompID { get; set; }
    public int BrandID { get; set; }
    public string PaymmentTransactionID { get; set; }
    public string TenderID { get; set; }
    public DateTime LogDate { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Virtual for lazy loading
}
public partial class Order

{
	public int OrderId { get; set; } // Primary key
    public string? OrderName { get; set; }
    public int? AlohaOrderID { get; set; } // Foreign key
    public DateTime OrderDate { get; set; }
	public string CustomerName { get; set; } // Optional
	public string Status { get; set; } // e.g., Pending, Completed, Cancelled
	public decimal TotalAmount { get; set; } // Calculated based on OrderDetails
    public int KiosikID { get; set; } // Foreign key
    public virtual Kiosik Kiosik { get; set; } // Virtual for lazy loading
    public int BranchID { get; set; }
    public int CompID { get; set; }
    public int BrandID { get; set; }
    public string PaymmentTransactionID { get; set; }
    public string TenderID { get; set; }
    public string? JsonString { get; set; } // Optional
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Virtual for lazy loading
    public bool? IsDeleted { get; set; } = false;// Optional
    public DateTime? LastModified { get; set; } // Optional

}

public class OrderDetail
{
	public int OrderDetailId { get; set; } // Primary key
	public int OrderId { get; set; } // Foreign key
  
    public virtual Order Order { get; set; } // Virtual for lazy loading
	public int ItemId { get; set; } // Foreign key for Item
	public virtual Items Items { get; set; } // Virtual for lazy loading
	public int Quantity { get; set; }
	public decimal Price { get; set; } // Price per unit
	public decimal TotalPrice => Quantity * Price; // Computed property
	public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>(); // Virtual for lazy loading
    public bool? IsDeleted { get; set; } = false;// Optional
    public DateTime? LastModified { get; set; } // Optional
}