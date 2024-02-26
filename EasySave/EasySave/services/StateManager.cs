
using EasySave.model;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using EasySave.utils;
using System.Runtime;

namespace EasySave.services
{
    public class StateManager
    {
        private bool bSecurityIsRecursive = true;

        Settings settings = new Settings();
        State state = new State();
        IStrategieSave typeSave;

        public StateManager()
        {
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

        //=======================================================================================================
        // Complete Version
        //=======================================================================================================
        public void InitState_Complete(string nameJob, string sourcePath, string targetPath)
        {
            if (bSecurityIsRecursive == true)
            {
                state.NameJob = nameJob;
                state.SourcePath = sourcePath;
                state.TargetPath = targetPath;
                state.State_Text = "INITIALISATION";
                state.TotalFileToCopy = 0;
                state.TotalFileSize = GetTotalFileSize_Complete(sourcePath);
                state.NbFilesLeftToDo = GetNbFilesLeftToDo_Complete(sourcePath);
                state.Progression = 0;

                bSecurityIsRecursive = false;
                SaveState();
            }

        }

        public void UpdateState_Complete(long NbOctetFile, string sourcePath, string targetPath)
        {
            state.TotalFileToCopy = state.TotalFileToCopy + NbOctetFile;

            state.NbFilesLeftToDo = state.NbFilesLeftToDo - 1;
            state.Progression = (int)(((float)state.TotalFileToCopy / (float)state.TotalFileSize) * 100);

            state.SourcePath = sourcePath;
            state.TargetPath = targetPath;

            //if (state.TotalFileSize == state.TotalFileToCopy)
            if (state.NbFilesLeftToDo == 0)
            {
                state.State_Text = "END";
                bSecurityIsRecursive = true;
            }
            else
            {
                state.State_Text = "ACTIVE";
            }

            SaveState();
        }

        private int GetTotalFileSize_Complete(string sourcePath)
        {
            int totalFileSize = 0;
            string[] files = System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                totalFileSize += (int)fi.Length;
            }
            return totalFileSize;
        }

        private int GetNbFilesLeftToDo_Complete(string sourcePath)
        {
            int nbFilesLeftToDo = 0;
            string[] files = System.IO.Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories);
            nbFilesLeftToDo = files.Length;
            return nbFilesLeftToDo;
        }

        //=======================================================================================================
        // Differential version
        //=======================================================================================================
        public void InitState_Differential(string nameJob, string sourcePath, string targetPath)
        {
            if (bSecurityIsRecursive == true)
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

                bSecurityIsRecursive = false;
                SaveState();
            }
        }

        private long[] GetTotalFileSize_Differential(string sourcePath, string targetPath)
        {
            long[] result = new long[2];
            result[0] = 0;
            result[1] = 0;

            var sourceFiles = new DirectoryInfo(sourcePath).GetFiles("*", SearchOption.AllDirectories);
            foreach (var sourceFile in sourceFiles)
            {
                var targetFilePath = Path.Combine(targetPath, sourceFile.FullName.Substring(sourcePath.Length + 1));
                var targetFile = new FileInfo(targetFilePath);

                if (!targetFile.Exists || targetFile.LastWriteTime < sourceFile.LastWriteTime)
                {
                    result[0]++; // Incrémenter le nombre de fichiers différents
                    result[1] += sourceFile.Length; // Ajouter la taille du fichier à la taille cumulée
                }
            }

            return result;
        }

        public void UpdateState_Differential(long NbOctetFile, string sourcePath, string targetPath)
        {
            state.TotalFileToCopy = state.TotalFileToCopy + NbOctetFile;

            state.NbFilesLeftToDo = state.NbFilesLeftToDo - 1;
            state.Progression = (int)(((float)state.TotalFileToCopy / (float)state.TotalFileSize) * 100);

            state.SourcePath = sourcePath;
            state.TargetPath = targetPath;

            if (state.NbFilesLeftToDo == 0)
            {
                state.State_Text = "END";
                bSecurityIsRecursive = true;
            }
            else
            {
                state.State_Text = "ACTIVE";
            }

            SaveState();
        }

        //=======================================================================================================
        // Sauvegarde dans le fichier (JSON OU XML)
        //=======================================================================================================
        private void SaveState()
        {
            typeSave.SaveState(state);
        }
    }
}