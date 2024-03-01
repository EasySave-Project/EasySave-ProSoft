using EasySave.utils;

namespace EasySave.model
{
    public abstract class BackUpJob
    {
        /** Name of the job */
        protected string name;
        
        protected string sourceDirectory;
        

        protected string targetDirectory;

        protected FileUtils fileTransfer;
        
        protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        
        protected BackUpJob(string name, string sourceDirectory, string targetDirectory)
        {
            this.name = name;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            fileTransfer = new FileUtils();
            
        }

        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
        }
        public string TargetDirectory { 
            get { 
                return targetDirectory; 
            }
            set { 
                targetDirectory = value; 
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string SourceDirectory
        {
            get
            {
                return sourceDirectory;
            }
            set
            {
                sourceDirectory = value;
            }
        }
        public FileUtils FileTransfert
        {
            get
            {
                return fileTransfer;
            }
            set
            {
                fileTransfer = value;
            }
        }
        public abstract void Execute(CancellationToken cs);

        public abstract BackUpJob CloneToType(BackUpType type);

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
        
        public void ResetJob()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

    }
}