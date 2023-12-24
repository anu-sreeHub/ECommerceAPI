namespace ECommerce.ViewModel;

public class ReturnVM
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = default!;
    public object ReturnValue { get; set; } = default!;
}
