namespace ECommerce.ViewModel;

public class CustomerOrderVM
{
    public CustomerVM Customer { get; set; } = default!;
    public OrderVM? Order { get; set; } = default!;
}
