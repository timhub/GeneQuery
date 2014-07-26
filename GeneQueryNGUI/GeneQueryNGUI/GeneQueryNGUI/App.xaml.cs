using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GeneQueryNGUI.Utilities.IniUtilities;
using GeneQueryNGUI.Utilities.Constrant;
using FirstFloor.ModernUI.Presentation;

namespace GeneQueryNGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const String LIGHT_THEME_STRING = "light";
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            String iniPath = System.Environment.CurrentDirectory + "\\config.ini";
            String theme = IniOperationUtilities.GetString("basic", "theme", "", iniPath);

            if (LIGHT_THEME_STRING.Equals(theme))
            {
                AppearanceManager.Current.ThemeSource = new Uri("FirstFloor.ModernUI;component/Assets/ModernUI.Light.xaml", UriKind.Relative);
            }
            else
            {
                AppearanceManager.Current.ThemeSource = new Uri("FirstFloor.ModernUI;component/Assets/ModernUI.Dark.xaml", UriKind.Relative);
            }
        }
    }
}
