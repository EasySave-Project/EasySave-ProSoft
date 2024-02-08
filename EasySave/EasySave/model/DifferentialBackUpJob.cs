using EasySave.utils;
using EasySave.view;
using EasySave.services;
namespace EasySave.model
{
    public class DifferentialBackUpJob : BackUpJob
    {
<<<<<<< HEAD
        public static long Result { get; internal set; }

=======
        private static ConsoleView cv = new ConsoleView();
>>>>>>> lang
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
<<<<<<< HEAD
                long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Console.WriteLine("Sauvegarde complète réussie.");
                Result = TimeAfterSave - TimeBeforeSave;
                Console.WriteLine($"Temps d'execution: {Result}");
=======
                Console.WriteLine(cv.GetLineLanguage(53));

>>>>>>> lang
            }
            catch (Exception e)
            {
                Console.WriteLine(cv.GetLineLanguage(54) + e.Message);
            }
        }
    }
}

