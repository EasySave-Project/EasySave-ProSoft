using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using EasySave.controller;
using EasySave.model;
using EasySave.services;
using EasySave.utils;
namespace EasySave.view
{
    public class ConsoleView
    {
        // Variables globales
        private static string sLanguage;

        private Settings settings = new Settings();
        public BackUpController backUpController { get; set; }

        public void InitConfFolder()
        {
            // Partie Save Job
            // Vérifier la présence du dossier "conf"
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\conf";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            // Vérifier la présence du fichier "SaveBackUpJob.json" puis écrire rien dedans
            string filePath = destPath + "\\SaveBackUpJob.json";
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
            }
            // Partie Log
            // Vérifier la présence du dossier "log"
            destPath = sCurrentDir + "\\EasySave\\log";
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            
        }


        // Afficher l'écran de sélection de la langue
        public void ShowSelectLanguage()
        {
            bool bSecurity = false;

            Console.WriteLine("=======================EasySave=======================");
            Console.WriteLine("(1) - English");
            Console.WriteLine("(2) - Français");
            while (bSecurity == false)
            {
                Console.Write("Please select a language : ");
                sLanguage = Console.ReadLine();
                switch (sLanguage)
                {
                    case "1":
                        ManageLang.ChangeLanguage("");
                        settings.Lang = "en";
                        bSecurity = true;
                        break;
                    case "2":
                        ManageLang.ChangeLanguage("fr");
                        settings.Lang = "fr";
                        bSecurity = true;
                        break;
                    default:
                        Console.WriteLine(ManageLang.GetString("error_Caract")+"\n");
                        break;
                }
            }
            Console.WriteLine("======================================================");
        }

        // Renvoie la chaine de caractère de la langue sélectionnée à partir du fichier correspondant
        public static string GetLineLanguage(int iCodeLine)
        {
            string sReturnLang = "";
            string sCurrentDir = Environment.CurrentDirectory; // Obtenir le répertoire courant
            string sRelativePath = "";
            switch (sLanguage)
            {
                case "1":
                    sRelativePath = Path.Combine(sCurrentDir, "EasySave\\lang\\en_EN.txt");
                    break;
                case "2":
                    sRelativePath = Path.Combine(sCurrentDir, "EasySave\\lang\\fr_FR.txt");
                    break;

            }
            // Ouvrir le fichier en lecture
            using (StreamReader fileLang = new StreamReader(sRelativePath))
            {
                int i = 0;
                // Lire le fichier jusqu'à la fin
                while (!fileLang.EndOfStream)
                {
                    // Lire une ligne
                    string line = fileLang.ReadLine();
                    // Vérifier si le compteur de lignes correspond au paramètre
                    if (i == iCodeLine)
                    {
                        sReturnLang = line;
                    }
                    i++;
                }
            }
            return sReturnLang;
        }

        public void ShowMainMenu()
        {
            bool bSecurity = false;
            string sAnswer;
            
            if(settings.Lang == "fr")
            {
                ManageLang.ChangeLanguage("fr");
            }else if(settings.Lang == "en")
            {
                ManageLang.ChangeLanguage("");
            }
            else
            {
                ShowSelectLanguage();
            }
            
            
            while (bSecurity == false)
            {
                Console.WriteLine("\n=======================EasySave=======================");

                // Code=> remplir le tableau
               
                List<String> sNameJob = new List<string>();

                foreach (BackUpJob bj in BackUpManager.listBackUps)
                {
                    sNameJob.Add(bj.name);
                }
 

                // Code=> Boucle d'affichage des jobs
                for (int i = 0; i < sNameJob.Count; i++)
                {
                    // S'il y a un nom de job alors on l'affiche
                    if (sNameJob[i].Length != 1)
                    {
                        Console.WriteLine("(" + (i + 1) + ") - " + sNameJob[i] + " | " + ManageLang.GetString("view_menu_allCmd"));
                    }
                    else
                    {
                        Console.WriteLine("> ...");
                    }
                }

                Console.WriteLine("\n" + ManageLang.GetString("view_menu_add"));
                Console.WriteLine(ManageLang.GetString("view_menu_all"));
                Console.WriteLine(ManageLang.GetString("view_menu_lang"));
                Console.WriteLine(ManageLang.GetString("view_menu_param"));
                Console.WriteLine(ManageLang.GetString("view_menu_exit"));

                Console.Write(ManageLang.GetString("view_waitingAswer"));
                sAnswer = Console.ReadLine();
                switch (sAnswer)
                {
                    case "add":
                        ShowAddJob();
                        break;
                    case "all":
                        backUpController.InitiateAllBackUpJobs();
                        Console.WriteLine(ManageLang.GetString("view_menu_affichAll"));
                        break;
                    case "lang":
                        ShowSelectLanguage();
                        break;
                    case "param":
                        ShowParam();
                        break;
                    case "exit":
                        bSecurity = true;
                        Console.WriteLine(ManageLang.GetString("view_menu_affichExit"));
                        break;
                    default:
                        string[] sListOfJob = new string[sNameJob.Count];

                        // Vérifier si le premier caractère de la réponse est un chiffre
                        if (char.IsDigit(sAnswer[0]))
                        {
                            ChainAnalysis(sAnswer);
                        }
                        else
                        {
                            Console.WriteLine(ManageLang.GetString("error_Caract"));
                        }
                        break;
                }
                Console.WriteLine("======================================================");
            }// fin de boucle
            Console.WriteLine(ManageLang.GetString("view_modif_exit"));
            Environment.Exit(0);
        }

        // Fonction qui analyse la chaine et exécuter le(s) job(s) correspondant(s)
        private void ChainAnalysis(string sAnswerJob)
        {
            // Séparation de la réponse en deux parties
            string[] sAnswerSplit = sAnswerJob.Split(' ');
            if (sAnswerSplit.Length == 2)
            {
                // Type 1 : Juste un job
                if (!string.IsNullOrEmpty(sAnswerSplit[0]))
                {
                    // Conversion du premier caractère en entier
                    int iJob;
                    bool bValid = int.TryParse(sAnswerSplit[0], out iJob);
                    // Vérification si le premier caractère est un entier positif et ne contient pas de "-" ou de ","
                    if (bValid && iJob >= 0 && !sAnswerSplit[0].Contains("-") && !sAnswerSplit[0].Contains(","))
                    {
                        // Exécution du job correspondant
                        Console.WriteLine(ManageLang.GetString("view_menu_exe") + iJob);
                        CommandAnalysis(sAnswerSplit[1], iJob - 1);
                    }
                    else
                    {
                        // Type 2 : Liste de jobs
                        string[] sAnswerSplit_List = sAnswerSplit[0].Split(',');
                        if (sAnswerSplit_List.Length > 1)
                        {
                            Console.WriteLine(ManageLang.GetString("view_menu_chainSplit1"));
                            for (int i = 0; i < sAnswerSplit_List.Length; i++)
                            {
                                Console.WriteLine(ManageLang.GetString("view_menu_exe") + i);
                                CommandAnalysis(sAnswerSplit[1], int.Parse(sAnswerSplit_List[i]));
                            }
                        }
                        else
                        {
                            // Type 3 : Séquence de jobs
                            sAnswerSplit_List = sAnswerSplit[0].Split('-');
                            if (sAnswerSplit_List.Length > 1)
                            {
                                Console.WriteLine(ManageLang.GetString("view_menu_chainSplit2"));
                                // Vérifier si le tableau contient bien deux éléments
                                if (sAnswerSplit_List.Length == 2)
                                {
                                    int iStartIndex = int.Parse(sAnswerSplit_List[0]);
                                    int iEndIndex = int.Parse(sAnswerSplit_List[1]);
                                    // Vérifier si le premier index est inférieur au deuxième
                                    if (iStartIndex < iEndIndex)
                                    {
                                        for (int i = iStartIndex; i <= iEndIndex; i++)
                                        {
                                            Console.WriteLine(ManageLang.GetString("view_menu_exe") + i);
                                            CommandAnalysis(sAnswerSplit[1], i);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(ManageLang.GetString("error_index"));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ManageLang.GetString("error_listCaract"));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(ManageLang.GetString("error_Caract"));
            }
        }

        private void CommandAnalysis(string sAnswerCmd, int iNbJob)
        {
            switch (sAnswerCmd)
            {
                case "S":
                    Console.WriteLine(ManageLang.GetString("view_menu_exe") + iNbJob +1 );
                    try
                    {
                        backUpController.InitiateBackUpJob(BackUpManager.listBackUps[iNbJob]);
                    }
                    catch
                    {
                        Console.WriteLine(ManageLang.GetString("error_save"));
                    }

                    break;
                case "M":
                    Console.WriteLine(ManageLang.GetString("view_modif") + iNbJob+ 1);
                    try
                    {
                        ShowModifyJob(iNbJob);
                    }
                    catch
                    {
                        Console.WriteLine(ManageLang.GetString("error_Loading"));
                    }
                    break;
                case "D":
                    Console.WriteLine(ManageLang.GetString("view_suppr") + iNbJob + 1);
                    try
                    {
                        ShowDeleteJob(iNbJob);
                    }
                    catch
                    {
                        Console.WriteLine(ManageLang.GetString("error_Loading"));
                    }
                    break;
                default:
                    Console.WriteLine(ManageLang.GetString("error_UnknowCmd"));
                    break;
            }
        }

        private void ShowAddJob()
        {
            string sNameJob;
            string sSourcePath;
            string sDestinationPath;
            string sValidation;
            string sBackupMode;
            bool isValid;
            int iBackupMode;
            int iNbJob;

            // Code=> Mettre le code qui renvoie le numéro du job courant
            iNbJob = 4;

            Console.WriteLine("\n=======================EasySave=======================");

            // Partie 1 : Nom du job
            Console.WriteLine("\n" + ManageLang.GetString("view_menu_nameJob") + " (" + iNbJob + ") : ...");
            Console.Write(ManageLang.GetString("view_add_name"));
            sNameJob = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                return;
            }

            isValid = sNameJob.Length > 0 && !sNameJob.Contains(" ");
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine(ManageLang.GetString("error_NoneValidJob"));
                    Console.Write(ManageLang.GetString("view_menu_nameJob"));
                    sNameJob = Console.ReadLine();
                    isValid = sNameJob.Length > 0 && !sNameJob.Contains(" ");

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                        return;
                    }
                }
            }

            // Partie 2 : Source du job
            Console.WriteLine("\n" + ManageLang.GetString("view_menu_nameJob") + " (" + iNbJob + ") : " + sNameJob + " ...");
            Console.Write(ManageLang.GetString("view_add_sourcePath"));
            sSourcePath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                return;
            }

            isValid = sSourcePath.Length > 0 && sSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine(ManageLang.GetString("error_NoneSourcePath"));
                    Console.Write(ManageLang.GetString("view_add_sourcePath"));
                    sSourcePath = Console.ReadLine();
                    isValid = sSourcePath.Length > 0 && sSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                        return;
                    }
                }
            }

            // Partie 3 : Destination du job
            Console.WriteLine("\n" + ManageLang.GetString("view_menu_nameJob") + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> ...");
            Console.Write(ManageLang.GetString("view_add_destPath"));
            sDestinationPath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                return;
            }

            isValid = sDestinationPath.Length > 0 && sDestinationPath.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine(ManageLang.GetString("error_NoneDestPath"));
                    Console.Write(ManageLang.GetString("view_add_destPath"));
                    sDestinationPath = Console.ReadLine();
                    isValid = sDestinationPath.Length > 0 && sDestinationPath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                        return;
                    }
                }
            }

            // Partie 4 : Mode de sauvegarde
            Console.WriteLine("\n" + ManageLang.GetString("view_menu_nameJob") + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | ...");
            Console.WriteLine(ManageLang.GetString("view_add_modComplet"));
            Console.WriteLine(ManageLang.GetString("view_add_modDiff"));
            Console.Write(ManageLang.GetString("view_add_SelectMod"));
            sBackupMode = Console.ReadLine();
            if (sBackupMode == "exit")
            {
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                return;
            }
            if (sBackupMode != "1" && sBackupMode != "2")
            {
                while (sBackupMode != "1" && sBackupMode != "2")
                {
                    Console.WriteLine(ManageLang.GetString("error_NoneCodeBackup"));
                    Console.Write(ManageLang.GetString("view_add_SelectMod"));
                    sBackupMode = Console.ReadLine();

                    if (sBackupMode == "exit")
                    {
                        Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                        return;
                    }
                }
            }
            iBackupMode = int.Parse(sBackupMode);
           
            // Partie 5 : Confirmation
            Console.WriteLine("\n" + ManageLang.GetString("view_menu_nameJob") + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | " + (iBackupMode == 1 ? ManageLang.GetString("view_add_complet") : ManageLang.GetString("view_add_diff")));
            Console.Write(ManageLang.GetString("view_add_confirm"));
            sValidation = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                return;
            }

            if (sValidation != "Y" && sValidation != "N")
            {
                while (sValidation != "Y" && sValidation != "N")
                {
                    Console.WriteLine(ManageLang.GetString("error_CaractValid"));
                    Console.Write(ManageLang.GetString("view_add_confirm"));
                    sValidation = Console.ReadLine();
                    if (sNameJob == "exit")
                    {
                        Console.WriteLine(ManageLang.GetString("view_modif_exit"));
                        return;
                    }
                }
            }
            if (sValidation == "Y")
            {
                BackUpType type;
                backUpController.InitiateAddJob(type = iBackupMode == 1 ? BackUpType.Complete : BackUpType.Differential, sNameJob, sSourcePath, sDestinationPath);
            }
            else
            {
                Console.WriteLine(ManageLang.GetString("view_add_AffichJobIsNoAdd"));
            }
        }

        private void ShowModifyJob(int iIndexJob)
        {
            string sNameJob_Old = BackUpManager.listBackUps[iIndexJob].name;
            string sSourcePath_Old = BackUpManager.listBackUps[iIndexJob].sourceDirectory;
            string sDestinationPath_Old = BackUpManager.listBackUps[iIndexJob].targetDirectory;
            Type backUpType = BackUpManager.listBackUps[iIndexJob].GetType();
            string sbackUpMode; 
            string typeJob = backUpType.FullName;
            BackUpType type;
            BackUpType type_old;
            if (typeJob.Contains("Complete"))
            {
                sbackUpMode = ManageLang.GetString("view_add_complet");
                type = BackUpType.Complete;
                type_old = BackUpType.Complete;
            }
            else {
                sbackUpMode = ManageLang.GetString("view_add_diff");
                type = BackUpType.Differential;
                type_old = BackUpType.Differential;
            }

            bool bSecurity = false;
            while (bSecurity == false)
            {
                string sAnswer = "";

                Console.WriteLine("\n=======================EasySave=======================");
                Console.WriteLine(ManageLang.GetString("view_modif_affich") + "\n");
                Console.WriteLine(ManageLang.GetString("view_modif_name") + sNameJob_Old);
                Console.WriteLine(ManageLang.GetString("view_modif_sourcePath") + sSourcePath_Old);
                Console.WriteLine(ManageLang.GetString("view_modif_destPath") + sDestinationPath_Old);
                Console.WriteLine(ManageLang.GetString("view_modif_modAffich") + sbackUpMode);
                Console.WriteLine("\n" + ManageLang.GetString("view_modif_apply"));
                Console.WriteLine(ManageLang.GetString("view_modif_exit"));

                Console.Write(ManageLang.GetString("view_waitingAswer"));
                sAnswer = Console.ReadLine();

                // Diviser la réponses en deux parties
                string[] sAnswerSplit = sAnswer.Split(' ');

                // Vérifier si la réponse contient bien deux parties
                if (sAnswerSplit.Length == 2)
                {
                    switch (sAnswerSplit[0])
                    {
                        case "1":
                            sNameJob_Old = sAnswerSplit[1];
                            break;
                        case "2":
                            sSourcePath_Old = sAnswerSplit[1];
                            break;
                        case "3":
                            sDestinationPath_Old = sAnswerSplit[1];
                            break;
                        case "4":
                            sbackUpMode = sAnswerSplit[1];
                            if (sAnswerSplit[1] == "1")
                            {
                                type = BackUpType.Complete;
                            }
                            else if (sAnswerSplit[1] == "2")
                            {
                                type = BackUpType.Differential;
                            }
                            else
                            {
                                Console.WriteLine(ManageLang.GetString("error_Caract"));
                            }
                            break;
                        default:
                            Console.WriteLine(ManageLang.GetString("error_Caract"));
                            break;
                    }
                }
                else if (sAnswerSplit.Length == 1)
                {
                    switch (sAnswerSplit[0])
                    {
                        case "apply":
                            backUpController.IntiateModifyJobName(iIndexJob, sNameJob_Old);
                            backUpController.InitiateModifyJobSourceDir(iIndexJob, sSourcePath_Old);
                            backUpController.InitiateModifyJobTargetDir(iIndexJob, sDestinationPath_Old);
                            if (type != type_old)
                            {
                                backUpController.IniateModifyJobType(iIndexJob, type);
                            }
                            break;
                        case "exit":
                            bSecurity = true;
                            break;
                        default:
                            Console.WriteLine(ManageLang.GetString("error_UnknowCmd"));
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(ManageLang.GetString("error_Caract"));
                }
            }
            
        }

     

        private void ShowDeleteJob(int iIndexJob)
        {
            string sAnswer;
            string sNameJob = BackUpManager.listBackUps[iIndexJob].name ;
            string sSourcePath = BackUpManager.listBackUps[iIndexJob].sourceDirectory;
            string sDestinationPath = BackUpManager.listBackUps[iIndexJob].targetDirectory;
            Type type = BackUpManager.listBackUps[iIndexJob].GetType();
            
            int iBackupMode = 1;

            Console.WriteLine("\n=======================EasySave=======================");
            Console.WriteLine(ManageLang.GetString("view_menu_nameJob") + " (" + iIndexJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | " + (iBackupMode == 1 ? ManageLang.GetString("view_add_diff") : ManageLang.GetString("view_add_confirm")));
            Console.WriteLine(ManageLang.GetString("view_supp_confirm"));
            Console.Write(ManageLang.GetString("view_waitingAswer"));
            sAnswer = Console.ReadLine();

            if (sAnswer == "Y")
            {
                backUpController.InitiateRemoveBackup(sNameJob);
                Console.WriteLine(ManageLang.GetString("view_supp_AffichJobIsSupp"));              
            }
            else
            {
                Console.WriteLine(ManageLang.GetString("view_supp_AffichJobIsNoSupp"));
            }
        }

        private void ShowParam()
        {
            string sAnswer;
            bool bSecurity = false;

            Console.WriteLine("\n=======================EasySave=======================");
            Console.WriteLine(ManageLang.GetString("view_param_titre"));
            Console.WriteLine(ManageLang.GetString("view_param_json"));
            Console.WriteLine(ManageLang.GetString("view_param_xml"));
            Console.WriteLine("\n"+ ManageLang.GetString("view_modif_exit"));

            while (bSecurity == false)
            {
                Console.Write(ManageLang.GetString("view_waitingAswer"));
                sAnswer = Console.ReadLine();
                switch (sAnswer)
                {
                    case "1":
                        Console.WriteLine(ManageLang.GetString("view_param_AffichJSON"));
                        settings.LogType = "JSON";
                        settings.StateType = "JSON";
                        bSecurity = true;
                        break;
                    case "2":
                        Console.WriteLine(ManageLang.GetString("view_param_AffichXML"));
                        settings.LogType = "XML";
                        settings.StateType = "XML";
                        bSecurity = true;
                        break;
                    case "exit":
                        bSecurity = true;
                        break;
                    default:
                        Console.WriteLine(ManageLang.GetString("error_Caract")+"\n");
                        break;
                }

            }
        }
    }
}
