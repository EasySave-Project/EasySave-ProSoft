using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class ListJob_ViewModel
    {
        private static ListJob_ViewModel instance;
        private ObservableCollection<JobObject> jobs;

        public ObservableCollection<JobObject> Jobs
        {
            get { return jobs; }
            private set { jobs = value; }
        }

        // Constructeur privé pour empêcher l'instanciation externe
        private ListJob_ViewModel()
        {
            Jobs = new ObservableCollection<JobObject>();
            PopulateJobs();
        }

        // Méthode pour obtenir l'instance unique de ListJob_ViewModel
        public static ListJob_ViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ListJob_ViewModel();
            }
            return instance;
        }

        // Méthode pour remplir la liste des travaux
        private void PopulateJobs()
        {
            // Création d'une instance de JobObjectFactory pour obtenir les données des jobs
            JobObjectFactory jobFactory = new JobObjectFactory();
            // Remplissage de la liste observable avec les données des jobs
            foreach (JobObject job in jobFactory.CreateJobObject())
            {
                Jobs.Add(job);
            }
        }

        public void UpdateJobProgressInList(string jobName, int progress)
        {
            Task.Run(() =>
            {
                // Mettre à jour la progression du travail sur le thread principal
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < Jobs.Count; i++)
                    {
                        // Vérification si le nom de l'emploi correspond à celui fourni en argument
                        if (Jobs[i].JobName == jobName)
                        {
                            // Mise à jour de la progression du travail pour l'emploi actuel
                            if (progress == 0)
                            {
                                Jobs[i].JobProgress = 1;
                            } else
                            {
                                Jobs[i].JobProgress = progress;
                            }
                        }
                    }
                });
            });
        }

        public void ReloadJobs()
        {
            Jobs.Clear(); // Effacer tous les emplois actuels
            PopulateJobs(); // Recharger les emplois depuis la source de données
        }
    }

}
