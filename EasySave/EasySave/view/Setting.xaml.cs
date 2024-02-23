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

namespace EasySave.view
{
    /// <summary>
    /// Logique d'interaction pour Setting.xaml
    /// </summary>
    public partial class Setting : Page
    {

        private Settings settings = new Settings();
        public Setting()
        {
            ManageLang.ChangeLanguage(settings.Lang);
            this.DataContext = settings;

            InitializeComponent();
            Initialize_NbKo();
        }


        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des valeurs actuelles des ComboBox
            settings.LogType = ComboBox_LogType.SelectedValue.ToString();
            settings.StateType = ComboBox_StateType.SelectedValue.ToString();
            settings.Lang = ComboBox_Lang.SelectedValue.ToString();

            if (string.IsNullOrEmpty(TextBox_nbKo.Text))
            {
                // Si la TextBox est vide, affectez -1 à settings.NbKo
                settings.NbKo = -1;
            }
            else if (int.TryParse(TextBox_nbKo.Text, out int nbKoValue) && nbKoValue >= 0)
            {
                // La valeur saisie est un entier valide et est supérieure ou égale à 0
                settings.NbKo = nbKoValue;
            }
            else
            {
                // La valeur saisie n'est pas un entier valide ou est inférieure à 0
                System.Windows.MessageBox.Show(ManageLang.GetString("error_ValueNbKo"), ManageLang.GetString("error_title"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }



            // Appel de la méthode pour sauvegarder les paramètres
            settings.SaveSettings();
            ManageLang.ChangeLanguage(settings.Lang);
            System.Windows.MessageBox.Show(ManageLang.GetString("msgbox_save"));

            // Actualiser la page
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
        // Extensions à chiffrer
        //==============================================

        private void AddExtensionBtn_click(object sender, RoutedEventArgs e)
        {
            string extension = TextBox_cryptage.Text.Trim();
            // on autorise seulement les extensions donc commencant par un .
            string regexExtension = @"^\.\w+$";

            if (Regex.IsMatch(extension, regexExtension))
            {
                // Ajouter l'extension à la liste si elle n'est pas déjà présente
                if (!settings.ExtensionsToCrypt.Contains(extension))
                {
                    settings.ExtensionsToCrypt.Add(extension);
                    settings.SaveSettings(); // Sauvegarder les changements dans le fichier JSON

                    // Mettre à jour l'interface utilisateur
                    ListBoxExtensions.Items.Refresh();
                    TextBox_cryptage.Clear(); // Effacer le TextBox après l'ajout
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
            // Supposons que vous ayez un ListBox nommé ListBoxExtensions pour lister les extensions
            var selectedExtension = ListBoxExtensions.SelectedItem as string;
            if (selectedExtension != null)
            {
                settings.ExtensionsToCrypt.Remove(selectedExtension);
                settings.SaveSettings();

                // Mettre à jour la liste des extensions affichée
                ListBoxExtensions.Items.Refresh();
            }
        }

        //==============================================
        // Extensions prioritaires
        //==============================================

        private void btn_add_Priority_Click(object sender, RoutedEventArgs e)
        {
            string extension = TextBox_Priority.Text.Trim();
            // on autorise seulement les extensions donc commencant par un .
            string regexExtension = @"^\.\w+$";

            if (Regex.IsMatch(extension, regexExtension))
            {
                // Ajouter l'extension à la liste si elle n'est pas déjà présente
                if (!settings.ExtensionsToPriority.Contains(extension))
                {
                    settings.ExtensionsToPriority.Add(extension);
                    settings.SaveSettings(); // Sauvegarder les changements dans le fichier JSON

                    // Mettre à jour l'interface utilisateur
                    ListBoxExtensions_Priority.Items.Refresh();
                    TextBox_Priority.Clear(); // Effacer le TextBox après l'ajout
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
            // Supposons que vous ayez un ListBox nommé ListBoxExtensions pour lister les extensions
            var selectedExtension = ListBoxExtensions_Priority.SelectedItem as string;
            if (selectedExtension != null)
            {
                settings.ExtensionsToPriority.Remove(selectedExtension);
                settings.SaveSettings();

                // Mettre à jour la liste des extensions affichée
                ListBoxExtensions_Priority.Items.Refresh();
            }
        }



        //==============================================
        // Bouton de navigation
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
            System.Windows.Application.Current.Shutdown();
        }
    }
}
