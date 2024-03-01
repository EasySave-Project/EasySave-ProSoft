using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class JobObject
    {
        public ObservableCollection<JobObject> Jobs { get; set; }

        // Déclaration des propriétés du message
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int JobProgress { get; set; }

        // Constructeur du message
        public JobObject(int jobId, string jobName, int jobProgress)
        {
            JobId = jobId;
            JobName = jobName;
            JobProgress = jobProgress;
        }
    }
}
