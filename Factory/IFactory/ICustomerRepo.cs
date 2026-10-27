using SelfKOrderWeb.DTO;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface ICustomerRepo
    {
        public Task<IEnumerable<CustomerDTO>> GetCustomerList( );
        public Task<CustomerDTO> GetCustomer(  int id);
        public int DeleteCustomer(  int id);
        public int EditCustomer(  CustomerDTO obj);
        public int SaveCustomer(  CustomerDTO obj);
    }
}
