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
        private string nameJob;
        private string sourcePath;
        private string targetPath;
        private string state_Text;
        private long totalFileToCopy;
        private long totalFileSize;
        private int nbFilesLeftToDo;
        private int progression;

        public State()
        {

        }

        public int NbFilesLeftToDo
        {  
            get { return nbFilesLeftToDo; } 
            set { nbFilesLeftToDo = value;} 
        }
        public string NameJob
        {
            get 
            {
                return nameJob; 
            }
            set { nameJob = value; }
        }
        public string SourcePath
        {
            get
            {
                return sourcePath;
            }
            set
            {
                sourcePath = value;
            }
        }
        
        public string TargetPath
        {
            get
            {
                return targetPath;
            }
            set
            {
                targetPath = value;
            }
        }
        public string State_Text
        {
            get { 
                return state_Text;
            }
            set
            {
                state_Text = value;
            }
        }
        public int Progression
        {
            get {
                return progression;  
            }
            set { 
                progression = value;
            }
        }
        public long TotalFileToCopy
        {
            get
            {
                return totalFileToCopy;
            }
            set
            {
                totalFileToCopy = value;
            }
        }
        public long TotalFileSize
        {
            get 
            { 
                return totalFileSize;
            }
            set
            {
                totalFileSize = value;
            }
        }
        
       
    }
}
