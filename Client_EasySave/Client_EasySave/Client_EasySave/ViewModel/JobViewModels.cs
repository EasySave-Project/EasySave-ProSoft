using Client_EasySave.services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client_EasySave.ViewModel
{
    public class JobViewModels : INotifyPropertyChanged
    {
        // Déclaration de l'événement PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Méthode pour déclencher l'événement
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Déclaration de la liste des jobs
        private ObservableCollection<Job> _jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set
            {
                _jobs = value;
                OnPropertyChanged("Jobs");
            }
        }

        // Déclaration du singleton
        private static JobViewModels instance;
        public static JobViewModels GetInstance()
        {
            if (instance == null)
            {
                instance = new JobViewModels();
            }
            return instance;
        }

        // Méthode pour charger les données JSON reçues du serveur
        public void LoadJson(string json)
        {

            // Désérialiser le JSON en une liste de jobs
            List<Job> jobs = JsonSerializer.Deserialize<List<Job>>(json);
            
            // Mettre à jour la liste des jobs avec les données reçues            
            // Si la liste n'existe pas, la créer
            if (Jobs == null)
            {
                Jobs = new ObservableCollection<Job>();
            } else
            {
                Jobs.Clear();
            }

            foreach (Job job in jobs)
            {
                Jobs.Add(job);
            }
        }
    }

}
