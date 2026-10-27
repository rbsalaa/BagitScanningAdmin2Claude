using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{
    public class AddUserDTO
    {
        [Display(Name = "NormalizedUserName")]
        public string NormalizedUserName { get; set; }
        [Display(Name = "UserID")]
        public string Id { get; set; }
        [Required]
            
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
        public int? FranchID { get; set; }
        public int? BranchID { get; set; }
        public int? BrandID { get; set; }
        public int? CompID { get; set; }
        public bool IsRBSUser { get; set; }
        public bool IsCreatorRBSUser { get; set; }
        public bool IsSuperMainAdmin { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
