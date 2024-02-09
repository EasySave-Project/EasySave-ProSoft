using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using System;

namespace EasySave.utils
{
    public class GenerateLang
    {
        public void AddFiles()
        {
            // Vérification de l'existence du dossier
            string sCurrentDir = Environment.CurrentDirectory;
            string destPath = sCurrentDir + "\\EasySave\\lang";
            if (!System.IO.Directory.Exists(destPath))
            {
                System.IO.Directory.CreateDirectory(destPath);
            }
            // Vérification de l'existence des fichiers FR et EN
            string destFile = sCurrentDir + "\\EasySave\\lang\\fr_FR.txt";
            if (!System.IO.File.Exists(destFile))
            {
                System.IO.File.WriteAllText(destFile, "");
                WriteFiles_FR();
            }
            destFile = sCurrentDir + "\\EasySave\\lang\\en_EN.txt";
            if (!System.IO.File.Exists(destFile))
            {
                System.IO.File.WriteAllText(destFile, "");
                WriteFiles_EN();
            }
        }

        private void WriteFiles_FR()
        {
            // Écriture des fichiers FR
            string sCurrentDir = Environment.CurrentDirectory;
            string destFile = sCurrentDir + "\\EasySave\\lang\\fr_FR.txt";

            // Création d'un objet StreamWriter pour écrire dans le fichier
            using (StreamWriter sw = new StreamWriter(destFile))
            {
                // Écriture du texte sous forme de ligne
                sw.WriteLine("Lancer (S) Modifier (M) Supprimer (D)");
                sw.WriteLine("(add) – Ajouter un travail");
                sw.WriteLine("(all) - Exécution Séquentielle");
                sw.WriteLine("(lang) - Changer de langue");
                sw.WriteLine("(exit) – Quitter le logiciel");
                sw.WriteLine("Travail");
                sw.WriteLine("> Nom du travail :");
                sw.WriteLine("> Adresse répertoire source :");
                sw.WriteLine("> Adresse répertoire cible :");
                sw.WriteLine("(1) - Mode complet");
                sw.WriteLine("(2) – Mode différentiel");
                sw.WriteLine("> Sélectionner le mode de sauvegarde :");
                sw.WriteLine("Complet");
                sw.WriteLine("Différentiel");
                sw.WriteLine("> Confirmer la création du travail ? (Y / N)");
                sw.WriteLine("> Voulez - vous supprimer ce travail ? (Y / N)");
                sw.WriteLine("Sélectionnez ce que vous vouliez modifier et entrez la nouvelle données :");
                sw.WriteLine("(1) Nom du travail:");
                sw.WriteLine("(2) Adresse du répertoire source :");
                sw.WriteLine("(3) Adresse du répertoire cible :");
                sw.WriteLine("(4) Mode de sauvegarde(1 - Complet / 2 - Différentiel) :");
                sw.WriteLine("(apply) - Appliquer la modification");
                sw.WriteLine("(exit) -Retour au menu");
                sw.WriteLine("Erreur: Caractère illégal ou numéro inconnu.");
                sw.WriteLine("Tout ouvrir");
                sw.WriteLine("Quitter le programme");
                sw.WriteLine("Ouvrir un travail de sauvegarde 1");
                sw.WriteLine("Ouvrir un travail de sauvegarde 2");
                sw.WriteLine("Ouvrir un travail de sauvegarde 3");
                sw.WriteLine("Ouvrir un travail de sauvegarde 4");
                sw.WriteLine("Ouvrir un travail de sauvegarde 5");
                sw.WriteLine("La chaîne a été split via des ','");
                sw.WriteLine("Ouvrir un travail de sauvegarde");
                sw.WriteLine("La chaine a été split via des '-'");
                sw.WriteLine("Erreur: L’index de début doit être inférieur à l’index de fin.");
                sw.WriteLine("Erreur : Caractère illégal, il ne doit y avoir que deux indices séparés par \"-\". ");
                sw.WriteLine("Execution du travail numéro :" );
                sw.WriteLine("Modification du travail numéro :");
                sw.WriteLine("Supprimer travail numéro:");
                sw.WriteLine("Erreur: Commande inconnu");
                sw.WriteLine("Erreur: Valeur saisie non reconnu");
                sw.WriteLine("Erreur: Le nom du travail n'est pas valide."  );
                sw.WriteLine("Erreur: La valeur de la source n'est pas valide");
                sw.WriteLine("Erreur: La valeur de la destionation n'est pas valide");
                sw.WriteLine("Error: Caractère illégal. Ecrire \"Y\" pour oui, ou \"N\" for no.");
                sw.WriteLine("Travail Ajouter");
                sw.WriteLine("Travail Non Ajouter");
                sw.WriteLine("Erreur code BackUpMode non reconnu");
                sw.WriteLine("Travail Supprimer");
                sw.WriteLine("Travail Non Supprimer");
                sw.WriteLine("Sauvegarde complète réussie.");
                sw.WriteLine("Erreur lors de la sauvegarde complète :");
                sw.WriteLine("Type de sauvegarde invalide");
                sw.WriteLine("Sauvegarde différentiel réussie.");
                sw.WriteLine("Erreur lors de la sauvegarde complète : ");
                sw.WriteLine("Erreur lors de la sauvegarde:");
                sw.WriteLine("Erreur, l'indice du job ne peux être > 5");
                sw.WriteLine("Le nombre maximal de jobs est atteint.");
                sw.WriteLine("Un job avec le même nom existe déjà.");
                sw.WriteLine("Le répertoire source n'existe pas ou n'a pas pu être trouvé:");
                sw.WriteLine("Le répertoire source est vide:");
                sw.WriteLine("Le lecteur spécifié dans le chemin cible");
                sw.WriteLine("n'est pas disponible.");
                sw.WriteLine("Erreur lors du chargement :");
                sw.WriteLine("Temps d'execution :");
            }
        }

        private void WriteFiles_EN()
        {
            // Écriture des fichiers FR
            string sCurrentDir = Environment.CurrentDirectory;
            string destFile = sCurrentDir + "\\EasySave\\lang\\en_EN.txt";

            // Création d'un objet StreamWriter pour écrire dans le fichier
            using (StreamWriter sw = new StreamWriter(destFile))
            {
                // Écriture du texte sous forme de ligne
                sw.WriteLine("Launch (S) Modify (M) Delete (D)");
                sw.WriteLine("(add) - Add a job");
                sw.WriteLine("(all) - Sequential execution");
                sw.WriteLine("(lang) - Change language");
                sw.WriteLine("(exit) - Exit software");
                sw.WriteLine("Job");
                sw.WriteLine("> Job name :");
                sw.WriteLine("> Source directory address :");
                sw.WriteLine("> Target directory address :");
                sw.WriteLine("(1) - Full mode");
                sw.WriteLine("(2) - Differential mode");
                sw.WriteLine("> Select backup mode :");
                sw.WriteLine("Full");
                sw.WriteLine("Differential");
                sw.WriteLine("> Confirm job creation (Y/N)");
                sw.WriteLine(" > Do you want to delete this job?");
                sw.WriteLine("Select what you wanted to change and enter the new data:");
                sw.WriteLine("(1) Job name :");
                sw.WriteLine("(2) Source directory address :");
                sw.WriteLine("(3) Target directory address :");
                sw.WriteLine("(4) Backup mode (1-Complete / 2-Differential) :");
                sw.WriteLine("(apply) - Apply modification");
                sw.WriteLine("(exit) - Return to menu");
                sw.WriteLine("Error: Illegal character or unknown number.");
                sw.WriteLine("Open all");
                sw.WriteLine("Exit program");
                sw.WriteLine("Open backup job 1");
                sw.WriteLine("Open backup job 2");
                sw.WriteLine("Open backup job 3");
                sw.WriteLine("Open backup job 4");
                sw.WriteLine("Open backup job 5");
                sw.WriteLine("The string has been split using ','.");
                sw.WriteLine("Open backup job");
                sw.WriteLine("String has been split with '-' characters");
                sw.WriteLine("Error: Start index must be less than end index.");
                sw.WriteLine("Error: Illegal character, there must only be two indices separated by \"-\".");
                sw.WriteLine("Execute job number: ");
                sw.WriteLine("Modify job number: ");
                sw.WriteLine("Delete job number: ");
                sw.WriteLine("Error: Unknown order");
                sw.WriteLine("Error: Input value not recognized");
                sw.WriteLine("Error: Invalid job name.");
                sw.WriteLine("Error: Invalid source value.");
                sw.WriteLine("Error : Invalid destionation value.");
                sw.WriteLine("Error : Illegal character. Write \"Y\" for yes, or \"N\" for no.");
                sw.WriteLine("Work Add");
                sw.WriteLine("Work No Add");
                sw.WriteLine("Error BackUpMode code not recognized");
                sw.WriteLine("Work Delete");
                sw.WriteLine("Work No Delete");
                sw.WriteLine("Complete backup successful.");
                sw.WriteLine("Full backup error : ");
                sw.WriteLine("Invalid backup type");
                sw.WriteLine("Differential backup successful.");
                sw.WriteLine("Full backup error : ");
                sw.WriteLine("Error during backup :");
                sw.WriteLine("Error, job index cannot be > 5");
                sw.WriteLine("The maximum number of jobs has been reached.");
                sw.WriteLine("A job with the same name already exists.");
                sw.WriteLine("Source directory does not exist or could not be found:");
                sw.WriteLine("The source directory is empty:");
                sw.WriteLine("The drive specified in the target path");
                sw.WriteLine("is not available.");
                sw.WriteLine("Error loading :");
                sw.WriteLine("Execution time :");
            }
 
        }
    }
}
