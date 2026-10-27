using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Data;
using System.Reflection;
using System.Security.Claims;

namespace SelfKOrderWeb.Authoraization
{
    public enum TenantEntity
    {
        Item, Category, Group, Menu, Modifier, ModifierCategory, Advertisement, Banner,
        Brand, Branch, Company, Franchise, Kiosik, PaymentMethod, Tax, Discount, ShopDiscount, Inventory
    }

    /// <summary>
    /// Keeps a user inside the companies/brands/branches they are allowed to see.
    /// The list pages were already filtered by <see cref="IAccessFilterService"/>, but the detail, edit and
    /// delete actions took any id from the URL or form, so a user of one company could open or change
    /// another company's records by changing the id. This filter runs the same access filter for the
    /// requested record (404 when it is not visible to the user) and, for posted forms, checks that the
    /// CompID/BrandID/BranchID being saved are ones the user may use (403 otherwise).
    /// RBS users are not restricted, exactly as in the list filters.
    /// </summary>
    /// <param name="idSource">"id" for an action argument, or "obj.ItemID" for a property of a posted DTO.</param>
    /// <param name="scopeOnly">For create forms: the id is new (or typed by the user), so only the
    /// CompID/BrandID/BranchID being saved are checked.</param>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TenantScopeAttribute : TypeFilterAttribute
    {
        public TenantScopeAttribute(TenantEntity entity, string idSource, bool scopeOnly = false) : base(typeof(TenantScopeFilter))
        {
            Arguments = new object[] { entity, idSource, scopeOnly };
        }
    }

    public class TenantScopeFilter : IAsyncActionFilter
    {
        private readonly TenantEntity _entity;
        private readonly string _idSource;
        private readonly bool _scopeOnly;
        private readonly IAccessFilterService _filter;
        private readonly SelfKOrderWebContext _db;
        private readonly ILogger<TenantScopeFilter> _logger;

        public TenantScopeFilter(TenantEntity entity, string idSource, bool scopeOnly, IAccessFilterService filter,
            SelfKOrderWebContext db, ILogger<TenantScopeFilter> logger)
        {
            _entity = entity;
            _idSource = idSource;
            _scopeOnly = scopeOnly;
            _filter = filter;
            _db = db;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.Claims.FirstOrDefault(c => c.Type == "IsRBSUser")?.Value == "true")
            {
                await next();
                return;
            }

            var parts = _idSource.Split('.', 2);
            var arg = GetArgument(context, parts[0]);
            object? dto = parts.Length == 2 ? arg : null;
            int? id = parts.Length == 2 ? ToInt(GetProperty(arg, parts[1])) : ToInt(arg);

            try
            {
                if (!_scopeOnly && id is > 0 && !await CanAccessAsync(user, id.Value))
                {
                    _logger.LogWarning("Blocked access to {Entity} {Id} for user {User}", _entity, id, user.Identity?.Name);
                    context.Result = new NotFoundResult();
                    return;
                }

                if (dto != null && !await CanUseScopeAsync(user, dto))
                {
                    _logger.LogWarning("Blocked saving {Entity} into another company/brand/branch for user {User}", _entity, user.Identity?.Name);
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception ex)
            {
                // The access filters parse the user's claims; if they cannot, deny rather than allow.
                _logger.LogError(ex, "Access check failed for {Entity} {Id}", _entity, id);
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }

        private Task<bool> CanAccessAsync(ClaimsPrincipal u, int id) => _entity switch
        {
            TenantEntity.Item => Exists(_filter.ItemFilter(u, _db.Items.Where(x => x.ItemID == id))),
            TenantEntity.Category => Exists(_filter.catFilter(u, _db.Category.Where(x => x.CategoryID == id))),
            TenantEntity.Group => Exists(_filter.GroupFilter(u, _db.Groups.Where(x => x.GroupID == id))),
            TenantEntity.Menu => Exists(_filter.MenuFilter(u, _db.Menu.Where(x => x.MenuID == id))),
            TenantEntity.Modifier => Exists(_filter.ModifierFilter(u, _db.Modifiers.Where(x => x.ModifierID == id))),
            TenantEntity.ModifierCategory => Exists(_filter.ModifierCategoryFilter(u, _db.ModifierCategory.Where(x => x.ModifierCategoryID == id))),
            TenantEntity.Advertisement => Exists(_filter.AdverFilter(u, _db.Advertisment.Where(x => x.AdsID == id))),
            TenantEntity.Banner => Exists(_filter.BannerFilter(u, _db.Banner.Where(x => x.BannerID == id))),
            TenantEntity.Brand => Exists(_filter.BrandFilter(u, _db.Brand.Where(x => x.BrandID == id))),
            TenantEntity.Branch => Exists(_filter.BranchFilter(u, _db.Branch.Where(x => x.BranchID == id))),
            TenantEntity.Company => Exists(_filter.CompFilter(u, _db.Company.Where(x => x.CompID == id))),
            TenantEntity.Franchise => Exists(_filter.FranchFilter(u, _db.Franchising.Where(x => x.FranchID == id))),
            TenantEntity.Kiosik => Exists(_filter.KiosikFilter(u, _db.Kiosik.Where(x => x.KiosikID == id))),
            TenantEntity.PaymentMethod => Exists(_filter.PaymentMethodFilter(u, _db.PaymentMethod.Where(x => x.PaymentMethodID == id))),
            TenantEntity.Tax => Exists(_filter.TAXFilter(u, _db.TAXGroup.Where(x => x.TAXID == id))),
            TenantEntity.Discount => Exists(_filter.DiscountFilter(u, _db.Discounts.Where(x => x.DiscountID == id))),
            TenantEntity.ShopDiscount => Exists(_filter.ShopDiscountFilter(u, _db.StoreMenuDiscounts.Where(x => x.DiscountMenuID == id))),
            TenantEntity.Inventory => Exists(_filter.InventoryFilter(u, _db.InventoryWarehouses.Where(x => x.InvID == id))),
            _ => Task.FromResult(true)
        };

        // The company/brand/branch written by a create or edit form must be one the user can see.
        private async Task<bool> CanUseScopeAsync(ClaimsPrincipal u, object dto)
        {
            // A company record's own CompID is its key, not a scope value.
            var compId = _entity == TenantEntity.Company ? null : ToInt(GetProperty(dto, "CompID"));
            if (compId is > 0 && !await Exists(_filter.CompFilter(u, _db.Company.Where(x => x.CompID == compId))))
                return false;

            // A brand record's own BrandID is its key (already checked above), not a scope value.
            var brandId = _entity == TenantEntity.Brand ? null : ToInt(GetProperty(dto, "BrandID"));
            if (brandId is > 0 && !await Exists(_filter.BrandFilter(u, _db.Brand.Where(x => x.BrandID == brandId))))
                return false;

            var branchId = _entity == TenantEntity.Branch ? null : ToInt(GetProperty(dto, "BranchID"));
            if (branchId is > 0 && !await Exists(_filter.BranchFilter(u, _db.Branch.Where(x => x.BranchID == branchId))))
                return false;

            return true;
        }

        // Runs the access-filtered query for the one requested row the same way the list pages do (ToListAsync).
        private static async Task<bool> Exists<T>(IQueryable<T> query) =>
            (await query.Take(1).ToListAsync()).Count > 0;

        private static object? GetArgument(ActionExecutingContext context, string name)
        {
            foreach (var kv in context.ActionArguments)
                if (string.Equals(kv.Key, name, StringComparison.OrdinalIgnoreCase))
                    return kv.Value;
            return null;
        }

        private static object? GetProperty(object? obj, string name) =>
            obj?.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?.GetValue(obj);

        private static int? ToInt(object? value) => value switch
        {
            int i => i,
            string s when int.TryParse(s, out var i) => i,
            _ => null
        };
    }
}
