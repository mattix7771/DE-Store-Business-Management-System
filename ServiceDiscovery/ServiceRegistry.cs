namespace ServiceDiscovery
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers service to service registry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public void RegisterService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        /// <summary>
        /// Retrieves service from service registry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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