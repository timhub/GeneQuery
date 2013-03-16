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
using CommonMysql;
using BackingFunctions;

namespace GeneQueryMainPanel.PageContent
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        LoginFilter login = new LoginFilter();
        MainPage mp = new MainPage();

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string input = this.passwordInput.Password;
            bool flag = login.userCheck(input);
            if (flag)
            {
                this.Content = mp.Content;
            }
            else
            {
                this.elertBox.Visibility = System.Windows.Visibility.Visible;
                this.passwordInput.Password = "";
            }
        }

        private void passwordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            this.passwordInput.Password = "";
            this.elertBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void login_cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.passwordInput.Password = "";
            this.elertBox.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
