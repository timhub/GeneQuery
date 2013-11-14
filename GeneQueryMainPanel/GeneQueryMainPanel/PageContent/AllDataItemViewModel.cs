using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommonMysql;
using System.Collections.ObjectModel;
using GeneQueryMainPanel;

namespace GeneQueryMainPanel
{
    public class AllDataItemViewModel : INotifyPropertyChanged
    {
        BullAction ba = new BullAction();
        
        public GeneQueryMainPanel.PageContent.MainPage view { get; set; }

        public AllDataItemViewModel()
        {
            this.allDataListDisplayList = ba.GetAllItemsInOberv();
            this.currentDataListDisplayList = ba.GetAllCurrentItemsInOberv();
        }

        private ObservableCollection<ItemDataBean> allDataListDisplayList;
        public ObservableCollection<ItemDataBean> AllDataListDisplayList
        {
            get
            {
                return this.allDataListDisplayList;
            }
            set
            {
                if (this.allDataListDisplayList != value)
                {
                    this.allDataListDisplayList = value;
                    OnPropertyChanged("AllDataListDisplayList");
                }
            }
        }

        private ObservableCollection<ItemDataBean> currentDataListDisplayList;
        public ObservableCollection<ItemDataBean> CurrentDataListDisplayList
        {
            get
            {
                return this.currentDataListDisplayList;
            }
            set
            {
                if (this.currentDataListDisplayList != value)
                {
                    this.currentDataListDisplayList = value;
                    OnPropertyChanged("CurrentDataListDisplayList");
                }
            }
        }

        private ObservableCollection<ItemDataBean> searchResultDisplayList;
        public ObservableCollection<ItemDataBean> SearchResultDisplayList
        {
            get
            {
                return this.searchResultDisplayList;
            }
            set
            {
                if (this.searchResultDisplayList != value)
                {
                    this.searchResultDisplayList = value;
                    OnPropertyChanged("SearchResultDisplayList");
                }
            }
        }

        private ObservableCollection<ResultDataBean> resultDataList = new ObservableCollection<ResultDataBean>();
        public ObservableCollection<ResultDataBean> ResultDataList
        {
            get { return this.resultDataList; }
            set
            {
                if (this.resultDataList != value)
                {
                    this.resultDataList = value;
                    OnPropertyChanged("ResultDataList");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
