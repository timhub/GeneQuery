using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySQLDriverCS;
namespace CommonMysql
{
    public class UserIdentification
    {
        private BullQuery bquery;
        public UserIdentification()
        {
            bquery = new BullQuery();
        }
        public bool RowsCount(string Id)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from UsrTable where Id=?Id";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Id", MySqlDbType.VarChar, 50);
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

        public bool UserValidate(String Name, String Pwd)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select Pwd from UsrTable where Name=?Name";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Name", MySqlDbType.VarChar, 50);
            temp_id.Value = Name;
            cmd.Parameters.Add(temp_id);
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

        public bool InsertUserInfo(String Id, String Name, String Pwd)
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
            bquery.sqlcom = "insert into UsrTable values ('" + Id + "','" + Name + "','" + Pwd + "')";
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

        public bool DeleteUserInfo(String Id)
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
            bquery.sqlcom = "delete from UsrTable where Id='" + Id + "'";
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


    }


}
