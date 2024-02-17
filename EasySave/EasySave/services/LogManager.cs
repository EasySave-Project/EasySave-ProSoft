using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using EasySave.model;
using EasySave.controller;
using System.Diagnostics;
using EasySave.utils;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace EasySave
{

    public class LogManager
    {

        //=======================================================================================================
        // Log 
        //=======================================================================================================

        Log log = new Log();
        Settings settings = new Settings();
        DateTime dateHeure = DateTime.Now;
        private long long_FileTransferTime;
        private long long_AfterFileTransferTime;

        public void InitLog(string nameJob, string sourcePath, string targetPath)
        {
            log.Name = nameJob;
            log.FileSource = sourcePath;
            log.FileTarget = targetPath;
            log.FileSize = 0;
            //Time before the transfer
            long_FileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            log.Time = "";
        }


        public void PushLog(long NbOctetFile)
        {
            log.FileSize = NbOctetFile;

            //Time after the transfer
            long_AfterFileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - long_FileTransferTime;

            // Créer un objet TimeSpan à partir des millisecondes
            TimeSpan ts = TimeSpan.FromMilliseconds(long_AfterFileTransferTime);

            // Formater le TimeSpan en HH:MM:SS:MS
            string format = @"hh\:mm\:ss\:fff";
            log.FileTransferTime = ts.ToString(format);

            log.Time = dateHeure.ToString("dd/MM/yyyy HH:mm:ss");

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
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<Log>(log, options);

            // Déclaration et initialisation d'une variable de type chaîne pour stocker le chemin du fichier JSON
            string filePath = destPath + "\\log_backup.json";
            if(settings.LogType == "" || settings.LogType == null)
            {
                settings.LogType = "JSON";
            }

            if (settings.LogType == "JSON")
            {
                // Si le fichier JSON existe déjà dans le dossier de destination
                if (File.Exists(filePath))
                {
                    // Lecture du contenu du fichier JSON existant
                    string oldJson = System.IO.File.ReadAllText(filePath);
                    string newJson = oldJson + "\n" + json;
                    File.WriteAllText(filePath, newJson);
                }
                else
                {
                    filePath = destPath + "\\log_backup.json";
                    File.WriteAllText(filePath, json);
                }
            } else if (settings.LogType == "XML")
            {
                // FICHIER XML
                //=========================

                // Création d'une instance de la classe XmlSerializer pour sérialiser l'objet courant de type LogManager en XML
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Log));

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
                        xmlSerializer.Serialize(xmlWriter, log);

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


}