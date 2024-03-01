using EasySave.view;
using EasySave.services;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using MessageBox = System.Windows.MessageBox;
using System.Threading;
namespace EasySave.utils
{
    
    public class FileUtils
    {
        // Class instantiation
        private StateManager stateManager = new StateManager();

        private LogManager logManager = new LogManager();

        private static Settings settings = Settings.Instance;

        private bool isPaused;


        // String array variable containing two fields: file name and file path
        private static List<string[]> tab_PriorityFiles;


        public FileUtils()
        {
            isPaused = false;
        }
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                isPaused = value;
            }
        }
        public void Pause()
        {
            IsPaused = true;
        }

        public void Reset()
        {
            isPaused=false;
        }
        public void Resume()
        {
            IsPaused = false;
        }



        public void Wait(CancellationToken cancellationToken)
        {
            while (isPaused && !cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }





        public void DifferentialCopyDirectory(string name, string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            // Retrieve the list of processes that should prevent the backup from running
            string[] businessApplication = settings.BusinessApplication.ToArray();
            // See if a process is running
            bool isRunning = false;
            foreach (string process in businessApplication)
            {
                if (Process.GetProcessesByName(process).Length > 0)
                {
                    isRunning = true;
                }
            }           

            if (!isRunning)
            {
                // If the calculator is not open :
                Wait(cancellationToken);
               
                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // create target directory if it doesn't already exist 
                

                cancellationToken.ThrowIfCancellationRequested();


                Directory.CreateDirectory(targetDir);
                CopyModifierOrAddedFile(sourceDir, targetDir, name,cancellationToken);
                DeleteObsoleteFiles(sourceDir, targetDir, cancellationToken);
                CopySubdirectoriesRecursivelyForDifferential(name, sourceDir, targetDir, cancellationToken);
                
            }
            else
            {
                Pause();               
            }
        }
        public  void CompleteCopyDirectory(string name, string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            // Retrieve the list of processes that should prevent the backup from running
            string[] businessApplication = settings.BusinessApplication.ToArray();
            // See if a process is running
            bool isRunning = false;
            foreach (string process in businessApplication)
            {
                if (Process.GetProcessesByName(process).Length > 0)
                {
                    isRunning = true;
                }
            }

            if (!isRunning)
            {
                // If the blocnote is not open :
                cancellationToken.ThrowIfCancellationRequested();
                Wait(cancellationToken);
                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // create target directory if it doesn't already exist 
                // we take the liberty of creating the folder if it hasn't already been created.
                Directory.CreateDirectory(targetDir);
               
                CopyFilesTo(sourceDir, targetDir, name, cancellationToken);
                DeleteObsoleteFiles(sourceDir, targetDir, cancellationToken);
                CopySubdirectoriesRecursively(name, sourceDir, targetDir, cancellationToken);
               
                
                
            }
            else
            {
                Pause();                
            }
        }

        

        public void VerifyDirectoryAndDrive(string sourceDir, string targetDir)
        {
            VerifyDirectoryExists(sourceDir);
            VerifyDriveAvailable(sourceDir);
            VerifyDriveAvailable(targetDir);
        }
        private void VerifyDirectoryExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(ManageLang.GetString("error_SourcePath_NotFound") + dir);
            }
        }

        private void VerifyDriveAvailable(string dir)
        {
            if (!DriveInfo.GetDrives().Any(d => d.IsReady && dir.StartsWith(d.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DriveNotFoundException(ManageLang.GetString("view_CantDestPath_1") + dir+ ManageLang.GetString("view_CantDestPath_2"));
            }
        }

        private void CopyFilesTo(string sourceDir, string targetDir,string name, CancellationToken cancellationToken)
        {

            // Initiating the stateManager and logManager
            logManager.InitLog(name, sourceDir, targetDir);
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                
                string tempPath = Path.Combine(targetDir, file.Name);
                // Check whether the file corresponds to one of the files in the priority table.
                if (CheckFilePriority(file.Name, tempPath))
                {
                    continue;
                }
                // Initiating the stateManager and logManager
                stateManager.InitState_Complete(name, sourceDir, targetDir);

                // Check that the file does not exceed the size limit of Ko
                if (settings.NbKo != -1 && file.Length > settings.NbKo * 1024)
                {
                    continue;
                }
                if (settings.ExtensionsToCrypt.Contains(Path.GetExtension(file.Name).ToLower()))
                {
                    // todo executer crypto soft sur sourceFile, targetFile
                    string sSourcePath_File = Path.Combine(sourceDir, file.Name);
                    string sTargetPath_File = Path.Combine(targetDir, file.Name);
                    string sClef = "secret";
                    Wait(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    FileToCryptoSoft(sSourcePath_File, sTargetPath_File, sClef);
                }
                else
                {
                    Wait(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    file.CopyTo(tempPath, true);
                }
                stateManager.UpdateState_Complete(file.Length, sourceDir, targetDir);

                logManager.PushLog(file.Length);
            }
        }

        private void CopySubdirectoriesRecursively(string name, string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                Wait(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                string tempPath = Path.Combine(targetDir, subdir.Name);
                Directory.CreateDirectory(tempPath); // Ensures that the target subdirectory exists 
                CompleteCopyDirectory(name, subdir.FullName, tempPath,cancellationToken);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir, cancellationToken);
        }
        private  void CopySubdirectoriesRecursivelyForDifferential(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {

            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                Wait(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.ThrowIfCancellationRequested();
                string tempPath = Path.Combine(targetDir, subdir.Name);
                Directory.CreateDirectory(tempPath); // Ensures that the target subdirectory exists
                DifferentialCopyDirectory(name, subdir.FullName, tempPath, cancellationToken);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir, cancellationToken);
        }
        public  void CopyModifierOrAddedFile(string sourceDir, string targetDir,string name,CancellationToken cancellationToken)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles("*", SearchOption.AllDirectories);

            // logManager initialization
            logManager.InitLog(name, sourceDir, targetDir);

            foreach (var sourceFile in sourceFiles)
            {
                var targetFilePath = Path.Combine(targetDir, sourceFile.FullName.Substring(sourceDir.Length + 1));
                var targetFile = new FileInfo(targetFilePath);

                if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                {
                    // Check whether the file corresponds to one of the files in the priority table.
                    if (CheckFilePriority(sourceFile.Name, targetFilePath))
                    {
                        continue;
                    }

                    // stateManager initialization
                    stateManager.InitState_Differential(name, sourceDir, targetDir);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                    // Check that the file does not exceed the size limit of Ko
                    if (settings.NbKo != -1 && sourceFile.Length > settings.NbKo * 1024)
                    {
                        continue;
                    }
                    else
                    {
                        if (settings.ExtensionsToCrypt.Contains(Path.GetExtension(sourceFile.Name).ToLower()))
                        {
                            // todo executer crypto soft sur sourceFile, targetFile
                            string sSourcePath_File = sourceFile.FullName;
                            string sTargetPath_File = targetFilePath; //targetDir
                            string sClef = "secret";
                            Wait(cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                            FileToCryptoSoft(sSourcePath_File, sTargetPath_File, sClef);
                        }
                        else
                        {
                            Wait(cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                            sourceFile.CopyTo(targetFilePath, true);
                        }
                        stateManager.UpdateState_Differential(sourceFile.Length, sourceDir, targetDir);
                        logManager.PushLog(sourceFile.Length);
                    }
                   
                }
            }
        }
        public void DeleteObsoleteFiles(string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles().Select(f => f.Name).ToHashSet();
            foreach (FileInfo file in new DirectoryInfo(targetDir).GetFiles())
            {
                Wait(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (!sourceFiles.Contains(file.Name))
                {
                    file.Delete();
                }
            }
        }

        private void DeleteObsoleteDirectories(string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            var sourceDirs = new DirectoryInfo(sourceDir).GetDirectories().Select(d => d.Name).ToHashSet();
            foreach (DirectoryInfo dir in new DirectoryInfo(targetDir).GetDirectories())
            {
                Wait(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (!sourceDirs.Contains(dir.Name))
                {
                    dir.Delete(true);
                }
            }
        }

        private  void FileToCryptoSoft(string sSourcePath_File, string sTargetPath_File, string sClef)
        {
            string argument = "\"" + sSourcePath_File + "\" \"" + sTargetPath_File + "\" \"" + sClef + "\"";
            // Get the resource file
            var resource = cryptoSoft.ressource_cryptosoft.cryptoSoft_V5;
            // Create a temporary file with the contents of the resource file
            string tempPath_crypto = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(tempPath_crypto, resource);
            // Call .EXE with parameters
            var process = Process.Start(tempPath_crypto, argument);
            // Wait for .EXE to finish
            process.WaitForExit();
            // Delete temporary file
            File.Delete(tempPath_crypto);
        }

        //===============================================================
        // Function to manage the priority table
        //===============================================================

        // Late initialization method for tab_PriorityFiles
        private void InitializeTab_PriorityFiles()
        {
            if (tab_PriorityFiles == null)
            {
                tab_PriorityFiles = new List<string[]>();
            }
        }

        // Add a method to add items to tab_PriorityFiles
        public void AddToTab_PriorityFiles(string fileName, string filePath)
        {
            InitializeTab_PriorityFiles(); // Make sure the list is initialized
            tab_PriorityFiles.Add(new string[] { fileName, filePath });
        }

        // How to check if a file to be copied and pasted has priority
        public bool CheckFilePriority(string fileName, string filePath)
        {
            foreach (var file in tab_PriorityFiles)
            {
                if (file[0] == fileName && file[1] == filePath)
                {
                    // The file and its path correspond to a priority entry
                    return true;
                }
            }
            // The file and its path do not correspond to any priority entry
            return false;
        }


        //===============================================================
        // Function for priority files COMPLETE
        //===============================================================

        // Function for priority files in complete mode
        public void CompleteCopyDirectory_Priority(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {
            
            InitializeTab_PriorityFiles();
            // If there are items in the priority list, we copy them first.
            if (settings.ExtensionsToPriority.Count > 0)
            {
                // Retrieve the list of processes that should prevent the backup from running
                string[] businessApplication = settings.BusinessApplication.ToArray();
                // See if a process is running
                bool isRunning = false;
                foreach (string process in businessApplication)
                {
                    if (Process.GetProcessesByName(process).Length > 0)
                    {
                        isRunning = true;
                    }
                }

                if (!isRunning)
                {
                    // If the blocnote is not open :

                    VerifyDirectoryAndDrive(sourceDir, targetDir);
                    Wait(cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(targetDir);
                    CopyFilesTo_Priority(sourceDir, targetDir, name, cancellationToken);
                    CopySubdirectoriesRecursively_Priority(name, sourceDir, targetDir,cancellationToken);
                   
                    

                }
                else
                {
                    Pause();
                    
                }
            }
        }

        private  void CopyFilesTo_Priority(string sourceDir, string targetDir, string name, CancellationToken cancellationToken)
        {

            // Initiate stateManager and logManager
            logManager.InitLog(name, sourceDir, targetDir);
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                if (settings.ExtensionsToPriority.Contains(Path.GetExtension(file.Name).ToLower()))
                {

                    // Initiating the stateManager and logManager
                    stateManager.InitState_Complete(name, sourceDir, targetDir);
                    string tempPath = Path.Combine(targetDir, file.Name);

                    // Check that the file does not exceed the size limit of Ko
                    if (settings.NbKo != -1 && file.Length > settings.NbKo * 1024)
                    {
                        continue;
                    }
                    else
                    {
                        if (settings.ExtensionsToCrypt.Contains(Path.GetExtension(file.Name).ToLower()))
                        {
                            // todo executer crypto soft sur sourceFile, targetFile
                            string sSourcePath_File = Path.Combine(sourceDir, file.Name);
                            string sTargetPath_File = Path.Combine(targetDir, file.Name);
                            string sClef = "secret";
                            Wait(cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                            FileToCryptoSoft(sSourcePath_File, sTargetPath_File, sClef);
                        }
                        else
                        {
                            Wait(cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                            file.CopyTo(tempPath, true);
                        }
                        // Put the copy file in the priority table
                        AddToTab_PriorityFiles(file.Name, tempPath);

                        stateManager.UpdateState_Complete(file.Length, sourceDir, targetDir);
                        logManager.PushLog(file.Length);
                    }
                }
            }
        }

        private  void CopySubdirectoriesRecursively_Priority(string name, string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                // Copy only folders containing priority files
                if (new DirectoryInfo(subdir.FullName).GetFiles().Any(f => settings.ExtensionsToPriority.Contains(Path.GetExtension(f.Name).ToLower())))
                {
                    string tempPath = Path.Combine(targetDir, subdir.Name);
                    cancellationToken.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(tempPath); // Ensures that the target subdirectory exists
                    CompleteCopyDirectory_Priority(name, subdir.FullName, tempPath, cancellationToken);
                }
            }
        }

        //===============================================================
        // Function for priority files DIFFERENTIAL
        //===============================================================

        // Function for priority files in differential mode
        public void DifferentialCopyDirectory_Priority(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {

            InitializeTab_PriorityFiles();
            // If there are items in the priority list, we copy them first.
            if (settings.ExtensionsToPriority.Count > 0)
            {
                // Retrieve the list of processes that should prevent the backup from running
                string[] businessApplication = settings.BusinessApplication.ToArray();
                // See if a process is running
                bool isRunning = false;
                foreach (string process in businessApplication)
                {
                    if (Process.GetProcessesByName(process).Length > 0)
                    {
                        isRunning = true;
                    }
                }

                if (!isRunning)
                {
                    // If the calculator is not open :

                    VerifyDirectoryAndDrive(sourceDir, targetDir);
                    // create target directory if it doesn't already exist
                    // we take the liberty of creating the folder if it hasn't already been created.
                    Wait(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(targetDir);

                    CopyModifierOrAddedFile_Priority(sourceDir, targetDir, name, cancellationToken);
                    CopySubdirectoriesRecursivelyForDifferential_Priority(name, sourceDir, targetDir,cancellationToken);
                    
                    
                    
                }
                else
                {
                    Pause();
                    
                }
            }
        }

        // Fonction CopyModifierOrAddedFile_Priority
        public void CopyModifierOrAddedFile_Priority(string sourceDir, string targetDir, string name, CancellationToken cancellationToken)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles("*", SearchOption.AllDirectories);

            // logManager initialization
            logManager.InitLog(name, sourceDir, targetDir);

            foreach (var sourceFile in sourceFiles)
            {
                if (settings.ExtensionsToPriority.Contains(Path.GetExtension(sourceFile.Name).ToLower()))
                {
                    var targetFilePath = Path.Combine(targetDir, sourceFile.FullName.Substring(sourceDir.Length + 1));
                    var targetFile = new FileInfo(targetFilePath);

                    if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                    {
                        // LogManager initialization
                        stateManager.InitState_Differential(name, sourceDir, targetDir);

                        Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                        // Check that the file does not exceed the size limit of Ko
                        if (settings.NbKo != -1 && sourceFile.Length > settings.NbKo * 1024)
                        {
                            continue;
                        }
                        else
                        {
                            if (settings.ExtensionsToCrypt.Contains(Path.GetExtension(sourceFile.Name).ToLower()))
                            {
                                // todo executer crypto soft sur sourceFile, targetFile
                                string sSourcePath_File = sourceFile.FullName;
                                string sTargetPath_File = targetFilePath; //targetDir
                                string sClef = "secret";
                                Wait(cancellationToken);
                                cancellationToken.ThrowIfCancellationRequested();
                                FileToCryptoSoft(sSourcePath_File, sTargetPath_File, sClef);
                            }
                            else
                            {
                                Wait(cancellationToken);
                                cancellationToken.ThrowIfCancellationRequested();
                                sourceFile.CopyTo(targetFilePath, true);
                            }
                            // Put the copy file in the priority table
                            AddToTab_PriorityFiles(sourceFile.Name, targetFilePath);

                            stateManager.UpdateState_Differential(sourceFile.Length, sourceDir, targetDir);
                            logManager.PushLog(sourceFile.Length);
                        }
                    }
                }
            }
        }

        // Fonction CopySubdirectoriesRecursivelyForDifferential_Priority
        private  void CopySubdirectoriesRecursivelyForDifferential_Priority(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                // Copy only folders containing priority files
                if (new DirectoryInfo(subdir.FullName).GetFiles().Any(f => settings.ExtensionsToPriority.Contains(Path.GetExtension(f.Name).ToLower())))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string tempPath = Path.Combine(targetDir, subdir.Name);
                    Directory.CreateDirectory(tempPath); // Ensures that the target subdirectory exists
                    DifferentialCopyDirectory_Priority(name, subdir.FullName, tempPath, cancellationToken);
                }
            }
        }

        
    }
}
