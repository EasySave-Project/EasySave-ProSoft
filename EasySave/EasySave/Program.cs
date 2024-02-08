using EasySave.model;
using EasySave.services;
using EasySave.utils;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace EasySave
{
    public class Program
    {

        public static void Main(string[] args)
        {

            /* on configure */
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("conf/confSave.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
       
            var filePath = configuration["BackUpSaveFile"];
            // On initialise le chemin d'accès à l'enregistrement des 
            JsonUtils.Initialize(configuration);
            
            String name = "backUpJob5";
            String sourceDir = @"C:\mt103";
            String targetDir = @"C:\sauve";
            BackUpManager bm = new BackUpManager();
            bm.AddBackUpJob(BackUpType.Differential, name, sourceDir, targetDir);



            Console.ReadKey();
        
        }
    }
}