using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using MySQLDriverCS;
using CommonMysql;
namespace TestMysql
{
    class Program
    {
       static void Main(string[] args)
        {
            BullAction baction = new BullAction();
            //baction.updateInfoById("44", "00", "56", "", "usa", "M", "");
            Console.WriteLine("请分别输入FID与MID，输入-1退出程序！");
            string FID = Console.ReadLine();
            string MID = Console.ReadLine();
            while (FID != "-1")
            {
                if (baction.RowsCount(FID) && baction.RowsCount(MID))
                {
                    Console.WriteLine(FID + "号与" + MID + "号未进行fa校正后的近交系数为" + 100 * baction.FamilyFertileCount(FID, MID) + "%");
                    foreach (KeyValuePair<String, double> a in baction.FamilyFertileCountWithFaIndex(FID, MID))
                    {
                        Console.WriteLine(FID + "号与" + MID + "号加入fa校正后的近交系数为" + 100 * a.Value + "%");
                        Console.WriteLine(FID + "号与" + MID + "号的最近公共祖先为" + a.Key.ToString());
                    }
                    // Console.WriteLine(FID + "号与" + MID + "号加入fa校正后的近交系数为" + 100 * baction.FamilyFertileCountWithFaIndex(FID, MID) + "%");
                }
                else
                {
                    Console.WriteLine("FID或MID在数据库中不存在！");
                }
                FID = Console.ReadLine();
                MID = Console.ReadLine();
            }
            List<string> t = baction.FindFather("15");
            t.Remove(t[t.Count - 1]);
            foreach (string w in t)
            {
                Console.WriteLine(w);
            }
        }
    }
}
