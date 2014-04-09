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
using System.Threading;
using GeneQueryMainPanel.Constants;

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
        private bool editItemFlag = false; //for intercept faults in edit panel

        BullAction ba = new BullAction(); // new DB function

        //--------------fallow variables are for user control---------------
        String userName;
        bool isAdmin;
        UserIdentification ui = new UserIdentification();

        List<String> EBVFCSelection = new List<String>();

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

            loadEBVFCSelection();
            newItemEBVFC.ItemsSource = EBVFCSelection;
            newItemEBVFC.SelectedIndex = 0;
            editItemEBVFC.ItemsSource = EBVFCSelection;
            editItemEBVFC.SelectedIndex = 0;
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

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
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
            optionGrid.Visibility = Visibility.Hidden;
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
            optionGrid.Visibility = Visibility.Hidden;
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            editGrid.Visibility = System.Windows.Visibility.Hidden;
            analysisGrid.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;
            cleanInput();
            cleanEditInput();
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
        }

        //trigger the validation function of the input text
        private void fidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = fidBox.Text;
            String grandFather = "";
            bool result = validate.checkInfoFormat(input);
            if (result)
            {
                if ((!"".Equals(input)) && ba.RowsCount(input))
                {
                    addItemFlag = true;
                    grandFather = ba.findFatherString(input);
                    
                    if (!"".Equals(grandFather))
                    {
                        midBox.Text = grandFather;
                        midBox.IsEnabled = false;
                        newMidOk.Visibility = System.Windows.Visibility.Visible;
                        newMidError.Visibility = System.Windows.Visibility.Hidden;
                        newMidOk.Text = "祖父代信息已录入";
                    }
                    else
                    {
                        midBox.IsEnabled = true;
                        midBox.Text = "";
                        newMidError.Visibility = System.Windows.Visibility.Visible;
                        newMidError.Text = "请输入祖父代ID，存入数据库";
                        newMidOk.Visibility = System.Windows.Visibility.Hidden;
                    }
                    fidAutoInsertText.Visibility = System.Windows.Visibility.Visible;
                    fidAutoInsertText.Text = "OK";
                }
            }
            else
            {
                addItemFlag = false;
                fidError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void detailViewEditBtn_Click(object sender, RoutedEventArgs e)
        {
            temBean = currentBean;

            //This block adds grandfather info to the database while user didn't input the infor for 
            //this case.
            if (!currentBean.FId.Equals(""))
            {
                String gfid = ba.findFatherString(currentBean.FId);
                if (currentBean.Gfid.Equals("") || !gfid.Equals(currentBean.Gfid))
                {
                    if (!gfid.Equals(""))
                    {
                        currentBean.Gfid = gfid;
                        ba.updateInfoById(currentBean);
                    }
                }
            }
            cleanEditInput();
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
            editMIdBox.Text = temBean.Gfid;
            editMfidBox.Text = temBean.Mfid;
            editConditionCheckbox.IsChecked = "Y".Equals(temBean.Condition) ? true : false;
            editItemD.Text = temBean.D;
            editItemEBCMS.Text = temBean.EBCMS;
            editItemEBVM.Text = temBean.EBVM;
            editItemEBVP.Text = temBean.EBVP;
            editItemFL.Text = temBean.FL;
            editItemH.Text = temBean.H;
            editItemNation.Text = temBean.Nation;
            editItemOthers.Text = temBean.Others;
            editItemR.Text = temBean.R;
            editItemSCS.Text = temBean.SCS;
            editItemT.Text = temBean.T;
            editItemTPI.Text = temBean.TPI;
            if (CommonConstants.EBVFC_SELECT_2.Equals(temBean.EBVFC))
            {
                editItemEBVFC.SelectedIndex = 1;
            }
            else if (CommonConstants.EBVFC_SELECT_3.Equals(temBean.EBVFC))
            {
                editItemEBVFC.SelectedIndex = 2;
            }
            else
            {
                editItemEBVFC.SelectedIndex = 0;
            }

            editIdDiff.Visibility = System.Windows.Visibility.Hidden;
            editIdError.Visibility = System.Windows.Visibility.Hidden;
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
            String MFId = editMfidBox.Text;
            String nation = editItemNation.Text;
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

            if (editConditionCheckbox.IsChecked == true)
            {
                condition = "Y";
                editItemFlag = true;
            }
            else 
            {
                condition = "N";
                editItemFlag = true;
            }

            if ("".Equals(editFIdBox.Text))
            {
                editItemFlag = false;
            }

            editInputCheck();

            if (!MFId.Equals(""))
            {
                if (ba.RowsCount(MFId))
                {
                    editMfidOk.Visibility = System.Windows.Visibility.Visible;
                    editMfidOk.Text = CommonConstants.SAVE_DATA_EXISTS;
                }
                else
                {
                    editMfidOk.Visibility = System.Windows.Visibility.Visible;
                    editMfidOk.Text = CommonConstants.SAVE_NEW_DATA;
                    ba.InsertBullInfo(MFId, "", "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "");
                }
            }

            if (!FId.Equals(""))
            {
                if (ba.RowsCount(FId))
                {
                    String fFather = ba.findFatherString(FId);
                    editFidOk.Visibility = System.Windows.Visibility.Visible;
                    editFidOk.Text = CommonConstants.SAVE_DATA_EXISTS;

                    //modify the grandfather's id for the corresponding item.
                    editMIdBox.Text = fFather;
                    MId = fFather;
                }
                else
                {
                    ba.InsertBullInfo(FId, "", "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "");
                    editFidOk.Visibility = System.Windows.Visibility.Visible;
                    editFidOk.Text = "";
                    editMIdBox.Text = CommonConstants.SAVE_NEW_DATA;
                }
            }

            if (editItemFlag)
            {
                if (CommonConstants.EBVFC_SELECT_1.Equals(_EBVFC))
                {
                    _EBVFC = "";
                }
                ba.updateInfoById(Id, MId, FId, "", nation, MFId, condition, _EBVFC, _TPI, _D, _H, _R, _EBVM,
                _T, _EBVP, _EBCMS, _FL, _SCS, _Others);
                refreshView();
                editGrid.Visibility = System.Windows.Visibility.Hidden;
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

        /**
         * Main entrance for analyzing, it controls the fron end display and behavior.
         */
        private void analysisBtn_Click(object sender, RoutedEventArgs e)
        {
            allResultGrid.Visibility = Visibility.Hidden;
            if ((!"".Equals(analysisFid.Text)) && (!"".Equals(analysisMid.Text)) 
                   && ba.RowsCount(analysisFid.Text) && ba.RowsCount(analysisMid.Text))
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
                MessageBox.Show("ID输入有误，无法进行分析");
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

        /**
         * The action for analyze all the items that avaliable.
         */
        private void getAllAnalysisResult(String id)
        {
            ViewModel = new AllDataItemViewModel();
            foreach (ItemDataBean bean in ViewModel.AllDataListDisplayList)
            {
                if (ba.RowsCount(id))
                {
                    if (!id.Equals(bean.Id))
                    {
                        ResultDataBean result = new ResultDataBean();
                        result.Id = bean.Id;
                        result.Condition = bean.Condition;
                        foreach (KeyValuePair<String, double> iterator in ba.FamilyFertileCountWithFaIndex(id, bean.Id))
                        {
                            result.Result = 100 * iterator.Value + "%";
                        }
                        ViewModel.ResultDataList.Add(result);
                    }
                }
                else
                {
                    MessageBox.Show("ID输入有误");
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

        /**
         * This function is for displaying the family tree for analyzing process.
         */
        private void displayFamilyTree()
        {
            String fid = analysisFid.Text;
            String mfid = analysisMid.Text;

            ItemDataBean fDataBean = ba.GetItemsById(fid);
            ItemDataBean mfDataBean = ba.GetItemsById(mfid);
            ItemDataBean f0DataBean = new ItemDataBean();
            ItemDataBean mf0DataBean = new ItemDataBean();

            f0DataBean.FId = "";
            f0DataBean.Gfid = "";
            mf0DataBean.FId = "";
            mf0DataBean.Gfid = "";

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
                fid10Text.Text = CommonConstants.NO_RECORD;
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
                mfid10Text.Text = CommonConstants.NO_RECORD;
            }

            //set left 100 tree node
            if (!"".Equals(f0DataBean.FId))
            {
                fid100Text.Text = f0DataBean.FId;
            }
            else
            {
                fid100Text.Text = CommonConstants.NO_RECORD;
            }
           
           
            //set right 100 tree node
            if (!"".Equals(mf0DataBean.FId))
            {
                mfid100Text.Text = mf0DataBean.FId;
            }
            else
            {
                mfid100Text.Text = CommonConstants.NO_RECORD;
            }
          
        }

        /**
         * For displaying corresponding data in edit pannel when selected an item from the first page.
         */
        private void currentBeanDataDisplay()
        {
            editItemIdBox.Text = currentBean.Id;
            editFIdBox.Text = currentBean.FId;
            String condition = currentBean.Condition;
            editItemNation.Text = currentBean.Nation;
            editMfidBox.Text = currentBean.Mfid;

            editItemD.Text = currentBean.D;
            editItemEBCMS.Text = currentBean.EBCMS;
            editItemEBVM.Text = currentBean.EBVM;
            editItemEBVP.Text = currentBean.EBVP;
            editItemFL.Text = currentBean.FL;
            editItemH.Text = currentBean.H;
            editItemR.Text = currentBean.R;
            editItemSCS.Text = currentBean.SCS;
            editItemT.Text = currentBean.T;
            editItemTPI.Text = currentBean.TPI;
            editItemOthers.Text = currentBean.Others;

            if (CommonConstants.EBVFC_SELECT_2.Equals(currentBean.EBVFC))
            {
                editItemEBVFC.SelectedIndex = 1;
            }
            else if (CommonConstants.EBVFC_SELECT_3.Equals(currentBean.EBVFC))
            {
                editItemEBVFC.SelectedIndex = 2;
            }
            else
            {
                editItemEBVFC.SelectedIndex = 0;
            }


            if ("".Equals(currentBean.Gfid))
            {
                editMIdBox.Text = currentBean.Gfid;
            }
            else
            {
                editMIdBox.Text = currentBean.Gfid;
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

        /**
         * Main entrance of saving data from adding new item pannel.
         */
        private void saveData()
        {
            bool addGFNodeFlag = false;

            if ("".Equals(itemidBox.Text))
            {
                addItemFlag = false;
                itemidError.Visibility = System.Windows.Visibility.Visible;
                itemidError.Text = "输入有误，请重新输入";
            }
            else
            {
                addItemFlag = true;
                itemidError.Visibility = System.Windows.Visibility.Hidden;
            }

            if ("".Equals(midBox.Text))
            {
                addItemFlag = true;
                newMidOk.Visibility = Visibility.Visible;
                newMidOk.Text = "祖先节点";
            }
            else
            {
                if (ba.RowsCount(midBox.Text))
                {
                    addItemFlag = true;
                    newMidOk.Visibility = System.Windows.Visibility.Visible;
                    newMidError.Visibility = System.Windows.Visibility.Hidden;
                    newMidOk.Text = "信息可用";
                }
                else
                {
                    addItemFlag = true;
                    newMidOk.Visibility = Visibility.Visible;
                    newMidError.Visibility = System.Windows.Visibility.Hidden;
                    newMidOk.Text = "新个体，自动加入数据库";
                    addGFNodeFlag = true;
                }
            }

            if ("".Equals(fidBox.Text))
            {
                addItemFlag = false;
                fidError.Visibility = System.Windows.Visibility.Visible;
            }
            else if (!"".Equals(fidBox.Text))
            {
                addItemFlag = true;
                fidError.Visibility = System.Windows.Visibility.Hidden;
            }

            if (itemidBox.Text.Equals(fidBox.Text) || itemidBox.Text.Equals(midBox.Text)
                || itemidBox.Text.Equals(mfidBox.Text))
            {
                MessageBox.Show("个体ID与前代ID不可相同");
                addItemFlag = false;
            }

            if (addItemFlag)
            {
                String itemId = itemidBox.Text;
                String fId = fidBox.Text;
                String gfid = midBox.Text;
                String mfId = mfidBox.Text;
                String condition = "";
                String nation = newItemNation.Text;

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

                if (!mfId.Equals(""))
                {
                    if (ba.RowsCount(mfId))
                    {
                        addMfidOk.Visibility = System.Windows.Visibility.Visible;
                        addMfidOk.Text = CommonConstants.SAVE_DATA_EXISTS;
                    }
                    else
                    {
                        addMfidOk.Visibility = System.Windows.Visibility.Visible;
                        addMfidOk.Text = CommonConstants.SAVE_NEW_DATA;
                        ba.InsertBullInfo(mfId, "", "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "");
                    }
                }

                if (addConditionCheckbox.IsChecked == true)
                {
                    condition = "Y";
                }
                else 
                {
                    condition = "N";
                }

                if (CommonConstants.EBVFC_SELECT_1.Equals(_EBVFC))
                {
                    _EBVFC = "";
                }

                ba.InsertBullInfo(itemId, fId, gfid, mfId, nation, "", condition, _EBVFC, _TPI, _D, _H, _R, 
                    _EBVM, _T, _EBVP, _EBCMS, _FL, _SCS, _Others);

                if (addGFNodeFlag)
                {
                    ba.InsertBullInfo(gfid, "", "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "");
                }

                //insert new fid
                if (!ba.RowsCount(fId))
                {
                    ba.InsertBullInfo(fId, gfid, "", "", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "");
                }
                cleanInput();
                refreshView();
            }
        }

        private void cleanInput()
        {
            itemidBox.Text = "";
            fidBox.Text = "";
            midBox.Text = "";
            mfidBox.Text = "";
            addConditionCheckbox.IsChecked = false;
            newItemD.Text = "";
            newItemEBCMS.Text = "";
            newItemEBVFC.SelectedIndex = 0;
            newItemEBVM.Text = "";
            newItemEBVP.Text = "";
            newItemFL.Text = "";
            newItemH.Text = "";
            newItemNation.Text = "";
            newItemOthers.Text = "";
            newItemR.Text = "";
            newItemSCS.Text = "";
            newItemT.Text = "";
            newItemTPI.Text = "";

            itemidError.Visibility = System.Windows.Visibility.Hidden;
            addToEditBtn.Visibility = System.Windows.Visibility.Hidden;
            fidError.Visibility = System.Windows.Visibility.Hidden;
            addItemIdOk.Visibility = System.Windows.Visibility.Hidden;
            newMidError.Visibility = System.Windows.Visibility.Hidden;
            newMidOk.Visibility = System.Windows.Visibility.Hidden;
            fidAutoInsertText.Visibility = System.Windows.Visibility.Hidden;
            addMfidOk.Visibility = System.Windows.Visibility.Hidden;

            if (midBox.IsEnabled == false)
            {
                midBox.IsEnabled = true;
            }
        }

        /**
         * Refresh the display view of the data grids.
         */
        private void refreshView()
        {
            ViewModel.AllDataListDisplayList = ba.GetAllItemsInOberv();
            ViewModel.CurrentDataListDisplayList = ba.GetAllCurrentItemsInOberv();
            overviewAllNumText.Text = ViewModel.AllDataListDisplayList.Count + "";
            overviewCurrentNumText.Text = ViewModel.CurrentDataListDisplayList.Count + "";
        }

        private void cleanEditInput()
        {
            editItemIdBox.Text = "";
            editFIdBox.Text = "";
            editMIdBox.Text = "";
            editConditionCheckbox.IsChecked = false;
            editItemD.Text = "";
            editItemEBCMS.Text = "";
            editItemEBVFC.SelectedIndex = 0;
            editItemEBVM.Text = "";
            editItemEBVP.Text = "";
            editItemFL.Text = "";
            editItemH.Text = "";
            editItemNation.Text = "";
            editItemOthers.Text = "";
            editItemR.Text = "";
            editItemSCS.Text = "";
            editItemT.Text = "";
            editItemTPI.Text = "";
            editMfidBox.Text = "";

            editIdError.Visibility = System.Windows.Visibility.Hidden;
            editIdDiff.Visibility = System.Windows.Visibility.Hidden;
            editMfidOk.Visibility = System.Windows.Visibility.Hidden;
            editFidOk.Visibility = System.Windows.Visibility.Hidden;
            editMfidOk.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showDetailArea()
        {
            detailGrid.Visibility = System.Windows.Visibility.Visible;
            detailItemIdText.Text = currentBean.Id;
            detailFidText.Text = currentBean.FId;
            detailMIdText.Text = currentBean.Gfid;
            detailNationText.Text = currentBean.Nation;
            detailGenderText.Text = "M";
            deleteItemBtn.Visibility = System.Windows.Visibility.Visible;
            deleteCfmBtn.Visibility = System.Windows.Visibility.Hidden;
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
            cleanEditInput();
            cleanInput();
            optionGrid.Visibility = Visibility.Hidden;
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
            cleanEditInput();
        }

        private void addBackBtn_Click(object sender, RoutedEventArgs e)
        {
            ba.closeConnection();
            addNewItemGrid.Visibility = System.Windows.Visibility.Hidden;
            cleanInput();
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

        /**
         * Action for backing up data.
         */
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
                        dataBean.Gfid = dataList[2];
                        dataBean.Mfid = dataList[3];
                        dataBean.Condition = dataList[4];
                        dataBean.Nation = dataList[5];
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

        private void anaAllResultBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(analysisFid.Text) && (ba.RowsCount(analysisFid.Text)) && (ba.RowsCount(analysisMid.Text)))
            {
                String id = analysisMid.Text;
                getAllAnalysisResult(id);

                analysisResultGrid.Visibility = Visibility.Hidden;
                analysisFamilyTreeGrid.Visibility = Visibility.Hidden;
                allResultGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("输入ID有误，无法进行分析");
            }
            
        }

        private void midBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String mid = midBox.Text;
            if ("".Equals(midBox.Text))
            {
                newMidError.Visibility = Visibility.Visible;
                newMidOk.Visibility = System.Windows.Visibility.Hidden;
                addItemFlag = false;
            }
            else
            {
                if (ba.RowsCount(mid))
                {
                    addItemFlag = true;
                    newMidOk.Text = "个体信息存在";
                    newMidError.Visibility = System.Windows.Visibility.Hidden;
                    newMidOk.Visibility = System.Windows.Visibility.Visible;

                }
                else
                {
                    addItemFlag = true;
                    newMidOk.Text = "新个体，将自动加入";
                    newMidError.Visibility = System.Windows.Visibility.Hidden;
                    newMidOk.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void editItemIdBox_LostFocus(object sender, RoutedEventArgs e)
        {
            editInputCheck();
        }

        private void editInputCheck()
        {
            String newid = editItemIdBox.Text;
            if ("".Equals(newid.Trim()))
            {
                editItemFlag = false;
                editIdError.Visibility = System.Windows.Visibility.Visible;
                editIdDiff.Visibility = System.Windows.Visibility.Hidden;
                editIdError.Text = "ID不能为空！";
            }
            else
            {
                if (!newid.Trim().Equals(currentBean.Id))
                {
                    editItemFlag = true;
                    editIdDiff.Visibility = System.Windows.Visibility.Visible;
                    editIdError.Visibility = System.Windows.Visibility.Hidden;
                    editIdDiff.Text = "与原ID不同";
                }
                else
                {
                    editItemFlag = true;
                    editIdDiff.Visibility = System.Windows.Visibility.Hidden;
                    editIdError.Visibility = System.Windows.Visibility.Hidden;
                }
                editIdError.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void editFIdBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String fid = editFIdBox.Text;
            String grandf = "";
            if (!fid.Equals(""))
            {
                if (ba.RowsCount(fid))
                {
                    grandf = ba.findFatherString(fid);
                    editMIdBox.Text = grandf;
                    editFidOk.Text = CommonConstants.SAVE_DATA_EXISTS;
                }
                else
                {
                    editMIdBox.Text = "";
                    editFidOk.Text = "新数据，将自动加入";
                }
            }
        }

        private void editMfidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = editMfidBox.Text;
            if (!input.Equals(""))
            {
                if (ba.RowsCount(input))
                {
                    editMfidOk.Visibility = System.Windows.Visibility.Visible;
                    editMfidOk.Text = CommonConstants.SAVE_DATA_EXISTS;
                }
                else
                {
                    editMfidOk.Visibility = System.Windows.Visibility.Visible;
                    editMfidOk.Text = CommonConstants.SAVE_NEW_DATA;
                }
            }
        }

        private void mfidBox_LostFocus(object sender, RoutedEventArgs e)
        {
            String input = mfidBox.Text;
            if (!input.Equals(""))
            {
                if (ba.RowsCount(input))
                {
                    addMfidOk.Visibility = System.Windows.Visibility.Visible;
                    addMfidOk.Text = CommonConstants.SAVE_DATA_EXISTS;
                }
                else
                {
                    addMfidOk.Visibility = System.Windows.Visibility.Visible;
                    addMfidOk.Text = CommonConstants.SAVE_NEW_DATA;
                }
            }
        }

        private void deleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            deleteCfmBtn.Visibility = System.Windows.Visibility.Visible;
        }

        private void deleteCfmBtn_Click(object sender, RoutedEventArgs e)
        {
            ba.DeleteBullInfo(currentBean.Id);
            currentBean = null;
            refreshView();
            deleteCfmBtn.Visibility = System.Windows.Visibility.Hidden;
            detailGrid.Visibility = System.Windows.Visibility.Hidden;

            MessageBox.Show("删除成功");
        }

        private void loadEBVFCSelection()
        {
            EBVFCSelection.Add(CommonConstants.EBVFC_SELECT_1);
            EBVFCSelection.Add(CommonConstants.EBVFC_SELECT_2);
            EBVFCSelection.Add(CommonConstants.EBVFC_SELECT_3);
        }
    }
}
       