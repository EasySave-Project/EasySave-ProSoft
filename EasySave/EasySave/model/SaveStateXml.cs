using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EasySave.model
{
	public class SauvegardeXML : IStrategieSave
    {
        public void SauvegardeState(State etat, string cheminDossier)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(State));
            string xmlPath = Path.Combine(cheminDossier, "state_backup.xml");
            using (StreamWriter streamWriter = File.CreateText(xmlPath))
            {
                xmlSerializer.Serialize(streamWriter, etat);
            }
        }

        public void SaveLog(Log log)
        {
           // throw new NotImplementedException();
        }
    }

	
}