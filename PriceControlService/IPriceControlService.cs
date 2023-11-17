namespace PriceControlService
{
    /* Interface for Price Control Service */
    public interface IPriceControlService
    {
        Task SetProductPrice(string productName, double productPrice);
        double GetProductPrice(string productName);
    }
}
