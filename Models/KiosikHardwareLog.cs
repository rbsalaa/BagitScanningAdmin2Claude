using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class KiosikHardwareLog
    {


        [System.ComponentModel.DataAnnotations.Key]
        public int KiosikHardwareID { get; set; }
        public string HardWareName { get; set; }
        public DateTime? LastUpateDate { get; set; }

        public string? KiosikName { get; set; }
        public string HardWareCode { get; set; }
        public bool Stauts { get; set; }
        public int KiosikID { get; set; }
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsDeleted { get; set; } = false;// Optional
        public DateTime? LastModified { get; set; } // Optional
    }

 
}
