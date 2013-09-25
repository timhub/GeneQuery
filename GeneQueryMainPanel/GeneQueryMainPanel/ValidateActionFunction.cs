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
        //BullAction dbAction = new BullAction();

        //public void insertNewItem(String Id, String MId, String FId, String Action, String Nation, String Gender, String Condition)
        //{
        //    dbAction.InsertBullInfo(Id,MId, FId, Action, Nation, Gender, Condition);
        //}

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
