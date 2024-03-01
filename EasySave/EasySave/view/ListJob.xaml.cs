using EasySave.model;
using EasySave.services;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Threading;
using static EasySave.services.StateManager;
using System;
using System.Diagnostics;
using EasySave.utils;
using System.Xml;
using EasySave.services;
using Button = System.Windows.Controls.Button;
using System.Collections.ObjectModel;


namespace EasySave.view
{

    public partial class ListJob : Page
    {
        private MainWindow _MainWindows = new MainWindow();

        private ListJob_ViewModel viewModel;

        private int indexPage = 1;
        private int nbPage = 0;
        private List<String> sNameJob = new List<string>();
        private static Thread thread_ProgressBar;
        private Settings settings_state = Settings.Instance;



        string sCurrentDir = Environment.CurrentDirectory + "\\EasySave\\conf";
        StateManager stateManager = new StateManager();
        private List<bool> jobsRunning = new List<bool>();
        private List<bool> jobsPaused = new List<bool>();
        private List<bool> jobsStopped = new List<bool>();

        public ListJob()
        {            
            InitializeComponent();
            initJobs();

            viewModel = ListJob_ViewModel.GetInstance();
            viewModel.ReloadJobs();
            DataContext = viewModel;
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
        // Jobs button
        //==============================================

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
            // PLAY
            ExecuteJob(job.JobId);
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            // PAUSE
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
            PauseJob(job.JobId);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            // STOP
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
            StopJob(job.JobId);
        }

        private void BtnResume_Click(object sender, RoutedEventArgs e)
        {
            // RESUME
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // EDIT
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
            if (_MainWindows.backUpController.backUpManager.jobCompleted(job.JobId))
            {
                // cas du lancement du thread d'execution on cherche pas à comprendre tu peux pas modifier ton job 
                //case of launching the execution thread we do not seek to understand you can not modify your job
                System.Windows.MessageBox.Show(ManageLang.GetString("threadErrorEdit"));
                return;
            }
            EditJob editjob = new EditJob(job.JobId);
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = editjob;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // DELETE
            var job = ((System.Windows.Controls.Button)sender).DataContext as JobObject;
            try
            {
                string sMsgBox = ManageLang.GetString("view_suppr") + job.JobName;
                MessageBoxResult result = System.Windows.MessageBox.Show(sMsgBox, ManageLang.GetString("view_supp_title"), MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (_MainWindows.backUpController.backUpManager.jobCompleted(job.JobId))
                    {
                        // if you want to delete a job that's already been launched.
                        System.Windows.MessageBox.Show(ManageLang.GetString("error_suppresion"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _MainWindows.backUpController.InitiateRemoveBackup(job.JobName);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_suppresion"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            viewModel.ReloadJobs();
        }

        //==============================================
        // ADD job
        //==============================================
        private void Btn_AddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJob addjobg = new AddJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = addjobg;
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
            if (_MainWindows.backUpController.backUpManager.AreAllJobsCompleted())
            {
                System.Windows.MessageBox.Show("Des sauvegardes sont encore en cours. L'application se fermera automatiquement une fois les sauvegardes terminées.");

                
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1); 
                timer.Tick += (s, args) =>
                {
                    if (_MainWindows.backUpController.backUpManager.AreAllJobsCompleted())
                    {
                        timer.Stop(); 
                        System.Windows.Application.Current.Shutdown(); 
                    }
                };
                timer.Start(); 

           
                Btn_Leavee.IsEnabled = false;
            }
            else
            {
                System.Windows.Application.Current.Shutdown(); 
            }

        }

        //==============================================
        // PLAY jobs
        //==============================================
        private void ExecuteJob(int jobID)
        {
            
            int index_SelectJob = jobID;


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

        private void StopJob(int jobID)
        {
            int index_SelectJob = jobID;
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
        // Button ALL
        //==============================================
        private void Btn_RunAll_Click(object sender, RoutedEventArgs e)
        {            
            for (int i = 0; i < BackUpManager.listBackUps.Count; i++)
            {
                ExecuteJob(i);
            }
        }

        private void btnStopAllJob_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < BackUpManager.listBackUps.Count; i++)
            {
                StopJob(i);
            }
        }


        private void PauseJob(int jobId)
        {
            int index_SelectJob = jobId;

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
            
            for (int i = 0; i < BackUpManager.listBackUps.Count; i++)
            {
                PauseJob(i);
            }
            
            
        }
 
    }

}