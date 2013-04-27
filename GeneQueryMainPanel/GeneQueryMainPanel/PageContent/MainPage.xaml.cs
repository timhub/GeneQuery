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
using BackingFunctions;
using CommonMysql;
using System.Data;
using System.Windows.Navigation;
using System.Text.RegularExpressions;

namespace GeneQueryMainPanel.PageContent
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public ItemDataBean currentBean = new ItemDataBean();
        public List<ItemDataBean> recentItemList = new List<ItemDataBean>();
        public bool firstLogedIn = true; //wait to delete after confirm
        ItemDetailPage detailPage = new ItemDetailPage();

        ValidateActionFunction validate = new ValidateActionFunction(); // input validator

        private List<ItemDataBean> allDataList;
        private bool displayFlag = true; //for displaying the all item form or current item form
        private bool addItemFlag = false; //for intercept faults in input date of add new item component

        BullAction ba = new BullAction(); // new DB function
       

        public MainPage()
        {
            InitializeComponent();
        }

        private void allItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ItemDisplay id = new ItemDisplay();
            allDataList = id.displayAllItems();
            allItemGrid.ItemsSource = allDataList;
            allItemGrid.SelectedValuePath = "Id";
        }

        private void switchViewBtn_click(object sender, RoutedEventArgs e)
        {
            if (displayFlag == true)
            {
                allItemGrid.Visibility = System.Windows.Visibility.Hidden;
                currentItemGrid.Visibility = System.Windows.Visibility.Visible;
                displayFlag = false;
            }
            else 
            {
                allItemGrid.Visibility = System.Windows.Visibility.Visible;
                currentItemGrid.Visibility = System.Windows.Visibility.Hidden;
                displayFlag = true;
            }
        }

        private void allItemGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentBean = (ItemDataBean)allItemGrid.CurrentItem;
            recentItemList.Add(currentBean);
            showDetailArea();
        }

        private void allItemGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var a = allItemGrid.SelectedItem as ItemDataBean;
            currentBean = a;
        }

        private void mainAddBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewItemGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void mainHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void saveItemBtn_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }

        private void itemidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = itemidBox.Text;
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                addItemFlag = true;
            }
            else
            {
                addItemFlag = false;
                itemidError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void addCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // add new item cancel button function, clean the input text
            cleanInput();
        }

        private void fidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = fidBox.Text;
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                addItemFlag = true;
            }
            else
            {
                addItemFlag = false;
                fidError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void midBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = midBox.Text;
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                addItemFlag = true;
            }
            else
            {
                addItemFlag = false;
                midError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void maleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(femaleCheckbox.IsChecked == true)
            {
                femaleCheckbox.IsChecked = false;
            }
        }

        private void femaleCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (maleCheckBox.IsChecked == true)
            {
                maleCheckBox.IsChecked = false;
            }
        }

        //=======================================below is action functions===================================
        private void saveData()
        {
            checkBoxCheck();
            if (addItemFlag)
            {
                String itemId = itemidBox.Text;
                String fId = fidBox.Text;
                String mId = midBox.Text;
                String gender = "";

                if (maleCheckBox.IsChecked == true && femaleCheckbox.IsChecked == false)
                {
                    gender = "M";
                }
                else if (maleCheckBox.IsChecked == false && femaleCheckbox.IsChecked == true)
                {
                    gender = "F";
                }

                ba.InsertBullInfo(itemId, mId, fId, "", "", gender, "");
                cleanInput();
            }
        }

        private void cleanInput()
        {
            itemidBox.Text = "";
            fidBox.Text = "";
            midBox.Text = "";
            maleCheckBox.IsChecked = false;
            femaleCheckbox.IsChecked = false;
        }

        private void checkBoxCheck()
        {
            if (maleCheckBox.IsChecked == false && femaleCheckbox.IsChecked == false)
            {
                addItemFlag = false;
                genderError.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void showDetailArea()
        {
            detailGrid.Visibility = System.Windows.Visibility.Visible;
            detailItemIdText.Text = currentBean.Id;
            detailFidText.Text = currentBean.FId;
            detailMIdText.Text = currentBean.MId;
            detailNationText.Text = currentBean.Nation;
        }

        //===========================some assistant functions=======================================

        private void itemidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            itemidError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void fidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fidError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void midBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            midError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void maleCheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            genderError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void femaleCheckbox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            genderError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
