using Microsoft.AspNetCore.Mvc.Rendering;

namespace SelfKOrderWeb.DTO
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLocation { get; set; }
        public bool IsSupscriped { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerPhone { get; set; }
        public bool IsActive { get; set; }
        public string? SupscrubtionID { get; set; }
        public string? SupscrubtionTitle { get; set; }
        public string? SupscrubtionDescription { get; set; }
        public DateTime? SupscrubtionStartDate { get;  set; }
        public DateTime? SupscrubtionEndDate { get;  set; }
        public bool? SubIsCurrentSupscriped { get;  set; }
        public int? SubKiosikCount { get;  set; }
        public List<SelectListItem>? SupscriptionList { get; set; }
             public List<KiosikDTO>? KiossiksList { get; set; }
    }

    public class SubscrubtionInfoDTO
    {



        public string SupscrubtionID { get; set; }

        public string? carrierString { get; set; }
        
        public bool IsCurrentSupscriped { get; set; }
        public int? KiosikCount { get; set; }
        public int? CompID { get; set; }
        public bool IsActive { get; set; }
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public string? CompanyName { get; set; }
        public string SubscriptionTitle { get; set; }
        public string SubscriptionDescription { get; set; }
        public int OldCompId { get; set; }
        public int OldBrndId { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public int? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public List<SelectListItem>? CustomerList { get; set; }
    }

    public class SubscrubtionInfoDetailsDTO
    {



        public string SupscrubtionDetailID { get; set; }
        public string SupscrubtionID { get; set; }
        public string carrierString { get; set; }
        public string Key { get; set; }
        public DateTime SupscrubtionStartDate { get; set; }
        public DateTime SupscrubtionEndDate { get; set; }
        
             public bool ActivationStauts { get; set; }
        public int CompID { get; set; }
        public string? BrandName { get; set; }
        public string? BranchName { get; set; }
        public string? KiosikName { get; set; }
        public int BrandID { get; set; }
        public int BranchID { get; set; }
        public int KiosikID { get; set; }
        public bool IsActive { get; set; }
     
        public string? CompanyName { get; set; }
        public string SubscriptionTitle { get; set; }
        public string SubscriptionDescription { get; set; }
        public int OldCompId { get; set; }
        public int OldBrndId { get; set; }
        public List<SelectListItem>? CompList { get; set; }
        public List<SelectListItem>? BranchList { get; set; }
        public List<SelectListItem>? KiosikList { get; set; }
        public List<SelectListItem>? BrandList { get; set; }
        public List<SubscrubtionInfoDetailsDTO>? LisecsesList { get; set; }
    }

}
    
