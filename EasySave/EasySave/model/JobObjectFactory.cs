using EasySave.services;
using EasySave.utils;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace EasySave.model
{
    public class JobObjectFactory
    {
        private Settings settings_state = Settings.Instance;

        public List<JobObject> CreateJobObject()
        {
            // Message constructor
            List<JobObject> messages = new List<JobObject>();

            // Browse the customer's job list
            int message_Index = 0;
            foreach (BackUpJob bj in BackUpManager.listBackUps)
            {
                // Add Message object to Message object list
                JobObject message = new JobObject(message_Index, bj.Name, GetJobProgress(bj.Name));
                messages.Add(message);

                message_Index++;
            }

            // Add Message object to Message object list
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
                        // Add Message object to Message object list
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
