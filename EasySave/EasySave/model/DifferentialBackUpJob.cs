using EasySave.utils;
using EasySave.view;
using EasySave.services;
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
                FileUtils.DifferentialCopyDirectory(name, sourceDirectory, targetDirectory);
                Console.WriteLine(ManageLang.GetString("view_SaveDifferential"));
            }
            catch (Exception e)
            {
                Console.WriteLine(ManageLang.GetString("error_saveDifferential") + " : " + e.Message);
            }
        }
    }
}

