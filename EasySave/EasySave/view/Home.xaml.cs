using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Forms;

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private MainWindow mainWindow;
        public Home()
        {
            InitializeComponent();
        }

        private void Btn_Home_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = home;
        }

        private void Btn_ListJob_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }

        private void BtnUserManuel_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le fichier PDF à partir du fichier ressource
            byte[] pdfBytes = lang.resource_pdf.pdf_test;

            // Créer un nom de fichier temporaire
            string tempFile = System.IO.Path.GetTempFileName();

            // Sauvegarder le fichier PDF dans le fichier temporaire
            File.WriteAllBytes(tempFile, pdfBytes);

            // Créer un objet ProcessStartInfo avec la propriété UseShellExecute à true
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            };

            // Lancer le processus avec l'objet ProcessStartInfo
            Process.Start(psi);
        }

        private void BtnUpdateNote_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le fichier PDF à partir du fichier ressource
            byte[] pdfBytes = lang.resource_pdf.pdf_test;

            // Créer un nom de fichier temporaire
            string tempFile = System.IO.Path.GetTempFileName();

            // Sauvegarder le fichier PDF dans le fichier temporaire
            File.WriteAllBytes(tempFile, pdfBytes);

            // Créer un objet ProcessStartInfo avec la propriété UseShellExecute à true
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            };

            // Lancer le processus avec l'objet ProcessStartInfo
            Process.Start(psi);
        }
    }
}
