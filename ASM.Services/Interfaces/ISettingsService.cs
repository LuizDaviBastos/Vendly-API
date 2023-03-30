using ASM.Services.Models;

namespace ASM.Services.Interfaces
{
    public interface ISettingsService
    {
        public AsmAppSettings GetAppSettings();
        public AsmAppSettings SetAppSettings(AsmAppSettings settings);
    }
}
