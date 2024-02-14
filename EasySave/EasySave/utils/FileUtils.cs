using EasySave.view;
using EasySave.services;
using System.Xml.Linq;
using System.IO;
namespace EasySave.utils
{
    
    public static class FileUtils
    {
        // Connexion avec le StateManager
        private static StateManager stateManager = new StateManager();

        private static LogManager logManager = new LogManager();    
        public static void DifferentialCopyDirectory(string name, string sourceDir, string targetDir)
        {
            VerifyDirectoryAndDrive(sourceDir, targetDir);
            // créer le répertoire target s'il n'existe pas déjà 
            // on se permet de créer le dossier si il n'est pas déjà créer.
            Directory.CreateDirectory(targetDir);


            CopyModifierOrAddedFile(sourceDir, targetDir, name);
            DeleteObsoleteFiles(sourceDir, targetDir);
            CopySubdirectoriesRecursivelyForDifferential(name, sourceDir, targetDir);
        }
        public static void CompleteCopyDirectory(string name, string sourceDir, string targetDir)
        {

            VerifyDirectoryAndDrive(sourceDir, targetDir);
            // créer le répertoire target s'il n'existe pas déjà 
            // on se permet de créer le dossier si il n'est pas déjà créer.
            Directory.CreateDirectory(targetDir);

            CopyFilesTo(sourceDir, targetDir,name);
            DeleteObsoleteFiles(sourceDir, targetDir);
            CopySubdirectoriesRecursively(name, sourceDir, targetDir);

        }

        public static void VerifyDirectoryAndDrive(string sourceDir, string targetDir)
        {
            VerifyDirectoryExists(sourceDir);
            VerifyDriveAvailable(sourceDir);
            VerifyDriveAvailable(targetDir);
        }
        private static void VerifyDirectoryExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(ConsoleView.GetLineLanguage(59) + dir);
            }
        }

        private static void VerifyDriveAvailable(string dir)
        {
            if (!DriveInfo.GetDrives().Any(d => d.IsReady && dir.StartsWith(d.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DriveNotFoundException(ConsoleView.GetLineLanguage(61)+ dir+ ConsoleView.GetLineLanguage(62));
            }
        }

        private static void CopyFilesTo(string sourceDir, string targetDir,string name)
        {
            // initilisation du stateManager
            stateManager.InitState_Complete(name, sourceDir, targetDir);
            logManager.InitLog(name, sourceDir, targetDir);
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                string tempPath = Path.Combine(targetDir, file.Name);
                file.CopyTo(tempPath, true);
                stateManager.UpdateState_Complete(file.Length);
                logManager.PushLog(file.Length);
            }
        }
        private static void CopySubdirectoriesRecursively(string name, string sourceDir, string targetDir)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                string tempPath = Path.Combine(targetDir, subdir.Name);
                Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                CompleteCopyDirectory(name, subdir.FullName, tempPath);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir);
        }
        private static void CopySubdirectoriesRecursivelyForDifferential(string name, string sourceDir, string targetDir)
        {
            foreach (DirectoryInfo subdir in new DirectoryInfo(sourceDir).GetDirectories())
            {
                string tempPath = Path.Combine(targetDir, subdir.Name);
                Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                DifferentialCopyDirectory(name, subdir.FullName, tempPath);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir);
        }
        public static void CopyModifierOrAddedFile(string sourceDir, string targetDir,string name)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles("*", SearchOption.AllDirectories);

            // initilisation du stateManager
            stateManager.InitState_Differential(name, sourceDir, targetDir);
            logManager.InitLog(name, sourceDir, targetDir);

            foreach (var sourceFile in sourceFiles)
            {
                var targetFilePath = Path.Combine(targetDir, sourceFile.FullName.Substring(sourceDir.Length + 1));
                var targetFile = new FileInfo(targetFilePath);

                if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                    sourceFile.CopyTo(targetFilePath, true);
                    stateManager.UpdateState_Differential(sourceFile.Length);
                    logManager.PushLog(sourceFile.Length);
                }
            }
        }
        public static void DeleteObsoleteFiles(string sourceDir, string targetDir)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles().Select(f => f.Name).ToHashSet();
            foreach (FileInfo file in new DirectoryInfo(targetDir).GetFiles())
            {
                if (!sourceFiles.Contains(file.Name))
                {
                    file.Delete();
                }
            }
        }

        private static void DeleteObsoleteDirectories(string sourceDir, string targetDir)
        {
            var sourceDirs = new DirectoryInfo(sourceDir).GetDirectories().Select(d => d.Name).ToHashSet();
            foreach (DirectoryInfo dir in new DirectoryInfo(targetDir).GetDirectories())
            {
                if (!sourceDirs.Contains(dir.Name))
                {
                    dir.Delete(true);
                }
            }
        }

        

        

    }
}
