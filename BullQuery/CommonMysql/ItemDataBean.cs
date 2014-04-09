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

        private String gfid;
        public String Gfid
        {
            get
            {
                return this.gfid;
            }
            set
            {
                if (this.gfid != value)
                {
                    this.gfid = value;
                    OnPropertyChanged("Gfid");
                }
            }
        }

        private String mfid;
        public String Mfid
        {
            get
            {
                return this.mfid;
            }
            set
            {
                if (this.mfid != value)
                {
                    this.mfid = value;
                    OnPropertyChanged("Mfid");
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

        private String temp;
        public String Temp
        {
            get
            {
                return this.temp;
            }
            set
            {
                if (this.temp != value)
                {
                    this.temp = value;
                    OnPropertyChanged("Temp");
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

        /// <summary>
        /// EBVFC－外貌等级分估计育种值  
        /// </summary>
        /// 
        private String ebvfc;
        public String EBVFC
        {
            get
            {
                return this.ebvfc;
            }
            set
            {
                if (this.ebvfc != value)
                {
                    this.ebvfc = value;
                    OnPropertyChanged("EBVFC");
                }
            }
        }

        /// <summary>
        /// TPI－总性能指数  
        /// </summary>
        /// 
        private String tpi;
        public String TPI
        {
            get
            {
                return this.tpi;
            }
            set
            {
                if (this.tpi != value)
                {
                    this.tpi = value;
                    OnPropertyChanged("TPI");
                }
            }
        }

        /// <summary>
        /// H－牛群数  
        /// </summary>
        /// 
        private String h;
        public String H
        {
            get
            {
                return this.h;
            }
            set
            {
                if (this.h != value)
                {
                    this.h = value;
                    OnPropertyChanged("H");
                }
            }
        }

        /// <summary>
        /// D－女儿数（DAUS）  
        /// </summary>
        /// 
        private String d;
        public String D
        {
            get
            {
                return this.d;
            }
            set
            {
                if (this.d != value)
                {
                    this.d = value;
                    OnPropertyChanged("D");
                }
            }
        }

        /// <summary>
        /// EBVM－产奶量估计育种值  
        /// </summary>
        /// 
        private String ebvm;
        public String EBVM
        {
            get
            {
                return this.ebvm;
            }
            set
            {
                if (this.ebvm != value)
                {
                    this.ebvm = value;
                    OnPropertyChanged("EBVM");
                }
            }
        }

        /// <summary>
        /// T－体型 
        /// </summary>
        /// 
        private String t;
        public String T
        {
            get
            {
                return this.t;
            }
            set
            {
                if (this.t != value)
                {
                    this.t = value;
                    OnPropertyChanged("T");
                }
            }
        }

        /// <summary>
        /// EBVP－乳蛋白量估计育种值  
        /// </summary>
        /// 
        private String ebvp;
        public String EBVP
        {
            get
            {
                return this.ebvp;
            }
            set
            {
                if (this.ebvp != value)
                {
                    this.ebvp = value;
                    OnPropertyChanged("EBVP");
                }
            }
        }

        /// <summary>
        /// EBCMS-泌乳系统估计育种值 
        /// </summary>
        /// 
        private String ebcms;
        public String EBCMS
        {
            get
            {
                return this.ebcms;
            }
            set
            {
                if (this.ebcms != value)
                {
                    this.ebcms = value;
                    OnPropertyChanged("EBCMS");
                }
            }
        }

        /// <summary>
        /// FL（FLC）－肢蹄
        /// </summary>
        /// 
        private String fl;
        public String FL
        {
            get
            {
                return this.fl;
            }
            set
            {
                if (this.fl != value)
                {
                    this.fl = value;
                    OnPropertyChanged("FL");
                }
            }
        }

        /// <summary>
        /// SCS－体细胞分
        /// </summary>
        /// 
        private String scs;
        public String SCS
        {
            get
            {
                return this.scs;
            }
            set
            {
                if (this.scs != value)
                {
                    this.scs = value;
                    OnPropertyChanged("SCS");
                }
            }
        }

        /// <summary>
        /// R%－重复力（预期遗传力可靠性）  
        /// </summary>
        /// 
        private String r;
        public String R
        {
            get
            {
                return this.r;
            }
            set
            {
                if (this.r != value)
                {
                    this.r = value;
                    OnPropertyChanged("R");
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        /// 
        private String others;
        public String Others
        {
            get
            {
                return this.others;
            }
            set
            {
                if (this.others != value)
                {
                    this.others = value;
                    OnPropertyChanged("Others");
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
