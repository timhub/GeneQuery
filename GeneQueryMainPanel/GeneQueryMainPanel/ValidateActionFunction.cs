using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMysql;
using System.Text.RegularExpressions;

namespace BackingFunctions
{
    class ValidateActionFunction
    {
        public bool checkInfoFormat(String str)
        {
            Regex rex = new Regex("[a-z0-9A-Z_]+");
            Match ma = rex.Match(str);
            if (ma.Success && (!str.Equals("")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
