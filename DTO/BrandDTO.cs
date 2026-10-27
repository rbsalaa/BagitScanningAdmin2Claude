using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.DTO
{
    public class BrandDTO
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandPic { get; set; }
        public string? CompName { get; set; } = null;
        public List<SelectListItem>? CompList { get; set; } = null;
        public string BrandCode { get; set; }
        public bool IsActive { get; set; }
        public int CompID { get; set; }

        //[Required(ErrorMessage = "The {0} field is required")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }
    }
    public class BranchDTO
    {


       
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }
        public int? FranchID { get; set; }
        public string BrandCode { get; set; }
        public int BrandID { get; set; }
        public int CompID { get; set; }
        public bool IsActive { get; set; }
        public string? BrandName { get; set; }
        public string? FranchName { get; set; }
        public string? CompanyName { get; set; }
        public int OldCompId { get;  set; }
        public int OldbranchId { get; set; }
        public int OldBrndId { get;  set; }
        public List<SelectListItem>? CompList { get;  set; }
        public List<SelectListItem>? BrandList { get;  set; }
        public List<SelectListItem>? FranchList { get; set; }

    }

    public class FranchisingDTO
    {


       
        public int FranchID { get; set; }
        public string FranchName { get; set; }
        public string FranchLocation { get; set; }
        public string? CompanyName { get; set; }
        public string? BrandName { get; set; }
        public string FranchCode { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public bool IsActive { get; set; }
        public int OldCompId { get; set; }
        public int OldBrndId { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }

    }
}
