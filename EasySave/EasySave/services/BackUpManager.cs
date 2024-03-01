
using EasySave.model;
using EasySave.utils;
using Newtonsoft.Json;
using EasySave.view;
using EasySave.services;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Collections.Concurrent;

namespace EasySave.services;

public class BackUpManager
{
    public static List<BackUpJob> listBackUps;

    private ConcurrentBag<Thread> _runningThreads = new ConcurrentBag<Thread>();

    private static BackUpManager _instance;

    public static BackUpManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new BackUpManager();
            }
            return _instance;
        }
    }

    public ConcurrentBag<Thread> RunningThreads
    {
        get { return _runningThreads; }
    }
    private BackUpManager() {
        listBackUps = JsonUtils.LoadJobsFromJson(JsonUtils.filePath);   
    }

    public bool AreAllJobsCompleted()
    {
        return RunningThreads.All(t => !t.IsAlive);
    }
    public  void SaveJobsToJson()
    {
        
        try
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var json = JsonConvert.SerializeObject(listBackUps, Formatting.Indented, settings);
            File.WriteAllText(JsonUtils.filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ManageLang.GetString("error_save") + ex.Message);
        }
    
    }

    public void PauseBackup(BackUpJob bj)
    {
        bj.FileTransfert.Pause();
       // System.Windows.MessageBox.Show("Pause du job : " + bj.Name);
    }

    public void ResumeBackup(BackUpJob bj)
    { 
        bj.FileTransfert.Resume();
        //System.Windows.MessageBox.Show("Reprise du job : " + bj.Name);
    }
    public void ResetStopJob(BackUpJob bj)
    {
        bj.FileTransfert.Reset();
    }

    public void StopBackup(BackUpJob bj)
    {
        bj.Stop();
    }
    public void StopAll()
    {
        foreach (BackUpJob bj in listBackUps)
        {
            bj.Stop();
        }
    }
    public void ExecuteBackup(BackUpJob job)
    {
        job.ResetJob();
        Thread jobThread = new Thread(() =>
        {
            try
            {
                job.Execute(job.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Backup {job.Name} cancelled and cannot be resumed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup {job.Name} encountered an error: {ex.Message}.");
            }
        });
        RunningThreads.Add(jobThread);
        jobThread.Start();
    }

    public void PauseAll()
    {
        foreach(BackUpJob bj in listBackUps)
        {
            PauseBackup(bj);
        }
    }
     

    
    public BackUpJob FindBackupJobById(int indexJob)
    {
        if (indexJob < 0)
        {
            System.Windows.MessageBox.Show(ManageLang.GetString("error_JobSuperior5"), "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
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
        if (listBackUps.Any(j => j.Name == newName))
        {
            System.Windows.MessageBox.Show(ManageLang.GetString("view_add_sameNameJob"), "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        listBackUps[index].Name = newName;
        SaveJobsToJson();
    }
    
    public void UpdateBackUpJobSourceDir(int index, string sourceDir)
    {
        listBackUps[index].SourceDirectory = sourceDir;
        SaveJobsToJson();
    }

    public void UpdateBackUpJobTargetDir(int index, string targetDir)
    {
        if(listBackUps.Any(j => j.TargetDirectory == targetDir))
        {
            System.Windows.MessageBox.Show(ManageLang.GetString("sameTargetDirectoryError"));
            return;
        }
        listBackUps[index].TargetDirectory = targetDir;
        
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
        if (listBackUps.Any(j => j.Name == jobName))
        {
            System.Windows.MessageBox.Show(ManageLang.GetString("view_add_sameNameJob"), "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if(listBackUps.Any(j => j.TargetDirectory == targetDir))
        {
            System.Windows.MessageBox.Show(ManageLang.GetString("sameTargetDirectoryError"));
            return;
        }


        BackUpJob addBackUpJob = BackUpJobFactory.CreateBackupJob(type, jobName, sourceDir, targetDir);
        
        //listBackUps.Add(addBackUpJob);
        listBackUps.Insert(0, addBackUpJob);

        SaveJobsToJson();
    }


         
    public BackUpJob findBackupJobByName(string sJobName)
    {
        foreach (BackUpJob job in listBackUps)
        {
            if (job.Name == sJobName)
            { 
                return job;
            }
        }
        return null;
    }
}
