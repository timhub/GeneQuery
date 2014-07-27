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
using System.Web.UI.WebControls;
using System.Drawing;

namespace GeneQueryNGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ColorConverter converter = new WebColorConverter();
            String iniPath = System.Environment.CurrentDirectory + CommonConstrants.INI_FILE_PATH; 
            String theme = IniOperationUtilities.GetString("basic", "theme", "", iniPath);
            String color = IniOperationUtilities.GetString("basic", "themecolor", "", iniPath);

            if (CommonConstrants.LIGHT_THEME_STRING.Equals(theme))
            {
                AppearanceManager.Current.ThemeSource = new Uri(CommonConstrants.LIGHT_THEME_PATH, UriKind.Relative);
            }
            else
            {
                AppearanceManager.Current.ThemeSource = new Uri(CommonConstrants.DARK_THEME_PATH, UriKind.Relative);
            }

            if (!"".Equals(color))
            {
                System.Drawing.Color convertedColor = (System.Drawing.Color) converter.ConvertFromString(color);
                AppearanceManager.Current.AccentColor = System.Windows.Media.Color.FromArgb(convertedColor.A, convertedColor.R, convertedColor.G, convertedColor.B);
            }
        }
    }
}
