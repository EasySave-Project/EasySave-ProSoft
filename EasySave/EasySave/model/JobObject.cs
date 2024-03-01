using System.ComponentModel;

namespace EasySave.model
{
    public class JobObject : INotifyPropertyChanged
    {
        // Declaration of message properties
        public int JobId { get; set; }
        public string JobName { get; set; }

        private int jobProgress;
        public int JobProgress
        {
            get { return jobProgress; }
            set
            {
                if (jobProgress != value)
                {
                    jobProgress = value;
                    OnPropertyChanged("JobProgress");
                }
            }
        }

        // Message constructor
        public JobObject(int jobId, string jobName, int jobProgress)
        {
            JobId = jobId;
            JobName = jobName;
            JobProgress = jobProgress;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
