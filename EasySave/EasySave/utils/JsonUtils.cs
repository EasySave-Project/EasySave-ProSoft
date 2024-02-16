using EasySave.model;
using Newtonsoft.Json;
using EasySave.view;
using EasySave.services;
using System.IO;


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
                Console.WriteLine(ManageLang.GetString("error_Loading") + e.Message);
                return new List<BackUpJob>();
            }
        }

        
    }
}




