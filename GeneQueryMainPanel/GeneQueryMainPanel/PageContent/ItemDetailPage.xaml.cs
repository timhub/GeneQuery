using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ItemDetailPage.xaml
    /// </summary>
    public partial class ItemDetailPage : Window
    {
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage().Content;
        }
    }
}
