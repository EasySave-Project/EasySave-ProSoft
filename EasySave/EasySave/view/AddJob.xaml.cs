using EasySave.model;
using EasySave.services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;



namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour AddJob.xaml
    /// </summary>
    public partial class AddJob : Page
    {
        private MainWindow _MainWindows = new MainWindow();
        public AddJob()
        {
            InitializeComponent();
        }

        //==============================================
        // Code to open folders
        //==============================================
        private void OnSelectFolderClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            
            dialog.FileName = "Sélection de dossier";
            
            DialogResult result = dialog.ShowDialog();
            
            if (result == System.Windows.Forms.DialogResult.OK)
            {
            
                string path = dialog.FileName;
            
                if (path.EndsWith("Sélection de dossier"))
                {
            
                    path = path.Replace("\\Sélection de dossier", "");
                }
            
                TextBox_SelectedFolderA.Text = path;
            }
        }
        private void OnSelectFolderClickB(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
           
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
           
            dialog.FileName = "Sélection de dossier";
           
            DialogResult result = dialog.ShowDialog();
           
            if (result == System.Windows.Forms.DialogResult.OK)
            {
               
                string path = dialog.FileName;
               
                if (path.EndsWith("Sélection de dossier"))
                {
               
                    path = path.Replace("\\Sélection de dossier", "");
                }
               
                TextBox_SelectedFolderB.Text = path;
            }
        }

        //==============================================
        // Add a job
        //==============================================
        private void Btn_AddJob_Click(object sender, RoutedEventArgs e)
        {
            string sNameJob = TextBox_NameJob.Text;
            string sSourceFolder = TextBox_SelectedFolderA.Text;
            string sDestinationFolder = TextBox_SelectedFolderB.Text;
            ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_TypeSave.SelectedItem;
            string sType = selectedItem.Content.ToString();

            // Gestion des erreurs
            if (string.IsNullOrWhiteSpace(sNameJob) || sNameJob.Contains(" ") || sNameJob.Contains(";"))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneValidJob"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if (string.IsNullOrEmpty(sSourceFolder) || !System.IO.Directory.Exists(sSourceFolder))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneSourcePath"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if (string.IsNullOrEmpty(sDestinationFolder) || !System.IO.Directory.Exists(sDestinationFolder))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneDestPath"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if (string.IsNullOrEmpty(sType))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneCodeBackup"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        
            BackUpType type;
            if (sType == "Complète")
            {
                type = BackUpType.Complete;
                _MainWindows.backUpController.InitiateAddJob(type, sNameJob, sSourceFolder, sDestinationFolder);
            }
            else
            {
                type = BackUpType.Differential;
                _MainWindows.backUpController.InitiateAddJob(type, sNameJob, sSourceFolder, sDestinationFolder);
            }

            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }

        //==============================================
        // Navigation button
        //==============================================

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
        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
