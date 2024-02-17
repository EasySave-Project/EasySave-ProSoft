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


namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour AddJob.xaml
    /// </summary>
    public partial class AddJob : Page
    {
        public AddJob()
        {
            InitializeComponent();
        }
        private void OnSelectFolderClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner un dossier",
                Filter = "Dossiers|*.none",
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Sélectionnez un dossier"
            };

            // Afficher la boîte de dialogue de sélection de dossier
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Récupérer le chemin du dossier sélectionné
                string selectedFolderPath = System.IO.Path.GetDirectoryName(dialog.FileName);

                // Afficher le chemin dans le TextBox
                TextBox_SelectedFolderA.Text = selectedFolderPath;
            }
        }
        private void OnSelectFolderClickB(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner un dossier",
                Filter = "Dossiers|*.none",
                CheckFileExists = false,
                CheckPathExists = true,

            };

            // Afficher la boîte de dialogue de sélection de dossier
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Récupérer le chemin du dossier sélectionné
                string selectedFolderPath = System.IO.Path.GetDirectoryName(dialog.FileName);

                // Afficher le chemin dans le TextBox
                TextBox_SelectedFolderB.Text = selectedFolderPath;
            }
        }

        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = home;
        }

        private void Btn_ListJob_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }

        private void Btn_AddJob_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }
    }
}
