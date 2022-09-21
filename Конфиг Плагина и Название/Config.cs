using Resto.Front.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RNCB_Plugin
{


    public sealed class Config
    {

        #region Допы
        private const string ConfigFileName = Plugin.Name + "_Config.xml";
        private static string FilePath
        {
            get { return Path.GetFullPath(Path.Combine(PluginContext.Integration.GetConfigsDirectoryPath(), ConfigFileName)); }
        }
        private static Config instance;
        #endregion
        /// <summary>
        /// Перечисляем параметры и значения по умолчанию, если конфиг уже был создан, он перезапишется,
        /// если параметра нет то используется значение по умолчанию
        /// </summary>
       
        public string CertName { get; set; }
        public string CertPass { get; set; }
        public string rncbURL { get; set; }
        public string TerminalID { get; set; }
        public string PayTypeName { get; set; }
        public string qrsize { get; set; }
        public bool ShowOkPopupAboutPay { get; set; }
        public bool print_on_receipt_printer { get; set; }
        public bool print_on_precheque { get; set; }
        public bool FullLogs { get; set; }

        public Config() // Значения по умолчанию Если нет конфига
        {
            CertName = @"C:\certificate\client_keystore.p12";
            CertPass = "#rPUxWCK9)";
            rncbURL = "https://qr.rncb.ru:10443";
            TerminalID = "45698723";
            qrsize = "small";
            print_on_precheque = true;
            print_on_receipt_printer = false;
            PayTypeName = "РНКБ банк";
            ShowOkPopupAboutPay = false;
            FullLogs = true;
        }

        public static Config Instance
        {
            get
            {
                return instance ?? (instance = Load());
            }
        }
        private static Config Load()
        {
            var config = new Config();
            try
            {
                PluginContext.Log.InfoFormat("Загрузка настроек плагина из {0}", FilePath);
                using (var stream = new FileStream(FilePath, FileMode.Open))

                using (var reader = new StreamReader(stream))
                {
                    config = (Config)new XmlSerializer(typeof(Config)).Deserialize(reader);
                }
                config.Save();
                return config;
            }
            catch (Exception e)
            {
                PluginContext.Log.Error("Не удалось загрузить настройки плагина. Используются настройки по умолчанию.");
            }
            config.Save();
            return config;
        }
        private void Save()
        {
            try
            {
                PluginContext.Log.InfoFormat("Сохранение настроек плагина в {0}", FilePath);
                using (Stream stream = new FileStream(FilePath, FileMode.Create))
                {
                    new XmlSerializer(typeof(Config)).Serialize(stream, this);
                }
            }
            catch (Exception e)
            {
                PluginContext.Log.Error("Ошибка сохранение настроек плагина: ", e);
            }
        }

    }

}
