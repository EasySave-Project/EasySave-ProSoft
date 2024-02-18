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
        }

        private void list_Click(object sender, RoutedEventArgs e)
        {
            ListJob listJob = new ListJob();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = listJob;
        }

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

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Récupération des valeurs actuelles des ComboBox
            settings.LogType = ComboBox_LogType.SelectedValue.ToString();
            settings.StateType = ComboBox_StateType.SelectedValue.ToString();
            settings.Lang = ComboBox_Lang.SelectedValue.ToString();

            // Appel de la méthode pour sauvegarder les paramètres
            settings.SaveSettings();
            ManageLang.ChangeLanguage(settings.Lang);
            System.Windows.MessageBox.Show(ManageLang.GetString("msgbox_save"));

            // Actualiser la page
            Setting setting = new Setting();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = setting;
        }


        private void AddExtensionBtn_click(object sender, RoutedEventArgs e)
        {
            string extension = TextBox_cryptage.Text.Trim();
            // on autorise seulement les extensions donc commencant par un .
            string regexExtension = @"^\.\w+$";


            if (Regex.IsMatch(extension,regexExtension))
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
                    System.Windows.MessageBox.Show(ManageLang.GetString("error_add_encrypt"));
                }
            }
            else
            {
                System.Windows.MessageBox.Show(ManageLang.GetString("error_add_format"));
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
    }
}
