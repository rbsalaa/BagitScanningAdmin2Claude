using IdentityCore.Factory.RepositoryBannersDTO;
using IdentityCore.Factory.RepositoryBrandDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Authoraization.IHandler;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Diagnostics;
using System.Text.Json;
using static SelfKOrderWeb.Factory.Enums;


namespace SelfKOrderWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;


        private readonly SignInManager<SelfKOrderWebUser> _signInManager;
        private readonly IDataAccessService _dataAccessService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<SelfKOrderWebUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;
        DashboardRepo dashboardRepo;
        KiosikRepo _kiosikRepo;
        OrderRepo _OrderRepo;
        private readonly IAccessFilterService _accessFilter;
        //    RlsContextService _rlsContextService;
        private readonly SelfKOrderWebContext DBcontext;


        public DashboardController(IDataAccessService dataAccessService, IAccessFilterService accessFilter, SignInManager<SelfKOrderWebUser> signInManager, SelfKOrderWebContext _DBcontext, ILogger<DashboardController> logger, UserManager<SelfKOrderWebUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _kiosikRepo = new KiosikRepo(_DBcontext, accessFilter);
            _OrderRepo = new OrderRepo(_DBcontext, accessFilter);
            _dataAccessService = dataAccessService;
            DBcontext = _DBcontext;
            dashboardRepo = new DashboardRepo(DBcontext, accessFilter);
            _kiosikRepo = new KiosikRepo(DBcontext, accessFilter);
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _accessFilter = accessFilter;
          //  _rlsContextService = rlsContextService;
        }
        // GET: Dashboard
       //[Authorize]
        public async Task<IActionResult> Index()
        {
            // await _rlsContextService.SetRlsContextAsync();
            // await _rlsContextService.SetRlsContextAsync();
            var dashboard = new DashBoardDTO();
            dashboard.dashObj = await dashboardRepo.GetCountKiosiksberFranch(User);
            var x1 = await dashboardRepo.GetTopItemsList(User);
            // Order count / total amount / kiosk count were queried here but the view does not show them
            // (and two of them were not limited to the user's company). Removed to save three queries per load.
            var x5 = await dashboardRepo.GetKioskOrder(User);



            //  IEnumerable<IGrouping<string, KiosikDTO>> StoreList = _kiosikRepo.GetKiosikListbyCustomer(User);
            dashboard.KiiskSauts = await dashboardRepo.KiosikHardwareLog(User);
         
            ViewBag.top = x1; // Example image path
            ViewBag.kioskStat = JsonSerializer.Serialize(x5); // Example image path


            return View(dashboard);
        }
        [HttpPost]
        public List<SelectListItem> KiosiksList()
        {
            var PreferenceValues = Factory.SelectList.AllKisoikSelectList(DBcontext, User, _accessFilter).ToList();
            return PreferenceValues;
        }
        public IActionResult KIOSIKDashBoard()
        {
           
            ViewBag.KioskList = KiosiksList();
            return View();
        }
        public async Task<IActionResult> OrdersDashBoard()
        {

            var orders = await  _OrderRepo.GetOrderList(User);
            return View(orders);
        }
        public async Task<IActionResult> HardwareDashBoard()
        {

            var orders = await _OrderRepo.GetHardwareStautsList(User);
            return View(orders);
        }
        public async Task<IActionResult> Details(int id)
        {
            // Only orders the user may see (same rule as the orders list).
            if (!await _accessFilter.OrderFilter(User, DBcontext.Order.Where(o => o.OrderId == id)).AnyAsync())
                return NotFound();

            var OrderCarry = new OrderDetailsViewModel();
            OrderCarry.OrderDetails = await _OrderRepo.GetOrderDetails(User,id);
            var first = OrderCarry.OrderDetails.FirstOrDefault();
            OrderCarry.OrderJsonParsed = first?.JsonString == null ? null : ParseOrderJson(first.JsonString);
            return View(OrderCarry);
        }
        public static OrderResponse? ParseOrderJson(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<OrderResponse>(json, options);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return null;
            }
        }
        public IActionResult KIOSIKDashBoardpost(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var parts = id.Split('*');
            if (parts.Length != 4)
                return BadRequest("Invalid kiosk value");

            if (!int.TryParse(parts[0], out int compId) || !int.TryParse(parts[1], out int brandId) ||
                !int.TryParse(parts[2], out int branchId) || !int.TryParse(parts[3], out int kiosikId))
                return BadRequest("Invalid kiosk value");

            // The kiosk must be one the user may see (same rule as the kiosk list).
            if (!_accessFilter.KiosikFilter(User, DBcontext.Kiosik.Where(k => k.KiosikID == kiosikId)).Any())
                return NotFound();

            var kiosk = DBcontext.Kiosik
                .Include(k => k.Company)
                .Include(k => k.Brand)
                .Include(k => k.Branch)
                .FirstOrDefault(k =>
                    k.CompID == compId &&
                    k.BrandID == brandId &&
                    k.BranchID == branchId &&
                    k.KiosikID == kiosikId);

            if (kiosk == null)
                return NotFound();

            // Filter orders only for this kiosk
            var ordersQuery = DBcontext.Order.Where(o => o.KiosikID == kiosikId);
            var monthNames = new[] { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            var dto = new KioskDashboardDTO
            {
                KiosikID = kiosk.KiosikID,
                KiosikName = kiosk.KiosikName,
                Region = kiosk.KiosikRegion,
                CompanyName = kiosk.Company?.CompName,
                BrandName = kiosk.Brand?.BrandName,
                BranchName = kiosk.Branch?.BranchName,
                // 1 == completed , 2== cancled
                TotalOrders = ordersQuery.Count(),
                CompletedOrders = ordersQuery.Count(o => o.Status =="1"),
                CancelledOrders = ordersQuery.Count(o => o.Status =="2"),
                TotalSales = ordersQuery.Where(o => o.Status == "1").Sum(o => (decimal?)o.TotalAmount) ?? 0,
                AverageOrderValue = ordersQuery.Any() ? ordersQuery.Average(o => o.TotalAmount) : 0,

                OrdersPerMonth = ordersQuery
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new MonthlyOrdersDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    OrdersCount = g.Count()
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToList(),

                OrdersPerYear = ordersQuery
                    .GroupBy(o => o.OrderDate.Year)
                    .Select(g => new YearlyOrdersDTO
                    {
                        Year = g.Key,
                        OrdersCount = g.Count()
                    })
                    .OrderBy(g => g.Year)
                    .ToList(),

                RecentOrders = ordersQuery
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new OrderSummaryDTO
                    {
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        Status = o.Status,
                        TotalAmount = o.TotalAmount
                    }).ToList(),
                    TopSoldItems = ordersQuery
                            .SelectMany(o => o.OrderDetails)                  // Flatten all OrderDetails
                            .GroupBy(od => od.Items.ItemName)                // Group by item name
                            .Select(g => new TopSoldItemDTO
                            {
                                ItemName = g.Key,
                                QuantitySold = g.Sum(x => x.Quantity)        // Sum quantities sold
                            })
                            .OrderByDescending(x => x.QuantitySold)          // Sort descending
                            .Take(5)                                         // Take top 5
                            .ToList()

            };

            return PartialView("_KioskDetailsPartial", dto);

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}