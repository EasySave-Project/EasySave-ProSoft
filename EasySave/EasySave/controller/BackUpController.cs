using EasySave.model;
using EasySave.services;

namespace EasySave.controller
{

    public class BackUpController
    {
        public BackUpManager backUpManager { get; set; }
        public LogManager logManager { get; set; }
        public StateManager stateManager { get; set; }


        //public BackUpController(BackUpManager backUpManager, LogManager logManager, StateManager stateManager)
        //{
        //    this._backUpManager = backUpManager;
        //    this._logManager = logManager;
        //    this._stateManager = stateManager;
        //}
        public void initiateBackup(String sJobName)
        {
            try
            {
                BackUpJob job = backUpManager.findBackupJobByName(sJobName);
                if (job != null)
                {
                    backUpManager.ExecuteBackup(job);
                    // Mise à jour du log et de l'état
                    //logManager.writeLog(job, /* fileInfo */);
                    //stateManager.updateState(job, /* progressInfo */);
                }
            }
            catch (Exception e)
            {
                // Gérer les exceptions et éventuellement mettre à jour la vue avec un message d'erreur.
                Console.WriteLine(e.Message);
            }
        }

    }
}
