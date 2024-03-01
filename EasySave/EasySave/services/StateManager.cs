using EasySave.model;
using System.IO;
using EasySave.utils;

namespace EasySave.services
{
    // The StateManager class is responsible for managing the state of backup jobs within the application.
    // It handles the initialization and updates of states for both complete and differential backups,
    // and supports saving the state in different formats (JSON or XML) based on application settings.
    public class StateManager
    {
        // A flag to prevent recursive calls during state initialization.
        private bool _bSecurityIsRecursive = true;

        // Settings instance to access application configurations, such as state saving format.
        Settings settings = Settings.Instance;

        // The current state of a backup job.
        State state;

        // Strategy for saving the state, allowing for different implementations (JSON, XML).
        IStrategieSave typeSave;

        // A lock object for thread-safe operations on the state.
        private readonly object _lockState = new object();

        // Constructor: Initializes the StateManager by setting up the appropriate state saving strategy
        // based on the application settings. Throws an exception if an invalid state type is configured.
        public StateManager()
        {
            state = new State();
            if (settings.StateType == "Json")
            {
                typeSave = new SaveJson();
            }
            else if (settings.StateType == "Xml")
            {
                typeSave = new SaveXML();
            }
            else
            {
                throw new Exception("State type invalid");
            }
        }

        // Initializes the state for a complete backup job with specific parameters.
        // This method calculates the total file size and number of files to backup,
        // and marks the job's state as "INITIALISATION".
        public void InitState_Complete(string nameJob, string sourcePath, string targetPath)
        {
            if (_bSecurityIsRecursive == true)
            {
                state.NameJob = nameJob;
                state.SourcePath = sourcePath;
                state.TargetPath = targetPath;
                state.State_Text = "INITIALISATION";
                state.TotalFileToCopy = 0;
                state.TotalFileSize = GetTotalFileSize_Complete(sourcePath);
                state.NbFilesLeftToDo = GetNbFilesLeftToDo_Complete(sourcePath);
                state.Progression = 0;
                _bSecurityIsRecursive = false;
                SaveState();
            }
        }

        // Updates the state during a complete backup process by tracking the number of bytes copied
        // and the number of files left to do. It calculates the progression percentage and updates
        // the job's state accordingly.
        public void UpdateState_Complete(long NbOctetFile, string sourcePath, string targetPath)
        {
            state.TotalFileToCopy += NbOctetFile;
            state.NbFilesLeftToDo -= 1;
            state.Progression = (int)(((float)state.TotalFileToCopy / (float)state.TotalFileSize) * 100);
            
            UpdateJobProgress(state.NameJob, state.Progression);


            state.SourcePath = sourcePath;
            state.TargetPath = targetPath;
            if (state.NbFilesLeftToDo == 0)
            {
                state.State_Text = "END";
                _bSecurityIsRecursive = true;
            }
            else
            {
                state.State_Text = "ACTIVE";
            }
            SaveState();
        }

        // Calculates the total file size for a complete backup by summing the sizes of all files
        // in the source directory and its subdirectories.
        private long GetTotalFileSize_Complete(string sourcePath)
        {
            long totalFileSize = 0;
            string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                totalFileSize += fi.Length;
            }
            return totalFileSize;
        }

        // Counts the total number of files to be copied for a complete backup.
        private int GetNbFilesLeftToDo_Complete(string sourcePath)
        {
            string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
            return files.Length;
        }

        // Initializes the state for a differential backup job by determining
        // which files need to be copied based on their existence or modification date
        // in the target directory compared to the source directory.
        public void InitState_Differential(string nameJob, string sourcePath, string targetPath)
        {
            if (_bSecurityIsRecursive == true)
            {
                long[] result = GetTotalFileSize_Differential(sourcePath, targetPath);
                state.NameJob = nameJob;
                state.SourcePath = sourcePath;
                state.TargetPath = targetPath;
                state.State_Text = "Initialisation";
                state.TotalFileToCopy = 0;
                state.TotalFileSize = result[1];
                state.NbFilesLeftToDo = (int)result[0];
                state.Progression = 0;
                _bSecurityIsRecursive = false;
                SaveState();
            }
        }

        // Determines the total file size and the number of files that differ between the source
        // and target directories for a differential backup. It compares file existence and last write time.
        private long[] GetTotalFileSize_Differential(string sourcePath, string targetPath)
        {
            long[] result = new long[2]; // [0]: number of different files, [1]: total size of different files
            var sourceFiles = new DirectoryInfo(sourcePath).GetFiles("*", SearchOption.AllDirectories);
            foreach (var sourceFile in sourceFiles)
            {
                var targetFilePath = Path.Combine(targetPath, sourceFile.FullName.Substring(sourcePath.Length + 1));
                var targetFile = new FileInfo(targetFilePath);
                if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                {
                    result[0]++; // Increment different file count
                    result[1] += sourceFile.Length; // Add file size to total
                }
            }
            return result;
        }

        // Updates the state during a differential backup process similarly to the complete backup,
        // but based on the differential calculation of files to copy.
        public void UpdateState_Differential(long NbOctetFile, string sourcePath, string targetPath)
        {

            state.TotalFileToCopy = state.TotalFileToCopy + NbOctetFile;

            state.NbFilesLeftToDo = state.NbFilesLeftToDo - 1;
            state.Progression = (int)(((float)state.TotalFileToCopy / (float)state.TotalFileSize) * 100);
            UpdateJobProgress(state.NameJob, state.Progression);

            state.SourcePath = sourcePath;
            state.TargetPath = targetPath;

            if (state.NbFilesLeftToDo == 0)
            {
                state.State_Text = "END";
                _bSecurityIsRecursive = true;
            }
            else
            {
                state.State_Text = "ACTIVE";
            }

            SaveState();
        }

        //=======================================================================================================
        // Update Progress bar
        //=======================================================================================================

        public void UpdateJobProgress(string jobName, int progress)
        {
            // Accéder à l'instance unique de Jobs depuis ListJob_ViewModel
            ListJob_ViewModel.GetInstance().UpdateJobProgressInList(jobName, progress);
        }

        //=======================================================================================================
        // Sauvegarde dans le fichier (JSON OU XML)
        //=======================================================================================================


        // Saves the current state using the configured strategy (JSON or XML).
        // This operation is thread-safe, ensured by locking on the `lockState` object.

        private void SaveState()
        {
            lock (_lockState)
            {
                typeSave.SaveState(state);
            }
        }
    }

}