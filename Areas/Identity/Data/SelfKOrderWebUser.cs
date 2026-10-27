using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SelfKOrderWeb.Models;

namespace SelfKOrderWeb.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SelfKOrderWebUser class
public class SelfKOrderWebUser : IdentityUser
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? CompID { get; set; }
    public int? BrandID { get; set; }
    public int? BranchID { get; set; }
    public int? FranchID { get; set; }
    public bool IsRBSUser { get; set; }
    public bool IsSuperMainAdmin { get; set; }

    [ForeignKey("BrandID")]
    public virtual Brand? Brands { get; set; }

    [ForeignKey("CompID")]
    public virtual Company? Companys { get; set; }
    [ForeignKey("FranchID")]
    public virtual Franchising? Franchis { get; set; }
    [ForeignKey("BranchID")]
    public virtual Branch? Branchs { get; set; }

    // public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}

public class ApplicationRole : IdentityRole
{
    public string? RoleName { get; set; }
    public int? CompID { get; set; }
    public int? BrandID { get; set; }
    public int? BranchID { get; set; }
    public int? FranchID { get; set; }
}
public class ApplicationUserRole : IdentityUserRole<string>
{
//  // public virtual SelfKOrderWebUser User { get; set; }
//  // public virtual ApplicationRole Role { get; set; }

   
   
}


