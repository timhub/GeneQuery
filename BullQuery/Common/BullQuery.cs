using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public class BullQuery
{
    public BullQuery()
    {

        conn.Open();
        connstr = "uid=gqadmin; pwd=111111;Database=BullDb ; Data source=(local)";
    }

    public string connstr;
    public DataSet ds = new DataSet();
    public SqlDataReader MyReader;
    public SqlConnection conn = new SqlConnection("uid=gqadmin; pwd=111111;Database=BullDb ; Data source=(local)");
    public void KillConnection()
    {
        conn.Close();
    }
    public SqlCommand cmd(string sqlString)
    {
        SqlCommand cmd = new SqlCommand(sqlString, conn);
        return cmd;
    }
    public SqlDataAdapter adapterfill(SqlCommand cmd)
    {
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        return sda;
    }

    public DataSet resultFill(SqlCommand cmd)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter sda = adapterfill(cmd);
        sda.Fill(ds);
        return ds;

    }

    public DataSet resultFill(SqlCommand cmd, string tablename)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter sda = adapterfill(cmd);
        sda.Fill(ds, tablename);
        return ds;

    }

    public string sqlcom = " ";

    public void sqlExcute(SqlCommand cmd)
    {
        ds = new DataSet();
        SqlDataAdapter sda = adapterfill(cmd);
        sda.SelectCommand.CommandTimeout = 1000;
        sda.Fill(ds);

    }

    public void sqlExcute(string sqlcmd)
    {
        ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(sqlcmd, conn);
        sda.Fill(ds);

    }

    public void sqlExcute(string sqlcmd, string tablename)
    {
        ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(sqlcmd, conn);
        sda.Fill(ds, tablename);

    }

    public void sqlExcute(SqlCommand cmd, string tablename)
    {
        ds = new DataSet();
        SqlDataAdapter sda = adapterfill(cmd);
        sda.Fill(ds, tablename);
    }






}