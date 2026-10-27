using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IBrandRepo
    {
        public Task<IEnumerable<BrandDTO>> GetBrandList(ClaimsPrincipal User);
        public Task<BrandDTO> GetBrand(  int id);
        public int DeleteBrand(  int id);
        public int EditBrand(  BrandDTO obj);
        public int SaveBrand(  BrandDTO obj);
    }
}
