using EasySave.controller;
using EasySave.model;
using EasySave.services;
using EasySave.utils;
using System.Configuration;
using System.IO;
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

namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BackUpController backUpController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitConfFolder();
            this.WindowStyle = WindowStyle.None;
            this.MouseLeftButtonDown += delegate { this.DragMove(); };

            Settings s = new Settings();
             
            ManageLang.ChangeLanguage(s.Lang);

            // Instanciation
            BackUpManager bmManager = new BackUpManager();
            StateManager stateManager = new StateManager();
            LogManager logManager = new LogManager();
            BackUpController controller = new BackUpController(bmManager, logManager, stateManager);
            backUpController = controller;

            // Démarrage du serveur
            ServerManager serverManager = new ServerManager();
            serverManager.StartServer();

            view.Home home = new view.Home();
            // Affichez la première page au démarrage de l'application
            Content = home;
        }

        private void initSettings(object sender, EventArgs e)
        {
            
        }

        private void InitConfFolder()
        {
            // Partie Save Job
            // Vérifier la présence du dossier "conf"
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\conf";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            // Vérifier la présence du fichier "SaveBackUpJob.json" puis écrire rien dedans
            string filePath = destPath + "\\SaveBackUpJob.json";
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
            }
            // Partie Log
            // Vérifier la présence du dossier "log"
            destPath = sCurrentDir + "\\EasySave\\log";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
        }

    }
}