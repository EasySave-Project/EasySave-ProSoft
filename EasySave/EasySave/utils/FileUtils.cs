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
        // Instanciation des classes
        private StateManager stateManager = new StateManager();

        private LogManager logManager = new LogManager();

        private static Settings settings = new Settings();

        private bool isPaused;


        // Variable tableau string contenant deux champs : nom du fichier et le path du fichier
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
            // Vérifier si l'application de la calculatrice Windows est ouverte
            bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
           

            if (!isNotepadRunning)
            {
                // Si la calculatrice n'est pas ouverte :
                Wait(cancellationToken);
               
                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // créer le répertoire target s'il n'existe pas déjà 
                // on se permet de créer le dossier si il n'est pas déjà créer.

                cancellationToken.ThrowIfCancellationRequested();


                Directory.CreateDirectory(targetDir);
                CopyModifierOrAddedFile(sourceDir, targetDir, name,cancellationToken);
                DeleteObsoleteFiles(sourceDir, targetDir, cancellationToken);
                CopySubdirectoriesRecursivelyForDifferential(name, sourceDir, targetDir, cancellationToken);
               
                
            }
            else
            {
                Pause();
               // System.Windows.MessageBox.Show(ManageLang.GetString("error_notepad_open"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public  void CompleteCopyDirectory(string name, string sourceDir, string targetDir,CancellationToken cancellationToken)
        {
            
            // Vérifier si le processus de la calculatrice est en cours d'exécution
            bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
            if (!isNotepadRunning)
            {
                // Si le blocnote n'est pas ouverte :
                cancellationToken.ThrowIfCancellationRequested();
                Wait(cancellationToken);
                VerifyDirectoryAndDrive(sourceDir, targetDir);
                // créer le répertoire target s'il n'existe pas déjà 
                // on se permet de créer le dossier si il n'est pas déjà créer.
                Directory.CreateDirectory(targetDir);
               
                CopyFilesTo(sourceDir, targetDir, name, cancellationToken);
                DeleteObsoleteFiles(sourceDir, targetDir, cancellationToken);
                CopySubdirectoriesRecursively(name, sourceDir, targetDir, cancellationToken);
               
                
                
            }
            else
            {
                Pause();
                //System.Windows.MessageBox.Show(ManageLang.GetString("error_notepad_open"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
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
            
            // Initilisation du stateManager et du logManager
            logManager.InitLog(name, sourceDir, targetDir);
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                
                string tempPath = Path.Combine(targetDir, file.Name);
                // Vérification si le fichier correspond à un des fichiers du tablea prioritaire
                if (CheckFilePriority(file.Name, tempPath))
                {
                    continue;
                }
                // Initilisation du stateManager et du logManager
                stateManager.InitState_Complete(name, sourceDir, targetDir);

                // Vérifier si le fichier ne dépasse pas la taille limite de Ko
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
                Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe 
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
                Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                DifferentialCopyDirectory(name, subdir.FullName, tempPath, cancellationToken);
            }

            DeleteObsoleteDirectories(sourceDir, targetDir, cancellationToken);
        }
        public  void CopyModifierOrAddedFile(string sourceDir, string targetDir,string name,CancellationToken cancellationToken)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles("*", SearchOption.AllDirectories);

            // initilisation du logManager
            logManager.InitLog(name, sourceDir, targetDir);

            foreach (var sourceFile in sourceFiles)
            {
                var targetFilePath = Path.Combine(targetDir, sourceFile.FullName.Substring(sourceDir.Length + 1));
                var targetFile = new FileInfo(targetFilePath);

                if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                {
                    // Vérification si le fichier correspond à un des fichiers du tablea prioritaire
                    if (CheckFilePriority(sourceFile.Name, targetFilePath))
                    {
                        continue;
                    }
                    
                    // initilisation du stateManager
                    stateManager.InitState_Differential(name, sourceDir, targetDir);

                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                    // Vérifier si le fichier ne dépasse pas la taille limite de Ko
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
            // Obtenir le fichier ressource
            var resource = cryptoSoft.ressource_cryptosoft.cryptoSoft_V4;
            // Créer un fichier temporaire avec le contenu du fichier ressource
            string tempPath_crypto = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(tempPath_crypto, resource);
            // Appeler le .EXE avec les paramètres
            var process = Process.Start(tempPath_crypto, argument);
            // Attendre que le .EXE se termine
            process.WaitForExit();
            // Supprimer le fichier temporaire
            File.Delete(tempPath_crypto);
        }

        //===============================================================
        // Fonction pour gérer le tableau de priorité
        //===============================================================

        // Méthode d'initialisation tardive pour tab_PriorityFiles
        private void InitializeTab_PriorityFiles()
        {
            if (tab_PriorityFiles == null)
            {
                tab_PriorityFiles = new List<string[]>();
            }
        }

        // Ajouter une méthode pour ajouter des éléments à tab_PriorityFiles
        public void AddToTab_PriorityFiles(string fileName, string filePath)
        {
            InitializeTab_PriorityFiles(); // Assurez-vous que la liste est initialisée
            tab_PriorityFiles.Add(new string[] { fileName, filePath });
        }

        // Méthode pour vérifier si un fichier à copier coller est prioritaire
        public  bool CheckFilePriority(string fileName, string filePath)
        {
            foreach (var file in tab_PriorityFiles)
            {
                if (file[0] == fileName && file[1] == filePath)
                {
                    // Le fichier et son chemin d'accès correspondent à une entrée de priorité
                    return true;
                }
            }
            // Le fichier et son chemin d'accès ne correspondent à aucune entrée de priorité
            return false;
        }



        //===============================================================
        // Fonction pour les fichiers prioritaires COMPLETE
        //===============================================================

        // Fonction pour les fichiers prioritaires en mode complete
        public  void CompleteCopyDirectory_Priority(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {
            
            InitializeTab_PriorityFiles();
            // Si il y a des choses dans la liste de priorité, on les copie en priorité
            if (settings.ExtensionsToPriority.Count > 0)
            {       
                // Vérifier si le processus de la calculatrice est en cours d'exécution
                bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
                if (!isNotepadRunning)
                {
                    // Si le blocnote n'est pas ouverte :

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
                    //System.Windows.MessageBox.Show(ManageLang.GetString("error_notepad_open"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private  void CopyFilesTo_Priority(string sourceDir, string targetDir, string name, CancellationToken cancellationToken)
        {

            // Initilisation du stateManager et du logManager
            logManager.InitLog(name, sourceDir, targetDir);
            foreach (FileInfo file in new DirectoryInfo(sourceDir).GetFiles())
            {
                if (settings.ExtensionsToPriority.Contains(Path.GetExtension(file.Name).ToLower()))
                {
                   
                    // Initilisation du stateManager et du logManager
                    stateManager.InitState_Complete(name, sourceDir, targetDir);
                    string tempPath = Path.Combine(targetDir, file.Name);

                    // Vérifier si le fichier ne dépasse pas la taille limite de Ko
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
                        // Mettre le fichier copier dans le tableau de priorité
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
                // Copier seulement les dossiers qui contiennent des fichiers prioritaires
                if (new DirectoryInfo(subdir.FullName).GetFiles().Any(f => settings.ExtensionsToPriority.Contains(Path.GetExtension(f.Name).ToLower())))
                {
                    string tempPath = Path.Combine(targetDir, subdir.Name);
                    cancellationToken.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                    CompleteCopyDirectory_Priority(name, subdir.FullName, tempPath, cancellationToken);
                }
            }
        }

        //===============================================================
        // Fonction pour les fichiers prioritaires DIFFERENTIAL
        //===============================================================

        // Fonction pour les fichiers prioritaires en mode différentiel
        public  void DifferentialCopyDirectory_Priority(string name, string sourceDir, string targetDir, CancellationToken cancellationToken)
        {

            InitializeTab_PriorityFiles();
            // Si il y a des choses dans la liste de priorité, on les copie en priorité
            if (settings.ExtensionsToPriority.Count > 0)
            {
                // Vérifier si l'application de la calculatrice Windows est ouverte
                bool isNotepadRunning = Process.GetProcessesByName("notepad").Length > 0;
                if (!isNotepadRunning)
                {
                    // Si la calculatrice n'est pas ouverte :

                    VerifyDirectoryAndDrive(sourceDir, targetDir);
                    // créer le répertoire target s'il n'existe pas déjà 
                    // on se permet de créer le dossier si il n'est pas déjà créer.
                    Wait(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    Directory.CreateDirectory(targetDir);

                    CopyModifierOrAddedFile_Priority(sourceDir, targetDir, name, cancellationToken);
                    CopySubdirectoriesRecursivelyForDifferential_Priority(name, sourceDir, targetDir,cancellationToken);
                    
                    
                    
                }
                else
                {
                    Pause();
                    //System.Windows.MessageBox.Show(ManageLang.GetString("error_notepad_open"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Fonction CopyModifierOrAddedFile_Priority
        public void CopyModifierOrAddedFile_Priority(string sourceDir, string targetDir, string name, CancellationToken cancellationToken)
        {
            var sourceFiles = new DirectoryInfo(sourceDir).GetFiles("*", SearchOption.AllDirectories);

            // initilisation du logManager
            logManager.InitLog(name, sourceDir, targetDir);

            foreach (var sourceFile in sourceFiles)
            {
                if (settings.ExtensionsToPriority.Contains(Path.GetExtension(sourceFile.Name).ToLower()))
                {
                    var targetFilePath = Path.Combine(targetDir, sourceFile.FullName.Substring(sourceDir.Length + 1));
                    var targetFile = new FileInfo(targetFilePath);

                    if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                    {
                        // initilisation du stateManager
                        stateManager.InitState_Differential(name, sourceDir, targetDir);

                        Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                        // Vérifier si le fichier ne dépasse pas la taille limite de Ko
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
                            // Mettre le fichier copier dans le tableau de priorité
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
                // Copier seulement les dossiers qui contiennent des fichiers prioritaires
                if (new DirectoryInfo(subdir.FullName).GetFiles().Any(f => settings.ExtensionsToPriority.Contains(Path.GetExtension(f.Name).ToLower())))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string tempPath = Path.Combine(targetDir, subdir.Name);
                    Directory.CreateDirectory(tempPath); // Assure que le sous-répertoire cible existe
                    DifferentialCopyDirectory_Priority(name, subdir.FullName, tempPath, cancellationToken);
                }
            }
        }

        
    }
}
