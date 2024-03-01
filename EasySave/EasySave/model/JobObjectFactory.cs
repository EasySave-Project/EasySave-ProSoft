using EasySave.services;
using EasySave.utils;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace EasySave.model
{
    public class JobObjectFactory
    {
        private Settings settings_state = new Settings();

        public List<JobObject> CreateJobObject()
        {
            // Création d'une liste vide d'objets Message
            List<JobObject> messages = new List<JobObject>();

            // Parcours de la liste de jobs du client
            int message_Index = 0;
            foreach (BackUpJob bj in BackUpManager.listBackUps)
            {
                // Ajout de l'objet Message à la liste d'objets Message
                JobObject message = new JobObject(message_Index, bj.Name, GetJobProgress(bj.Name));
                messages.Add(message);

                message_Index++;
            }

            // Retour de la liste d'objets Message
            return messages;
        }

        private int GetJobProgress(string jobName)
        {
            int returnProgress = 0;

            if (settings_state.StateType == "Xml")
            {
                // XML
                string filePath = Path.Combine(Environment.CurrentDirectory, "EasySave", "state", $"state_backup_{jobName}.xml");
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // Lecture du contenu du fichier
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string xmlContent = sr.ReadToEnd();
                            string[] stateElements = xmlContent.Split(new string[] { "</State>" }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string stateElement in stateElements)
                            {
                                string stateXml = stateElement + "</State>";

                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(stateXml);

                                XmlNode progressNode = xmlDoc.SelectSingleNode("//Progression");
                                if (progressNode != null)
                                {
                                    returnProgress = int.Parse(progressNode.InnerText);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // JSON
                string filePath = Path.Combine(Environment.CurrentDirectory, "EasySave", "state", $"state_backup_{jobName}.json");
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    int lastProgress = 0;
                    using (var jsonReader = new JsonTextReader(new StringReader(fileContent)) { SupportMultipleContent = true })
                    {
                        var serializer = new JsonSerializer();
                        while (jsonReader.Read())
                        {
                            dynamic fileObject = serializer.Deserialize<dynamic>(jsonReader);
                            lastProgress = fileObject?.Progression ?? lastProgress;
                        }
                        returnProgress = lastProgress;
                    }
                }
            }

            return returnProgress;
        }

    }

}
