using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class Log
    {
        // Déclaration des variables objet
        private string name;
        private string fileSource;
        private string fileTarget;
        private long fileSize;
        private string fileTransferTime;
        private string time;

        public string Name { 
            get { return name; }
            set { name = value; }
        }
        public string FileSource { 
            get {  return fileSource; } 
            set {  fileSource = value; }
        }
        public string FileTarget { 
            get {  return fileTarget; }
            set { fileTarget = value; }
        }
    
        public string FileTransferTime { 
            get {  return fileTransferTime; }
            set { fileTransferTime = value; }
        }
        public string Time {
            get { return time; } 
            set { time = value; }
        }

        public long FileSize {  
            get { return fileSize; }
            set { fileSize = value; }
        }

        
    }
}
