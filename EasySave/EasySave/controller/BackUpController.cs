using System;
using System.Diagnostics;
using EasySave.model;
using EasySave.services;

namespace EasySave.controller
{
    internal class BackUpController
    {
        private BackUpManager _backUpManager;
        private LogManager _logManager;
        private StateManager _stateManager;

        public BackUpController(BackUpManager backUpManager, LogManager logManager, StateManager stateManager)
        {
            _backUpManager = backUpManager;
            _logManager = logManager;
            _stateManager = stateManager;
        }

        public void InitiateBackup(string jobName)
        {
            try
            {
                var job = _backUpManager.FindBackupJobById(jobName);
                if (job != null)
                {
                    // Démarrez un chronomètre pour mesurer le temps de transfert
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    // Ici, vous exécutez votre travail de sauvegarde
                    // Remarque : Assurez-vous d'implémenter ExecuteBackup dans _backUpManager ou adapter cette ligne en fonction de votre logique
                    _backUpManager.ExecuteBackup(job);

                    // Arrêtez le chronomètre une fois la sauvegarde terminée
                    stopwatch.Stop();

                    // Obtenez le temps de transfert
                    TimeSpan transferTime = stopwatch.Elapsed;

                    // Calculez la taille totale des fichiers sauvegardés
                    // Remarque : Vous devrez implémenter cette logique selon votre besoin
                    long totalSize = CalculateTotalSize(job);

                    // Enregistrez l'événement dans le fichier log
                    _logManager.WriteLog(job, transferTime, totalSize);

                    // Mettez à jour l'état ici si nécessaire
                    // _stateManager.UpdateState(job, /* informations de progrès */);
                }
            }
            catch (Exception e)
            {
                // Log l'erreur avant de l'afficher
                _logManager.WriteError(e);

                // Affichez l'erreur dans la console ou via une interface utilisateur
                Console.WriteLine(e.Message);
            }
        }

        private long CalculateTotalSize(BackUpJob job)
        {
            // Implémentez la logique pour calculer la taille totale des fichiers sauvegardés
            // Cette fonction doit retourner la taille totale des fichiers que le travail de sauvegarde a traités
            // Pour cet exemple, je retourne 0. Vous devrez remplacer cela par votre propre logique
            return 0;
        }
    }
}
