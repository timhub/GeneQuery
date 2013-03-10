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

namespace GeneQueryMainPanel.PageContent
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string userPass = "yangz2007";
        MainPage mp = new MainPage();

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string input = this.passwordInput.Password;
            if (input.Equals(userPass))
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
