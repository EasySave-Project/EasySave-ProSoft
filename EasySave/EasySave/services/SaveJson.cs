using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using EasySave.model;
using EasySave.services;

namespace EasySave.services
{
    public class SaveJson : IStrategieSave
    {
        
        private static object Lockobject = new object();

        public  void SaveState(State state)
        {

            
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\state";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<State>(state, options);
            string filePath = destPath + "\\state_backup_" + state.NameJob + ".json";

            if (File.Exists(filePath))
            {
                string oldJson = File.ReadAllText(filePath);
                string newJson = oldJson + "\n" + json;
                File.WriteAllText(filePath, newJson);
            }
            else
            {
                File.WriteAllText(filePath, json);
            }
        
           

        }

        public void SaveLog(Log log)
        {
           
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\log";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<Log>(log, options);
            string filePath = destPath + "\\log_backup_" + log.Name + ".json";

            if (File.Exists(filePath))
            {
                string oldJson = File.ReadAllText(filePath);
                string newJson = oldJson + "\n" + json;
                File.WriteAllText(filePath, newJson);
            }
            else
            {
                File.WriteAllText(filePath, json);
            }
            
          
        }
    }


}