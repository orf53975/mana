using mana.Foundation.Network.Sever;
using Newtonsoft.Json;

namespace mana.Server.Test
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string appSettingFile = "sev.setting.json";
        private static ServerSetting _appSetting = null;
        internal static ServerSetting AppSetting
        {
            get
            {
                if (_appSetting == null)
                {
                    _appSetting = ConfigUtil.LoadOrNewSave<ServerSetting>(appSettingFile,
                        (str) => JsonConvert.DeserializeObject<ServerSetting>(str),
                        (cfg) => JsonConvert.SerializeObject(cfg, Formatting.Indented));
                }
                return _appSetting;
            }
        }
        #endregion
    }
}
