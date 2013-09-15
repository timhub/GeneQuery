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
