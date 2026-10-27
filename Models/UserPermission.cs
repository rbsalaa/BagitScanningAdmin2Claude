using MessagePack;
using SelfKOrderWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class UserPermission
    {
       [System.ComponentModel.DataAnnotations.Key]
        public int PermissionId { get; set; }
        public string Id { get; set; }  // FK to ApplicationUser
        public int? BranchID { get; set; }
        public int? CompID { get; set; }
        public int? BrandID { get; set; }
        public int? FranchID { get; set; }

        public bool? IsRBSUser { get; set; }
        public bool? IsSuperMainAdmin { get; set; }
        [ForeignKey("Id")]
        public SelfKOrderWebUser User { get; set; }
    }

    public class DataModel
    {
        public int PermissionId { get; set; }
        public int CompID { get; set; }
        public int BrandID { get; set; }
        public int FranchID { get; set; }
        public string Description { get; set; }
    }
}
