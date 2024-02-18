using EasySave.model;
using Newtonsoft.Json;
using EasySave.view;
using EasySave.services;
using System.IO;
using System.Windows;


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
                System.Windows.MessageBox.Show(ManageLang.GetString("error_Loading") + e.Message, ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return new List<BackUpJob>();
            }
        }

        
    }
}




