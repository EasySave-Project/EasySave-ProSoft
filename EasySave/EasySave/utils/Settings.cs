using EasySave.services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.utils
{
    public class Settings : INotifyPropertyChanged
    {
        private static string _logType;
        private static string _lang;
        private static string path = Environment.CurrentDirectory + "\\EasySave\\Setting";
        private static readonly string _filePath = path + "\\settings.json"; 
        private static string _stateType;
        private static List<string> _extensionsToCrypt = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Settings()
        {
            LoadSettings(); // Charge ou initialise les paramètres à partir du fichier JSON.
        }

        public string LogType
        {
            get { return _logType ?? "Json"; }
            set
            {
                if (_logType != value)
                {
                    _logType = value;
                    OnPropertyChanged(nameof(LogType));
                }
            }
        }
        public List<string> ExtensionsToCrypt
        {
            get { return _extensionsToCrypt ?? new List<string>(); }
            set
            {
                if (_extensionsToCrypt != value)
                {
                    _extensionsToCrypt = value;
                    OnPropertyChanged(nameof(ExtensionsToCrypt));
                }
            }
        }
        public string StateType
        {
            get { return _stateType ?? "Json"; }
            set
            {
                if (_stateType != value)
                {
                    _stateType = value;
                    OnPropertyChanged(nameof(StateType));
                }
            }
        }

        public string Lang
        {
            get { return _lang ?? "fr"; }
            set
            {
                if (_lang != value)
                {
                    _lang = value;
                    OnPropertyChanged(nameof(Lang));
                    ManageLang.ChangeLanguage(_lang);
                }
            }
        }

        

        public void LoadSettings()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                dynamic settings = JsonConvert.DeserializeObject(json);
                _logType = settings?.LogType;
                _stateType = settings?.StateType;
                _lang = settings?.Lang;
                if (settings?.ExtensionsToCrypt != null)
                {
                    _extensionsToCrypt = new List<string>(settings.ExtensionsToCrypt.ToObject<string[]>());
                }
                else
                {
                    _extensionsToCrypt = new List<string>();
                }
            }
            else
            {
                _logType = "Json";
                _stateType = "Json";
                _lang = "fr";
                _extensionsToCrypt = new List<string>();
                var directoryPath = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                SaveSettings(); // Crée le fichier JSON avec les valeurs par défaut
            }
        }

        public void SaveSettings()
        {
            var settings = new
            {
                LogType = _logType,
                StateType = _stateType,
                Lang = _lang,
                ExtensionsToCrypt = _extensionsToCrypt.ToArray(),
            };
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
