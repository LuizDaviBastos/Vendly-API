using ASM.Services.Interfaces;
using ASM.Services.Models;
using Google.Cloud.Firestore;
using MongoDB.Driver;

namespace ASM.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly FirestoreDb fireStore;

        public SettingsService(FirestoreDb firebaseClient)
        {
            this.fireStore = firebaseClient;
        }

        public async Task<AsmAppSettings> GetAppSettings()
        {
            AsmAppSettings response = new();
            var querySnapshot = await fireStore.Collection("settings").GetSnapshotAsync();
            var snapshot = querySnapshot.FirstOrDefault();
            if(snapshot != null) 
            {
                response = snapshot.ConvertTo<AsmAppSettings>();
            }

            return response;
        }

    }
}
