using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.model
{
    public class SauvegardeJson : IStrategieSave
    {
        public void SauvegardeState(State etat, string cheminDossier)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<State>(etat, options);
            string filePath = Path.Combine(cheminDossier, "state_backup.json");
            File.WriteAllText(filePath, json);
        }

        public void SaveLog(Log log)
        {
        }
    }


}