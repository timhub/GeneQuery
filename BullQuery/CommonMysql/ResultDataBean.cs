using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CommonMysql
{
    public class ResultDataBean
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String id;
        public String Id
        {
            get
            {
                return id;
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

        private String result;
        public String Result
        {
            get { return result; }
            set
            {
                if (this.result != value)
                {
                    this.result = value;
                    OnPropertyChanged("Result");
                }
            }
        }

        private String condition;
        public String Condition
        {
            get { return condition; }
            set
            {
                if (this.condition != value)
                {
                    this.condition = value;
                    OnPropertyChanged("Condition");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
