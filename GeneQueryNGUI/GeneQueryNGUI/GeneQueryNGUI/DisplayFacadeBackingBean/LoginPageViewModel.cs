using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneQueryNGUI.DisplayFacadeBackingBean
{
    public class LoginPageViewModel : BaseDataBindingBean
    {
        private String userName;
        public String UserName 
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private String userPass;
        public String UserPass
        {
            get
            {
                return this.userPass;
            }
            set
            {
                if (this.userPass != value)
                {
                    this.userPass = value;
                    OnPropertyChanged("UserPass");
                }
            }
        }

        private int loginFlag;
        public int LoginFlag
        {
            get
            {
                return this.loginFlag;
            }
            set
            {
                if (this.loginFlag != value)
                {
                    this.loginFlag = value;
                    OnPropertyChanged("LoginFlag");
                }
            }
        }

    }
}
