using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class Log
    {
    
        private string _name;
    
        private string _fileSource;
        
        private string _fileTarget;
        
        private long _fileSize;
        
        private string _fileTransferTime;
        
        private string _time;

        public string Name { 
            get { return _name; }
            set { _name = value; }
        }
        public string FileSource { 
            get {  return _fileSource; } 
            set {  _fileSource = value; }
        }
        public string FileTarget { 
            get {  return _fileTarget; }
            set { _fileTarget = value; }
        }
    
        public string FileTransferTime { 
            get {  return _fileTransferTime; }
            set { _fileTransferTime = value; }
        }
        public string Time {
            get { return _time; } 
            set { _time = value; }
        }

        public long FileSize {  
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        
    }
}
