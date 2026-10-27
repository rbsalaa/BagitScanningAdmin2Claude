using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SelfKOrderWeb.DTO
{
    public class GroupDTO
    {
        [Required]
        public int GroupID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters.", MinimumLength = 3)]
        public string GroupName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters.", MinimumLength = 3)]
        public string GroupName_AR { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "The price must be between {1} and {2}.")]
        public decimal ItemPrice { get; set; }

        public int? GroupItemsCount { get; set; }
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
}
