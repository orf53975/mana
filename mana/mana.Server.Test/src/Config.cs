using mana.Foundation;
using mana.Foundation.Network.Sever;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace mana.Server.Test
{
    internal static class Config
    {
        #region <<AppSetting>>
        private const string appSettingFile = "appsetting.json";
        private static ServerSetting _appSetting = null;
        internal static ServerSetting AppSetting
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
                            var aps = new ServerSetting();
                            var str = JsonConvert.SerializeObject(aps, Formatting.Indented);
                            File.WriteAllText(filePath, str, Encoding.UTF8);
                            _appSetting = aps;
                        }
                        else
                        {
                            var str = File.ReadAllText(filePath);
                            _appSetting = JsonConvert.DeserializeObject<ServerSetting>(str);
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
