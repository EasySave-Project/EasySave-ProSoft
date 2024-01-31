using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.model;

namespace EasySave.controller
{

    internal class BackUpController
    {
        private BackUpManager backUpManager;
        private LogManager logManager;
        private StateManager stateManager;


        public BackUpController(BackUpManager backUpManager, LogManager logManager, StateManager stateManager )
        {
            this.backUpManager = backUpManager;
            this.logManager = logManager;
            this.stateManager = stateManager;
        }
        public void initiateBackup(String sJobName)
        {
            try
            {
                BackupJob job = backUpManager.findBackupJobById(sJobName);
                if (job != null)
                {
                    backUpManager.executeBackup(job);
                    // Mise à jour du log et de l'état
                    //logManager.writeLog(job, /* fileInfo */);
                    //stateManager.updateState(job, /* progressInfo */);
                }
            }
            catch (Exception e)
            {
                // Gérer les exceptions et éventuellement mettre à jour la vue avec un message d'erreur.
            }
        }

    }
}
