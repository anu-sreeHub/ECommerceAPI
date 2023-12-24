namespace ECommerce.Interface
{
    public interface IDatabaseService
    {
        string ConnectionString { get; }

        public void ConnectToDatabase();
    }
}
