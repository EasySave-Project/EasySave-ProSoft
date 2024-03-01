using EasySave.services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    public class Settings : INotifyPropertyChanged
    {
        private static string path = Environment.CurrentDirectory + "\\EasySave\\Setting";
        private static readonly string _filePath = path + "\\settings.json";
        private static string _logType;
        private static string _lang;
        private static string _stateType;
        private static int? _nbKo;
        private static List<string> _extensionsToCrypt = new List<string>();
        private static List<string> _extensionsToPriority = new List<string>();
        private static List<string> _businessApplication = new List<string>();

        private static Settings _instance;


        public static Settings Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Settings()
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
        public List<string> ExtensionsToPriority
        {
            get { return _extensionsToPriority ?? new List<string>(); }
            set
            {
                if (_extensionsToPriority != value)
                {
                    _extensionsToPriority = value;
                    OnPropertyChanged(nameof(ExtensionsToPriority));
                }
            }
        }
        public List<string> BusinessApplication
        {
            get { return _businessApplication ?? new List<string>(); }
            set
            {
                if (_businessApplication != value)
                {
                    _businessApplication = value;
                    OnPropertyChanged(nameof(BusinessApplication));
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

        public int NbKo
        {
            get { return _nbKo ?? -1; }
            set
            {
                if (_nbKo != value)
                {
                    _nbKo = value;
                    OnPropertyChanged(nameof(NbKo));
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
                _nbKo = settings?.NbKo;
                // Extensions to crypt
                if (settings?.ExtensionsToCrypt != null)
                {
                    _extensionsToCrypt = new List<string>(settings.ExtensionsToCrypt.ToObject<string[]>());
                }
                else
                {
                    _extensionsToCrypt = new List<string>();
                }
                // Extensions to Priority
                if (settings?.ExtensionsToPriority != null)
                {
                    _extensionsToPriority = new List<string>(settings.ExtensionsToPriority.ToObject<string[]>());
                }
                else
                {
                    _extensionsToPriority = new List<string>();
                }
                // Business application
                if (settings?.BusinessApplication != null)
                {
                    _businessApplication = new List<string>(settings.BusinessApplication.ToObject<string[]>());
                }
                else
                {
                    _businessApplication = new List<string>();
                }
            }
            else
            {
                _logType = "Json";
                _stateType = "Json";
                _lang = "fr";
                _nbKo = -1;
                _extensionsToCrypt = new List<string>();
                _extensionsToPriority = new List<string>();
                _businessApplication = new List<string>();
                var directoryPath = System.IO.Path.GetDirectoryName(_filePath);
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
                NbKo = _nbKo,
                ExtensionsToCrypt = _extensionsToCrypt.ToArray(),
                ExtensionsToPriority = _extensionsToPriority.ToArray(),
                BusinessApplication = _businessApplication.ToArray()
            };
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
