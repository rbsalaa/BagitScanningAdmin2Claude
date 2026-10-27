using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace SelfKOrderWeb.DTO
{
    public class DashBoardDTO
    {
        public DashBoardDTO? dashObj { get; set; }
        public List<KiosikHardwareLogDTO>? KiiskSauts { get; set; }
        public int? FraKioskConNum { get; set; }
        public int? BraKioskConNum { get; set; }

        public string? Kiosikdata { get; set; }

    }
    public class KioskDashboardDTO
    {
        // Basic kiosk info
        public int KiosikID { get; set; }
        public string KiosikName { get; set; }
        public string Region { get; set; }
        public string CompanyName { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }

        // Order stats
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }

        //topselled 
        public List<TopSoldItemDTO> TopSoldItems { get; set; } = new List<TopSoldItemDTO>();
        // Monthly stats
        public List<MonthlyOrdersDTO> OrdersPerMonth { get; set; } = new();

        // Yearly stats
        public List<YearlyOrdersDTO> OrdersPerYear { get; set; } = new();

        // Recent
        public List<OrderSummaryDTO> RecentOrders { get; set; } = new();
    }

    public class MonthlyOrdersDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrdersCount { get; set; }
    }

    public class YearlyOrdersDTO
    {
        public int Year { get; set; }
        public int OrdersCount { get; set; }
    }

    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public int KiosikID { get; set; }
        public string? OrderName { get; set; }
        public int? AlohaOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string KiosikName { get; internal set; }
        public int BranchID { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public string PaymmentTransactionID { get; set; }
        public string TenderID { get; set; }
        public string? JsonString { get; internal set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>(); // Virtual for lazy loading
    }
    public class TopSoldItemDTO
    {
        public string ItemName { get; set; }
        public int QuantitySold { get; set; }
    }
}
