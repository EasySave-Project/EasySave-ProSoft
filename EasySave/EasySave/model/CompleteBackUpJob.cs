using EasySave.view;
using EasySave.utils;
using System.Threading;
using EasySave.services;


namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob

    {

        public static long Result { get; internal set; }


        private static ConsoleView cv = new ConsoleView();

        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override BackUpJob CloneToType(Type type)
        {
            if(type == typeof(DifferentialBackUpJob))
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
                long TimeBeforeSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                FileUtils.CompleteCopyDirectory(sourceDirectory, targetDirectory);

                long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Console.WriteLine(ConsoleView.GetLineLanguage(50));
                Result = TimeAfterSave - TimeBeforeSave;
                Console.WriteLine($"Temps d'execution: {Result}");
            }
            catch (Exception e)
            {
                Console.WriteLine(ConsoleView.GetLineLanguage(51) + e.Message);
            }
        }
    }
}
