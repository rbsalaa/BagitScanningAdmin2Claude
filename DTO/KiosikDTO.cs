using Microsoft.AspNetCore.Mvc.Rendering;
using SelfKOrderWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.DTO
{
    public class KiosikDTO
    {
        public int KiosikID { get; set; }
        public string KiosikName { get; set; }
        public string KiosikRegion { get; set; }
        public int BranchID { get; set; }
        public string? BranchName { get; set; }
        public int CompID { get; set; }
        public string? CompName { get; set; }
        public int BrandID { get; set; }
        public string? BrandName { get; set; }
        public int? MenuID { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public bool? IsCurrentSupscriped { get; set; }

        public bool IsActive { get; set; }
        public bool NFC{ get; set; }
        public bool Printer{ get; set; }

        public int OldCompID { get; set; }
        public int OldBrandID { get; set; }
        public int OldBranchID { get; set; }
        public int OldCustomerID { get; set; }
        public List<SelectListItem>? BrandList { get;  set; }
        public List<SelectListItem>? CompList { get;  set; }
        public List<SelectListItem>? BranchList { get;  set; }
        public List<SelectListItem>? CustomerList { get;  set; }
        public List<SelectListItem>? MenuList { get; set; }

    }

    public class AdvertismentsDTO
    {


       
        public int AdsID { get; set; }
        public string AdsTitle { get; set; }
        public DateTime AdsStartDate { get; set; }
        [Required(ErrorMessage = "The {0} field is required")]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile FileAds { get; set; }

        public DateTime AdsEndDate { get; set; }
        public string? AdsURL { get; set; }
        public bool IsActive { get; set; }

        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }

    }

    public class KiosikAdsDTO
    {


        [Display(Name = "Image")]
       
        public int KiosikAdsID { get; set; }
        public int AdsID { get; set; }

        public int KiosikID { get; set; }

        public bool IsActive { get; set; }

        public string KiosikName { get; set; }
        public string AdsTitle { get; set; }


    }


    public class BannersDTO
    {


      //  [System.ComponentModel.DataAnnotations.Key]
        public int BannerID { get; set; }
        public string BannerTitle { get; set; }
        public DateTime BannerStartDate { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile FileAds { get; set; }
        public string? TargetUrl { get; set; }
        public int? TargetID { get; set; }
        public string? TargetType { get; set; }
        public DateTime BannerEndDate { get; set; }
        public string? BannerURL { get; set; }
        public bool IsActive { get; set; }

        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public List<SelectListItem>? BrandList { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
        public string? BannerClickURL { get; internal set; }
    }

    public class KiosikBannersDTO
    {


      //  [System.ComponentModel.DataAnnotations.Key]
        public int KiosikBannerID { get; set; }
        public int Banner { get; set; }

        public int KiosikID { get; set; }

        public bool IsActive { get; set; }


        [ForeignKey("KiosikID")]
        public virtual Kiosik Kiosik { get; set; }
        [ForeignKey("AdsID")]
        public virtual Banners Banners { get; set; }

    }
    public class KiosikHardwareLogDTO
    {


        public int KiosikHardwareID { get; set; }
       
        public DateTime? LastUpateDate { get; set; }

        public string? KiosikName { get; set; }
        public string HardWareCode { get; set; }
        public bool Stauts { get; set; }
        public bool POS { get; set; }
        public bool Printer { get; set; }
        public bool Payment { get; set; }

        public int KiosikID { get; set; }
     


    }
}
