using SelfKOrderWeb.Areas.Identity.Data;

namespace SelfKOrderWeb.DTO
{
    public class CarrierDTO
    {
        public AddUserDTO obj { get; set; }= new AddUserDTO();
        public  IEnumerable< SelfKOrderWebUser> UsersList { get; set; }=new List< SelfKOrderWebUser>();
        public IEnumerable<AdvertismentsDTO> AdsList { get; set; } = new List<AdvertismentsDTO>();
        public IEnumerable<BannersDTO> BannersList { get; set; } = new List<BannersDTO>();
        public string RoleID { get; set; }
        public int KiosikID { get; set; }
    }
}
