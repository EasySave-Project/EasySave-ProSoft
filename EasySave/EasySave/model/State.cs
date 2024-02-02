using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.model
{

    
    internal class State
    {
        private string _name {  get; set; }

        private string _totalFiles { get; set; }

        private int _totalFileSize {get; set ; }

        private string _state { get; set; }

        private int _nbFileRemaining { get; set; }

        private string _currentTime { get; set; }

        private int _sizeOfRemainingFiles { get; set; }


    }
}
