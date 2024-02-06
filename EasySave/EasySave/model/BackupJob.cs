namespace EasySave.model
{
    public abstract class BackUpJob
    {
        public string name { get; set; }
        public string sourceDirectory { get; set; }

        public string targetDirectory { get; set; }


        protected BackUpJob(string name, string sourceDirectory, string targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
        }

        public abstract void Excecute();

    }
}