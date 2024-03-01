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

            Settings s = Settings.Instance;
             
            ManageLang.ChangeLanguage(s.Lang);

            // Instanciation
            BackUpManager bmManager = BackUpManager.Instance;
            StateManager stateManager = new StateManager();
            LogManager logManager = new LogManager();
            BackUpController controller = new BackUpController(bmManager, logManager, stateManager);
            backUpController = controller;

            // Server startup
            ServerManager serverManager = new ServerManager();
            serverManager.StartServer();

            view.Home home = new view.Home();
            // Display first page at application startup
            Content = home;
        }

        private void initSettings(object sender, EventArgs e)
        {
            
        }

        private void InitConfFolder()
        {
            // Save Job section
            // Check for the presence of the "conf" folder
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\conf";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            // Check that the file "SaveBackUpJob.json" is present, then write nothing in it
            string filePath = destPath + "\\SaveBackUpJob.json";
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
            }
            // Log section
            // Check the presence of the "log" folder
            destPath = sCurrentDir + "\\EasySave\\log";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
        }

    }
}