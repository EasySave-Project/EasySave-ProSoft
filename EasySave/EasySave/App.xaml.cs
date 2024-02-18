using EasySave.services;
using EasySave.utils;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Obtenez le nom du processus de votre application
            var currentProcess = Process.GetCurrentProcess();
            var currentProcessName = currentProcess.ProcessName;

            // Trouvez tous les processus ayant le même nom que votre application
            var processes = Process.GetProcessesByName(currentProcessName);

            // S'il existe plus d'une instance de ce processus, considérez qu'une instance est déjà en cours d'exécution
            if (processes.Length > 1)
            {
                foreach (var process in processes)
                {
                    if (process.Id != currentProcess.Id) // Ignorez le processus actuel
                    {
                        // Ici, vous pourriez également implémenter une logique pour terminer les processus enfants si nécessaire
                        process.Kill(); // Termine le processus
                    }
                }
            }
        }
    }
}

