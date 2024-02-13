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

        public void InitiateBackUpJob(BackUpJob bj)
        {
            backUpManager.ExecuteBackup(bj);
        }
        public void InitiateAllBackUpJobs()
        {
            backUpManager.ExcecuteAllBackUps();
        }
        public void InitiateRemoveBackup(string jobName)
        {
            backUpManager.RemoveBackUpJob(jobName); 
        }
        public void InitiateAddJob(BackUpType type, string name ,string sourceDir,string targetDir)
        {
            backUpManager.AddBackUpJob(type, name, sourceDir, targetDir);
        }
        public void IntiateModifyJobName(int index, string newName)
        {
            backUpManager.UpdateBackUpJobName(index, newName);
        }

        public void InitiateModifyJobSourceDir(int index, string sourceDir)
        {
            backUpManager.UpdateBackUpJobSourceDir(index, sourceDir);
        }
        public void IniateModifyJobType(int index, BackUpType type)
        {
            backUpManager.UpdateBackUpJobType(index, type);
        }
        public void InitiateModifyJobTargetDir(int index, string targetDir)
        {
            backUpManager.UpdateBackUpJobTargetDir(index, targetDir);
        }
        


    }
}
