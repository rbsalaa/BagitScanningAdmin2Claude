using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{

    public class ModifierDTO
    {

        public int ModifierID { get; set; }
        public string ModifierName { get; set; }
        public string ModifierName_AR { get; set; }
        public decimal ModifierPrice{ get; set; }
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
        public string? AlohProductID { get; set; }
        public string? ModifierPic { get; set; }
   //     public List<SelectListItem>? modifierCategory { get; set; } = null;
        public int ModifierCategoryID { get; set; }
        public string? ModifierCategoryName { get; set; } = null;
        public string? ModifierCategoryName_Ar { get; set; } = null;
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
