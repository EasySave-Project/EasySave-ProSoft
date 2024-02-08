
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using EasySave.model;
using EasySave.services;
using EasySave.controller;
using System.Diagnostics;
using EasySave.utils;

namespace EasySave
{
    public class LogManager
    {
        // Déclaration des variables objet
        public string? SourcePath { get; set; }
        public string? TargetPath { get; set; }
        public string? NameJob { get; set; }
        public string? BackupDate { get; set; }
        public long TransactionTime { get; set; }
        public long TotalSize { get; set; }



        //=======================================================================================================
        // Complete Version
        //=======================================================================================================



        public void InitLog(string nameJob, string sourcePath, string targetPath)
        {

            NameJob = nameJob;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            BackupDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            TransactionTime = CompleteBackUpJob.Result;
            TotalSize = GetTotalFileSize_Complete(sourcePath);
            SaveLog();
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





        //=======================================================================================================
        // Sauvegarde dans le fichier JSON
        //=======================================================================================================
        private void SaveLog()
        {
            string destPath = "C:\\Users\\linol\\Downloads\\EasySave-ProSoft-dev (1)\\EasySave-ProSoft-dev\\EasySave\\EasySave\\bin\\Log";

            // Appel de la méthode Serialize de la classe JsonSerializer pour convertir l'objet courant de type State en une chaîne JSON
            //string json = JsonSerializer.Serialize<State>(this);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<LogManager>(this, options);

            // Déclaration et initialisation d'une variable de type chaîne pour stocker le chemin du fichier JSON
            string filePath = destPath + "\\log_backup.json";

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
                filePath = destPath + "\\log_backup.json";
                System.IO.File.WriteAllText(filePath, json);
            }

        }


    }


}