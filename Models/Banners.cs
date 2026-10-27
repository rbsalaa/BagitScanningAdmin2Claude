using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Banners
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int BannerID { get; set; }
        public string BannerTitle { get; set; }
        public DateTime BannerStartDate { get; set; }
        public string? BannerClickURL { get; set; } //  
        public DateTime BannerEndDate { get; set; }
        public string BannerURL { get; set; }
        public bool IsActive { get; set; }
        public string? TargetUrl { get; set; }
        public int? TargetID { get; set; }
        public string? TargetType { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class KiosikBanners
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int KiosikBannerID { get; set; }
        public int Banner { get; set; }

        public int KiosikID { get; set; }

        public bool IsActive { get; set; }


        [ForeignKey("KiosikID")]
        public virtual Kiosik Kiosik { get; set; }
        [ForeignKey("Banner")]
        public virtual Banners Banners { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
}
