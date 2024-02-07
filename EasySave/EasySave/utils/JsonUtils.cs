using EasySave.model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace EasySave.utils
{
    public static class JsonUtils
    {


        public static string filePath;
        public static void Initialize(IConfiguration configuration)
        {
            filePath = configuration["BackUpSaveFile"];
        }

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
                Console.WriteLine($"Erreur lors du chargement : {e.Message}");
                return new List<BackUpJob>();
            }
        }

        
    }
}




