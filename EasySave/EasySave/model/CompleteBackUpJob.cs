using EasySave.view;
using EasySave.utils;
using System.Threading;
using EasySave.services;
using System.Windows;


namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob

    {

       

        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override BackUpJob CloneToType(BackUpType type)
        {
            if(type.Equals(BackUpType.Differential))
            {
                DifferentialBackUpJob newJob = new DifferentialBackUpJob(name, sourceDirectory, targetDirectory);
                
                return newJob;
            }
            return null; 
        }


        public override void Excecute()
        {
            try
            {
                FileUtils.CompleteCopyDirectory_Priority(name, sourceDirectory, targetDirectory);
                FileUtils.CompleteCopyDirectory(name, sourceDirectory, targetDirectory);
                
                System.Windows.MessageBox.Show(ManageLang.GetString("view_exe_successful"), ManageLang.GetString("exe_job_title"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_saveComplete") + " : " + e.Message, ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
