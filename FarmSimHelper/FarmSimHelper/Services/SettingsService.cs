using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public static class SettingsService
    {
        private static string SettingsFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "settings.xml");
        
        public static void SaveSettings(Settings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter(SettingsFilePath);

            serializer.Serialize(writer, settings);

            Console.WriteLine($"Saved settings to {SettingsFilePath}");
            writer.Close();
        }

        public static Settings LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            FileStream fs = new FileStream(SettingsFilePath, FileMode.Open, FileAccess.Read);

            Settings settings = (Settings)serializer.Deserialize(fs);

            fs.Close();

            return settings;
        }

        public static bool SettingsExist()
        {
            return File.Exists(SettingsFilePath);
        }
    }
}
