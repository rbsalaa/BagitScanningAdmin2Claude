using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Models;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IOrderRepo
    {
        public Task<IEnumerable<OrderSummaryDTO>> GetOrderList(ClaimsPrincipal User);
        public Task<IEnumerable<KiosikHardwareLog>> GetHardwareStautsList(ClaimsPrincipal User);
    }
}
