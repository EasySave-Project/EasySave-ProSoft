using EasySave.model;
namespace EasySave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String name = "backUpJob1";
            String sourceDir = @"C:\Users\linol\OneDrive\Bureau\4L_documents";
            String targetDir = @"C:\Users\linol\OneDrive\Bureau\TEST";
            BackUpJob bj = BackUpJobFactory.CreateBackupJob(BackUpType.Complete,  name,  sourceDir, targetDir);
            bj.Excecute();
            Console.ReadKey();
        }
    }
}