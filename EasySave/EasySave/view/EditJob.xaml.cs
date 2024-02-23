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
using System.Windows.Forms;
using EasySave.model;
using EasySave.services;
using EasySave.controller;

namespace EasySave.view
{
    public partial class EditJob : Page
    {
        private int indexJob_Transfert;
        private MainWindow _MainWindows = new MainWindow();

        private string sNameJob_Old;
        private string sSourceFolder_Old;
        private string sDestinationFolder_Old;
        private string sType_Old;

        public EditJob(int indexJob)
        {
            indexJob_Transfert = indexJob;
            InitializeComponent();
            InitFields();
        }

        //==============================================
        // Initialisation des champs
        //==============================================

        private void InitFields()
        {
            // Récupérer les informations du job par rapport à l'index en paramètre
            TextBox_NameJob.Text = BackUpManager.listBackUps[indexJob_Transfert].name;
            TextBox_SelectedFolderA.Text = BackUpManager.listBackUps[indexJob_Transfert].sourceDirectory;
            TextBox_SelectedFolderB.Text = BackUpManager.listBackUps[indexJob_Transfert].targetDirectory;

            Type backUpType = BackUpManager.listBackUps[indexJob_Transfert].GetType();
            string typeJob = backUpType.FullName;
            if (typeJob.Contains("Complete"))
            {
                ComboBox_TypeSave.SelectedIndex = 0;
            }
            else
            {
                ComboBox_TypeSave.SelectedIndex = 1;
            }

            // Stocker les valeurs initiales
            sNameJob_Old = TextBox_NameJob.Text;
            sSourceFolder_Old = TextBox_SelectedFolderA.Text;
            sDestinationFolder_Old = TextBox_SelectedFolderB.Text;
            ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_TypeSave.SelectedItem;
            sType_Old = selectedItem.Content.ToString();
        }

        //==============================================
        // Modifier le job
        //==============================================
        private void Btn_ModifyJob_Click(object sender, RoutedEventArgs e)
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
            }
            else if (string.IsNullOrEmpty(sSourceFolder) || !System.IO.Directory.Exists(sSourceFolder))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneSourcePath"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(sDestinationFolder) || !System.IO.Directory.Exists(sDestinationFolder))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneDestPath"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(sType))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_NoneCodeBackup"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Modifier si changement observé
            if (sNameJob != sNameJob_Old)
            {
                _MainWindows.backUpController.IntiateModifyJobName(indexJob_Transfert, sNameJob);
            }
            if (sSourceFolder != sSourceFolder_Old)
            {
                _MainWindows.backUpController.InitiateModifyJobSourceDir(indexJob_Transfert, sSourceFolder);
            }
            if (sDestinationFolder != sDestinationFolder_Old)
            {
                _MainWindows.backUpController.InitiateModifyJobTargetDir(indexJob_Transfert, sDestinationFolder);
            }
            if (sType != sType_Old)
            {
                BackUpType type;
                if (sType == "Complète")
                {
                    type = BackUpType.Complete;
                    _MainWindows.backUpController.IniateModifyJobType(indexJob_Transfert, type);
                }
                else
                {
                    type = BackUpType.Differential;
                    _MainWindows.backUpController.IniateModifyJobType(indexJob_Transfert, type);
                }
            }

            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
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

        private void Btn_AddJob_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }
        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
