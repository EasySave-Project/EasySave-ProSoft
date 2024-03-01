using System.Windows;
using EasySaveClient.Services;
using Client_EasySave.services;
using Client_EasySave.ViewModel;

namespace Client_EasySave
{
    public partial class MainWindow : Window
    {
        private ServerManager serverManager;
        private JobViewModels viewModel;
        private List<String> sNameJob = new List<string>();
        private int indexPage = 1;
        private int nbPage = 0;

        public MainWindow()
        {
            InitializeComponent();
            serverManager = new ServerManager(this);
            viewModel = JobViewModels.GetInstance();
        }

        public void ReloadData()
        {
            // Récupérer la liste des noms des jobs à partir de la liste des jobs
            sNameJob.Clear();
            foreach (Job job in viewModel.Jobs)
            {
                sNameJob.Add(job.JobName);
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
                            ProgressBar_Job1.Value = viewModel.Jobs[index].JobProgress;
                            Grid_Job1.Visibility = Visibility.Visible;
                            Grid_Job1.IsEnabled = true;
                            break;
                        case 1:
                            LabelName_Job2.Content = sNameJob[index];
                            ProgressBar_Job2.Value = viewModel.Jobs[index].JobProgress;
                            Grid_Job2.Visibility = Visibility.Visible;
                            Grid_Job2.IsEnabled = true;
                            break;
                        case 2:
                            LabelName_Job3.Content = sNameJob[index];
                            ProgressBar_Job3.Value = viewModel.Jobs[index].JobProgress;
                            Grid_Job3.Visibility = Visibility.Visible;
                            Grid_Job3.IsEnabled = true;
                            break;
                        case 3:
                            LabelName_Job4.Content = sNameJob[index];
                            ProgressBar_Job4.Value = viewModel.Jobs[index].JobProgress;
                            Grid_Job4.Visibility = Visibility.Visible;
                            Grid_Job4.IsEnabled = true;
                            break;
                        case 4:
                            LabelName_Job5.Content = sNameJob[index];
                            ProgressBar_Job5.Value = viewModel.Jobs[index].JobProgress;
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
        // Exécution des jobs
        //==============================================
        private void ExecuteJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Exécuter du job
            index_SelectJob--;

            // Lancer le message
            serverManager.SendAction(index_SelectJob, ServerAction.start);
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
        // Pause des jobs
        //==============================================

        private void PauseJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Exécuter du job
            index_SelectJob--;

            // Lancer le message
            serverManager.SendAction(index_SelectJob, ServerAction.suspend);
        }

        private void Btn_pause1_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(1);
        }

        private void Btn_pause2_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(2);
        }

        private void Btn_pause3_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(3);
        }

        private void Btn_pause4_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(4);
        }

        private void Btn_pause5_Click(object sender, RoutedEventArgs e)
        {
            PauseJob(5);
        }

        //==============================================
        // Stop des jobs
        //==============================================

        private void StopJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Exécuter du job
            index_SelectJob--;

            // Lancer le message
            serverManager.SendAction(index_SelectJob, ServerAction.stop);
        }
        private void Btnstop_Job1_Click(object sender, RoutedEventArgs e)
        {
            StopJob(1);
        }

        private void Btnstop_Job2_Click(object sender, RoutedEventArgs e)
        {
            StopJob(2);
        }

        private void Btnstop_Job3_Click(object sender, RoutedEventArgs e)
        {
            StopJob(3);
        }

        private void Btnstop_Job4_Click(object sender, RoutedEventArgs e)
        {
            StopJob(4);
        }

        private void Btnstop_Job5_Click(object sender, RoutedEventArgs e)
        {
            StopJob(5);
        }

        //==============================================
        // Resume des jobs
        //==============================================
        private void ResumeJob(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;

            // Exécuter du job
            index_SelectJob--;

            // Lancer le message
            serverManager.SendAction(index_SelectJob, ServerAction.resume);
        }

        private void Btn_resume1_Click(object sender, RoutedEventArgs e)
        {
            ResumeJob(1);
        }

        private void Btn_resume2_Click(object sender, RoutedEventArgs e)
        {
            ResumeJob(2);
        }

        private void Btn_resume3_Click(object sender, RoutedEventArgs e)
        {
            ResumeJob(3);
        }

        private void Btn_resume4_Click(object sender, RoutedEventArgs e)
        {
            ResumeJob(4);
        }

        private void Btn_resume5_Click(object sender, RoutedEventArgs e)
        {
            ResumeJob(5);
        }

        //==============================================
        // Bouton de navigation
        //==============================================

        private void BtnConnexionServ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serverManager.ConnectToServer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }

            if (serverManager.IsConnected())
            {
                BtnConnexionServ.Content = "Connected to server";
            }
            else
            {
                BtnConnexionServ.Content = "Connect to server";
            }
        }

        private void Btn_RunAll_Click(object sender, RoutedEventArgs e)
        {
            serverManager.SendAction(0, ServerAction.allstart);
        }

        private void btn_PauseAll_click(object sender, RoutedEventArgs e)
        {
            serverManager.SendAction(0, ServerAction.allsuspend);
        }

        private void btnStopAllJob_Click(object sender, RoutedEventArgs e)
        {
            serverManager.SendAction(0, ServerAction.allstop);
        }

        private void Btn_LeftIndexPages_Click(object sender, RoutedEventArgs e)
        {
            if (indexPage > 1)
            {
                indexPage--;
                Label_IndexPages.Content = indexPage + " / " + nbPage;
                ReloadData();
            }
        }

        private void Btn_RightIndexPages_Click(object sender, RoutedEventArgs e)
        {
            if (indexPage < nbPage)
            {
                indexPage++;
                Label_IndexPages.Content = indexPage + " / " + nbPage;
                ReloadData();
            }
        }
    
    }

}

