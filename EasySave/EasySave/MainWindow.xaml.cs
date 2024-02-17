
using EasySave.controller;
using EasySave.services;
using EasySave.utils;
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

namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SettingManager.ReadAllSettings();
        
            InitializeComponent();

            // Instanciation
            BackUpManager bmManager = new BackUpManager();
            StateManager stateManager = new StateManager();
            LogManager logManager = new LogManager();
            BackUpController controller = new BackUpController(bmManager, logManager, stateManager);

            view.Home home = new view.Home();
            // Affichez la première page au démarrage de l'application
            Content = home;
        }

        private void initSettings(object sender, EventArgs e)
        {
            
        }
    }
}