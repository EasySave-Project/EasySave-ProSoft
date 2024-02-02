using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.model;
using EasySave.services;

namespace EasySave.controller
{

    internal class BackUpController
    {
        private BackUpManager _backUpManager;
        private LogManager    _logManager;
        private StateManager  _stateManager;


        public BackUpController(BackUpManager backUpManager, LogManager logManager, StateManager stateManager )
        {
            this._backUpManager = backUpManager;
            this._logManager = logManager;
            this._stateManager = stateManager;
        }
        public void initiateBackup(String sJobName)
        {
            try
            {
                BackupJob job = _backUpManager.findBackupJobById(sJobName);
                if (job != null)
                {
                    _backUpManager.executeBackup(job);
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
