using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class State
    {
        // Déclaration des variables objet
        private string _nameJob;
        private string _sourcePath;
        private string _targetPath;
        private string _state_Text;
        private long _totalFileToCopy;
        private long _totalFileSize;
        private int _nbFilesLeftToDo;
        private int _progression;

        public State()
        {

        }

        public int NbFilesLeftToDo
        {  
            get { return _nbFilesLeftToDo; } 
            set { _nbFilesLeftToDo = value;} 
        }
        public string NameJob
        {
            get 
            {
                return _nameJob; 
            }
            set { _nameJob = value; }
        }
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
            }
        }
        
        public string TargetPath
        {
            get
            {
                return _targetPath;
            }
            set
            {
                _targetPath = value;
            }
        }
        public string State_Text
        {
            get { 
                return _state_Text;
            }
            set
            {
                _state_Text = value;
            }
        }
        public int Progression
        {
            get {
                return _progression;  
            }
            set { 
                _progression = value;
            }
        }
        public long TotalFileToCopy
        {
            get
            {
                return _totalFileToCopy;
            }
            set
            {
                _totalFileToCopy = value;
            }
        }
        public long TotalFileSize
        {
            get 
            { 
                return _totalFileSize;
            }
            set
            {
                _totalFileSize = value;
            }
        }
        
       
    }
}
