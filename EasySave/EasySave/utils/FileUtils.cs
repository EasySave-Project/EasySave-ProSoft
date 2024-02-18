using EasySave.view;
using EasySave.services;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
namespace EasySave.utils
{
    
    public static class FileUtils
    {
        // Connexion avec le StateManager
        private static StateManager stateManager = new StateManager();

        private static LogManager logManager = new LogManager();

        private static Settings settings = new Settings();

        public static void DifferentialCopyDirectory(string name, string sourceDir, string targetDir)
        {
            // Vérifier si l'application de la calculatrice Windows est ouverte
            bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
            if (!isNotepadRunning)
            {
                // Si la calculatrice n'est pas ouverte :

                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // créer le répertoire target s'il n'existe pas déjà 
                // on se permet de créer le dossier si il n'est pas déjà créer.
                Directory.CreateDirectory(targetDir);

                CopyModifierOrAddedFile(sourceDir, targetDir, name);
                DeleteObsoleteFiles(sourceDir, targetDir);
                CopySubdirectoriesRecursivelyForDifferential(name, sourceDir, targetDir);
            }
            else
            {
                System.Windows.MessageBox.Show("Erreur : Impossible d'exécuter le Travail, l'application de la calculatrice est ouverte en arrière-plan.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public static void CompleteCopyDirectory(string name, string sourceDir, string targetDir)
        {
            // Vérifier si le processus de la calculatrice est en cours d'exécution
            bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
            if (!isNotepadRunning)
            {
                // Si la calculatrice n'est pas ouverte :

                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // créer le répertoire target s'il n'existe pas déjà 
                // on se permet de créer le dossier si il n'est pas déjà créer.
                Directory.CreateDirectory(targetDir);

                CopyFilesTo(sourceDir, targetDir,name);
                DeleteObsoleteFiles(sourceDir, targetDir);
                CopySubdirectoriesRecursively(name, sourceDir, targetDir);
            }
            else
            {
                System.Windows.MessageBox.Show("Erreur : Impossible d'exécuter le Travail, l'application de la calculatrice est ouverte en arrière-plan.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
                throw new DirectoryNotFoundException(ManageLang.GetString("error_SourcePath_NotFound") + dir);
            }
        }

        private static void VerifyDriveAvailable(string dir)
        {
            if (!DriveInfo.GetDrives().Any(d => d.IsReady && dir.StartsWith(d.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DriveNotFoundException(ManageLang.GetString("view_CantDestPath_1") + dir+ ManageLang.GetString("view_CantDestPath_2"));
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

                    if (settings.ExtensionsToCrypt.Contains(Path.GetExtension(sourceFile.Name).ToLower()))
                    {
                        // todo executer crypto soft sur sourceFile, targetFile
                        string sSourcePath_File = sourceFile.FullName;
                        string sTargetPath_File = targetDir;
                        string sClef = "secret";

                        // Obtenir le fichier ressource
                        var resource = cryptoSoft.ressource_cryptosoft.cryptosoft_V2;
                        // Créer un fichier temporaire avec le contenu du fichier ressource
                        string tempPath = System.IO.Path.GetTempFileName();
                        File.WriteAllBytes(tempPath, resource);
                        // Appeler le .EXE avec les paramètres
                        var process = Process.Start(tempPath, sSourcePath_File + " " + sTargetPath_File + " " + sClef);
                        // Attendre que le .EXE se termine
                        process.WaitForExit();
                        // Supprimer le fichier temporaire
                        File.Delete(tempPath);
                    } else
                    {
                        sourceFile.CopyTo(targetFilePath, true);
                    }
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
