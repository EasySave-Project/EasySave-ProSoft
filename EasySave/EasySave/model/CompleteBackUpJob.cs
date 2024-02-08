using EasySave.view;
using EasySave.utils;
using EasySave.services;

namespace EasySave.model
{
    public class CompleteBackUpJob : BackUpJob
    {
        private static ConsoleView cv = new ConsoleView();
        public CompleteBackUpJob(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {
        }

        public override void Excecute()
        {
            try
            {
                FileUtils.CompleteCopyDirectory(sourceDirectory, targetDirectory);
                Console.WriteLine(cv.GetLineLanguage(50));
                
            }
            catch (Exception e)
            {
                Console.WriteLine(cv.GetLineLanguage(51) + e.Message);
            }
        }
    }
}