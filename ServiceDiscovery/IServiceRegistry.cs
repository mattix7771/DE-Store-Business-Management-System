namespace ServiceDiscovery
{
    public interface IServiceRegistry
    {
        void RegisterService<T>(T service);
        T GetService<T>();
    }
}
