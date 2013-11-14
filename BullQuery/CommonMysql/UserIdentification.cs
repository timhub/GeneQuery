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
using System.Management;
using System.Management.Instrumentation;
using System.IO;
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

        public bool RowsCountByName(string Name)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select count(*) from UsrTable where Name=?Name";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_Name = new MySqlParameter("?Name", MySqlDbType.VarChar, 50);
            temp_Name.Value = Name;
            cmd.Parameters.Add(temp_Name);
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
            if (validatePC())
            {
                
            }
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

        public bool IsAdmin(String Name)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select Type from UsrTable where Name=?Name";
            MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
            MySqlParameter temp_id = new MySqlParameter("?Name", MySqlDbType.VarChar, 50);
            temp_id.Value = Name;
            cmd.Parameters.Add(temp_id);
            bquery.sqlExcute(cmd, "UsrTable");
            if (bquery.ds.Tables["UsrTable"].Rows.Count > 0)
            {
                if ("A" == bquery.ds.Tables["UsrTable"].Rows[0]["Type"].ToString())
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            bquery.conn.Close();
            return flag;
        }

        public bool InsertUserInfo(String Name, String Pwd)
        {
            bool flag = false;

            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (RowsCountByName(Name))
            {
                return false;
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "insert into UsrTable (`Name`, `Pwd`)  values ('" + Name + "','" + Pwd + "')";
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

        public bool UpdateUserInfo(String Id, String Name, String Pass)
        {
            bool flag = false;
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            //if (RowsCountByName(Name))
            //{
            //    return false;
            //}
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "UPDATE `bulldb`.`usrtable` SET `Name`='"+ Name +"', `Pwd`='"+Pass+"' WHERE `Id`='" + Id + "';";
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

        public string getHostIpName()
        {
            string mac = "";
            ManagementClass mc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return mac;
        }

        public UserBean getUserByName(String Name)
        {
            UserBean ub = new UserBean();
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            if (bquery.conn.State == ConnectionState.Closed)
            {
                bquery.conn.Open();
            }
            bquery.sqlcom = "select * from UsrTable where  `Name` = '"+ Name +"'";
            try
            {
                MySqlCommand cmd = new MySqlCommand(bquery.sqlcom, bquery.conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                ub.Id = sdr["Id"].ToString();
                ub.Name = sdr["Name"].ToString();
                ub.Password = sdr["Pwd"].ToString();
                ub.Type = sdr["Type"].ToString();
                sdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return ub;
        }

        public string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool validatePC()
        {
            bool flag = true;
            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\Public\\Program Data\\GeneQuery\\muted.txt", Encoding.Default);
                string realmac = sr.ReadLine();
                if (realmac != this.getMd5Hash(this.getMd5Hash(this.getHostIpName())))
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
    }


}
