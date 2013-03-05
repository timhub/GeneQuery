using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Common;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BullAction baction = new BullAction();
            Console.WriteLine("请分别输入FID与MID，输入-1退出程序！");
            string FID=Console.ReadLine();
            string MID = Console.ReadLine();
            while (FID != "-1")
            {
                
                if (baction.RowsCount(FID) && baction.RowsCount(MID))
                {
                    Console.WriteLine(FID + "号与" + MID + "号未进行fa校正后的近交系数为" + 100 * baction.FamilyFertileCount(FID, MID) + "%");
                    Console.WriteLine(FID + "号与" + MID + "号加入fa校正后的近交系数为" + 100 * baction.FamilyFertileCountWithFa(FID, MID) + "%");
                }
                else
                {
                    Console.WriteLine("FID或MID在数据库中不存在！");
                }
                FID = Console.ReadLine();
                MID = Console.ReadLine();
            }
            List<string> t = baction.FindFather("15");
            t.Remove(t[t.Count-1]);
            foreach (string w in t)
            {
                Console.WriteLine(w);
            }
            //Console.WriteLine(t[0]);
            //Console.WriteLine(t[1]);
            Console.WriteLine(baction.RowsCount("1234"));
            
        }

       /* public static void Main(string[] args)
        {
            UserControl usrctr = new UserControl();
            Console.WriteLine(usrctr.InsertUserInfo("5", "winolinux", "sucker"));
            //Console.WriteLine(usrctr.DeleteUserInfo("5"));

            BullAction baction = new BullAction();
            bool flag = baction.InsertBullInfo("32","64","65","1","USA");
            Console.WriteLine(flag);
            Console.WriteLine(baction.DeleteBullInfo("32"));
            //Console.WriteLine(usrctr.UserValidate("admin", "admin"));
        }*/

    }
}
