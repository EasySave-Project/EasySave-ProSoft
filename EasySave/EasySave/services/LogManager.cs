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
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace EasySave
{

    public class LogManager
    {
        // Déclaration des variables objet
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public long FileTransferTime { get; set; }
        public string Time { get; set; }


        //=======================================================================================================
        // Log 
        //=======================================================================================================

        DateTime dateHeure = DateTime.Now;

        public void InitLog(string nameJob, string sourcePath, string targetPath)
        {
            Name = nameJob;
            FileSource = sourcePath;
            FileTarget = targetPath;
            FileSize = 0;
            //Time before the transfer
            FileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Time = "";

            //long TimeBeforeSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //long TimeAfterSave = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }


        public void PushLog(long NbOctetFile)
        {
            FileSize = NbOctetFile;

            //Time after the transfer
            FileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - FileTransferTime;

            Time = dateHeure.ToString("dd/MM/yyyy HH:mm:ss");

            SaveLog();
        }





        //=======================================================================================================
        // Sauvegarde dans le fichier JSON
        //=======================================================================================================
        private void SaveLog()
        {
            // FICHIER JSON
            //=========================
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\log";

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