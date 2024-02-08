using EasySave.view;
using EasySave.utils;
<<<<<<< HEAD
using System.Threading;
=======
using EasySave.services;
>>>>>>> lang

namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob

    {
<<<<<<< HEAD
        public static long Result { get; internal set; }

=======
        private static ConsoleView cv = new ConsoleView();
>>>>>>> lang
        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override void Excecute()
        {
            try
            {
                long TimeBeforeSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                FileUtils.CompleteCopyDirectory(sourceDirectory, targetDirectory);
<<<<<<< HEAD
                long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Console.WriteLine("Sauvegarde complète réussie.");
                Result = TimeAfterSave - TimeBeforeSave;
                Console.WriteLine($"Temps d'execution: {Result}");
=======
                Console.WriteLine(cv.GetLineLanguage(50));
                
>>>>>>> lang
            }
            catch (Exception e)
            {
                Console.WriteLine(cv.GetLineLanguage(51) + e.Message);
            }
        }
    }
}
