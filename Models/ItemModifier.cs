using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public partial class ItemModifier
    {
       

        public int ItemId { get; set; }        // FK to Items
    
        public string? AlohaProductID { get; set; }
        public int ModifierCategoryID { get; set; }  // FK to ModifierCategory

        [ForeignKey(nameof(ItemId))]
        public virtual Items Item { get; set; } = null!;

        [ForeignKey(nameof(ModifierCategoryID))]
        public virtual ModifierCategory ModifierCat { get; set; } = null!;

        // If ModifierID is a FK to a Modifiers table:
        // [ForeignKey(nameof(ModifierID))]
        // public virtual Modifier? Modifier { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

}
