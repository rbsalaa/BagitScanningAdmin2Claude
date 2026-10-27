using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SelfKOrderWeb.Areas.Identity.Data;
using SelfKOrderWeb.Authoraization;
using SelfKOrderWeb.Data;
using SelfKOrderWeb.DTO;
using SelfKOrderWeb.Factory;
using SelfKOrderWeb.Factory.IFactory;
using SelfKOrderWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using static SelfKOrderWeb.Factory.Enums;


namespace IdentityCore.Factory.RepositoryBannersDTO
{
   
        public class OrderRepo : IOrderRepo
        {

            SelfKOrderWebContext DBcontext;
        IAccessFilterService _accessFilter;
        public OrderRepo(SelfKOrderWebContext _DBcontext, IAccessFilterService accessFilter)
            {
            DBcontext = _DBcontext;
            _accessFilter = accessFilter;
            }

        public async Task<IEnumerable<KiosikHardwareLog>> GetHardwareStautsList(ClaimsPrincipal User)
        {

            IQueryable<KiosikHardwareLog> filtered2 = _accessFilter.HardwareFilter(User, DBcontext.KiosikHardwareLog);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }

        public async Task<IEnumerable<OrderSummaryDTO>> GetOrderList(ClaimsPrincipal User)
            {
                
            IQueryable<OrderSummaryDTO> filtered2 = _accessFilter.OrderFilter(User, DBcontext.Order);
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
            }
        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetails(ClaimsPrincipal User,int id)
        {

            IQueryable<OrderDetailDTO> filtered2 =  DBcontext.OrderDetail.Where(x=>x.OrderId==id).ToOrderDetailDTO();

           
            return await filtered2.ToListAsync(); // run the query here (async) instead of lazily inside the view
        }
      


    }
}
