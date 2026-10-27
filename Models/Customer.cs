using System.ComponentModel.DataAnnotations.Schema;

namespace SelfKOrderWeb.Models
{
    public class Customer
    { 


            [System.ComponentModel.DataAnnotations.Key]
            public int CustomerID { get; set; }
             public string CustomerName { get; set; }
            public string CustomerLocation { get; set; }
            public bool IsSupscriped { get; set; }
            public string CustomerMail { get; set; }
            public string CustomerPhone { get; set; }
            public bool IsActive { get; set; }
        //    public string? SupscrubtionID { get; set; }
        //[ForeignKey("SupscrubtionID")]
        //public virtual SubscrubtionInfo? SubscrubtionInfo { get; set; }

    }
}
