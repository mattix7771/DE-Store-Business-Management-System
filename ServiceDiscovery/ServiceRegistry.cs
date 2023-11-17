namespace ServiceDiscovery
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void RegisterService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public T GetService<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            throw new InvalidOperationException($"Service of type {typeof(T)} not registered");
        }
    }
}