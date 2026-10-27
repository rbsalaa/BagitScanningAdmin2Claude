using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace SelfKOrderWeb.DTO
{
    public class MenuDTO
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }

        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCurrentMenu { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<ItemDTO>? menuItems { get; set; }
        public List<CategoryDTO>? categories { get; set; }
        public List<MenuItems>? menuItems2 { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
    }
}
