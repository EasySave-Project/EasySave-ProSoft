
using EasySave.utils;

namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob
    {
        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override void Excecute()
        {
            try
            {
                FileUtils.CompleteCopyDirectory(sourceDirectory, targetDirectory);
                Console.WriteLine("Sauvegarde compl�te r�ussie.");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde compl�te: {e.Message}");
            }
        }
    }
}