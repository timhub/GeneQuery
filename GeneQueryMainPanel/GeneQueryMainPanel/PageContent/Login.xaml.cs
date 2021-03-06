﻿using System;
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
using System.IO;
using BackingFunctions;

namespace GeneQueryMainPanel.PageContent
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BackingFunctions.LoginFilter login = new LoginFilter();

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string passInput = this.passwordInput.Password;
            string nameInput = this.usernameInput.Text;
            bool flag = login.userCheck(nameInput, passInput);
            if (flag)
            {
                MainPage mp = new MainPage(nameInput);
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
            //this.passwordInput.Password = "";
            this.elertBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void login_cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.passwordInput.Password = "";
            this.elertBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void usernameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            this.elertBox.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
