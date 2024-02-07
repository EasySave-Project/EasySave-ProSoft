using EasySave.model;
namespace EasySave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String name = "backUpJob1";
            String sourceDir = @"/Users/teuletcorentin/Desktop/SourceDir";
            String targetDir = @"/Users/teuletcorentin/Desktop/BackupLogTest";
            BackUpJob bj = BackUpJobFactory.CreateBackupJob(BackUpType.Complete,  name,  sourceDir, targetDir);
            bj.Excecute();
            Console.ReadKey();
        }
    }
}