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
    // Manages the view model for jobs, implementing INotifyPropertyChanged for data binding
    public class JobViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Triggers the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Holds the collection of jobs
        private ObservableCollection<Job> _jobs;
        public ObservableCollection<Job> Jobs
        {
            get { return _jobs; }
            set
            {
                _jobs = value;
                OnPropertyChanged(nameof(Jobs));
            }
        }

        // Singleton instance to ensure a single instance of the view model
        private static JobViewModels instance;
        public static JobViewModels GetInstance()
        {
            if (instance == null)
            {
                instance = new JobViewModels();
            }
            return instance;
        }

        // Loads jobs data from a JSON string received from the server
        public void LoadJson(string json)
        {
            List<Job> jobs = JsonSerializer.Deserialize<List<Job>>(json);

            if (Jobs == null)
            {
                Jobs = new ObservableCollection<Job>();
            }
            else
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
