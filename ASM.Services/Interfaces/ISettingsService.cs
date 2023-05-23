using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface ISettingsService
    {
        public Task<AsmAppSettings> GetAppSettings();
    }
}
