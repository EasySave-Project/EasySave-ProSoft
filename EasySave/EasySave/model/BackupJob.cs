namespace EasySave.model
{
    public class BackupJob
    {
        
        public string sourceDirectory { get; set; }
        
        public string targetDirectory { get; set; }
        
        public string name { get; set; }
        
        public char type { get; set; }

        public BackupJob(String sourceDirectory, String targetDirectory, string name, char type)
        {
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            this.name = name;
            this.type = type;
        }


    }
}