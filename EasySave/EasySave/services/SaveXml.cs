using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using EasySave.model;

namespace EasySave.services
{
    public class SaveXML : IStrategieSave
    {
        public void SaveLog(Log log)
        {
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\log";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Log));
            string xmlPath = destPath + "\\log_backup_" + log.Name + ".xml";

            using (StreamWriter streamWriter = File.AppendText(xmlPath))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings))
                {
                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteStartElement("Logs");
                    }

                    xmlSerializer.Serialize(xmlWriter, log);

                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteEndElement();
                    }
                }
            }

        }

        public void SaveState(State state)
        {
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\log";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(State));
            string xmlPath = destPath + "\\state_backup_" + state.NameJob + ".xml";

            using (StreamWriter streamWriter = File.AppendText(xmlPath))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings))
                {
                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteStartElement("Snippets");
                    }

                    xmlSerializer.Serialize(xmlWriter, state);

                    if (!File.Exists(xmlPath))
                    {
                        xmlWriter.WriteEndElement();
                    }
                }
            }
        }
    }


}