using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{

    public class ModifierCategoryDTO
    {

        public int ModifierCategoryID { get; set; }
        public string ModifierCategoryName { get; set; }
        public string ModifierCategoryName_AR { get; set; }
        public  List<ModifierDTO> Modifiers { get; set; }
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
