
using EasySave.model;
using EasySave.utils;
using Newtonsoft.Json;
using EasySave.view;
using EasySave.services;

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
            Console.WriteLine(ConsoleView.GetLineLanguage(55) + ex.Message);
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
            throw new ArgumentException(ConsoleView.GetLineLanguage(56));
        }
        return listBackUps[indexJob];
    }
    
    public void RemoveBackUpJob(String jobName)
    {
        listBackUps.Remove(findBackupJobByName(jobName));
        SaveJobsToJson();
    }
    
    public void UpdateBackUpJobName(int index, string newName)
    {
        listBackUps[index].name = newName;
        SaveJobsToJson();
    }
    
    public void UpdateBackUpJobSourceDir(int index, string sourceDir)
    {
        listBackUps[index].sourceDirectory = sourceDir;
        SaveJobsToJson();
    }

    public void UpdateBackUpJobTargetDir(int index, string targetDir)
    {
        listBackUps[index].targetDirectory = targetDir;
        
        SaveJobsToJson();
        
    }

    public void UpdateBackUpJobType(int index, BackUpType type)
    {
        if (!(listBackUps[index].GetType().Equals(type)))
        {
            listBackUps[index] = listBackUps[index].CloneToType(type);
            SaveJobsToJson();
        }
        
    }    
    public void AddBackUpJob(BackUpType type, String jobName, String sourceDir, String targetDir)
    {
        if (listBackUps.Count >= 5)
        {
            Console.WriteLine(ConsoleView.GetLineLanguage(57));
            return; 
            //throw new InvalidOperationException("Le nombre maximal de jobs est atteint.");
        }
        if (listBackUps.Any(j => j.name == jobName))
        {
            Console.WriteLine(ConsoleView.GetLineLanguage(59));
            return;
            //throw new InvalidOperationException("Un job avec le même nom existe déjà.");

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
