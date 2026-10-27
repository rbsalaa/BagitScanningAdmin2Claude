using Microsoft.AspNetCore.Mvc.Rendering;

namespace SelfKOrderWeb.DTO
{
    public class RolesDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }
    public class GroupedAspNavigationMenuDTO
    {
        public List<AspNavigationMenuDTO> Items { get; set; }
        public string RoleID { get; set; }
        public string code { get; set; }
    }
}
