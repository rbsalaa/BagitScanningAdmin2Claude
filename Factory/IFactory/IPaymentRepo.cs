using SelfKOrderWeb.DTO;
using System.Security.Claims;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface IPaymentMethodRepo
    {
        public Task<IEnumerable<PaymentMethodDTO>> GetPaymentMethodList(ClaimsPrincipal User);
        public Task<PaymentMethodDTO> GetPaymentMethod(  int id);
        public int DeletePaymentMethod(  int id);
        public int EditPaymentMethod(PaymentMethodDTO obj);
        public int SavePaymentMethod(PaymentMethodDTO obj);
    }
    public interface ITAXGroupRepo
    {
        public Task<IEnumerable<TAXGroupDTO>> GetTAXGroupList(ClaimsPrincipal User);
        public Task<TAXGroupDTO> GetTAXGroup(int id);
        public int DeleteTAXGroup(int id);
        public int EditTAXGroup(TAXGroupDTO obj);
        public int SaveTAXGroup(TAXGroupDTO obj);
    }
    public interface IDiscountsRepo
    {
        public Task<IEnumerable<DiscountsDTO>> GetDiscountsList(ClaimsPrincipal User);
        public Task<DiscountsDTO> GetDiscounts(int id);
        public int DeleteDiscounts(int id);
        public int EditDiscounts(DiscountsDTO obj);
        public int SaveDiscounts(DiscountsDTO obj);
    }
    public interface IShopDiscountsRepo
    {
        public Task<IEnumerable<StoreMenuDiscountsDTO>> GetDiscountsList(ClaimsPrincipal User);
        public Task<StoreMenuDiscountsDTO> GetDiscounts(int id);
        public int DeleteDiscounts(int id);
        public int EditDiscounts(StoreMenuDiscountsDTO obj);
        public int SaveDiscounts(StoreMenuDiscountsDTO obj);
    }
}
