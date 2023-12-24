namespace ECommerce.ViewModel;

public class OrderVM
{
    public int OrderNo { get; set; } = default!;
    public DateTime OrderDate { get; set; } = default!;
    public string DeliveryAddress { get; set; } = default!;
    public List<OrderItemsVM>? OrderItems { get; set; }
    public DateTime DeliveryExpected { get; set; } = default!;
}

public class OrderItemsVM
{
    public string Product { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public decimal PriceEach { get; set; } = default!;
}
