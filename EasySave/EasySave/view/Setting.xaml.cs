using EasySave.view;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using EasySave.utils;
using EasySave.services;
using System.Text.RegularExpressions;
using System.Globalization;
using EasySave.controller;

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {


        private Settings settings = Settings.Instance;

        private BackUpController backUpController;
        public Setting()
        {
            ManageLang.ChangeLanguage(settings.Lang);
            this.DataContext = settings;
            InitializeComponent();
            Initialize_NbKo();
        }


        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Retrieving current ComboBox values
            settings.LogType = ComboBox_LogType.SelectedValue.ToString();
            settings.StateType = ComboBox_StateType.SelectedValue.ToString();
            settings.Lang = ComboBox_Lang.SelectedValue.ToString();

            if (string.IsNullOrEmpty(TextBox_nbKo.Text))
            {
                // If the TextBox is empty, assign -1 to settings.NbKo
                settings.NbKo = -1;
            }
            else if (int.TryParse(TextBox_nbKo.Text, out int nbKoValue) && nbKoValue >= 0)
            {
                // The value entered is a valid integer and is greater than or equal to 0
                settings.NbKo = nbKoValue;
            }
            else
            {
                // The value entered is not a valid integer or is less than 0
                System.Windows.MessageBox.Show(ManageLang.GetString("error_ValueNbKo"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }



            // Call method to save parameters
            settings.SaveSettings();
            ManageLang.ChangeLanguage(settings.Lang);
            System.Windows.MessageBox.Show(ManageLang.GetString("msgbox_save"));

            // Call the method to save parameters
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }

        private void Initialize_NbKo()
        {
            if (settings.NbKo == -1)
            {
                TextBox_nbKo.Text = "";
            }
            else
            {
                TextBox_nbKo.Text = settings.NbKo.ToString();
            }
        }

        //==============================================
        // Extensions to be costed
        //==============================================

        private void AddExtensionBtn_click(object sender, RoutedEventArgs e)
        {
            string extension = TextBox_cryptage.Text.Trim();
            
            string regexExtension = @"^\.\w+$";

            if (Regex.IsMatch(extension, regexExtension))
            {
                
                if (!settings.ExtensionsToCrypt.Contains(extension))
                {
                    settings.ExtensionsToCrypt.Add(extension);

                
                    ListBoxExtensions.Items.Refresh();
                    TextBox_cryptage.Clear(); 
                }
                else
                {
                    System.Windows.MessageBox.Show(ManageLang.GetString("error_add_encrypt"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_add_format"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteBtn_click(object sender, RoutedEventArgs e)
        {
            
            var selectedExtension = ListBoxExtensions.SelectedItem as string;
            if (selectedExtension != null)
            {
                settings.ExtensionsToCrypt.Remove(selectedExtension);

                
                ListBoxExtensions.Items.Refresh();
            }
        }

        //==============================================
        // Extensions must be encrypted
        //==============================================

        private void btn_add_Priority_Click(object sender, RoutedEventArgs e)
        {
            string extension = TextBox_Priority.Text.Trim();
            
            string regexExtension = @"^\.\w+$";

            if (Regex.IsMatch(extension, regexExtension))
            {
            
                if (!settings.ExtensionsToPriority.Contains(extension))
                {
                    settings.ExtensionsToPriority.Add(extension);

            
                    ListBoxExtensions_Priority.Items.Refresh();
                    TextBox_Priority.Clear(); 
                }
                else
                {
                    System.Windows.MessageBox.Show(ManageLang.GetString("error_add_encrypt"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_add_format"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btn_delete_Priority_Click(object sender, RoutedEventArgs e)
        {
         
            var selectedExtension = ListBoxExtensions_Priority.SelectedItem as string;
            if (selectedExtension != null)
            {
                settings.ExtensionsToPriority.Remove(selectedExtension); 

              
                ListBoxExtensions_Priority.Items.Refresh();
            }
        }

        //==============================================
        // Extensions prioritaires
        //==============================================

        private void btn_add_ApplicationBusiness_Click(object sender, RoutedEventArgs e)
        {
          
            if (string.IsNullOrEmpty(TextBox_ApplicationBusiness.Text))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_Caract"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        
            if (settings.BusinessApplication.Contains(TextBox_ApplicationBusiness.Text))
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_setting_app_same"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
           
            settings.BusinessApplication.Add(TextBox_ApplicationBusiness.Text);

         
            ListBoxExtensions_ApplicationBusiness.Items.Refresh();
            TextBox_ApplicationBusiness.Clear();

        }

        private void btn_delete_ApplicationBusiness_Click(object sender, RoutedEventArgs e)
        {
            var selectedApplication = ListBoxExtensions_ApplicationBusiness.SelectedItem as string;
            if (selectedApplication != null)
            {
                settings.BusinessApplication.Remove(selectedApplication);
                ListBoxExtensions_ApplicationBusiness.Items.Refresh();
            }
        }

        //==============================================
        // Navigation button
        //==============================================

        private void home_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = home;
        }

        private void setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }

        private void Btn_ListJob_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }
        private void Btn_Leave_Click(object sender, RoutedEventArgs e)
        {
            if (!backUpController.backUpManager.AreAllJobsCompleted())
            {
                System.Windows.MessageBox.Show("Des sauvegardes sont encore en cours. L'application se fermera automatiquement une fois les sauvegardes terminées.");

                
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1); 
                timer.Tick += (s, args) =>
                {
                    if (backUpController.backUpManager.AreAllJobsCompleted())
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
