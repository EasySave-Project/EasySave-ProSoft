
using EasySave.model;
using System.Text.Json;

namespace EasySave.services
{
    public class StateManager
    {
        // Déclaration des variables objet
        public string NameJob { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string State_Text { get; set; }
        public int TotalFileToCopy { get; set; }
        public int TotalFileSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; }

        //=======================================================================================================
        // Complete Version
        //=======================================================================================================
        public void InitState_Complete(string nameJob, string sourcePath, string targetPath)
        {
            NameJob = nameJob;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            State_Text = "INITIALISATION";
            TotalFileToCopy = 0;
            TotalFileSize = GetTotalFileSize_Complete(sourcePath);
            NbFilesLeftToDo = GetNbFilesLeftToDo_Complete(sourcePath);
            Progression = 0;

            SaveState();
        }

        public void UpdateState_Complete()
        {
            State_Text = "ACTIVE";

            //Code => pour savoir le fichier sélectionné
            //TotalFileToCopy = totalFileToCopy;

            NbFilesLeftToDo = NbFilesLeftToDo - 1;
            Progression = (int)(((float)TotalFileToCopy / (float)TotalFileSize) * 100);
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
            foreach (string file in files)
            {
                nbFilesLeftToDo++;
            }
            return nbFilesLeftToDo;
        }

        //=======================================================================================================
        // Differential version
        //=======================================================================================================
        public void InitState_Differential(string nameJob, string sourcePath, string targetPath)
        {
            NameJob = nameJob;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            State_Text = "Initialisation";
            TotalFileToCopy = 0;
            TotalFileSize = 0;
            NbFilesLeftToDo = 0;
            Progression = 0;
        }

        //=======================================================================================================
        // Sauvegarde dans le fichier JSON
        //=======================================================================================================
        private void SaveState()
        {
            string sCurrentDir = Environment.CurrentDirectory;

            string destPath = sCurrentDir + "EasySave\\log";

            // Appel de la méthode Serialize de la classe JsonSerializer pour convertir l'objet courant de type State en une chaîne JSON
            //string json = JsonSerializer.Serialize<State>(this);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<StateManager>(this, options);

            // Déclaration et initialisation d'une variable de type chaîne pour stocker le chemin du fichier JSON
            string filePath = destPath + "\\state_backup.json";

            // Si le fichier JSON existe déjà dans le dossier de destination
            if (System.IO.File.Exists(filePath))
            {
                // Lecture du contenu du fichier JSON existant
                string oldJson = System.IO.File.ReadAllText(filePath);
                string newJson = oldJson + "\n" + json;
                System.IO.File.WriteAllText(filePath, newJson);
            }
            else
            {
                filePath = destPath + "\\state_backup.json";
                System.IO.File.WriteAllText(filePath, json);
            }

        }
    }
}