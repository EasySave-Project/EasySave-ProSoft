using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using EasySave.model;
using EasySave.controller;
using System.Diagnostics;
using EasySave.utils;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using EasySave.services;

namespace EasySave
{

    public class LogManager
    {

        //=======================================================================================================
        // Log 
        //=======================================================================================================

        Log log = new Log();
        Settings settings = Settings.Instance;
        DateTime dateHeure = DateTime.Now;
        private long long_FileTransferTime;
        private long long_AfterFileTransferTime;
        private readonly object lockLog = new object();
        IStrategieSave typeSave;
        public LogManager() {
            if (settings.LogType == "Json")
            {
                typeSave = new SaveJson();
            }
            else if (settings.LogType == "Xml")
            {
                typeSave = new SaveXML();
            }
            else
            {
                throw new Exception("Log type invalid");
            }

        }


        public void InitLog(string nameJob, string sourcePath, string targetPath)
        {
            log.Name = nameJob;
            log.FileSource = sourcePath;
            log.FileTarget = targetPath;
            log.FileSize = 0;
            //Time before the transfer
            long_FileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            log.Time = "";
        }


        public void PushLog(long NbOctetFile)
        {
            log.FileSize = NbOctetFile;

            //Time after the transfer
            long_AfterFileTransferTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - long_FileTransferTime;

            // Creating a TimeSpan object from milliseconds
            TimeSpan ts = TimeSpan.FromMilliseconds(long_AfterFileTransferTime);

            // Format TimeSpan as HH:MM:SS:MS
            string format = @"hh\:mm\:ss\:fff";
            log.FileTransferTime = ts.ToString(format);

            log.Time = dateHeure.ToString("dd/MM/yyyy HH:mm:ss");

            SaveLog();
        }





        //=======================================================================================================
        // cSave to JSON file
        //=======================================================================================================
        private void SaveLog()
        {    
            lock(lockLog)
            {
                typeSave.SaveLog(log);
            }
             
        }


    }


}