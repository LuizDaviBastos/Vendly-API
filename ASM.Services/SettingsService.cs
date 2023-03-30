using ASM.Services.Interfaces;
using ASM.Services.Models;
using MongoDB.Driver;

namespace ASM.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IMongoCollection<AsmAppSettings> collection;

        public SettingsService(IMongoDatabase mongoDatabase)
        {
            collection = mongoDatabase.GetCollection<AsmAppSettings>("AppSettings");
        }

        public AsmAppSettings GetAppSettings()
        {
            return collection.Find(x => true).First();
        }

        public AsmAppSettings SetAppSettings(AsmAppSettings settings)
        {
            collection.ReplaceOne(x => x.Id == settings.Id, settings);
            return settings;
        }
    }
}
