using mana.Foundation.Network.Server;
using Newtonsoft.Json;

namespace mana.Server.Game
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string severSettingFile = "sev.setting.json";
        private static AppSetting _serverSetting = null;
        internal static AppSetting ServerSetting
        {
            get
            {
                if (_serverSetting == null)
                {
                    _serverSetting = ConfigUtil.LoadOrNewSave<AppSetting>(severSettingFile,
                        (str) => JsonConvert.DeserializeObject<AppSetting>(str),
                        (cfg) => JsonConvert.SerializeObject(cfg, Formatting.Indented));
                }
                return _serverSetting;
            }
        }
        #endregion
    }
}
