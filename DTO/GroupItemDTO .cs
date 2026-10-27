using System.ComponentModel.DataAnnotations;

namespace SelfKOrderWeb.DTO
{
    public class GroupItemDTO
    {
        [Required]
        public int GroupsItemID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int ItemID { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "The price must be between {1} and {2}.")]
        public decimal ItemPrice { get; set; }

        public bool IsActive { get; set; }
    }
}
