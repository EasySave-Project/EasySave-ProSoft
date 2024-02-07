using System;
using System.IO;
using Newtonsoft.Json;

namespace EasySave.model
{
    public class LogManager
    {
        private readonly string _logFilePath;

        public LogManager()
        {
            // Construit le chemin vers le répertoire de logs dans le dossier 'bin' de l'application
            var logDirectory = "./bin/";


            // Assurez-vous que le répertoire de logs existe
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Configure le chemin du fichier de log pour inclure la date
            _logFilePath = Path.Combine(logDirectory, $"DailyLogs_{DateTime.Now:dd-MM-yyyy}.json");

            // La vérification de l'existence du fichier et sa création si nécessaire est déjà couverte
            // Vous n'avez pas besoin de répéter cette vérification ici
        }

        public void WriteLog(BackUpJob job, TimeSpan transferTime, long totalSize)
        {
            try
            {
                var logEntry = new
                {
                    SaveName = job.name,
                    SourceDir = job.sourceDirectory,
                    TargetDir = job.targetDirectory,
                    BackupDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    TransactionTime = transferTime.ToString(@"hh\:mm\:ss"),
                    TotalSize = totalSize
                };

                string json = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
                // Écriture en append pour ajouter à la fin du fichier sans écraser le contenu existant
                File.AppendAllText(_logFilePath, json + Environment.NewLine);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de l'écriture dans le fichier de log : {e.Message}");
            }
        }

        public void WriteError(Exception e)
        {
            try
            {
                var errorLogEntry = new
                {
                    Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    Message = e.Message,
                    StackTrace = e.StackTrace
                };

                string json = JsonConvert.SerializeObject(errorLogEntry, Formatting.Indented);
                // Utilise AppendAllText pour ajouter les erreurs sans écraser les logs existants
                File.AppendAllText(_logFilePath, json + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture de l'erreur dans le fichier de log : {ex.Message}");
            }
        }
    }
}
