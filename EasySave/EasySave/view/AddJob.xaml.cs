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
        // Code pour ouvrir les folders
        //==============================================
        private void OnSelectFolderClick(object sender, RoutedEventArgs e)
        {
            // Créer un dialogue d'ouverture de fichier
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            // Désactiver la validation du nom et de l'existence du fichier
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            // Définir le nom du fichier par défaut comme "Sélection de dossier"
            dialog.FileName = "Sélection de dossier";
            // Afficher le dialogue et obtenir le résultat
            DialogResult result = dialog.ShowDialog();
            // Si le résultat est OK
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Obtenir le chemin du fichier ou du dossier
                string path = dialog.FileName;
                // Si le nom du fichier est "Sélection de dossier", ça veut dire qu'on sélectionne un dossier
                if (path.EndsWith("Sélection de dossier"))
                {
                    // Supprimer le nom du fichier du chemin
                    path = path.Replace("\\Sélection de dossier", "");
                }
                // Stocker le chemin
                TextBox_SelectedFolderA.Text = path;
            }
        }
        private void OnSelectFolderClickB(object sender, RoutedEventArgs e)
        {
            // Créer un dialogue d'ouverture de fichier
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            // Désactiver la validation du nom et de l'existence du fichier
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            // Définir le nom du fichier par défaut comme "Sélection de dossier"
            dialog.FileName = "Sélection de dossier";
            // Afficher le dialogue et obtenir le résultat
            DialogResult result = dialog.ShowDialog();
            // Si le résultat est OK
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Obtenir le chemin du fichier ou du dossier
                string path = dialog.FileName;
                // Si le nom du fichier est "Sélection de dossier", ça veut dire qu'on sélectionne un dossier
                if (path.EndsWith("Sélection de dossier"))
                {
                    // Supprimer le nom du fichier du chemin
                    path = path.Replace("\\Sélection de dossier", "");
                }
                // Stocker le chemin
                TextBox_SelectedFolderB.Text = path;
            }
        }

        //==============================================
        // Ajouter le job
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

            // Exécution de l'ajout
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
        // Bouton de navigation
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
    }
}
