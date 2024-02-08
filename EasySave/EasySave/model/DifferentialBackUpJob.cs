using EasySave.utils;
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

        public override void Excecute()
        {
            try
            {
                FileUtils.DifferentialCopyDirectory(sourceDirectory, targetDirectory);
                Console.WriteLine("Sauvegarde diff�rentiel r�ussie.");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde compl�te: {e.Message}");
            }
        }
    }
}

