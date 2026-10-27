using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Models;
using System.Reflection.Emit;

namespace SelfKOrderWeb.Data;

public class SelfKOrderWebContext : IdentityDbContext<SelfKOrderWebUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public SelfKOrderWebContext(DbContextOptions<SelfKOrderWebContext> options)
        : base(options)
    {
    }
    public virtual DbSet<SelfKOrderWebUser> User { get; set; } = null!;
    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<DataModel> DataModels { get; set; }
    public virtual DbSet<Advertisments> Advertisment { get; set; } = null!;
    public virtual DbSet<MenuSuggestions> MenuSuggestions { get; set; } = null!;
    public virtual DbSet<Banners> Banner { get; set; } = null!;
    public virtual DbSet<KiosikBanners> KiosikBanner { get; set; } = null!;
    public virtual DbSet<KiosikAds> KiosikAds { get; set; } = null!;
    public virtual DbSet<Category> Category { get; set; } = null!;
    public virtual DbSet<ItemsCategory> ItemsCategory { get; set; } = null!;
    public virtual DbSet<Company> Company { get; set; } = null!;
    public virtual DbSet<Brand> Brand { get; set; } = null!;
    public virtual DbSet<Branch> Branch { get; set; } = null!;
    public virtual DbSet<Customer> Customer { get; set; } = null!;
    public virtual DbSet<Items> Items { get; set; } = null!;
    public virtual DbSet<Groups> Groups { get; set; } = null!;
    public virtual DbSet<GroupsItem> GroupsItem { get; set; } = null!;
    public virtual DbSet<Kiosik> Kiosik { get; set; } = null!;
    public virtual DbSet<MDFileUpload> MDFileUpload { get; set; } = null!;
    public virtual DbSet<Menu> Menu { get; set; } = null!;
    public virtual DbSet<MenuItems> MenuItems { get; set; } = null!;
    public virtual DbSet<Promotion> Promotion { get; set; } = null!;
    public virtual DbSet<ItemsPromotion> ItemsPromotion { get; set; } = null!;
    public virtual DbSet<RBSAdmin> RBSAdmin { get; set; } = null!;
    public virtual DbSet<SubscrubtionInfo> SubscrubtionInfo { get; set; } = null!;
    public virtual DbSet<TransactionSummery> TransactionSummery { get; set; } = null!;
    public virtual DbSet<TransactionDetails> TransactionDetails { get; set; } = null!;
    public virtual DbSet<TransactionStauts> TransactionStauts { get; set; } = null!;
    public virtual DbSet<PaymentMethod> PaymentMethod { get; set; } = null!;
    public virtual DbSet<TAXGroup> TAXGroup { get; set; } = null!;
    public virtual DbSet<NavigationMenu> NavigationMenu { get; set; } = null!;
    public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }
    public DbSet<Discounts> Discounts { get; set; }
    public DbSet<StoreMenuDiscounts> StoreMenuDiscounts { get; set; }
    public DbSet<SubscrubtionInfoDetails> SubscrubtionInfoDetails { get; set; }
    public DbSet<ModifierModifierCategory> ModifierModifierCategory { get; set; }
    public virtual DbSet<Modifier> Modifiers { get; set; }
    public virtual DbSet<ItemModifier> ItemModifier { get; set; }
    public virtual DbSet<ModifierCategory> ModifierCategory { get; set; }
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<OrderDetail> OrderDetail { get; set; }
    public virtual DbSet<Franchising> Franchising { get; set; }
    public virtual DbSet<KiosikHardwareLog> KiosikHardwareLog { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<InventoryWarehouse> InventoryWarehouses { get; set; }
    public DbSet<ItemInventory> ItemInventories { get; set; }
    public DbSet<ItemUnitConversion> ItemUnitConversions { get; set; }
    public DbSet<InventoryBranch> InventoryBranch { get; set; }
    public DbSet<BranchStock> BranchStocks { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<RoleMenuPermission>()
       .HasKey(c => new { c.RoleId, c.NavigationMenuId });
        builder.Entity<ItemModifier>()
           .HasKey(mm => new { mm.ItemId, mm.ModifierCategoryID });
        builder.Entity<DataModel>()
      .HasNoKey();
        builder.Entity<ModifierModifierCategory>()
       .HasKey(x => new { x.ModifierID, x.ModifierCategoryID });

        builder.Entity<ModifierModifierCategory>()
            .HasOne(x => x.Modifier)
            .WithMany(m => m.ModifierModifierCategories)
            .HasForeignKey(x => x.ModifierID).OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade delete;

        builder.Entity<ModifierModifierCategory>()
            .HasOne(x => x.ModifierCategory)
            .WithMany(c => c.ModifierModifierCategories)
            .HasForeignKey(x => x.ModifierCategoryID).OnDelete(DeleteBehavior.Restrict); // ✅ Prevent cascade delete;

        builder.Entity<ItemInventory>()
               .HasIndex(i => new { i.ItemID, i.InvID })
               .IsUnique();

        // Prevent multiple cascade paths (SQL Server restriction)
        builder.Entity<ItemInventory>()
            .HasOne(i => i.Item)
            .WithMany(x => x.ItemInventories)
            .HasForeignKey(i => i.ItemID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemInventory>()
            .HasOne(i => i.Warehouse)
            .WithMany(w => w.ItemInventories)
            .HasForeignKey(i => i.InvID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemInventory>()
            .HasOne(i => i.Unit)
            .WithMany(u => u.ItemInventories)
            .HasForeignKey(i => i.UnitID)
            .OnDelete(DeleteBehavior.Restrict);

        // ItemUnitConversion: two FKs to Units (From/To) — must configure both explicitly
        builder.Entity<ItemUnitConversion>()
            .HasOne(c => c.FromUnit)
            .WithMany(u => u.FromConversions)
            .HasForeignKey(c => c.FromUnitID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemUnitConversion>()
            .HasOne(c => c.ToUnit)
            .WithMany(u => u.ToConversions)
            .HasForeignKey(c => c.ToUnitID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemUnitConversion>()
            .HasOne(c => c.Item)
            .WithMany(i => i.UnitConversions)
            .HasForeignKey(c => c.ItemID)
            .OnDelete(DeleteBehavior.Restrict);

        // Computed column — tell EF this is DB-generated, not written by app
        builder.Entity<ItemInventory>()
        .Property(x => x.IsNearToFinish)
        .HasComputedColumnSql("CAST(CASE WHEN [Count] <= [MinThreshold] THEN 1 ELSE 0 END AS bit)");
        builder.Entity<InventoryBranch>(entity =>
        {
            entity.HasKey(x => new { x.InvID, x.BranchID });

            entity.HasOne(x => x.Inventory)
                  .WithMany(i => i.InventoryBranches)
                  .HasForeignKey(x => x.InvID);

            entity.HasOne(x => x.Branch)
                  .WithMany()
                  .HasForeignKey(x => x.BranchID);
        });
        builder.Entity<BranchStock>()
    .HasIndex(x => new { x.BranchID, x.ItemID })
    .IsUnique();
    }

}
