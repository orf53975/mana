using mana.Foundation;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace mana.Server.Game
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string appSettingFile = "appsetting.json";
        private static AppSetting _appSetting = null;
        internal static AppSetting AppSetting
        {
            get
            {
                if (_appSetting == null)
                {
                    try
                    {
                        var filePath = Utils.AdjustFilePath(appSettingFile);
                        if (!File.Exists(filePath))
                        {
                            var aps = new AppSetting();
                            var str = JsonConvert.SerializeObject(aps, Formatting.Indented);
                            File.WriteAllText(filePath, str, Encoding.UTF8);
                            _appSetting = aps;
                        }
                        else
                        {
                            var str = File.ReadAllText(filePath);
                            _appSetting = JsonConvert.DeserializeObject<AppSetting>(str);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.Message);
                    }
                }
                return _appSetting;
            }
        }
        #endregion
    }
}
