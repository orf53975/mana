using mana.Foundation.Network.Sever;
using Newtonsoft.Json;

namespace mana.Server.Test
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string appSettingFile = "sev.setting.json";
        private static SevSetting _appSetting = null;
        internal static SevSetting AppSetting
        {
            get
            {
                if (_appSetting == null)
                {
                    _appSetting = ConfigUtil.LoadOrNewSave<SevSetting>(appSettingFile,
                        (str) => JsonConvert.DeserializeObject<SevSetting>(str),
                        (cfg) => JsonConvert.SerializeObject(cfg, Formatting.Indented));
                }
                return _appSetting;
            }
        }
        #endregion
    }
}
