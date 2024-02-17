using EasySave.model;
using EasySave.services;
using EasySave.controller;
using EasySave.utils;
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
using System.Reflection;

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour ListJob.xaml
    /// </summary>
    public partial class ListJob : Page
    {
        private int indexPage = 1;
        private int nbPage = 0;
        private List<String> sNameJob = new List<string>();

        public ListJob()
        {
            InitializeComponent();
            ShowListJob();
        }

        //==============================================
        // Code de la page ListJob
        //==============================================

        public BackUpController backUpController { get; set; }

        string sCurrentDir = Environment.CurrentDirectory + "\\EasySave\\conf";

        private void ShowListJob()
        {
            // Récupération de la liste des jobs
            sNameJob.Clear();
            foreach (BackUpJob bj in BackUpManager.listBackUps)
            {
                sNameJob.Add(bj.name);
            }

            // Initialisation des pages
            nbPage = (int)Math.Ceiling((double)sNameJob.Count / 5);
            Label_IndexPages.Content = indexPage + " / " + nbPage;

            // Affichage des jobs
            int startIndex = (indexPage - 1) * 5;
            for (int i = 0; i < 5; i++)
            {
                int index = startIndex + i;
                if (index < sNameJob.Count)
                {
                    switch (i)
                    {
                        case 0:
                            LabelName_Job1.Content = sNameJob[index];
                            Grid_Job1.Visibility = Visibility.Visible;
                            Grid_Job1.IsEnabled = true;
                            break;
                        case 1:
                            LabelName_Job2.Content = sNameJob[index];
                            Grid_Job2.Visibility = Visibility.Visible;
                            Grid_Job2.IsEnabled = true;
                            break;
                        case 2:
                            LabelName_Job3.Content = sNameJob[index];
                            Grid_Job3.Visibility = Visibility.Visible;
                            Grid_Job3.IsEnabled = true;
                            break;
                        case 3:
                            LabelName_Job4.Content = sNameJob[index];
                            Grid_Job4.Visibility = Visibility.Visible;
                            Grid_Job4.IsEnabled = true;
                            break;
                        case 4:
                            LabelName_Job5.Content = sNameJob[index];
                            Grid_Job5.Visibility = Visibility.Visible;
                            Grid_Job5.IsEnabled = true;
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            LabelName_Job1.Content = "";
                            Grid_Job1.Visibility = Visibility.Hidden;
                            Grid_Job1.IsEnabled = false;
                            break;
                        case 1:
                            LabelName_Job2.Content = "";
                            Grid_Job2.Visibility = Visibility.Hidden;
                            Grid_Job2.IsEnabled = false;
                            break;
                        case 2:
                            LabelName_Job3.Content = "";
                            Grid_Job3.Visibility = Visibility.Hidden;
                            Grid_Job3.IsEnabled = false;
                            break;
                        case 3:
                            LabelName_Job4.Content = "";
                            Grid_Job4.Visibility = Visibility.Hidden;
                            Grid_Job4.IsEnabled = false;
                            break;
                        case 4:
                            LabelName_Job5.Content = "";
                            Grid_Job5.Visibility = Visibility.Hidden;
                            Grid_Job5.IsEnabled = false;
                            break;
                    }
                }
            }
        }

        //==============================================
        // PLAY jobs
        //==============================================
        private void ExecuteJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Exécuter du job
            index_SelectJob--;
            try
            {
                //backUpController.InitiateBackUpJob(BackUpManager.listBackUps[index_SelectJob]);
                MessageBox.Show("index du job : " + index_SelectJob);
            }
            catch
            {
                MessageBox.Show("Erreur lors de l'execution du job", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnPlay_Job1_Click(object sender, RoutedEventArgs e)
        {
            ExecuteJob(1);
        }

        private void BtnPlay_Job2_Click(object sender, RoutedEventArgs e)
        {
            ExecuteJob(2);
        }

        private void BtnPlay_Job3_Click(object sender, RoutedEventArgs e)
        {
            ExecuteJob(3);
        }

        private void BtnPlay_Job4_Click(object sender, RoutedEventArgs e)
        {
            ExecuteJob(4);
        }

        private void BtnPlay_Job5_Click(object sender, RoutedEventArgs e)
        {
            ExecuteJob(5);
        }

        //==============================================
        // AJOUTER jobs
        //==============================================
        private void Btn_AddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJob addjobg = new AddJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = addjobg;
        }

        //==============================================
        // MODIFIER jobs
        //==============================================

        private void EditJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Modifier du job
            index_SelectJob--;
            try
            {
                //EditJob editjob = new EditJob(index_SelectJob);
                //Window parentWindow = Window.GetWindow(this);
                //parentWindow.Content = editjob;
                MessageBox.Show("Modification : " + sNameJob[index_SelectJob] + " / Index : " + index_SelectJob);

            }
            catch
            {
                MessageBox.Show("Erreur lors de l'ouverture de la page Modifier Job", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEdit_job1_Click(object sender, RoutedEventArgs e)
        {
            EditJob(1);
        }

        private void BtnEdit_job2_Click(object sender, RoutedEventArgs e)
        {
            EditJob(2);
        }

        private void BtnEdit_job3_Click(object sender, RoutedEventArgs e)
        {
            EditJob(3);
        }

        private void BtnEdit_job4_Click(object sender, RoutedEventArgs e)
        {
            EditJob(4);
        }

        private void BtnEdit_job5_Click(object sender, RoutedEventArgs e)
        {
            EditJob(5);
        }

        //==============================================
        // SUPPRIMER jobs
        //==============================================

        private void DeleteJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Supprimer du job
            index_SelectJob--;
            try
            {
                string sMsgBox = "Voulez-vous vraiment supprimer le travaux : " + sNameJob[index_SelectJob];
                MessageBoxResult result = MessageBox.Show(sMsgBox, "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Oui
                    //backUpController.InitiateRemoveBackup(sNameJob[index_SelectJob]);
                    MessageBox.Show("Suppression : " + sNameJob[index_SelectJob] + " / Index : " + index_SelectJob);
                }
            }
            catch
            {
                MessageBox.Show("Erreur lors de la suppression du job", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void BtnDelete_job1_Click(object sender, RoutedEventArgs e)
        {
            DeleteJob(1);
        }

        private void BtnDelete_job2_Click(object sender, RoutedEventArgs e)
        {
            DeleteJob(2);
        }

        private void BtnDelete_job3_Click(object sender, RoutedEventArgs e)
        {
            DeleteJob(3);
        }

        private void BtnDelete_job4_Click(object sender, RoutedEventArgs e)
        {
            DeleteJob(4);
        }

        private void BtnDelete_job5_Click(object sender, RoutedEventArgs e)
        {
            DeleteJob(5);
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
        
        private void Btn_Right_Index_Click(object sender, RoutedEventArgs e)
        {
            if (indexPage < nbPage)
            {
                indexPage++;
                Label_IndexPages.Content = indexPage + " / " + nbPage;
                ShowListJob();
            }
        }

        private void Btn_Left_Index_Click(object sender, RoutedEventArgs e)
        {
            if (indexPage > 1)
            {
                indexPage--;
                Label_IndexPages.Content = indexPage + " / " + nbPage;
                ShowListJob();
            }
        }
    }

}