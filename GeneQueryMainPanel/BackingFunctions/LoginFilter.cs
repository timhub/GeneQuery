using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMysql;

namespace BackingFunctions
{
    public class LoginFilter
    {
        UserIdentification ui = new UserIdentification();

        public bool userCheck(string pass)
        {
            bool flag = ui.UserValidate("admin", pass);
            return flag;
        }
    }
}
