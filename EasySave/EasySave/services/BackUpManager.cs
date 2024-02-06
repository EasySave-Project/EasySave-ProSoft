
using EasySave.model;

namespace EasySave.services;

internal class BackUpManager
{

    public List<BackUpJob> listBackUps = new List<BackUpJob>();


    public void ExecuteBackup(BackUpJob job)
    {
        //TODO A faire 
    }
    public void ExcecuteAllBackUps()
    {
        // TODO à FAIRE
    }

    public BackUpJob FindBackupJobById(string jobName)
    {
        //TODO A faire
        return null;
    }

    public void SaveBackUpJob()
    {
        // TODO à FAIRE
    }

    public void RemoveBackUpJob(BackUpJob backUp)
    {
        // TODO à FAIRE
    }


    public void UpdateBackUpJob(BackUpJob backupJob)
    {

    }
    public void AddBackUpJob(String jobName, String sourceDir, String targetDir)
    {
        //TODO à FAIRE
    }

    internal BackUpJob findBackupJobById(string sJobName)
    {
        throw new NotImplementedException();
    }
}