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
        private ObservableCollection<ItemDataBean> allDataListDisplayList;
        public GeneQueryMainPanel.PageContent.MainPage view { get; set; }

        public event PropertyChangedEventHandler prtyChanged;

        public AllDataItemViewModel()
        {
            this.allDataListDisplayList = ba.GetAllItemsInOberv();

            this.PropertyChanged += ModelPropertyChanged;
        }

        void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.AllDataListDisplayList = ba.GetAllItemsInOberv();
        }

        public ObservableCollection<ItemDataBean> AllDataListDisplayList
        {
            get
            {
                return this.allDataListDisplayList;
            }
            set
            {
                if (prtyChanged != null)
                {
                    //this.allDataListDisplayList = value;
                    //OnPropertyChanged("AllDataListDisplayList");
                    prtyChanged(this, new PropertyChangedEventArgs("AllDataListDisplayList"));
                    
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
