using System;
using System.Collections.Generic;
using System.IO;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Win32;

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

        private bool displayFlag = true; //for displaying the all item form or current item form
        private bool addItemFlag = false; //for intercept faults in input date of add new item component

        BullAction ba = new BullAction(); // new DB function

        //--------------fallow variables are for user control---------------
        String userName;
        bool isAdmin;
        UserIdentification ui = new UserIdentification();

        //--------------Binding test---------------------------

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(String userName)
        {
            InitializeComponent();
            
            this.userName = userName;
            this.isAdmin = ui.IsAdmin(this.userName);
        }

        public AllDataItemViewModel ViewModel
        {
            get 
            {
                return this.DataContext as AllDataItemViewModel;
            }
            set 
            {
                this.DataContext = value;
            }
        }


        private void allItemGridNew_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel = new AllDataItemViewModel();
            this.ViewModel.view = this;
            this.overviewAllNumText.Text = ViewModel.AllDataListDisplayList.Count + "";
        }


        private void currentItemGridNew_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel = new AllDataItemViewModel();
            this.ViewModel.view = this;
            this.overviewCurrentNumText.Text = ViewModel.CurrentDataListDisplayList.Count + "";
        }


        private void searchResultItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel = new AllDataItemViewModel();
            this.ViewModel.view = this;
        }

        //switch the data grid from all item to current item and from the opposite side
        private void switchViewBtn_click(object sender, RoutedEventArgs e)
        {
            if (displayFlag == true)
            {
                allItemGridNew.Visibility = System.Windows.Visibility.Hidden;
                currentItemGridNew.Visibility = System.Windows.Visibility.Visible;
                displayFlag = false;
            }
            else 
            {
                allItemGridNew.Visibility = System.Windows.Visibility.Visible;
                currentItemGridNew.Visibility = System.Windows.Visibility.Hidden;
                displayFlag = true;
            }
        }

        //show the add new item grid
        private void mainAddBtn_Click(object sender, RoutedEventArgs e)
        {
            ba.closeConnection();
            addNewItemGrid.Visibility = System.Windows.Visibility.Visible;
            analysisGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            searchResultItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        //show home page
        private void mainHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ba.closeConnection();
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
            if (addItemFlag)
            {
                addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            }
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
                if (!("".Equals(input)) && !ba.RowsCount(input))
                {
                    addItemFlag = true;
                    fidAutoInsertText.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                addItemFlag = false;
                fidError.Visibility = System.Windows.Visibility.Visible;
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
            currentBeanDataDisplay();
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Visible;
            searchResultItemGrid.Visibility = System.Windows.Visibility.Hidden;
            currentItemGridNew.Visibility = System.Windows.Visibility.Hidden;
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
            String condition = "";

            String _D = editItemD.Text;
            String _EBCMS = editItemEBCMS.Text;
            String _EBVFC = editItemEBVFC.Text;
            String _EBVM = editItemEBVM.Text;
            String _EBVP = editItemEBVP.Text;
            String _FL = editItemFL.Text;
            String _H = editItemH.Text;
            String _R = editItemR.Text;
            String _SCS = editItemSCS.Text;
            String _T = editItemT.Text;
            String _TPI = editItemTPI.Text;
            String _Others = editItemOthers.Text;

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

            if (editConditionCheckbox.IsChecked == true)
            {
                condition = "Y";
            }
            else 
            {
                condition = "N";
            }

            ba.updateInfoById(Id, MId, FId, "", "", gender, condition, _EBVFC, _TPI, _D, _H, _R, _EBVM, 
                _T, _EBVP, _EBCMS, _FL, _SCS, _Others);

            ViewModel.AllDataListDisplayList = ba.GetAllItemsInOberv();
            ViewModel.CurrentDataListDisplayList = ba.GetAllCurrentItemsInOberv();
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
            List<String> result = ba.GetAllItemsIdLike(input);
            this.analysisMid.ItemsSource = result;
            this.analysisMid.IsDropDownOpen = true;
        }

        private void analysisFid_KeyDown(object sender, KeyEventArgs e)
        {
            hideAnalysisResultElements();
            String input = analysisFid.Text;
            List<String> result = ba.GetAllItemsIdLike(input);
            this.analysisFid.ItemsSource = result;
            this.analysisFid.IsDropDownOpen = true;
        }

        private void analysisBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((!"".Equals(analysisFid.Text)) && (!"".Equals(analysisMid.Text)))
            {
                bool analysisFlag = true;
                String fid = analysisFid.Text;
                String mid = analysisMid.Text;
                String mfid = analysisMid.Text;

                String resultText = "";
                String preGenResultText = "";
                String mffid = "";
                double result = 0;
                double preGenResult = 0;

                //get the father of the 2nd item
                List<String> mffidList = ba.FindFather(mfid);
                if (mffidList.Count > 1)
                {
                    mffid = mffidList[1];
                }
                else
                {
                    mffid = -1 + "";
                }

                //check if the IDs are avaliable
                if (!ba.RowsCount(fid) && ba.RowsCount(mfid))
                {
                    analysisFlag = false;
                    MessageBox.Show("Id输入有误！");
                }
                else
                {
                    analysisFlag = true;
                }

                //do the analysis
                if (analysisFlag)
                {
                    analysisResultGrid.Visibility = System.Windows.Visibility.Visible;
                    result = getAnalysisResult(fid, mfid);
                    preGenResult = getAnalysisResult(fid, mffid);

                    resultText = result + "%";
                    preGenResultText = preGenResult + "%";

                    analysisResultText.Text = resultText;
                    if (-1 != preGenResult)
                    {
                        analysisPreGenResultText.Text = preGenResultText;
                    }
                    else
                    {
                        analysisPreGenResultText.Text = "无可用结果";
                    }

                    if (result <= 6.25)
                    {
                        analysisRedRec.Visibility = System.Windows.Visibility.Hidden;
                        analysisGreenRec.Visibility = System.Windows.Visibility.Visible;
                        analysisResultTitleText_result.Text = "适合选配";
                    }
                    else
                    {
                        analysisGreenRec.Visibility = System.Windows.Visibility.Hidden;
                        analysisRedRec.Visibility = System.Windows.Visibility.Visible;
                        analysisResultTitleText_result.Text = "不适合选配";
                    }
                    showAnalysisResultElements();
                    displayFamilyTree();
                }
            }
            else
            {
                MessageBox.Show("ID输入有误");
            }
        }

        private double getAnalysisResult(String id1, String id2)
        {
            double result = 0;
            if (ba.RowsCount(id2))
            {
                foreach (KeyValuePair<String, double> iterator in ba.FamilyFertileCountWithFaIndex(id1, id2))
                {
                    result = 100 * iterator.Value;
                }

                return result;
            }
            else
            {
                return -1;
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

        // This function is used for displaying family tree items of analysis grid
        private void displayFamilyTree()
        {
            String fid = analysisFid.Text;
            String mfid = analysisMid.Text;

            ItemDataBean fDataBean = ba.GetItemsById(fid);
            ItemDataBean mfDataBean = ba.GetItemsById(mfid);
            ItemDataBean f0DataBean = new ItemDataBean();
            ItemDataBean mf0DataBean = new ItemDataBean();

            f0DataBean.FId = "";
            f0DataBean.MId = "";
            mf0DataBean.FId = "";
            mf0DataBean.MId = "";

            //left 0 tree node
            fid00Text.Text = fDataBean.Id;
            mfid00Text.Text = mfDataBean.Id;

            //left 10 tree node
            if (!"".Equals(fDataBean.FId))
            {
                fid10Text.Text = fDataBean.FId;
                //set left father's databean
                f0DataBean = ba.GetItemsById(fDataBean.FId);
            }
            else
            {
                fid10Text.Text = "无记录";
            }
            //left 11 tree node
            if (!"".Equals(fDataBean.MId))
            {
                fid11Text.Text = fDataBean.MId;
            }
            else
            {
                fid11Text.Text = "无记录";
            }
            //right 10 tree node
            if (!"".Equals(mfDataBean.FId))
            {
                mfid10Text.Text = mfDataBean.FId;
                //set right father's data bean
                mf0DataBean = ba.GetItemsById(mfDataBean.FId);
            }
            else
            {
                mfid10Text.Text = "无记录";
            }
            //right 11 tree node
            if (!"".Equals(mfDataBean.MId))
            {
                mfid11Text.Text = mfDataBean.MId;
            }
            else
            {
                mfid11Text.Text = "无记录";
            }

            //set left 100 tree node
            if (!"".Equals(f0DataBean.FId))
            {
                fid100Text.Text = f0DataBean.FId;
            }
            else
            {
                fid100Text.Text = "无记录";
            }
            //set left 101 tree node
            if (!"".Equals(f0DataBean.MId))
            {
                fid101Text.Text = f0DataBean.MId;
            }
            else
            {
                fid101Text.Text = "无记录";
            }
            //set right 100 tree node
            if (!"".Equals(mf0DataBean.FId))
            {
                mfid100Text.Text = mf0DataBean.FId;
            }
            else
            {
                mfid100Text.Text = "无记录";
            }
            //set right 101 tree node
            if (!"".Equals(mf0DataBean.MId))
            {
                mfid101Text.Text = mf0DataBean.MId;
            }
            else
            {
                mfid101Text.Text = "无记录";
            }
        }

        // For edit grid to display data
        private void currentBeanDataDisplay()
        {
            editItemIdBox.Text = currentBean.Id;
            editFIdBox.Text = currentBean.FId;
            editMIdBox.Text = currentBean.MId;
            String gender = currentBean.Gender;
            String condition = currentBean.Condition;

            editItemD.Text = currentBean.D;
            editItemEBCMS.Text = currentBean.EBCMS;
            editItemEBVFC.Text = currentBean.EBVFC;
            editItemEBVM.Text = currentBean.EBVM;
            editItemEBVP.Text = currentBean.EBVP;
            editItemFL.Text = currentBean.FL;
            editItemH.Text = currentBean.H;
            editItemR.Text = currentBean.R;
            editItemSCS.Text = currentBean.SCS;
            editItemT.Text = currentBean.T;
            editItemTPI.Text = currentBean.TPI;
            editItemOthers.Text = currentBean.Others;

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

            if (condition.Equals("Y"))
            {
                editConditionCheckbox.IsChecked = true;
            }
            else
            {
                editConditionCheckbox.IsChecked = false;
            }
        }


        private void saveData()
        {
            checkBoxCheck();
            if ("".Equals(itemidBox.Text))
            {
                itemidError.Visibility = System.Windows.Visibility.Visible;
            }
            if ("".Equals(fidBox.Text))
            {
                fidError.Visibility = System.Windows.Visibility.Visible;
            }
            else if (!"".Equals(fidBox.Text))
            {
                String str = fidBox.Text;
                ItemDataBean fItem = ba.GetItemsById(str);
                if (!"M".Equals(fItem.Gender))
                {
                    fidError.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    fidError.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            if (addItemFlag)
            {
                String itemId = itemidBox.Text;
                String fId = fidBox.Text;
                String mId = midBox.Text;
                String gender = "";
                String condition = "";

                String _D = newItemD.Text;
                String _EBCMS = newItemEBCMS.Text;
                String _EBVFC = newItemEBVFC.Text;
                String _EBVM = newItemEBVM.Text;
                String _EBVP = newItemEBVP.Text;
                String _FL = newItemFL.Text;
                String _H = newItemH.Text;
                String _R = newItemR.Text;
                String _SCS = newItemSCS.Text;
                String _T = newItemT.Text;
                String _TPI = newItemTPI.Text;
                String _Others = newItemOthers.Text;

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

                ba.InsertBullInfo(itemId, mId, fId, "", "", gender, condition, _EBVFC, _TPI, _D, _H, _R, 
                    _EBVM, _T, _EBVP, _EBCMS, _FL, _SCS, _Others);
                ItemDataBean newItem = new ItemDataBean();
                newItem.Id = itemId;
                newItem.MId = mId;
                newItem.FId = fId;
                newItem.Gender = gender;
                newItem.Condition = condition;

                ViewModel.AllDataListDisplayList.Add(newItem);

                //insert new fid
                if (!ba.RowsCount(fId))
                {
                    ba.InsertBullInfo(fId, "", "", "", "", "M", "", "", "", "", "", "", "", "", "", "", "", "", "");
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
            analysisPreGenTitle.Visibility = System.Windows.Visibility.Visible;

            //wait to be checked when the display function is ready.
            analysisFamilyTreeGrid.Visibility = System.Windows.Visibility.Visible;
            analysisResultGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void hideAnalysisResultElements()
        {
            analysisResultTitleText.Visibility = System.Windows.Visibility.Hidden;
            analysisResultTitleText_result.Visibility = System.Windows.Visibility.Hidden;
            analysisDataTitleText.Visibility = System.Windows.Visibility.Hidden;
            analysisPreGenTitle.Visibility = System.Windows.Visibility.Hidden;
            analysisFamilyTreeGrid.Visibility = System.Windows.Visibility.Hidden;
            analysisResultGrid.Visibility = System.Windows.Visibility.Hidden;
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
            analysisPreGenResultText.Text = "";
        }

        //validate for the option's user info
        private bool optionUserInfoValidate()
        {
            bool saveFlag = true;

            if (validate.checkInfoFormat(optionUserNameModify.Text))
            {
                saveFlag = true;
            }
            else
            {
                saveFlag = false;
                optionUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionUserInfoError.Text = "用户名输入有误！";
            }

            if ("".Equals(optionUserPassModify.Password) || "".Equals(optionUserPassModifyConfirm.Password))
            {
                saveFlag = false;
                optionUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionUserInfoError.Text = "密码不可为空！";
            }

            if (optionUserPassModify.Password.Equals(optionUserPassModifyConfirm.Password))
            {
                saveFlag = true;
            } 
            else 
            {
                saveFlag = false;
                optionUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionUserInfoError.Text = "两次密码输入不匹配！";
            }

            return saveFlag;
        }

        //validate for the option's new user info
        private bool optionNewUserInfoValidate()
        {
            bool saveFlag = true;

            if (validate.checkInfoFormat(optionNewUserName.Text))
            {
                saveFlag = true;
            }
            else
            {
                saveFlag = false;
                optionNewUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionNewUserInfoError.Text = "用户名输入有误！";
            }

            if ("".Equals(optionNewUserPass.Password) || "".Equals(optionNewUserPassConfirm.Password))
            {
                saveFlag = false;
                optionNewUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionNewUserInfoError.Text = "密码不可为空！";
            }

            if (optionNewUserPass.Password.Equals(optionNewUserPassConfirm.Password))
            {
                saveFlag = true;
            }
            else
            {
                saveFlag = false;
                optionNewUserInfoError.Visibility = System.Windows.Visibility.Visible;
                optionNewUserInfoError.Text = "两次密码输入不匹配！";
            }

            return saveFlag;
        }

        // Corresponding the save button function of the option User Info Section
        private void optionUserInfoSave()
        {
            bool saveFlag = optionUserInfoValidate();
            if (saveFlag)
            {
                String name = optionUserNameModify.Text;
                String pass = optionUserPassModify.Password;
                UserBean ub = ui.getUserByName(userName);
                bool updateFlag = ui.UpdateUserInfo(ub.Id, name, pass);

                if (updateFlag)
                {
                    MessageBox.Show("修改成功！");
                    optionUserNameModify.Text = "";
                    optionUserPassModify.Password = "";
                    optionUserPassModifyConfirm.Password = "";
                }
                else
                {
                    MessageBox.Show("输入有误，请重新输入");
                    optionUserPassModify.Password = "";
                    optionUserPassModifyConfirm.Password = "";
                }
            }
        }

        private void optionNewUserInfoSave()
        {
            bool saveFlag = optionNewUserInfoValidate();
            if (saveFlag)
            {
                String userName = optionNewUserName.Text;
                String userPass = optionNewUserPass.Password;
                bool insertFlag = ui.InsertUserInfo(userName, userPass);
                if (insertFlag)
                {
                    MessageBox.Show("插入成功！");
                    optionNewUserName.Text = "";
                    optionNewUserPass.Password = "";
                    optionNewUserPassConfirm.Password = "";
                }
                else
                {
                    MessageBox.Show("插入有误，请重新输入");
                    optionNewUserPass.Password = "";
                    optionNewUserPassConfirm.Password = "";
                }
            }
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
            ba.closeConnection();
            resetInput();
            analysisGrid.Visibility = System.Windows.Visibility.Visible;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            searchResultItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addToEditBtn_Click(object sender, RoutedEventArgs e)
        {
            currentBean = ba.GetItemsById(itemidBox.Text);
            temBean = currentBean;
            currentBeanDataDisplay();
            editGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void editBackBtn_Click(object sender, RoutedEventArgs e)
        {
            editGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void addBackBtn_Click(object sender, RoutedEventArgs e)
        {
            ba.closeConnection();
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        private void optionBackBtn_Click(object sender, RoutedEventArgs e)
        {
            optionGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void optionUserInfoModifySave_Click(object sender, RoutedEventArgs e)
        {
            optionUserInfoSave();
        }

        private void optionUserNameModify_GotFocus(object sender, RoutedEventArgs e)
        {
            optionUserInfoError.Visibility = System.Windows.Visibility.Hidden;
            optionUserInfoError.Text = "";
        }

        private void optionUserPassModify_GotFocus(object sender, RoutedEventArgs e)
        {
            optionUserInfoError.Visibility = System.Windows.Visibility.Hidden;
            optionUserInfoError.Text = "";
        }

        private void optionUserPassModifyConfirm_GotFocus(object sender, RoutedEventArgs e)
        {
            optionUserInfoError.Visibility = System.Windows.Visibility.Hidden;
            optionUserInfoError.Text = "";
        }

        private void optionUserInfoModifyCancle_Click(object sender, RoutedEventArgs e)
        {
            optionUserInfoError.Visibility = System.Windows.Visibility.Hidden;
            optionUserNameModify.Text = "";
            optionUserPassModify.Password = "";
            optionUserPassModifyConfirm.Password = "";
        }

        private void optionNewUserInfoModifyCancle_Click(object sender, RoutedEventArgs e)
        {
            optionNewUserInfoError.Visibility = System.Windows.Visibility.Hidden;
            optionNewUserName.Text = "";
            optionNewUserPass.Password = "";
            optionNewUserPassConfirm.Password = "";
        }

        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            optionGrid.Visibility = System.Windows.Visibility.Visible;
            searchResultItemGrid.Visibility = System.Windows.Visibility.Hidden;
        }

        private void optionNewUserInfoModifySave_Click(object sender, RoutedEventArgs e)
        {
            optionNewUserInfoSave();
        }

        private void optionNewUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            optionNewUserInfoError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void optionNewUserPass_GotFocus(object sender, RoutedEventArgs e)
        {
            optionNewUserInfoError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void optionNewUserPassConfirm_GotFocus(object sender, RoutedEventArgs e)
        {
            optionNewUserInfoError.Visibility = System.Windows.Visibility.Hidden;
        }

        private void optionNewUserGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.isAdmin)
            {
                optionNewUserGrid.Visibility = System.Windows.Visibility.Visible;
            }
            else if (!this.isAdmin)
            {
                optionNewUserGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void optionUserNameModify_Loaded(object sender, RoutedEventArgs e)
        {
            optionUserNameModify.Text = userName;
        }

        private void allItemGridNew_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentBean = allItemGridNew.SelectedItem as ItemDataBean;
        }

        private void allItemGridNew_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentBean = allItemGridNew.SelectedItem as ItemDataBean;
            showDetailArea();
        }

        private void currentItemGridNew_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentBean = currentItemGridNew.SelectedItem as ItemDataBean;
            showDetailArea();
        }

        private void currentItemGridNew_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentBean = currentItemGridNew.SelectedItem as ItemDataBean;
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            String str = "";
            str = this.searchBar.Text;
            if (!"".Equals(str))
            {
                ObservableCollection<ItemDataBean> resultList = ba.GetAllItemsIdLikeInOberv(str);
                if (resultList.Count != 0)
                {
                    ViewModel.SearchResultDisplayList = resultList;
                }
                searchResultItemGrid.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ViewModel.SearchResultDisplayList = null;
                searchResultItemGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void searchResultItemGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            currentBean = searchResultItemGrid.CurrentItem as ItemDataBean;
            showDetailArea();
        }

        private void backupButton_Click(object sender, RoutedEventArgs e)
        {
            CsvFileOperator opt = new CsvFileOperator();
            opt.CsvFileSave(ViewModel.AllDataListDisplayList);
            MessageBox.Show("备份完成！");
        }

        private void option_dataInputBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdDialog = new OpenFileDialog();
            FileStream fsStream;
            StreamReader streamReader;
            fdDialog.DefaultExt = ".csv";
            
            if (fdDialog.ShowDialog() == true)
            {
                try
                {
                    fsStream = fdDialog.OpenFile() as FileStream;
                    streamReader = new StreamReader(fsStream);
                    streamReader.ReadLine();                        //jump to the data

                    while (!streamReader.EndOfStream)
                    {
                        ItemDataBean dataBean = new ItemDataBean();
                        String dataStr = streamReader.ReadLine();
                        List<String> dataList = dataStr.Split(',').ToList();
                        dataBean.Id = dataList[0];
                        dataBean.FId = dataList[1];
                        dataBean.MId = dataList[2];
                        dataBean.Nation = dataList[3];
                        dataBean.Gender = dataList[4];
                        dataBean.Condition = dataList[5];
                        dataBean.EBVFC = dataList[6];
                        dataBean.TPI = dataList[7];
                        dataBean.D = dataList[8];
                        dataBean.H = dataList[9];
                        dataBean.R = dataList[10];
                        dataBean.EBVM = dataList[11];
                        dataBean.T = dataList[12];
                        dataBean.EBVP = dataList[13];
                        dataBean.EBCMS = dataList[14];
                        dataBean.FL = dataList[15];
                        dataBean.SCS = dataList[16];
                        dataBean.Others = dataList[17];

                        if (!ba.RowsCount(dataBean.Id))
                        {
                            ba.InsertBullInfo(dataBean);
                        }
                        else
                        {
                            ba.updateInfoById(dataBean);
                        }
                        
                    }
                    this.ViewModel.AllDataListDisplayList = ba.GetAllItemsInOberv();
                    this.ViewModel.CurrentDataListDisplayList = ba.GetAllCurrentItemsInOberv();
                    MessageBox.Show("导入成功！");
                }
                catch (Exception)
                {
                    MessageBox.Show("文件读取异常，请关闭其他正在使用该文件的程序");
                }
            }
        }

    }
}
       