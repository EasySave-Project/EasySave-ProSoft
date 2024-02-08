using EasySave.model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EasySave.view;
using EasySave.services;


namespace EasySave.utils
{
    public static class JsonUtils
    {



        public static string sCurrentDir = Environment.CurrentDirectory;

        public static string filePath = sCurrentDir + "\\EasySave\\conf\\SaveBackUpJob.json";

        public static List<BackUpJob> LoadJobsFromJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<BackUpJob>();
                }
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
                return JsonConvert.DeserializeObject<List<BackUpJob>>(json, settings) ?? new List<BackUpJob>();
            }
            catch (Exception e)
            {
                Console.WriteLine(ConsoleView.GetLineLanguage(63) + e.Message);
                return new List<BackUpJob>();
            }
        }

        
    }
}




