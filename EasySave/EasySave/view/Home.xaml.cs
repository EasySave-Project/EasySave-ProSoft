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
using EasySave.utils;
using EasySave.controller;

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private Settings settings = Settings.Instance;
        private MainWindow mainWindow;
        public Home()
        {
            InitializeComponent();
        }

        private void BtnUserManuel_Click(object sender, RoutedEventArgs e)
        {
            // Get the PDF file from the resource file
            byte[] pdfBytes;
            if (settings.Lang == "fr")
            {
                // FR
                pdfBytes = lang.resource_pdf.User_Manual_fr;
            }
            else
            {
                // EN
                pdfBytes = lang.resource_pdf.User_Manual_en;
            }

            // Create a temporary file name
            string tempFile = System.IO.Path.GetTempFileName();

            // Save PDF file to temporary file
            File.WriteAllBytes(tempFile, pdfBytes);

            // Create a ProcessStartInfo object with the UseShellExecute property set to true
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            };

            // Start the process with the ProcessStartInfo object
            Process.Start(psi);
        }

        private void BtnUpdateNote_Click(object sender, RoutedEventArgs e)
        {
            
            byte[] pdfBytes;
            if (settings.Lang == "fr")
            {
            
                pdfBytes = lang.resource_pdf.patchNoteV3_fr;
            }
            else
            {
            
                pdfBytes = lang.resource_pdf.patchNoteV3_en;
            }

            
            string tempFile = System.IO.Path.GetTempFileName();

            
            File.WriteAllBytes(tempFile, pdfBytes);

            
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = tempFile,
                UseShellExecute = true
            };

            
            Process.Start(psi);
        }

        //==============================================
        // Navigation button
        //==============================================

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
        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.backUpController.backUpManager.AreAllJobsCompleted())
            {
                System.Windows.MessageBox.Show("Des sauvegardes sont encore en cours. L'application se fermera automatiquement une fois les sauvegardes terminées.");

                
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1); 
                timer.Tick += (s, args) =>
                {
                    if (mainWindow.backUpController.backUpManager.AreAllJobsCompleted())
                    {
                        timer.Stop(); 
                        System.Windows.Application.Current.Shutdown();
                    }
                };
                timer.Start(); 

                Btn_Leave.IsEnabled = false;
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
