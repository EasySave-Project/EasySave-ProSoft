
using EasySave.model;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace EasySave.services
{
    public class StateManager
    {
        // Déclaration des variables objet
        public string NameJob { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string State_Text { get; set; }
        public long TotalFileToCopy { get; set; }
        public long TotalFileSize { get; set; }
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

        public void UpdateState_Complete(long NbOctetFile)
        {
            TotalFileToCopy = TotalFileToCopy + NbOctetFile;

            NbFilesLeftToDo = NbFilesLeftToDo - 1;
            Progression = (int)(((float)TotalFileToCopy / (float)TotalFileSize) * 100);

            if (TotalFileSize == TotalFileToCopy)
            {
                State_Text = "END";
            }
            else
            {
                State_Text = "ACTIVE";
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
            long[] result = GetTotalFileSize_Differential(sourcePath, targetPath);

            NameJob = nameJob;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            State_Text = "Initialisation";
            TotalFileToCopy = 0;
            TotalFileSize = result[1];
            NbFilesLeftToDo = (int)result[0];
            Progression = 0;

            SaveState();
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
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                    // Incrémenter le nombre de fichiers différents
                    result[0] = result[0] + 1;
                    // Ajouter la taille du fichier source à la taille cumulée
                    result[1] = result[1] + sourceFile.Length;
                }
            }
            return result;
        }

        public void UpdateState_Differential(long NbOctetFile)
        {
            TotalFileToCopy = TotalFileToCopy + NbOctetFile;

            NbFilesLeftToDo = NbFilesLeftToDo - 1;
            Progression = (int)(((float)TotalFileToCopy / (float)TotalFileSize) * 100);

            if (TotalFileSize == TotalFileToCopy)
            {
                State_Text = "END";
            }
            else
            {
                State_Text = "ACTIVE";
            }

            SaveState();
        }




        //=======================================================================================================
        // Sauvegarde dans le fichier JSON
        //=======================================================================================================
        private void SaveState()
        {
            // FICHIER XML
            //=========================
            string sCurrentDir = Environment.CurrentDirectory;

            string destPath = sCurrentDir + "\\EasySave\\log";

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

            // FICHIER XML
            //=========================
            // Création d'une instance de la classe XmlSerializer pour sérialiser l'objet courant de type LogManager en XML
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogManager));

            // Déclaration et initialisation d'une variable de type chaîne pour stocker le chemin du fichier XML
            string xmlPath = destPath + "\\log_backup.xml";

            // Utilisation d'un bloc using pour créer un flux d'écriture vers le fichier XML
            using (StreamWriter streamWriter = File.AppendText(xmlPath))
            {
                // Création d'une instance de la classe XmlWriterSettings pour configurer le flux d'écriture XML
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.OmitXmlDeclaration = true; // Ne pas écrire la déclaration XML
                xmlWriterSettings.Indent = true; // Indenter le code XML

                // Création d'une instance de la classe XmlWriter pour écrire dans le flux d'écriture
                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings))
                {
                    // Si le fichier XML n'existe pas, écrire la balise racine <Snippets>
                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteStartElement("Snippets");
                    }

                    // Appel de la méthode WriteNode de la classe XmlWriter pour écrire l'objet courant de type LogManager en XML dans le flux d'écriture
                    xmlSerializer.Serialize(xmlWriter, this);

                    // Si le fichier XML n'existe pas, écrire la balise de fermeture </Snippets>
                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteEndElement();
                    }

                    // Fermer le flux d'écriture XML
                    xmlWriter.Close();
                }

                // Fermer le flux d'écriture
                streamWriter.Close();
            }

        }
    }
}