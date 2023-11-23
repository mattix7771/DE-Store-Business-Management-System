using DataAccessLayer;
using PriceControlService;
using SharedModels;
using ServiceDiscovery;
using InventoryControlService;
using LoyaltyCardService;
using PurchaseManagementService;
using UserManagementService;
using ReportAndAnalysisService;

namespace ServiceManagement
{
    public class ServiceManagement
    {
        public async Task ServiceInitialisation(ServiceRegistry serviceRegistry)
        {

        // Database Initialisation
        var db = new DataAccessLayer.Database();
        db.DatabaseInitialisation();

        // Initialisation and registration of services
        // Registration of price control service and its actions
        var priceControlServiceImplementation = new PriceControlService.PriceControlService(serviceRegistry);
        serviceRegistry.RegisterService<IPriceControlService>(priceControlServiceImplementation);
            serviceRegistry.RegisterService<IPriceControl>(db);

            // Registration of inventory control service and its actions
            var inventoryControlServiceImplementation = new InventoryControlService.InventoryControlService(serviceRegistry);
        serviceRegistry.RegisterService<IInventoryControlService>(inventoryControlServiceImplementation);
            serviceRegistry.RegisterService<IInventoryControl>(db);

            // Registration of loyalty card service and its actions
            var loyaltyCardServiceImplementation = new LoyaltyCardService.LoyaltyCardService(serviceRegistry);
        serviceRegistry.RegisterService<ILoyaltyCardService>(loyaltyCardServiceImplementation);
            serviceRegistry.RegisterService<ILoyaltyCard>(db);

            // Registration of purchase management service and its actions
            var purchaseManagementServiceImplementation = new PurchaseManagementService.PurchaseManagementService(serviceRegistry);
        serviceRegistry.RegisterService<IPurchaseManagementService>(purchaseManagementServiceImplementation);
            serviceRegistry.RegisterService<IPurchaseManagement>(db);

            // Registration of user management service and its actions
            var userManagementServiceImplementation = new UserManagementService.UserManagementService(serviceRegistry);
        serviceRegistry.RegisterService<IUserManagementService>(userManagementServiceImplementation);
            serviceRegistry.RegisterService<IUserManagement>(db);

            // Registration of report and analysis service and its actions
            var reportAndAnalysisServiceImplementation = new ReportAndAnalysisService.ReportAndAnalysisService(serviceRegistry);
        serviceRegistry.RegisterService<IReportAndAnalysisService>(reportAndAnalysisServiceImplementation);
            serviceRegistry.RegisterService<IReportAndAnalysis>(db);

            

            // Now we can retrieve all the services needed from the service registry
        //    var priceControlService = serviceRegistry.GetService<IPriceControlService>();
        //var inventoryControlService = serviceRegistry.GetService<IInventoryControlService>();
        //var loyaltyCardService = serviceRegistry.GetService<ILoyaltyCardService>();
        //var purchaseManagementService = serviceRegistry.GetService<IPurchaseManagementService>();
        //var userManagementService = serviceRegistry.GetService<IUserManagementService>();
        //var reportAndAnalysisService = serviceRegistry.GetService<IReportAndAnalysisService>();
        }
    }
}