using ECommerce.ViewModel;

namespace ECommerce.Interface
{
    public interface IOrderService
    {
        public ReturnVM GetRecentCustomerOrder(string customerId, string email);
    }
}
