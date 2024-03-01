using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Client_EasySave.services
{
    // Represents a job with properties that notify when they change
    public class Job : INotifyPropertyChanged
    {
        // Private fields
        private int _id;
        private string _name;
        private int _progress;

        // Public properties
        [JsonProperty("JobId")]
        public int JobId
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(JobId)); // Updated to use nameof for better refactoring support
            }
        }

        [JsonProperty("JobName")]
        public string JobName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(JobName)); // Updated to use nameof for better refactoring support
            }
        }

        [JsonProperty("JobProgress")]
        public int JobProgress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(JobProgress)); // Updated to use nameof for better refactoring support
            }
        }

        // Event declared for property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to invoke the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
