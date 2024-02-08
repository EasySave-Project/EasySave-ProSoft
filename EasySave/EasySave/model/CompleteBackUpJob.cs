using EasySave.view;
using EasySave.utils;
using System.Threading;
using EasySave.services;


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
               
                FileUtils.CompleteCopyDirectory(name, sourceDirectory, targetDirectory);
                Console.WriteLine(ConsoleView.GetLineLanguage(50));
                
            }
            catch (Exception e)
            {
                Console.WriteLine(ConsoleView.GetLineLanguage(51) + e.Message);
            }
        }
    }
}
