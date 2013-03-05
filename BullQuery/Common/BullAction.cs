using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Common
{
    public class BullAction
    {
        private double[,] dx=new double[5,5];
        
        protected int count = 0;
        private List<string> list = new List<string>();
        BullQuery bquery;
        public BullAction()
        {
            
            bquery = new BullQuery();
            dx[0,0] = 0.125;
            dx[0,1] = 0.0625;
            dx[0,2] = 0.0625;
            dx[0,3] = 0.03125;
            dx[0,4] = 0.03125;

            dx[1,0] = 0.0625;
            dx[1,1] = 0.03125;
            dx[1,2] = 0.03125;
            dx[1,3] = 0;
            dx[1,4] = 0;

            dx[2,0] = 0.0625;
            dx[2,1] = 0.03125;
            dx[2,2] = 0.03125;
            dx[2,3] = 0;
            dx[2,4] = 0;

            dx[3,0] = 0.03125;
            dx[3,1] = 0;
            dx[3,2] = 0;
            dx[3,3] = 0;
            dx[3,4] = 0;

            dx[4,0] = 0.03125;
            dx[4,1] = 0;
            dx[4,2] = 0;
            dx[4,3] = 0;
            dx[4,4] = 0;

        }
        public bool RowsCount(string Id)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from dbo.Cowdescription where Id=@Id";

            SqlCommand cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Id;
            int i=Convert.ToInt32(cmd.ExecuteScalar());
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
            bquery.sqlcom = "select FId,MId from dbo.Cowdescription where Id=@Id";
            SqlCommand cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Id;
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
            while (k>0)
            {
                result *= 2;
                k--;
            }
            return result;
        }

        public double FamilyFertileCount(string FId, string MId)         ///2013-2-23
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
                while (fcount>=0&&mlist.Contains(flist[fcount]))
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

        public double FamilyFertileCountWithFa(string FId, string MId)         ///2013-2-23
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
                if (fcount < 5 && mcount< 5)
                {
                    index *= (dx[fcount, mcount] + 1);
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            return index;
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
        

        public bool InsertBullInfo(String Id,String MId,String FId,String Action,String Nation)
        {
            bool flag = false;
            SqlCommand cmd = null;
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
            bquery.sqlcom = "insert into dbo.Cowdescription values (@Id,@MId,@FId,@Action,@Nation)";
            cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Id;
            cmd.Parameters.Add("@MId", SqlDbType.VarChar, 50).Value = MId;
            cmd.Parameters.Add("@FId", SqlDbType.VarChar, 50).Value = FId;
            cmd.Parameters.Add("@Action", SqlDbType.Char, 10).Value = Action;
            cmd.Parameters.Add("@Nation", SqlDbType.Char, 10).Value = Nation;
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


        public bool DeleteBullInfo(String Id)
        {
            bool flag = false;
            SqlCommand cmd = null;
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
            bquery.sqlcom = "delete from dbo.Cowdescription where Id=@Id";
            cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Id;
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
    }
}
