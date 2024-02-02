namespace EasySave.model
{
    public class BackupJob
    {
        
        private string _sourceDirectory { get; set; }

        private string _targetDirectory { get; set; }

        private string _name { get; set; }

        private char _type { get; set; }

        public BackupJob(String sourceDirectory, String targetDirectory, string name, char type)
        {
            this._sourceDirectory = sourceDirectory;
            this._targetDirectory = targetDirectory;
            this._name = name;
            this._type = type;
        }


    }
}