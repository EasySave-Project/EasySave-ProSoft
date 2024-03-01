using EasySave.model;
using EasySave.services;
using System.Windows;
using System.Windows.Controls;
using EasySave.services;
using Button = System.Windows.Controls.Button;

namespace EasySave.view
{

    public partial class ListJob : Page
    {
        private int indexPage = 1;
        private int nbPage = 0;
        private List<String> sNameJob = new List<string>();
        StateManager stateManager = new StateManager();
        private List<bool> jobsRunning = new List<bool>();
        private List<bool> jobsPaused = new List<bool>();
        private List<bool> jobsStopped = new List<bool>();
        public ListJob()
        {
            
            InitializeComponent();
            initJobs();
            ShowListJob();
            //stateManager.ProgressionChanged += StateManager_ProgressionChanged;
        }
        private void initJobs()
        {
            for (int i = 0; i < BackUpManager.listBackUps.Count; i++)
            {
                jobsPaused.Add(false);
                jobsRunning.Add(false);
                jobsStopped.Add(false);
            }
        }
        //==============================================
        // Code de la page ListJob
        //==============================================


        private MainWindow _MainWindows = new MainWindow();

        string sCurrentDir = Environment.CurrentDirectory + "\\EasySave\\conf";

        private void ShowListJob()
        {
            // Récupération de la liste des jobs
            sNameJob.Clear();
            foreach (BackUpJob bj in BackUpManager.listBackUps)
            {
                sNameJob.Add(bj.Name);
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
            EditJob editjob = new EditJob(index_SelectJob);
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = editjob;
            //MessageBox.Show("Modification : " + sNameJob[index_SelectJob] + " / Index : " + index_SelectJob); 
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
                string sMsgBox = ManageLang.GetString("view_suppr") + " : " + sNameJob[index_SelectJob];
                MessageBoxResult result = System.Windows.MessageBox.Show(sMsgBox, ManageLang.GetString("view_supp_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Oui
                    _MainWindows.backUpController.InitiateRemoveBackup(sNameJob[index_SelectJob]);
                    //System.Windows.MessageBox.Show("Suppression : " + sNameJob[index_SelectJob] + " / Index : " + index_SelectJob);
                    ShowListJob();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_suppresion"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
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




        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //==============================================
        // PLAY jobs
        //==============================================
        private void ExecuteJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;
            index_SelectJob--;
            BackUpManager.listBackUps[index_SelectJob].ResetJob();
            _MainWindows.backUpController.backUpManager.ResetStopJob(BackUpManager.listBackUps[index_SelectJob]);
            try
            {
                if (jobsPaused[index_SelectJob] == true )
                {
                    
                   jobsRunning[index_SelectJob] = true;
                    _MainWindows.backUpController.InitiateResumeBackUp(BackUpManager.listBackUps[index_SelectJob]);
                    jobsPaused[index_SelectJob] = false;
                    
                }
                else
                {
                    jobsRunning[index_SelectJob] = true;
                    _MainWindows.backUpController.InitiateBackUpJob(BackUpManager.listBackUps[index_SelectJob]);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_save"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopJob(int btnJob)
        {
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;
            index_SelectJob--;
            try
            {
                if (jobsRunning[index_SelectJob] == true)
                {
                    jobsStopped[index_SelectJob] = true;
                    _MainWindows.backUpController.backUpManager.StopBackup(BackUpManager.listBackUps[index_SelectJob]);
                }
                else
                {
                    System.Windows.MessageBox.Show(ManageLang.GetString("jobNotRunning"));
                }
                
                
            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Error on the pause :" + ex.Message);
            }
             
        }

        
        //==============================================
        // Bouton ALL
        //==============================================
        private void Btn_RunAll_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 1; i <= BackUpManager.listBackUps.Count; i++)
            {
                ExecuteJob(i);
            }
        }

        private void btnStopAllJob_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1;i<= BackUpManager.listBackUps.Count; i++)
            {
                StopJob(i);
            }
        }


        private void PauseJob(int btnJob)
        {
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Supprimer du job
            index_SelectJob--;

            try
            {
                if (jobsRunning[index_SelectJob] == true)
                {
                    jobsPaused[index_SelectJob] = true;
                    _MainWindows.backUpController.backUpManager.PauseBackup(BackUpManager.listBackUps[index_SelectJob]);
                }
                else
                {
                    System.Windows.MessageBox.Show(ManageLang.GetString("jobNotRunning"));
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("error pause job :  " + ex.Message);
            }
        }
        private void btn_PauseAll_click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 1; i <= BackUpManager.listBackUps.Count; i++)
            {
                PauseJob(i);
            }
            
            
        }











        

        
        
        
        
        
        
        
        
        
        
        
        
        private void btnPauseJob1_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(1);
        }

        private void btnPauseJob2_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(2);
        }

        private void btnPauseJob3_Click(object sender, RoutedEventArgs e)
        {       
            PauseJob(3);
        }
        private void btnPauseJob4_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(4);
        }
        private void btnPauseJob5_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(5);
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

        private void BtnStop_Job1_Click(object sender, RoutedEventArgs e)
        {
            StopJob(1);
        }
        private void BtnStop_Job2_Click(object sender, RoutedEventArgs e)
        {
            StopJob(2);
        }
        private void BtnStop_Job3_Click(object sender, RoutedEventArgs e)
        {
            StopJob(3);
        }
        private void BtnStop_Job4_Click(object sender, RoutedEventArgs e)
        {
            StopJob(4);
        }
        private void BtnStop_Job5_Click(object sender, RoutedEventArgs e)
        {
            StopJob(5);
        }
    }

}