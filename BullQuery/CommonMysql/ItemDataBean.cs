using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CommonMysql
{
    public class ItemDataBean : INotifyPropertyChanged
    {
        private String id;
        public String Id 
        {
            get
            {
                return this.id;
            }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private String fid;
        public String FId
        {
            get
            {
                return this.fid;
            }
            set
            {
                if (this.fid != value)
                {
                    this.fid = value;
                    OnPropertyChanged("FId");
                }
            }
        }

        private String mid;
        public String MId
        {
            get
            {
                return this.mid;
            }
            set
            {
                if (this.mid != value)
                {
                    this.mid = value;
                    OnPropertyChanged("MId");
                }
            }
        }

        private String action;
        public String Action
        {
            get
            {
                return this.action;
            }
            set
            {
                if (this.action != value)
                {
                    this.action = value;
                    OnPropertyChanged("Action");
                }
            }
        }

        private String nation;
        public String Nation
        {
            get
            {
                return this.nation;
            }
            set
            {
                if (this.nation != value)
                {
                    this.nation = value;
                    OnPropertyChanged("Nation");
                }
            }
        }

        private String gender;
        public String Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (this.gender != value)
                {
                    this.gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private String condition;
        public String Condition
        {
            get
            {
                return this.condition;
            }
            set
            {
                if (this.condition != value)
                {
                    this.condition = value;
                    OnPropertyChanged("Condition");
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
