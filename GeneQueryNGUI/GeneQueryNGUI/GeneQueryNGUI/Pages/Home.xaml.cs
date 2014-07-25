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

namespace GeneQueryNGUI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public HomePageViewModel ViewModel
        {
            get
            {
                return this.DataContext as HomePageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }
        public Home()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel = new HomePageViewModel();
            this.ViewModel.view = this;
        }
    }
}
