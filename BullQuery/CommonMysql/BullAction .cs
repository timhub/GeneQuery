using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySQLDriverCS;

namespace CommonMysql
{

    public class BullAction
    {
        private double[,] dx = new double[5, 5];
        protected int count = 0;
        private List<string> list = new List<string>();
        BullQuery bquery;
        public BullAction()
        {

            bquery = new BullQuery();
            dx[0, 0] = 0.125;
            dx[0, 1] = 0.0625;
            dx[0, 2] = 0.0625;
            dx[0, 3] = 0.03125;
            dx[0, 4] = 0.03125;

            dx[1, 0] = 0.0625;
            dx[1, 1] = 0.03125;
            dx[1, 2] = 0.03125;
            dx[1, 3] = 0;
            dx[1, 4] = 0;

            dx[2, 0] = 0.0625;
            dx[2, 1] = 0.03125;
            dx[2, 2] = 0.03125;
            dx[2, 3] = 0;
            dx[2, 4] = 0;

            dx[3, 0] = 0.03125;
            dx[3, 1] = 0;
            dx[3, 2] = 0;
            dx[3, 3] = 0;
            dx[3, 4] = 0;

            dx[4, 0] = 0.03125;
            dx[4, 1] = 0;
            dx[4, 2] = 0;
            dx[4, 3] = 0;
            dx[4, 4] = 0;

        }

        /// <summary>
        /// close the connection when doing actions on the UI side.
        /// </summary>
        public void closeConnection()
        {
            bquery.conn.Close();
        }

        public void openConnection()
        {
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
        }

        public bool RowsCount(string Id)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from bulldsp where Id=?Id";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Id", MySqlDbType.VarChar,50);
            temp_id.Value = Id;
            cmd.Parameters.Add(temp_id);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                flag = true;
            }
            bquery.conn.Close();
            return flag;
        }

        public List<string> FindParents(string Id)
        {
            List<string> list = new List<string>();
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select FId,MId from bulldsp where Id=?Id";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Id", MySqlDbType.VarChar,50);
            temp_id.Value = Id;
            cmd.Parameters.Add(temp_id);
            bquery.sqlExcute(cmd, "c1");
            list.Add(bquery.ds.Tables["c1"].Rows[0]["FId"].ToString());
            list.Add(bquery.ds.Tables["c1"].Rows[0]["MId"].ToString());
            bquery.conn.Close();
            return list;
        }

        private void FindFatherLine(string Id)
        {
            if (RowsCount(Id))
            {
                list.Add(Id);
                FindFatherLine(FindParents(Id)[0]);
            }
        }

        public List<string> FindFather(string Id)
        {
            list = new List<string>();
            FindFatherLine(Id);
            return list;
        }

        public int findindex(string FId, List<string> mlist)
        {
            int count = 0;
            for (int i = 0; i < mlist.Count; i++)
            {
                if (mlist[i] == FId)
                {
                    count = i;
                    break;
                }
            }
            return count;
        }

        private int twotimes(int k)
        {
            k += 1;
            int result = 1;
            while (k > 0)
            {
                result *= 2;
                k--;
            }
            return result;
        }

        public double FamilyFertileCount(string FId, string MId)         ///2013-3-11
        {
            //list = new List<string>();
            double index = 0;
            List<string> flist = FindFather(FId);
            List<string> mlist = FindFather(MId);
            //mlist.Remove(mlist[0]);
            if (!mlist.Contains(flist[flist.Count - 1]))
            {
                index = 0;
            }
            else
            {
                int fcount = flist.Count - 1;
                int mcount = 0;
                while (fcount >= 0 && mlist.Contains(flist[fcount]))
                {
                    mcount = findindex(flist[fcount], mlist);
                    fcount--;
                }
                fcount++;
                //  mcount++;
                index = 1 / Convert.ToDouble(twotimes(fcount + mcount));

                /////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            return index;
        }

        public double FamilyFertileCountWithFa(string FId, string MId)         ///2013-3-10
        {
            //list = new List<string>();
            double index = 0;
            List<string> flist = FindFather(FId);
            List<string> mlist = FindFather(MId);
            //mlist.Remove(mlist[0]);
            if (!mlist.Contains(flist[flist.Count - 1]))
            {
                index = 0;
            }
            else
            {
                int fcount = flist.Count - 1;
                int mcount = 0;
                while (fcount >= 0 && mlist.Contains(flist[fcount]))
                {
                    mcount = findindex(flist[fcount], mlist);
                    fcount--;
                }
                fcount++;
                //  mcount++;
                index = 1 / Convert.ToDouble(twotimes(fcount + mcount));
                if (fcount < 5 && mcount < 5)
                {
                    index *= (dx[fcount, mcount] + 1);
                }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            return index;
        }

        public Dictionary<string, double> FamilyFertileCountWithFaIndex(string FId, string MId)         ///2013-2-23
        {
            Dictionary<string, double> t = new Dictionary<string, double>();
            //list = new List<string>();
            double index = 0;
            List<string> flist = FindFather(FId);
            List<string> mlist = FindFather(MId);
            //mlist.Remove(mlist[0]);
            if (!mlist.Contains(flist[flist.Count - 1]))
            {
                index = 0;
                t["-1"] = index;
            }
            else
            {
                int fcount = flist.Count - 1;
                int mcount = 0;
                while (fcount >= 0 && mlist.Contains(flist[fcount]))
                {
                    mcount = findindex(flist[fcount], mlist);
                    fcount--;
                }
                fcount++;
                //  mcount++;
                index = 1 / Convert.ToDouble(twotimes(fcount + mcount));
                if (fcount < 5 && mcount < 5)
                {
                    index *= (dx[fcount, mcount] + 1);
                }
                t[flist[fcount]] = index;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            return t;
        }

        private void PreOrderFindParents(string Id)
        {
            if (RowsCount(Id))
            {
                list.Add(Id);
                PreOrderFindParents(FindParents(Id)[0]);
                PreOrderFindParents(FindParents(Id)[1]);
            }
        }

        public List<string> FamilyTreeFinder(string Id)
        {
            list = new List<string>();
            PreOrderFindParents(Id);
            return list;
        }

        public bool InsertBullInfo(String Id, String MId, String FId, String Action, String Nation, String Gender, String Condition)
        {
            bool flag = false;
            
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCount(Id))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into bulldsp values ('" + Id + "','" + FId + "','" + MId + "','" + Action + "','" + Nation + "','"+ Gender + "','" + Condition +"')";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);        
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            bquery.conn.Close();
            return flag;
        }


        public bool InsertBullInfo(String Id, String MId, String FId, String Action, String Nation)
        {
            bool flag = false;

            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCount(Id))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into bulldsp values ('" + Id + "','" + FId + "','" + MId + "','" + Action + "','" + Nation + "')";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            bquery.conn.Close();
            return flag;
        }


        public List<ItemDataBean> GetAllItems()
        {
            bquery.sqlcom = "select * from bulldsp";
            List<ItemDataBean> resultList = new List<ItemDataBean>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ItemDataBean dataBean = new ItemDataBean();
                    dataBean.Id = sdr["Id"].ToString();
                    dataBean.FId = sdr["FId"].ToString();
                    dataBean.MId = sdr["MId"].ToString();
                    dataBean.Action = sdr["Action"].ToString();
                    dataBean.Nation = sdr["Nation"].ToString();
                    dataBean.Gender = sdr["Gender"].ToString();
                    dataBean.Condition = sdr["Condition"].ToString();
                    resultList.Add(dataBean);
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultList;
        }


        public List<ItemDataBean> GetAllItemsLike(String Id)
        {
            bquery.sqlcom = "select * from bulldsp where Id like " + "'" + Id + "%" + "'";
            List<ItemDataBean> resultList = new List<ItemDataBean>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ItemDataBean dataBean = new ItemDataBean();
                    dataBean.Id = sdr["Id"].ToString();
                    dataBean.FId = sdr["FId"].ToString();
                    dataBean.MId = sdr["MId"].ToString();
                    dataBean.Action = sdr["Action"].ToString();
                    dataBean.Gender = sdr["Gender"].ToString();
                    dataBean.Condition = sdr["Nation"].ToString();
                    resultList.Add(dataBean);
                    
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return resultList;
        }

        public List<String> GetAllItemsIdLike(String Id)
        {
            bquery.sqlcom = "select * from bulldsp where Id like " + "'" + Id + "%" + "'";
            List<String> resultList = new List<String>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    String str = sdr["Id"].ToString();
                    resultList.Add(str);

                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return resultList;
        }



        public bool DeleteBullInfo(String Id)
        {
            bool flag = false;
            MySqlCommand cmd = null;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (!RowsCount(Id))
            {
                return flag;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "delete from bulldsp where Id='" + Id + "'";
            cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            bquery.conn.Close();
            return flag;
        }

        public bool BackUpData(string savfilepath)
        {
            
           //return  bquery.BackUp(savfilepath);
            return true;
        }

        public ItemDataBean GetItemsById(string Id)
        {
            ItemDataBean dataBean = new ItemDataBean();
            if (!RowsCount(Id))
            {
                dataBean.Id = "无";
                dataBean.FId =  "无";
                dataBean.MId = "无";
                dataBean.Action = "无";
                dataBean.Gender = "无";
                dataBean.Condition = "无";
                return dataBean;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select * from bulldsp where Id='" + Id + "'";
            
            
            try
            {
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                dataBean.Id = sdr["Id"].ToString();
                dataBean.FId = sdr["FId"].ToString();
                dataBean.MId = sdr["MId"].ToString();
                dataBean.Action = sdr["Action"].ToString();
                dataBean.Gender = sdr["Gender"].ToString();
                dataBean.Condition = sdr["Nation"].ToString();
                sdr.Close();
                        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dataBean;
        }

        /// <summary>
        /// wait to be assert
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MId"></param>
        /// <param name="FId"></param>
        /// <param name="Action"></param>
        /// <param name="Nation"></param>
        /// <param name="Gender"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public bool updateInfoById(String Id, String MId, String FId, String Action, String Nation, String Gender, String Condition)
        {
            bool flag = false;
            //if (bquery.conn.State == ConnectionState.Open)
            //{
            //    bquery.conn.Close();
            //}
            if (!RowsCount(Id))
            {
                return flag;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            try
            {
                bquery.sqlcom = "UPDATE `bulldb`.`bulldsp` SET `FId`='" + FId + "', `MId`='" + MId + "', `Gender`='" + Gender + "', `Condition`='"+ Condition + "' WHERE `Id`='" + Id + "';";
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            bquery.conn.Close();
            return flag;
        }

    }
}