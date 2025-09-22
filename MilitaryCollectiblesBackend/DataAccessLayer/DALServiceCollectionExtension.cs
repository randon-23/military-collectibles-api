using Microsoft.Extensions.ServiceDiscovery;
using Microsoft.Identity.Client;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public static class DALServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<ILiteraturesDataAccess, LiteraturesDataAccess>();
            services.AddScoped<IArtifactsDataAccess, ArtifactsDataAccess>();
            services.AddScoped<IInsigniasDataAccess, InsigniasDataAccess>();
            services.AddScoped<IEquipmentDataAccess, EquipmentDataAccess>();
            services.AddScoped<IMechanicalEquipmentDataAccess, MechanicalEquipmentDataAccess>();
            services.AddScoped<ILiteratureSeriesDataAccess, LiteratureSeriesDataAccess>();
            services.AddScoped<IInsigniaSeriesDataAccess, InsigniaSeriesDataAccess>();
            services.AddScoped<IArtifactSeriesDataAccess, ArtifactSeriesDataAccess>();
            services.AddScoped<IStorageAreaDataAccess, StorageAreaDataAccess>();
            services.AddScoped<Services.IPhotoUpdater, Services.PhotoUpdater>();
            return services;
        }
    }
}
