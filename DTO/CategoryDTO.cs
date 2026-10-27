using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SelfKOrderWeb.DTO
{

    public class CategoryDTO
    {

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryName_AR { get; set; }
        public int CategoryOrder { get; set; }
        public bool IsSuggestionCat { get; set; }
        public bool IsActive { get; set; }

        //[Required(ErrorMessage = "The {0} field is required")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public string? CategoryPic { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<ItemDTO>? Items { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? FranchList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }

    }






}
