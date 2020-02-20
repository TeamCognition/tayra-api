using Tayra.Common;

namespace Tayra.Services
{
    public class ProfileNotificationSettingsDTO
    {
        public SettingDTO[] Settings { get; set; }

        public class SettingDTO
        {
            public LogEvents LogEvent { get; set; }
            public bool IsEnabled { get; set; }
        }

    }
}
