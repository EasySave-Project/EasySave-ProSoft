using EasySave.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void list_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = home;
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }
    }
}
