using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface ICompanyRepo
    {
        public Task<IEnumerable<CompanyDTO>> GetCompanyList(ClaimsPrincipal _User);
        public Task<CompanyDTO> GetCompany( int id);
        public int DeleteCompany( int id);
        public int EditCompany( CompanyDTO obj);
        public int SaveCompany( CompanyDTO obj);
    }
}
