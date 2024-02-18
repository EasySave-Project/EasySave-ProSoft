using EasySave.services;
using EasySave.utils;
using System.Configuration;
using System.Data;
using System.Windows;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        Settings s = new Settings();

        //App()
        //{
        //    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(s.Lang);
        //}
        
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    ManageLang.ChangeLanguage(s.Lang);
        //}
    }

}
