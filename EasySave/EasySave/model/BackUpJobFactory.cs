using EasySave.view;
using EasySave.services;

namespace EasySave.model
{
    public static class BackUpJobFactory
    {
        private static ConsoleView cv = new ConsoleView();
        public static BackUpJob CreateBackupJob(BackUpType type, string name, string sourceDir, string targetDir)
        {
            switch (type)
            {
                case BackUpType.Complete:
                    return new CompleteBackUpJob(name, sourceDir, targetDir);
                case BackUpType.Differential:
                    return new DifferentialBackUpJob(name, sourceDir, targetDir);
                default:
                    throw new ArgumentException(cv.GetLineLanguage(52));
            }
        }

    }
}