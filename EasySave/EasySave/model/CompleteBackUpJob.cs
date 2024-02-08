
using EasySave.utils;
using System.Threading;

namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob

    {
        public static long Result { get; internal set; }

        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override void Excecute()
        {
            try
            {
                long TimeBeforeSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                FileUtils.CompleteCopyDirectory(sourceDirectory, targetDirectory);
                long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Console.WriteLine("Sauvegarde complète réussie.");
                Result = TimeAfterSave - TimeBeforeSave;
                Console.WriteLine($"Temps d'execution: {Result}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde complète: {e.Message}");
            }
        }
    }
}
