using mana.Foundation.Network.Sever;
using Newtonsoft.Json;

namespace mana.Server.Battle
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string sevSettingFile = "sev.setting.json";
        private static ServerSetting _sevSetting = null;
        internal static ServerSetting SevSetting
        {
            get
            {
                if (_sevSetting == null)
                {
                    _sevSetting = ConfigUtil.LoadOrNewSave<ServerSetting>(sevSettingFile,
                        (str) => JsonConvert.DeserializeObject<ServerSetting>(str),
                        (cfg) => JsonConvert.SerializeObject(cfg, Formatting.Indented));
                }
                return _sevSetting;
            }
        }
        #endregion
    }
}
