using EasySave.model;
namespace EasySave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String name = "backUpJob1";
            String sourceDir = @"C:\mt103\";
            String targetDir = @"C:\sauve\";
            BackUpJob bj = BackUpJobFactory.CreateBackupJob(BackUpType.Complete,  name,  sourceDir, targetDir);
            bj.Excecute();
            Console.ReadKey();
        }
    }
}