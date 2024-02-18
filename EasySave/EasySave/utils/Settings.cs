using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave.utils
{
    public class Settings
    {
        private static string _logType;
        private static string _lang;
        private static string path = Environment.CurrentDirectory + "\\EasySave\\Setting";
        private static string _filePath = path + "\\settings.json"; 
        private static string _stateType;

        public Settings()
        {
            LoadSettings(); // Charge ou initialise les paramètres à partir du fichier JSON.
        }

        public string LogType
        {
            get { return _logType ?? "JSON"; }
            set { _logType = value; SaveSettings(); }
        }

        public string StateType
        {
            get { return _stateType ?? "JSON"; }
            set { _stateType = value; SaveSettings(); }
        }

        public string Lang
        {
            get { return _lang ?? "fr"; }
            set { _lang = value; SaveSettings(); }
        }

        public void LoadSettings()
        {
            //if (File.Exists(_filePath))
            //{
            //    var json = File.ReadAllText(_filePath);
            //    dynamic settings = JsonConvert.DeserializeObject(json);
            //    _logType = settings?.LogType;
            //    _stateType = settings?.StateType;
            //    _lang = settings?.Lang;
            //}
            //else
            //{
            //    _logType = "JSON";
            //    _stateType = "JSON";
            //    _lang = "fr";

            //    var directoryPath = Path.GetDirectoryName(_filePath);
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }
            //    SaveSettings(); // Crée le fichier JSON avec les valeurs par défaut
            //}
        }

        private void SaveSettings()
        {
            //var settings = new
            //{
            //    LogType = _logType,
            //    StateType = _stateType,
            //    Lang = _lang
            //};
            //var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            //File.WriteAllText(_filePath, json);
        }

    }
}
