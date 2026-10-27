using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Company
    { 


            [System.ComponentModel.DataAnnotations.Key]
            public int CompID { get; set; }
            public string CompName { get; set; }
         
            public string CompCode { get; set; }
            public bool IsActive { get; set; }

        public virtual ICollection< Brand> Brand { get; set; }


    }

    public class Brand
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandPic { get; set; }

        public string BrandCode { get; set; }
        public bool IsActive { get; set; }
        public int CompID { get; set; }



        [ForeignKey("CompID")]
        public virtual Company Companys { get; set; }

    }

    public class Branch
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }

        public string BrandCode { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public int? FranchID { get; set; }
        public bool IsActive { get; set; }
     

        [ForeignKey("BrandID")]
        public virtual Brand Brands { get; set; }
        
        [ForeignKey("FranchID")]
        public virtual Franchising? Franchis { get; set; }
    }

    public class Franchising
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int FranchID { get; set; }
        public string FranchName { get; set; }
        public string FranchLocation { get; set; }

        public string FranchCode { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("BrandID")]
        public virtual Brand Brands { get; set; }

        [ForeignKey("CompID")]
        public virtual Company Companys { get; set; }

    }
}
