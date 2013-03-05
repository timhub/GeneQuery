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
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace WpfApplication1.PageContent
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>

    public partial class LoginPage : Window
    {
        private static string USER_PASSWORD = "yangz2007";
        MainWindow mw = new MainWindow();
        NavigationService ns {get;set;}     

        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            loginPassbox.Clear();
        }

        private void loginLogBtn_Click(object sender, RoutedEventArgs e)
        {
            string userPass = loginPassbox.Password;
            if (!USER_PASSWORD.Equals(userPass))
            {
                loginErrorRct.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                loginErrorRct.Visibility = System.Windows.Visibility.Hidden;
                this.Content = mw.Content;
            }
        }

        private void loginLogBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }
    }
}
