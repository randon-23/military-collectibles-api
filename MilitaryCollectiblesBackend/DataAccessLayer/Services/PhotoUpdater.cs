namespace MilitaryCollectiblesBackend.DataAccessLayer.Services
{
    public interface IPhotoUpdater
    {
        Task UpdatePhotoUrlAsync(string entityType, int entityId, string photoUrl);
    }
    public class PhotoUpdater : IPhotoUpdater
    {
        private readonly IServiceProvider _serviceProvider;
        public PhotoUpdater(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task UpdatePhotoUrlAsync(string entityType, int entityId, string photoUrl)
        {
            switch (entityType.ToLower())
            {
                case "literature":
                    var literatureDataAccess = _serviceProvider.GetRequiredService<ILiteraturesDataAccess>();
                    await literatureDataAccess.UpdatePhotoUrl(entityId, photoUrl);
                    break;
                case "artifact":
                    var artifactDataAccess = _serviceProvider.GetRequiredService<IArtifactsDataAccess>();
                    await artifactDataAccess.UpdatePhotoUrl(entityId, photoUrl);
                    break;
                case "insignia":
                    var insigniaDataAccess = _serviceProvider.GetRequiredService<IInsigniasDataAccess>();
                    await insigniaDataAccess.UpdatePhotoUrl(entityId, photoUrl);
                    break;
                case "equipment":
                    var equipmentDataAccess = _serviceProvider.GetRequiredService<IEquipmentDataAccess>();
                    await equipmentDataAccess.UpdatePhotoUrl(entityId, photoUrl);
                    break;
                case "mechanicalequipment":
                    var mechanicalEquipmentDataAccess = _serviceProvider.GetRequiredService<IMechanicalEquipmentDataAccess>();
                    await mechanicalEquipmentDataAccess.UpdatePhotoUrl(entityId, photoUrl);
                    break;
            }
        }
    }
}
