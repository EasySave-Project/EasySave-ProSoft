using EasySave.utils;
namespace EasySave.model
{
    public class DifferentialBackUpJob : BackUpJob
    {
        public static long Result { get; internal set; }

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

