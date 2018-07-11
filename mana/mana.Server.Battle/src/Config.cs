using mana.Foundation.Network.Server;
using Newtonsoft.Json;

namespace mana.Server.Battle
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string sevSettingFile = "sev.setting.json";
        private static CustomSevSetting _sevSetting = null;
        internal static CustomSevSetting SevSetting
        {
            get
            {
                if (_sevSetting == null)
                {
                    _sevSetting = ConfigUtil.LoadOrNewSave<CustomSevSetting>(sevSettingFile,
                        (str) => JsonConvert.DeserializeObject<CustomSevSetting>(str),
                        (cfg) => JsonConvert.SerializeObject(cfg, Formatting.Indented));
                }
                return _sevSetting;
            }
        }
        #endregion
    }
}
