using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.services
{
    public class Log
    {
        // Déclaration des variables objet
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public string FileTransferTime { get; set; }
        public string Time { get; set; }
    }
}
