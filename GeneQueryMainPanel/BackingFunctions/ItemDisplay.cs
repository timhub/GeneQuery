using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMysql;

namespace BackingFunctions
{
    public class ItemDisplay
    {
        BullAction ba = new BullAction();

        public List<ItemDataBean> displayAllItems()
        {
            List<ItemDataBean> resultList = new List<ItemDataBean>();
            resultList = ba.GetAllItems();

            return resultList;
        }
    }
}
