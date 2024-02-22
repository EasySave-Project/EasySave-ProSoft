using EasySave.utils;
using EasySave.view;
using EasySave.services;
using System.Windows;
namespace EasySave.model
{
    public class DifferentialBackUpJob : BackUpJob
    {


        public DifferentialBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
        }
        public override BackUpJob CloneToType(BackUpType type)
        {
            if (type.Equals(BackUpType.Complete))
            {
                CompleteBackUpJob newJob = new CompleteBackUpJob(this.name, this.sourceDirectory, this.targetDirectory);

                return newJob;
            }
            return null;
        }
        public override void Excecute()
        {
            try
            {
                FileUtils.DifferentialCopyDirectory_Priority(name, sourceDirectory, targetDirectory);
                FileUtils.DifferentialCopyDirectory(name, sourceDirectory, targetDirectory);

                System.Windows.MessageBox.Show(ManageLang.GetString("view_exe_successful"), ManageLang.GetString("exe_job_title"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_saveDifferential") + " : " + e.Message, ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

