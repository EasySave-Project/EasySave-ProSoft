using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.view
{
    public class ConsoleView
    {
        // Variables globales
        private static string sLanguage;

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
                    case "2":
                        bSecurity = true;
                        break;
                    default:
                        Console.WriteLine("\nError : Illegal character or unknown number.\n");
                        break;
                }
            }
            Console.WriteLine("======================================================");
        }

        // Renvoie la chaine de caractère de la langue sélectionnée à partir du fichier correspondant
        private string GetLineLanguage(int iCodeLine)
        {
            string sReturnLang = "";
            string sCurrentDir = Environment.CurrentDirectory; // Obtenir le répertoire courant
            string sRelativePath = "";
            sRelativePath = sLanguage switch
            {
                "1" => Path.Combine(sCurrentDir, "ProgSys_Menu_v1\\lang\\en_EN.txt"),
                "2" => Path.Combine(sCurrentDir, "ProgSys_Menu_v1\\lang\\fr_FR.txt")
            };
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

            while (bSecurity == false)
            {
                Console.WriteLine("\n=======================EasySave=======================");

                // Code=> remplir le tableau
                string[] sNameJob = new string[5];
                sNameJob[0] = "Compta";
                sNameJob[1] = "Catalogue client";

                // Code=> Boucle d'affichage des jobs
                for (int i = 0; i < sNameJob.Length; i++)
                {
                    // S'il y a un nom de job alors on l'affiche
                    if (sNameJob[i] != null)
                    {
                        Console.WriteLine("(" + (i + 1) + ") - " + sNameJob[i] + " | " + GetLineLanguage(0));
                    }
                    else
                    {
                        Console.WriteLine("> ...");
                    }
                }

                Console.WriteLine(GetLineLanguage(1));
                Console.WriteLine(GetLineLanguage(2));
                Console.WriteLine(GetLineLanguage(3));
                Console.WriteLine(GetLineLanguage(4));

                sAnswer = Console.ReadLine();
                switch (sAnswer)
                {
                    case "add":
                        ShowAddJob();
                        break;
                    case "all":
                        Console.WriteLine("Ouvrir all");
                        break;
                    case "lang":
                        ShowSelectLanguage();
                        break;
                    case "exit":
                        bSecurity = true;
                        break;
                    default:
                        string[] sListOfJob = new string[sNameJob.Length];

                        // Vérifier si le premier caractère de la réponse est un chiffre
                        if (char.IsDigit(sAnswer[0]))
                        {
                            ChainAnalysis(sAnswer);
                        }
                        else
                        {
                            Console.WriteLine("\nError : Illegal character or unknown number.\n");
                        }
                        break;
                }
                Console.WriteLine("======================================================");
            }// fin de boucle
            Console.WriteLine("Exit du programme");
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
                if (sAnswerSplit[0].Length == 1)
                {
                    switch (sAnswerSplit[0])
                    {
                        case "1":
                            Console.WriteLine("Ouvrir job 1");
                            CommandAnalysis(sAnswerSplit[1], 1);
                            break;
                        case "2":
                            Console.WriteLine("Ouvrir job 2");
                            CommandAnalysis(sAnswerSplit[1], 2);
                            break;
                        case "3":
                            Console.WriteLine("Ouvrir job 3");
                            CommandAnalysis(sAnswerSplit[1], 3);
                            break;
                        case "4":
                            Console.WriteLine("Ouvrir job 4");
                            CommandAnalysis(sAnswerSplit[1], 4);
                            break;
                        case "5":
                            Console.WriteLine("Ouvrir job 5");
                            CommandAnalysis(sAnswerSplit[1], 5);
                            break;
                        default:
                            Console.WriteLine("\nError : Illegal character or unknown number.\n");
                            break;
                    }
                }
                else
                {
                    // Type 2 : Liste de jobs
                    string[] sAnswerSplit_List = sAnswerSplit[0].Split(',');
                    if (sAnswerSplit_List.Length > 1)
                    {
                        Console.WriteLine("\nLa chaine a été split via des ','");
                        for (int i = 0; i < sAnswerSplit_List.Length; i++)
                        {
                            Console.WriteLine("Ouvrir job " + sAnswerSplit_List[i]);
                            CommandAnalysis(sAnswerSplit[1], int.Parse(sAnswerSplit_List[i]));
                        }
                    }
                    else
                    {
                        // Type 3 : Séquence de jobs
                        sAnswerSplit_List = sAnswerSplit[0].Split('-');
                        if (sAnswerSplit_List.Length > 1)
                        {
                            Console.WriteLine("\nLa chaine a été split via des '-'");
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
                                        Console.WriteLine("Ouvrir job " + i);
                                        CommandAnalysis(sAnswerSplit[1], i);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\nError : The start index must be less than the end index.\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nError : Illegal character, there must be only two subscripts separated by a \"-\".\n");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("\nError : Illegal character or unknown number.\n");
            }
        }

        private void CommandAnalysis(string sAnswerCmd, int iNbJob)
        {
            switch (sAnswerCmd)
            {
                case "S":
                    Console.WriteLine("Execution job nb : " + iNbJob);
                    break;
                case "M":
                    Console.WriteLine("Modifier job nb : " + iNbJob);
                    ShowModifyJob(iNbJob);
                    break;
                case "D":
                    Console.WriteLine("Supprimer job nb : " + iNbJob);
                    ShowDeleteJob(iNbJob);
                    break;
                default:
                    Console.WriteLine("\nError : Order not recognised.\n");
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
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : ...");
            Console.Write(GetLineLanguage(6));
            sNameJob = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = sNameJob.Length > 0 && !sNameJob.Contains(" ");
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The job name is not valid.\n");
                    Console.Write(GetLineLanguage(6));
                    sNameJob = Console.ReadLine();
                    isValid = sNameJob.Length > 0 && !sNameJob.Contains(" ");

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 2 : Source du job
            Console.WriteLine("\n" + GetLineLanguage(6) + " (" + iNbJob + ") : " + sNameJob + " ...");
            Console.Write(GetLineLanguage(7));
            sSourcePath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = sSourcePath.Length > 0 && sSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The source path is not valid.\n");
                    Console.Write(GetLineLanguage(7));
                    sSourcePath = Console.ReadLine();
                    isValid = sSourcePath.Length > 0 && sSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 3 : Destination du job
            Console.WriteLine("\n" + GetLineLanguage(6) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> ...");
            Console.Write(GetLineLanguage(8));
            sDestinationPath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = sDestinationPath.Length > 0 && sDestinationPath.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The destination path is not valid.\n");
                    Console.Write(GetLineLanguage(8));
                    sDestinationPath = Console.ReadLine();
                    isValid = sDestinationPath.Length > 0 && sDestinationPath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 4 : Mode de sauvegarde
            Console.WriteLine("\n" + GetLineLanguage(6) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | ...");
            Console.WriteLine(GetLineLanguage(9));
            Console.WriteLine(GetLineLanguage(10));
            Console.Write(GetLineLanguage(11));
            sBackupMode = Console.ReadLine();
            if (sBackupMode == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }
            if (sBackupMode != "1" && sBackupMode != "2")
            {
                while (sBackupMode != "1" && sBackupMode != "2")
                {
                    Console.WriteLine("\nError : Illegal character or unknown number.\n");
                    Console.Write(GetLineLanguage(11));
                    sBackupMode = Console.ReadLine();

                    if (sBackupMode == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }
            iBackupMode = int.Parse(sBackupMode);

            // Partie 5 : Confirmation
            Console.WriteLine("\n" + GetLineLanguage(6) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | " + (iBackupMode == 1 ? GetLineLanguage(13) : GetLineLanguage(14)));
            Console.Write(GetLineLanguage(15));
            sValidation = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            if (sValidation != "Y" && sValidation != "N")
            {
                while (sValidation != "Y" && sValidation != "N")
                {
                    Console.WriteLine("\nError : Illegal character. Just write \"Y\" for yes, or \"N\" for no.\n");
                    Console.Write(GetLineLanguage(15));
                    sValidation = Console.ReadLine();

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }
            if (sValidation == "Y")
            {
                Console.WriteLine("Job added");
            }
            else
            {
                Console.WriteLine("Job not added");
            }
        }

        private void ShowModifyJob(int iIndexJob)
        {
            string sNameJob_Old = "";
            string sSourcePath_Old = "";
            string sDestinationPath_Old = "";
            int iBackupMode_Old = 0;

            string sNameJob;
            string sSourcePath;
            string sDestinationPath;
            string sValidation;
            string sBackupMode;
            bool isValid;
            int iBackupMode = 0;
            int iNbJob;

            // Code=> Mettre le code qui renvoie le numéro du job courant
            iNbJob = iIndexJob;

            Console.WriteLine("\n=======================EasySave=======================");

            // Partie 1 : Nom du job
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : " + sNameJob_Old);
            Console.Write(GetLineLanguage(6));
            sNameJob = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = !String.IsNullOrEmpty(sNameJob) && !sNameJob.Contains(" ");
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The job name is not valid.\n");
                    Console.Write(GetLineLanguage(6));
                    sNameJob = Console.ReadLine();
                    isValid = sNameJob.Length > 0 && !sNameJob.Contains(" ");

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 2 : Source du job
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : " + sNameJob + " ...");
            Console.Write(GetLineLanguage(7));
            sSourcePath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = !String.IsNullOrEmpty(sNameJob) && !sNameJob.Contains(" ");
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The source path is not valid.\n");
                    Console.Write(GetLineLanguage(7));
                    sSourcePath = Console.ReadLine();
                    isValid = sSourcePath.Length > 0 && sSourcePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 3 : Destination du job
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> ...");
            Console.Write(GetLineLanguage(8));
            sDestinationPath = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            isValid = !String.IsNullOrEmpty(sNameJob) && !sNameJob.Contains(" ");
            if (!isValid)
            {
                while (!isValid)
                {
                    Console.WriteLine("\nError : The destination path is not valid.\n");
                    Console.Write(GetLineLanguage(8));
                    sDestinationPath = Console.ReadLine();
                    isValid = sDestinationPath.Length > 0 && sDestinationPath.IndexOfAny(Path.GetInvalidPathChars()) == -1;

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }

            // Partie 4 : Mode de sauvegarde
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | ...");
            Console.WriteLine(GetLineLanguage(9));
            Console.WriteLine(GetLineLanguage(10));
            Console.Write(GetLineLanguage(11));
            sBackupMode = Console.ReadLine();
            if (sBackupMode == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }
            if (sBackupMode != "1" && sBackupMode != "2")
            {
                while (sBackupMode != "1" && sBackupMode != "2")
                {
                    Console.WriteLine("\nError : Illegal character or unknown number.\n");
                    Console.Write(GetLineLanguage(11));
                    sBackupMode = Console.ReadLine();

                    if (sBackupMode == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }
            iBackupMode = int.Parse(sBackupMode);

            // Partie 5 : Confirmation
            Console.WriteLine("\n" + GetLineLanguage(5) + " (" + iNbJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | " + (iBackupMode == 1 ? GetLineLanguage(13) : GetLineLanguage(14)));
            Console.Write(GetLineLanguage(15));
            sValidation = Console.ReadLine();

            if (sNameJob == "exit")
            {
                Console.WriteLine("Exit du programme");
                return;
            }

            if (sValidation != "Y" && sValidation != "N")
            {
                while (sValidation != "Y" && sValidation != "N")
                {
                    Console.WriteLine("\nError : Illegal character. Just write \"Y\" for yes, or \"N\" for no.\n");
                    Console.Write(GetLineLanguage(14));
                    sValidation = Console.ReadLine();

                    if (sNameJob == "exit")
                    {
                        Console.WriteLine("Exit du programme");
                        return;
                    }
                }
            }
            if (sValidation == "Y")
            {
                Console.WriteLine("Job added");
            }
            else
            {
                Console.WriteLine("Job not added");
            }
        }

        private void ShowDeleteJob(int iIndexJob)
        {
            string sAnswer;
            string sNameJob = "";
            string sSourcePath = "";
            string sDestinationPath = "";
            int iBackupMode = 1;

            Console.WriteLine("\n=======================EasySave=======================");
            Console.WriteLine(GetLineLanguage(6) + " (" + iIndexJob + ") : " + sNameJob + " [" + sSourcePath + " -> " + sDestinationPath + "] | " + (iBackupMode == 1 ? GetLineLanguage(13) : GetLineLanguage(14)));
            Console.WriteLine(GetLineLanguage(15));
            sAnswer = Console.ReadLine();

            if (sAnswer == "Y")
            {
                Console.WriteLine("Job deleted");
            }
            else
            {
                Console.WriteLine("Job not deleted");
            }
        }
    }
}
