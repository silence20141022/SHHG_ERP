using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Aim.Examining.Web
{
    public class DbMgr
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["conStr"];
        public static DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection orclCon = null;
            try
            {
                using (orclCon = new SqlConnection(ConnectionString))
                {
                    SqlCommand oc = orclCon.CreateCommand();
                    oc.CommandText = sql;
                    if (orclCon.State.ToString().Equals("Open"))
                    {
                        orclCon.Close();
                    }
                    orclCon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = oc;
                    adapter.Fill(ds);
                }
            }
            catch (Exception e)
            {
                //log.Error(e.Message + e.StackTrace);
            }
            finally
            {
                orclCon.Close();
            }
            return ds;
        }

        public static DataTable GetDataTable(string sql)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            SqlConnection orclCon = null;
            try
            {
                using (orclCon = new SqlConnection(ConnectionString))
                {
                    SqlCommand oc = orclCon.CreateCommand();
                    oc.CommandText = sql;
                    if (orclCon.State.ToString().Equals("Open"))
                    {
                        orclCon.Close();
                    }
                    orclCon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = oc;
                    adapter.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                orclCon.Close();
                return dt;
            }
        }

        public static int ExecuteScalar(string sql)
        {
            int retcount = -1;
            SqlConnection orclCon = null;
            try
            {
                using (orclCon = new SqlConnection(ConnectionString))
                {
                    SqlCommand oc = new SqlCommand(sql, orclCon);
                    if (orclCon.State.ToString().Equals("Open"))
                    {
                        orclCon.Close();
                    }
                    orclCon.Open();
                    retcount = Convert.ToInt32(oc.ExecuteScalar());
                    oc.Parameters.Clear();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                orclCon.Close();
            }
            return retcount;
        }
        public static int ExecuteNonQuery(string sql)
        {
            int retcount = -1;
            SqlConnection orclCon = null;
            try
            {
                using (orclCon = new SqlConnection(ConnectionString))
                {
                    SqlCommand oc = new SqlCommand(sql, orclCon);
                    if (orclCon.State.ToString().Equals("Open"))
                    {
                        orclCon.Close();
                    }
                    orclCon.Open();
                    retcount = oc.ExecuteNonQuery();
                    oc.Parameters.Clear();
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                orclCon.Close();
            }
            return retcount;
        }
    }
}