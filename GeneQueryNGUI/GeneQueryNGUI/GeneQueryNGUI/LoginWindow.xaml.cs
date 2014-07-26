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
using GeneQueryNGUI.DisplayFacadeBackingBean;
using GeneQueryNGUI.Utilities.RuntimeUtilityBean;
using CommonMysql;

namespace GeneQueryNGUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserIdentification userIdentification;

        public LoginWindow()
        {
            InitializeComponent();
            userIdentification = new UserIdentification();
            this.ViewModel = new LoginPageViewModel();
        }

        public LoginPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void login_cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void login_logbtn_Click(object sender, RoutedEventArgs e)
        {
            String userPass = this.login_userPassBox.Password;
            if (userIdentification.validatePC())
            {
                if (userIdentification.UserValidate(ViewModel.UserName, userPass))
                {
                    RuntimeDataHandler.getInstance().IsAdmin = userIdentification.IsAdmin(ViewModel.UserName);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ViewModel.LoginFlag = 2;
                }
            }
            else
            {
                ViewModel.LoginFlag = 1;
            }
            manageErrorMessageDisplay();
        }

        private void manageErrorMessageDisplay()
        {
            if (ViewModel.LoginFlag == 1)
            {
                login_cdkeyerrortxt.Visibility = System.Windows.Visibility.Visible;
                login_loginfoerrortxt.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (ViewModel.LoginFlag == 2)
            {
                login_loginfoerrortxt.Visibility = System.Windows.Visibility.Visible;
                login_cdkeyerrortxt.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            login_loginfoerrortxt.Visibility = System.Windows.Visibility.Hidden;
            login_cdkeyerrortxt.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}