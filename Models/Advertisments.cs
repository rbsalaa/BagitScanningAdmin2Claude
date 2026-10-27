using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Advertisments
    { 


            [System.ComponentModel.DataAnnotations.Key]
            public int AdsID { get; set; }
             public string AdsTitle { get; set; }
            public DateTime AdsStartDate { get; set; }

            public DateTime AdsEndDate { get; set; }
            public string AdsURL { get; set; }
            public bool IsActive { get; set; }

        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

    public class KiosikAds
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int KiosikAdsID { get; set; }
        public int AdsID { get; set; }

        public int KiosikID { get; set; }

        public bool IsActive { get; set; }


        [ForeignKey("KiosikID")]
        public virtual Kiosik Kiosik { get; set; }
        [ForeignKey("AdsID")]
        public virtual Advertisments Advertisments { get; set; }
        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional

    }
}
