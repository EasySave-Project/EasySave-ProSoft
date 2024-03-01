using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Client_EasySave.services
{
    public class Job : INotifyPropertyChanged
    {
        // Déclaration des champs privés
        private int _id;
        private string _name;
        private int _progress;

        // Déclaration des propriétés publiques
        [JsonProperty("JobId")]
        public int JobId
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonProperty("JobName")]
        public string JobName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonProperty("JobProgress")]
        public int JobProgress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

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
    }

}
