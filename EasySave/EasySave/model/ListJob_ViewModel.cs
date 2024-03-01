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

        // Private constructor to prevent external instantiation
        private ListJob_ViewModel()
        {
            Jobs = new ObservableCollection<JobObject>();
            PopulateJobs();
        }

        // Private constructor to prevent external instantiation
        public static ListJob_ViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ListJob_ViewModel();
            }
            return instance;
        }

        // Private constructor to prevent external instantiation
        private void PopulateJobs()
        {
            // Private constructor to prevent external instantiation
            JobObjectFactory jobFactory = new JobObjectFactory();
            // Filling the observable list with job data
            foreach (JobObject job in jobFactory.CreateJobObject())
            {
                Jobs.Add(job);
            }
        }

        public void UpdateJobProgressInList(string jobName, int progress)
        {
            Task.Run(() =>
            {
                // Update work progress on the main thread
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < Jobs.Count; i++)
                    {
                        // Update work progress on the main thread
                        if (Jobs[i].JobName == jobName)
                        {
                            // Update work progress for current job
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
            Jobs.Clear(); // Delete all current jobs
            PopulateJobs(); // Reload jobs from data source
        }
    }

}
