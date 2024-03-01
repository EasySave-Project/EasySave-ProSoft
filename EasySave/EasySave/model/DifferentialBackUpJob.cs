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
        public override void Excecute(CancellationToken cs)
        {
            try
            {
                base.fileTransfer.DifferentialCopyDirectory_Priority(name, sourceDirectory, targetDirectory,cs);
                base.fileTransfer.DifferentialCopyDirectory(name, sourceDirectory, targetDirectory, cs);
            }
            catch (Exception e)
            {
               // System.Windows.MessageBox.Show(ManageLang.GetString("error_saveDifferential") + " : " + e.Message, ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

