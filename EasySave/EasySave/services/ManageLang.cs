using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace EasySave.services
{
    public static class ManageLang
    {
        private static ResourceManager _rm;

        static ManageLang()
        {
            _rm = new ResourceManager("EasySave.lang.language", Assembly.GetExecutingAssembly());
        }

        public static string? GetString(string name)
        {
            return _rm.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
    }
}
