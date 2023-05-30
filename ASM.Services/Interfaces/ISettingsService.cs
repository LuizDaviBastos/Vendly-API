using ASM.Services.Models.Settings;

namespace ASM.Services.Interfaces
{
    public interface ISettingsService
    {
        public Task<AsmAppSettings> GetAppSettingsAsync();
    }
}
