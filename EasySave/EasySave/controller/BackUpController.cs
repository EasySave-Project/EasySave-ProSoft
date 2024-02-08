using EasySave.model;
using EasySave.services;

namespace EasySave.controller
{

    public class BackUpController
    {
        public BackUpManager backUpManager { get; set; }
        public LogManager logManager { get; set; }
        public StateManager stateManager { get; set; }


        public BackUpController(BackUpManager backUpManager, LogManager logManager, StateManager stateManager)
        {
            this.backUpManager = backUpManager;
            this.logManager = logManager;
            this.stateManager = stateManager;
        }

    }
}
