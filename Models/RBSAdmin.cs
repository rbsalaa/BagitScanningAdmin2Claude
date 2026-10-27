using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class RBSAdmin { 


            [System.ComponentModel.DataAnnotations.Key]
            public int RBSAdminID { get; set; }
            public string RBSAdminUserID { get; set; }

            public string RBSAdminMail { get; set; }

            public string RBSAdminPassword { get; set; }
            public bool IsActive { get; set; }
          


      
    }
}
