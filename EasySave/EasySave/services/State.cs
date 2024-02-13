using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.services
{
    public class State
    {
        // Déclaration des variables objet
        public string NameJob { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string State_Text { get; set; }
        public long TotalFileToCopy { get; set; }
        public long TotalFileSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; }
    }
}
