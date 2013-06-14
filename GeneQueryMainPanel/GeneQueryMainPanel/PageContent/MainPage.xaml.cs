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

        //load the data grid when the window is loaded
        private void allItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //ItemDisplay id = new ItemDisplay();
            allDataList = ba.GetAllItems();
            allItemGrid.ItemsSource = allDataList;
            allItemGrid.SelectedValuePath = "Id";
            
        }

        //switch the data grid from all item to current item and from the opposite side
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

        //trigger the show detail infomation of the current selected item
        private void allItemGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentBean = (ItemDataBean)allItemGrid.CurrentItem;
            recentItemList.Add(currentBean);
            showDetailArea();
        }

        //select the current item
        private void allItemGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var a = allItemGrid.SelectedItem as ItemDataBean;
            currentBean = a;
        }

        //show the add new item grid
        private void mainAddBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewItemGrid.Visibility = System.Windows.Visibility.Visible;
            analysisGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        //show home page
        private void mainHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            analysisGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            resetInput();
        }

        //save the input data of the add new item grid
        private void saveItemBtn_Click(object sender, RoutedEventArgs e)
        {
            saveData();
            allDataList = ba.GetAllItems();
            if (addItemFlag)
            {
                addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            }
            //allItemGrid.ItemsSource = allDataList;
            //Binding allItemBinding = new Binding("allItemGrid.ItemsSource") { Source = this.allDataList };
            //this.allItemGrid.SetBinding(DataGrid.DataContextProperty, new Binding());
        }

        //trigger the validation function of the input text
        private void itemidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = itemidBox.Text;
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                if (ba.RowsCount(input))
                {
                    addItemFlag = false;
                    itemidError.Text = "该ID已存在";
                    itemidError.Visibility = System.Windows.Visibility.Visible;
                    addToEditBtn.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    itemidError.Visibility = System.Windows.Visibility.Hidden;
                    addItemFlag = true;
                }
            }
            else
            {
                addItemFlag = false;
                itemidError.Text = "输入有误，请重新输入";
                itemidError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void addCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // add new item cancel button function, clean the input text
            cleanInput();
            itemidError.Visibility = System.Windows.Visibility.Hidden;
            addToEditBtn.Visibility = System.Windows.Visibility.Hidden;
            midError.Visibility = System.Windows.Visibility.Hidden;
            fidError.Visibility = System.Windows.Visibility.Hidden;
            genderError.Visibility = System.Windows.Visibility.Hidden;
        }

        //trigger the validation function of the input text
        private void fidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = fidBox.Text;
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                addItemFlag = true;
                fidAutoInsertText.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                addItemFlag = false;
                fidError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //trigger the validation function of the input text
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
            if (!gender.Equals(""))
            {
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
            allDataList = ba.GetAllItems();
            
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

        private void analysisMid_KeyDown(object sender, KeyEventArgs e)
        {
            hideAnalysisResultElements();
            String input = analysisMid.Text;
            List<String> result = ba.getIdLike(input, "M");
            this.analysisMid.ItemsSource = result;
            this.analysisMid.IsDropDownOpen = true;
        }

        private void analysisFid_KeyDown(object sender, KeyEventArgs e)
        {
            hideAnalysisResultElements();
            String input = analysisFid.Text;
            List<String> result = ba.getIdLike(input, "F");
            this.analysisFid.ItemsSource = result;
            this.analysisFid.IsDropDownOpen = true;
        }

        private void analysisBtn_Click(object sender, RoutedEventArgs e)
        {
            bool analysisFlag = true;
            string fid = analysisFid.Text;
            string mid = analysisMid.Text;

            if (!ba.RowsCount(fid) && ba.RowsCount(mid))
            {
                analysisFlag = false;
                MessageBox.Show("父代或母代Id输入有误！");
            }
            else
            {
                analysisFlag = true;
            }
            if (analysisFlag)
            {
                foreach (KeyValuePair<String, double> result in ba.FamilyFertileCountWithFaIndex(fid, mid))
                {
                    string resultText = 100 * result.Value+"%";
                    analysisResultText.Text = resultText;
                    if (result.Value <= 0.0625)
                    {
                        analysisGreenRec.Visibility = System.Windows.Visibility.Visible;
                        analysisResultTitleText_result.Text = "适合选配";
                    }
                    else
                    {
                        analysisRedRec.Visibility = System.Windows.Visibility.Visible;
                        analysisResultTitleText_result.Text = "不适合选配";
                    }
                    showAnalysisResultElements();
                }
            }
        }

        private void analysisMid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            analysisMid.IsDropDownOpen = true;
            hideAnalysisResultElements();
        }

        private void analysisFid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            analysisFid.IsDropDownOpen = true;
            hideAnalysisResultElements();
        }

        //=======================================below is action functions===================================
        # region actions
        private void saveData()
        {
            checkBoxCheck();
            if (addItemFlag)
            {
                String itemId = itemidBox.Text;
                String fId = fidBox.Text;
                String mId = midBox.Text;
                String gender = "";
                String condition = "";

                //temporary variable, wait to implement till we change the BullAction code.
                //TextRange others = new TextRange(addOtherDataInput.Document.ContentStart, addOtherDataInput.Document.ContentEnd);
                //String othersContent = others.Text;

                if (maleCheckBox.IsChecked == true && femaleCheckbox.IsChecked == false)
                {
                    gender = "M";
                }
                else if (maleCheckBox.IsChecked == false && femaleCheckbox.IsChecked == true)
                {
                    gender = "F";
                }

                if (addConditionCheckbox.IsChecked == true)
                {
                    condition = "Y";
                }
                else 
                {
                    condition = "N";
                }

                ba.InsertBullInfo(itemId, mId, fId, "", "", gender, condition);

                //insert new fid
                if (!ba.RowsCount(fId))
                {
                    ba.InsertBullInfo(fId, "", "", "", "", "M", "");
                }
                cleanInput();
            }
        }

        //reset all input value when switching module.
        private void resetInput()
        {
            // reset add item grid boxes
            itemidBox.Text = "";
            fidBox.Text = "";
            midBox.Text = "";
            femaleCheckbox.IsChecked = false;
            maleCheckBox.IsChecked = false;
            itemidError.Visibility = System.Windows.Visibility.Hidden;
            midError.Visibility = System.Windows.Visibility.Hidden;
            fidError.Visibility = System.Windows.Visibility.Hidden;
            genderError.Visibility = System.Windows.Visibility.Hidden;

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
                genderError.Visibility = System.Windows.Visibility.Visible;
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

        private void showAnalysisResultElements()
        {
            analysisResultTitleText.Visibility = System.Windows.Visibility.Visible;
            analysisResultTitleText_result.Visibility = System.Windows.Visibility.Visible;
            analysisDataTitleText.Visibility = System.Windows.Visibility.Visible;
        }

        private void hideAnalysisResultElements()
        {
            analysisResultTitleText.Visibility = System.Windows.Visibility.Hidden;
            analysisResultTitleText_result.Visibility = System.Windows.Visibility.Hidden;
            analysisDataTitleText.Visibility = System.Windows.Visibility.Hidden;
            if (analysisGreenRec.Visibility == System.Windows.Visibility.Visible)
            {
                analysisGreenRec.Visibility = System.Windows.Visibility.Hidden;
            }
            if (analysisRedRec.Visibility == System.Windows.Visibility.Visible)
            {
                analysisRedRec.Visibility = System.Windows.Visibility.Hidden;
            }
            analysisResultText.Text = "";
            analysisResultTitleText_result.Text = "";
        }
        #endregion

        # region default actions
        //===========================some assistant functions=======================================
        private void itemidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            itemidError.Visibility = System.Windows.Visibility.Hidden;
            addToEditBtn.Visibility = System.Windows.Visibility.Hidden;
        }

        private void fidBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            fidError.Visibility = System.Windows.Visibility.Hidden;
            fidAutoInsertText.Visibility = System.Windows.Visibility.Hidden;
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
            resetInput();
            analysisGrid.Visibility = System.Windows.Visibility.Visible;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addToEditBtn_Click(object sender, RoutedEventArgs e)
        {
            editGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void editBackBtn_Click(object sender, RoutedEventArgs e)
        {
            editGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addBackBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion
    }
}
       