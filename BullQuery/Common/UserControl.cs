using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Common
{
    public class UserControl
    {
        BullQuery bquery = new BullQuery();

        public bool RowsCount(string Id)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from dbo.UsrTable where Id=" + Id;
            SqlCommand cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                flag = true;
            }
            bquery.conn.Close();
            return flag;
        }

        

        public bool UserValidate(String Name, String Pwd)      
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select Pwd from dbo.UsrTable where Name=@Name";
            SqlCommand cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = Name;
            bquery.sqlExcute(cmd, "UsrTable");
            if (bquery.ds.Tables["UsrTable"].Rows.Count > 0)
            {
                if (Pwd == bquery.ds.Tables["UsrTable"].Rows[0]["Pwd"].ToString())
                {
                    flag = true;
                }
            }
            bquery.conn.Close();
            return flag;
        }

        public bool InsertUserInfo(String Name,String Pwd)
        {
            bool flag = false;
            SqlCommand cmd = null;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCount(Name))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into dbo.UsrTable values (@Name,@Pwd)";
            cmd = new SqlCommand(bquery.sqlcom, bquery.conn);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = Name;
            cmd.Parameters.Add("@Pwd", SqlDbType.VarChar, 50).Value = Pwd;
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


        public bool DeleteUserInfo(String Id)
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
            bquery.sqlcom = "delete from dbo.UsrTable where Id=@Id";
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
