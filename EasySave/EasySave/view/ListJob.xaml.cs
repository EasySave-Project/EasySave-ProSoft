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

namespace EasySave.view
{

    public partial class ListJob : Page
    {
        private MainWindow _MainWindows = new MainWindow();
        private int indexPage = 1;
        private int nbPage = 0;
        private List<String> sNameJob = new List<string>();
        private static Thread thread_ProgressBar;
        private Settings settings_state = new Settings();

        public ListJob()
        {
            InitializeComponent();
            ShowListJob();
            //ProgressBar_Thread();
        }

        //==============================================
        // Code de la page ListJob
        //==============================================
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

            // Lancer le thread de la barre de progression
            ProgressBar_Thread(btnJob);

            // Exécuter du job dans un thread séparé
            Thread jobThread = new Thread(() =>
            {
                try
                {
                    _MainWindows.backUpController.InitiateBackUpJob(BackUpManager.listBackUps[index_SelectJob]);
                }
                catch
                {
                    System.Windows.MessageBox.Show(ManageLang.GetString("error_save"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            // Faire du thread un thread d'arrière-plan
            jobThread.IsBackground = true;

            // Démarrer le thread
            jobThread.Start();
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
        // Bouton ALL
        //==============================================
        private void Btn_RunAll_Click(object sender, RoutedEventArgs e)
        {
            _MainWindows.backUpController.InitiateAllBackUpJobs();
        }

        //==============================================
        // Récupération des barres de progression
        //==============================================

        private void ProgressBar_Thread(int btnJob)
        {   
            // Créer un nouveau thread pour ProgressTEST
            Thread thread_ProgressTEST = new Thread(() => ProgressTEST(btnJob));
            thread_ProgressTEST.IsBackground = true;
            thread_ProgressTEST.Start();
        }

        private void ProgressTEST(int btnJob)
        {
            // Calculer l'index global du job en fonction de la page actuelle
            int index_Start = (indexPage - 1) * 5;
            int index_SelectJob = index_Start + btnJob;
            index_SelectJob--;
            // Récupération du nom du job par rapport à l'index
            string jobNameCurrent = sNameJob[index_SelectJob];
            // Setup des éléments WPF
            System.Windows.Controls.Label label = null;
            System.Windows.Controls.ProgressBar progressBar = null;
            Dispatcher.Invoke(() =>
            {
                label = (System.Windows.Controls.Label)FindName($"LabelName_Job{btnJob}");
                progressBar = (System.Windows.Controls.ProgressBar)FindName($"ProgressBar_Job{btnJob}");
            });
            // Variables
            bool Security = false;
            int timeAt100 = 0;
            string filePath;

            while (Security == false)
            {
                if(settings_state.StateType == "Xml")
                {
                    filePath = Path.Combine(Environment.CurrentDirectory, "EasySave", "log", $"state_backup_{jobNameCurrent}.xml");
                } else
                {
                    filePath = Path.Combine(Environment.CurrentDirectory, "EasySave", "log", $"state_backup_{jobNameCurrent}.json");
                }

                Dispatcher.Invoke(() =>
                {
                    // Vérifier si la page affiché est toujours celle qui présente notre job
                    if (label.Content.ToString() == jobNameCurrent)
                    {
                        // Afficher la barre de progression
                        progressBar.Visibility = Visibility.Visible;
                        if (File.Exists(filePath))
                        {
                            if (settings_state.StateType == "Xml")
                            {
                                // Mode XML
                                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    // Lecture du contenu du fichier
                                    using (StreamReader sr = new StreamReader(fs))
                                    {
                                        string xmlContent = sr.ReadToEnd();
                                        string[] stateElements = xmlContent.Split(new string[] { "</State>" }, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (string stateElement in stateElements)
                                        {
                                            string stateXml = stateElement + "</State>";

                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.LoadXml(stateXml);

                                            XmlNode progressNode = xmlDoc.SelectSingleNode("//Progression");
                                            if (progressNode != null)
                                            {
                                                progressBar.Value = int.Parse(progressNode.InnerText);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Mode JSON
                                string fileContent = File.ReadAllText(filePath);
                                int lastProgress = 0;
                                using (var jsonReader = new JsonTextReader(new StringReader(fileContent)) { SupportMultipleContent = true })
                                {
                                    var serializer = new JsonSerializer();
                                    while (jsonReader.Read())
                                    {
                                        dynamic fileObject = serializer.Deserialize<dynamic>(jsonReader);
                                        lastProgress = fileObject?.Progression ?? lastProgress;
                                    }
                                    progressBar.Value = lastProgress;
                                }
                            }
                        }
                    }
                });

                // Si la progression est à 100%, on incrémente le temps écoulé
                Dispatcher.Invoke(() =>
                {
                    if (progressBar.Value == 100)
                    {
                        timeAt100 += 100;
                    }
                    // Sinon, on le remet à zéro
                    else
                    {
                        timeAt100 = 0;
                    }
                    // Si le temps écoulé à 100% dépasse 3 secondes, on passe Security à true
                    if (timeAt100 >= 3000)
                    {
                        Security = true;
                        progressBar.Visibility = Visibility.Hidden;
                    }
                });
                Thread.Sleep(100);
            }
        }

        private void HiddenAllProgressBar()
        {
            ProgressBar_Job1.Visibility = Visibility.Hidden;
            ProgressBar_Job2.Visibility = Visibility.Hidden;
            ProgressBar_Job3.Visibility = Visibility.Hidden;
            ProgressBar_Job4.Visibility = Visibility.Hidden;
            ProgressBar_Job5.Visibility = Visibility.Hidden;
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
                HiddenAllProgressBar();
            }
        }

        private void Btn_Left_Index_Click(object sender, RoutedEventArgs e)
        {
            if (indexPage > 1)
            {
                indexPage--;
                Label_IndexPages.Content = indexPage + " / " + nbPage;
                ShowListJob();
                HiddenAllProgressBar();
            }
        }

        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

}