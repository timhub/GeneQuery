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
using System.Collections.ObjectModel;

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
            bquery.sqlcom = "select count(*) from bulldsp where BULLID=?Id";
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
            bquery.sqlcom = "select FID,GFID from bulldsp where BULLID=?Id";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Id", MySqlDbType.VarChar,50);
            temp_id.Value = Id;
            cmd.Parameters.Add(temp_id);
            bquery.sqlExcute(cmd, "c1");
            list.Add(bquery.ds.Tables["c1"].Rows[0]["FID"].ToString());
            list.Add(bquery.ds.Tables["c1"].Rows[0]["GFID"].ToString());
            bquery.conn.Close();
            return list;
        }



        public int TotalCount()
        {

            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from bulldsp";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            bquery.conn.Close();
            return i;
        }


        public int CondationCount()
        {

            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from bulldsp where bulldsp.CONDITION ='Y'";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            bquery.conn.Close();
            return i;
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

        public String findFatherString(string id)
        {
            list = new List<string>();
            FindFatherLine(id);
            if (list.Count > 1)
            {
                return list[1];
            }
            else
            {
                return "";
            }
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

        public bool InsertBullInfo(String bullid, String fid, String gfid, String mfid, String Nation, String temp, String condition,
            String EBVFC, String TPI, String D, String H, String R, String EBVM, String T, String EBVP, String EBCMS, String FL,
            String SCS, String others)
        {
            bool flag = false;
            
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCount(bullid))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into bulldsp values ('" + bullid + "','" + fid + "','" + gfid + "','" + mfid + "','" + Nation + "','"+ 
                temp + "','" + condition + "','" + EBVFC + "','" + TPI + "','" + D + "','" + H + "','" + R + "','" + EBVM + "','" + 
                T + "','" + EBVP + "','" + EBCMS + "','" + FL + "','" + SCS + "','" + others + "')";
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

        public bool InsertBullInfo(ItemDataBean data)
        {
            bool flag = false;

            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCount(data.Id))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into bulldsp values ('" + data.Id + "','" + data.FId + "','" + data.Gfid + "','"+ data.Mfid + "','" + data.Nation + "','" +
                data.Temp + "','" + data.Condition + "','" + data.EBVFC + "','" + data.TPI + "','" + data.D + "','" + data.H + "','" + data.R + "','" + data.EBVM + "','" +
                data.T + "','" + data.EBVP + "','" + data.EBCMS + "','" + data.FL + "','" + data.SCS + "','" + data.Others + "')";
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
                    dataBean.Id = sdr["BULLID"].ToString();
                    dataBean.FId = sdr["FID"].ToString();
                    dataBean.Gfid = sdr["GFID"].ToString();
                    dataBean.Mfid = sdr["MFID"].ToString();
                    dataBean.Nation = sdr["NATION"].ToString();
                    dataBean.Temp = sdr["TEMP"].ToString();
                    dataBean.Condition = sdr["CONDITION"].ToString();

                    dataBean.D = sdr["D"].ToString();
                    dataBean.EBCMS = sdr["EBCMS"].ToString();
                    dataBean.EBVFC = sdr["EBVFC"].ToString();
                    dataBean.EBVM = sdr["EBVM"].ToString();
                    dataBean.EBVP = sdr["EBVP"].ToString();
                    dataBean.FL = sdr["FL"].ToString();
                    dataBean.H = sdr["H"].ToString();
                    dataBean.SCS = sdr["SCS"].ToString();
                    dataBean.T = sdr["T"].ToString();
                    dataBean.TPI = sdr["TPI"].ToString();
                    dataBean.R = sdr["R"].ToString();
                    dataBean.Others = sdr["OTHERS"].ToString();

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

        public ObservableCollection<ItemDataBean> GetAllItemsInOberv()
        {
            bquery.sqlcom = "select * from bulldsp";
            ObservableCollection<ItemDataBean> resultList = new ObservableCollection<ItemDataBean>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ItemDataBean dataBean = new ItemDataBean();
                    dataBean.Id = sdr["BULLID"].ToString();
                    dataBean.FId = sdr["FID"].ToString();
                    dataBean.Gfid = sdr["GFID"].ToString();
                    dataBean.Mfid = sdr["MFID"].ToString();
                    dataBean.Nation = sdr["NATION"].ToString();
                    dataBean.Temp = sdr["TEMP"].ToString();
                    dataBean.Condition = sdr["CONDITION"].ToString();

                    dataBean.D = sdr["D"].ToString();
                    dataBean.EBCMS = sdr["EBCMS"].ToString();
                    dataBean.EBVFC = sdr["EBVFC"].ToString();
                    dataBean.EBVM = sdr["EBVM"].ToString();
                    dataBean.EBVP = sdr["EBVP"].ToString();
                    dataBean.FL = sdr["FL"].ToString();
                    dataBean.H = sdr["H"].ToString();
                    dataBean.SCS = sdr["SCS"].ToString();
                    dataBean.T = sdr["T"].ToString();
                    dataBean.TPI = sdr["TPI"].ToString();
                    dataBean.R = sdr["R"].ToString();
                    dataBean.Others = sdr["OTHERS"].ToString();

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

        public ObservableCollection<ItemDataBean> GetAllCurrentItemsInOberv()
        {
            bquery.sqlcom = "select * from bulldsp where `CONDITION` ='Y'";
            ObservableCollection<ItemDataBean> resultList = new ObservableCollection<ItemDataBean>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ItemDataBean dataBean = new ItemDataBean();
                    dataBean.Id = sdr["BULLID"].ToString();
                    dataBean.FId = sdr["FID"].ToString();
                    dataBean.Gfid = sdr["GFID"].ToString();
                    dataBean.Mfid = sdr["MFID"].ToString();
                    dataBean.Nation = sdr["NATION"].ToString();
                    dataBean.Temp = sdr["TEMP"].ToString();
                    dataBean.Condition = sdr["CONDITION"].ToString();

                    dataBean.D = sdr["D"].ToString();
                    dataBean.EBCMS = sdr["EBCMS"].ToString();
                    dataBean.EBVFC = sdr["EBVFC"].ToString();
                    dataBean.EBVM = sdr["EBVM"].ToString();
                    dataBean.EBVP = sdr["EBVP"].ToString();
                    dataBean.FL = sdr["FL"].ToString();
                    dataBean.H = sdr["H"].ToString();
                    dataBean.SCS = sdr["SCS"].ToString();
                    dataBean.T = sdr["T"].ToString();
                    dataBean.TPI = sdr["TPI"].ToString();
                    dataBean.R = sdr["R"].ToString();
                    dataBean.Others = sdr["OTHERS"].ToString();

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

        public ObservableCollection<ItemDataBean> GetAllItemsIdLikeInOberv(String Id)
        {
            bquery.sqlcom = "select * from bulldsp where BULLID like " + "'" + Id + "%" + "'";
            ObservableCollection<ItemDataBean> resultList = new ObservableCollection<ItemDataBean>();
            List<String> idList = new List<string>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                

                while (sdr.Read())
                {
                    String str = sdr["BULLID"].ToString();
                    idList.Add(str);
                }
                sdr.Close();
                foreach (String str in idList)
                {
                    ItemDataBean bean = this.GetItemsById(str);
                    resultList.Add(bean);
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return resultList;
        }  

        public List<String> GetAllItemsIdLike(String Id)
        {
            bquery.sqlcom = "select * from bulldsp where BULLID like " + "'" + Id + "%" + "'";
            List<String> resultList = new List<String>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    String str = sdr["BULLID"].ToString();
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

        public List<ItemDataBean> GetAllCurrentItems()
        {
            bquery.sqlcom = "select * from bulldsp where `Condition` ='Y'";
            List<ItemDataBean> resultList = new List<ItemDataBean>();
            try
            {
                openConnection();
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ItemDataBean dataBean = new ItemDataBean();
                    dataBean.Id = sdr["BULLID"].ToString();
                    dataBean.FId = sdr["FID"].ToString();
                    dataBean.Gfid = sdr["GFID"].ToString();
                    dataBean.Mfid = sdr["MFID"].ToString();
                    dataBean.Nation = sdr["NATION"].ToString();
                    dataBean.Temp = sdr["TEMP"].ToString();
                    dataBean.Condition = sdr["CONDITION"].ToString();

                    dataBean.D = sdr["D"].ToString();
                    dataBean.EBCMS = sdr["EBCMS"].ToString();
                    dataBean.EBVFC = sdr["EBVFC"].ToString();
                    dataBean.EBVM = sdr["EBVM"].ToString();
                    dataBean.EBVP = sdr["EBVP"].ToString();
                    dataBean.FL = sdr["FL"].ToString();
                    dataBean.H = sdr["H"].ToString();
                    dataBean.SCS = sdr["SCS"].ToString();
                    dataBean.T = sdr["T"].ToString();
                    dataBean.TPI = sdr["TPI"].ToString();
                    dataBean.R = sdr["R"].ToString();
                    dataBean.Others = sdr["OTHERS"].ToString();

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
            bquery.sqlcom = "delete from bulldsp where BULLID='" + Id + "'";
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
                dataBean.Gfid = "无";
                dataBean.Mfid = "无";
                dataBean.Temp = "无";
                dataBean.Condition = "无";
                dataBean.D = "无";
                dataBean.EBCMS = "无";
                dataBean.EBVFC = "无";
                dataBean.EBVM = "无";
                dataBean.EBVP = "无";
                dataBean.FL = "无";
                dataBean.H = "无";
                dataBean.SCS = "无";
                dataBean.T = "无";
                dataBean.TPI = "无";
                dataBean.R = "无";
                dataBean.Others = "无";
                return dataBean;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select * from bulldsp where BULLID='" + Id + "'";
            
            
            try
            {
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                dataBean.Id = sdr["BULLID"].ToString();
                dataBean.FId = sdr["FID"].ToString();
                dataBean.Gfid = sdr["GFID"].ToString();
                dataBean.Mfid = sdr["MFID"].ToString();
                dataBean.Nation = sdr["NATION"].ToString();
                dataBean.Temp = sdr["TEMP"].ToString();
                dataBean.Condition = sdr["CONDITION"].ToString();

                dataBean.D = sdr["D"].ToString();
                dataBean.EBCMS = sdr["EBCMS"].ToString();
                dataBean.EBVFC = sdr["EBVFC"].ToString();
                dataBean.EBVM = sdr["EBVM"].ToString();
                dataBean.EBVP = sdr["EBVP"].ToString();
                dataBean.FL = sdr["FL"].ToString();
                dataBean.H = sdr["H"].ToString();
                dataBean.SCS = sdr["SCS"].ToString();
                dataBean.T = sdr["T"].ToString();
                dataBean.TPI = sdr["TPI"].ToString();
                dataBean.R = sdr["R"].ToString();
                dataBean.Others = sdr["OTHERS"].ToString();

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
        public bool updateInfoById(String bullid, String gfid, String fid, String mfid, String Nation, String temp, String Condition,
            String EBVFC, String TPI, String D, String H, String R, String EBVM, String T, String EBVP, String EBCMS, String FL,
            String SCS, String Others)
        {
            bool flag = false;
            //if (bquery.conn.State == ConnectionState.Open)
            //{
            //    bquery.conn.Close();
            //}
            if (!RowsCount(bullid))
            {
                return flag;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            try
            {
                bquery.sqlcom = "UPDATE `bulldb`.`bulldsp` SET `FID`='" + fid + "', `GFID`='" + gfid +
                    "', `TEMP`='" + temp + "', `MFID`='" + mfid + "', `NATION`='" + Nation + "', `CONDITION`='" +
                    Condition + "', `EBVFC`='" + EBVFC + "', `TPI`='" + TPI + "', `D`='" + D + 
                    "', `H`='"+ H + "', `R`='" + R + "', `EBVM`='" + EBVM + "', `T`='" + T + 
                    "', `EBVP`='" + EBVP + "', `EBCMS`='" + EBCMS + "', `FL`='" + FL + "', `SCS`='" + 
                    SCS + "', `OTHERS`='" + Others + "' WHERE `BULLID`='" + bullid + "';";
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

        public bool updateInfoById(ItemDataBean data)
        {
            bool flag = false;
            if (!RowsCount(data.Id))
            {
                return flag;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            try
            {
                bquery.sqlcom = "UPDATE `bulldb`.`bulldsp` SET `FID`='" + data.FId + 
                    "', `GFID`='" + data.Gfid + "', `TEMP`='" + data.Temp + "', `CONDITION`='" +
                    data.Condition + "', `MFID`='" + data.Mfid + "', `NATION`='" + data.Nation + "', `EBVFC`='" + data.EBVFC + 
                    "', `TPI`='" + data.TPI + "', `D`='" + data.D + "', `H`='" + 
                    data.H + "', `R`='" + data.R + "', `EBVM`='" + data.EBVM + "', `T`='" + 
                    data.T + "', `EBVP`='" + data.EBVP + "', `EBCMS`='" + 
                    data.EBCMS + "', `FL`='" + data.FL + "', `SCS`='" + data.SCS + "', `OTHERS`='" + 
                    data.Others + "' WHERE `BULLID`='" + data.Id + "';";
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