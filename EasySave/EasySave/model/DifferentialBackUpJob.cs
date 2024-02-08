using EasySave.utils;
using EasySave.view;
using EasySave.services;
namespace EasySave.model
{
    public class DifferentialBackUpJob : BackUpJob
    {

        public static long Result { get; internal set; }

        private static ConsoleView cv = new ConsoleView();

        public DifferentialBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
        }

        public override void Excecute()
        {
            try
            {
                long TimeBeforeSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                FileUtils.DifferentialCopyDirectory(sourceDirectory, targetDirectory);
                long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Result = TimeAfterSave - TimeBeforeSave;
                Console.WriteLine($"Temps d'execution: {Result}");
                Console.WriteLine(cv.GetLineLanguage(53));
            }
            catch (Exception e)
            {
                Console.WriteLine(cv.GetLineLanguage(54) + e.Message);
            }
        }
    }
}

