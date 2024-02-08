
using EasySave.model;
using EasySave.utils;
using Newtonsoft.Json;

namespace EasySave.services;

public class BackUpManager
{

    public static List<BackUpJob> listBackUps;

    public BackUpManager() {
        listBackUps = JsonUtils.LoadJobsFromJson(JsonUtils.filePath);
    }

    public static void SaveJobsToJson()
    {
        try
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var json = JsonConvert.SerializeObject(listBackUps, Formatting.Indented, settings);
            File.WriteAllText(JsonUtils.filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la sauvegarde : {ex.Message}");
        }
    }

    public void ExecuteBackup(BackUpJob job)
    {
        job.Excecute();
    }
    public void ExcecuteAllBackUps()
    {
        foreach (BackUpJob job in listBackUps)
        {
            job.Excecute();
        }
    }
    public void ExecuteMultipleBackup(List<BackUpJob> listOfJobs)
    {
        foreach (BackUpJob job in listOfJobs)
        {
            job.Excecute();
        }
    }

    public BackUpJob FindBackupJobById(int indexJob)
    {
        if (indexJob > 4 && indexJob < 0)
        {
            throw new ArgumentException("Erreur, l'indice du job ne peux être > 5 ");
        }
        return listBackUps[indexJob];
    }
    
    public void RemoveBackUpJob(String jobName)
    {
        listBackUps.Remove(findBackupJobByName(jobName));
        SaveJobsToJson();
    }
    
    public void UpdateBackUpJobName(BackUpJob backupJob, string newName)
    {
        backupJob.name = newName;
        SaveJobsToJson();
    }
    
    public void UpdateBackUpJobSourceDir(BackUpJob backupJob, string sourceDir)
    {
        backupJob.sourceDirectory = sourceDir;
    }

    public void UpdateBackUpJobTargetDir(BackUpJob backUpJob, string targetDir)
    {
        foreach(BackUpJob job in listBackUps)
        {
            if( job.targetDirectory == targetDir)
            {
                job.targetDirectory = targetDir;
                SaveJobsToJson() ;
            }
        }
        
    }
    
    public void AddBackUpJob(BackUpType type, String jobName, String sourceDir, String targetDir)
    {
        if (listBackUps.Count >= 5)
        {
            throw new InvalidOperationException("Le nombre maximal de jobs est atteint.");
        }
        if (listBackUps.Any(j => j.name == jobName))
        {
            throw new InvalidOperationException("Un job avec le même nom existe déjà.");
        }

        BackUpJob addBackUpJob = BackUpJobFactory.CreateBackupJob(type, jobName, sourceDir, targetDir);
         
        listBackUps.Add(addBackUpJob);
        SaveJobsToJson();


    }
         
    public BackUpJob findBackupJobByName(string sJobName)
    {
        foreach (BackUpJob job in listBackUps)
        {
            if (job.name == sJobName)
            {
                return job;
            }
        }
        return null;
    }
}
