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
        ItemDataBean temBean = new ItemDataBean();
        public List<ItemDataBean> recentItemList = new List<ItemDataBean>();
        public bool firstLogedIn = true; //wait to delete after confirm

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
            //ItemDisplay id = new ItemDisplay();
            allDataList = ba.GetAllItems();
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
            editGrid.Visibility = System.Windows.Visibility.Hidden;
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

        private void detailViewEditBtn_Click(object sender, RoutedEventArgs e)
        {
            temBean = currentBean;
            editItemIdBox.Text = currentBean.Id;
            editFIdBox.Text = currentBean.FId;
            editMIdBox.Text = currentBean.MId;
            String gender = currentBean.Gender;
            if (gender.Equals("M"))
            {
                editMaleCheckbox.IsChecked = true;
                editFemaleCheckbox.IsChecked = false;
            }
            else if (gender.Equals("F"))
            {
                editMaleCheckbox.IsChecked = false;
                editFemaleCheckbox.IsChecked = true;
            }
            else
            {
                editMaleCheckbox.IsChecked = false;
                editFemaleCheckbox.IsChecked = false;
            }
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void editCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            editItemIdBox.Text = temBean.Id;
            editFIdBox.Text = temBean.FId;
            editMIdBox.Text = temBean.MId;
            String gender = temBean.Gender;
            if (gender.Equals("M"))
            {
                editMaleCheckbox.IsChecked = true;
                editFemaleCheckbox.IsChecked = false;
            }
            else
            {
                editMaleCheckbox.IsChecked = false;
                editFemaleCheckbox.IsChecked = true;
            }
        }

        /// <summary>
        /// function of the updating function on edit detail page, wait to be done, empty input "Action", "Nation", "Condition"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            String Id = editItemIdBox.Text;
            String FId = editFIdBox.Text;
            String MId = editMIdBox.Text;

            String gender = "";
            if (editMaleCheckbox.IsChecked == true)
            {
                gender = "M";
            }
            else if (editFemaleCheckbox.IsChecked == true)
            {
                gender = "F";
            }
            else
            {
                gender = "";
            }

            ba.updateInfoById(Id,MId,FId,"","",gender,"");
            editGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void editMaleCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (editFemaleCheckbox.IsChecked == true) 
            {
                editFemaleCheckbox.IsChecked = false;
            }
        }

        private void editFemaleCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (editMaleCheckbox.IsChecked == true)
            {
                editMaleCheckbox.IsChecked = false;
            }
        }

        /// <summary>
        /// for analysis page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analysisMid_KeyDown(object sender, KeyEventArgs e)
        {
            String targetStr = analysisMid.Text;
            List<ItemDataBean> resultList = ba.GetAllItemsLike(targetStr);
            analysisMid.ItemsSource = resultList;
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
            detailGenderText.Text = currentBean.Gender;
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

        private void analysisBackBtn_Click(object sender, RoutedEventArgs e)
        {
            analysisGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void analysisPageBtn_Click(object sender, RoutedEventArgs e)
        {
            analysisGrid.Visibility = System.Windows.Visibility.Visible;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
