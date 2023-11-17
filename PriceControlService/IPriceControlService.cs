namespace PriceControlService
{
    /* Interface for Price Control Service */
    public interface IPriceControlService
    {
        Task SetProductPrice(string productName, double productPrice);
        Task<double> GetProductPrice(string productName);
    }
}
