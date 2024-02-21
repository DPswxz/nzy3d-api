using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzy3d_winformsDemo
{
    public class PtDB
    {
        public static string s_method;               //方法名，手动为Manual

        public static string s_methodInfo;           //方法信息

        public static string s_catalogName;          //谱图目录

        public static string s_ptName;               //谱图名称

        public static string s_userName = "N/A";     //用户名

        public static string SFileName
        {
            get
            {
                if (string.IsNullOrEmpty(s_fileName))
                {
                    return "";
                }
                else
                {
                    return s_fileName.Remove(0, 2);
                }
            }
        }

        private static string s_fileName = "";       //当前谱图数据路径

        private static string s_scanName = "";       //波段扫描数据表名

        private static string s_startDate = "";      //运行开始时间字符串


        /// <summary>
        /// 初始化表
        /// </summary>
        public void InitTable()
        {
            if (!HasTable())
            {
                CreateTable();
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns></returns>
        public bool CreateTable()
        {
            SqlConnection conn = new SqlConnection(PublicPath.DB);

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"CREATE TABLE ptinfo(
                [CreateTime] [varchar](14),
                [username] [nvarchar](32) NOT NULL,
                [Catalog] [nvarchar](MAX),
	            [Batch] [nvarchar](MAX),
                [MethodName] [nvarchar](MAX),
	            [CV] [float],
	            [ZG] [float],
                [Wave1] [int],
                [Wave2] [int],
	            [STime] [varchar](19),
	            [ETime] [varchar](19),
	            [MethodInfo] [nvarchar](MAX)
                );";
                cmd.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != conn)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns></returns>
        public bool HasTable()
        {
            SqlConnection conn = new SqlConnection(PublicPath.DB);
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from sys.tables where name ='ptinfo'";
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != reader)
                {
                    cmd.Cancel();
                    reader.Close();
                }
                if (null != conn)
                {
                    conn.Close();
                }
            }
        }

        public static bool CheckTableExist(string name)
        {
            SqlConnection conn = new SqlConnection(PublicPath.PtDB);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"IF OBJECT_ID(N'sc{name}', N'U') IS NOT NULL SELECT 1 AS res ELSE SELECT 0 AS res;", conn);
                //读取查询到的数据
                reader = cmd.ExecuteReader();
                bool result = false;
                while (reader.Read())
                {
                    result = (reader.GetInt32(0) == 1);
                }

                return result;
            }
            catch
            {

                return false;
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }

        /// <summary>
        /// 获取谱图列表
        /// </summary>
        /// <returns></returns>
        public static string GetPtInfo(string createTime)
        {
            string info = null;

            SqlConnection conn = new SqlConnection(PublicPath.DB);
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM ptinfo WHERE CreateTime ='{createTime}'";
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    info = reader.GetString(0) + "_" + reader.GetString(1) + "_" + reader.GetString(2)
                        + "_" + reader.GetString(3) + "_" + reader.GetString(4) + "_" + reader.GetDouble(5)
                        + "_" + reader.GetDouble(6) + "_" + reader.GetInt32(7) + "_" + reader.GetInt32(8);
                }
            }
            catch { }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
                if (null != conn)
                {
                    conn.Close();
                }
            }

            return info;
        }

        /// <summary>
        /// 获取谱图列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllPtName(string filter = "", string catalog = "")
        {
            List<string> nameList = new List<string>();

            SqlConnection conn = new SqlConnection(PublicPath.DB);
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                if ("" == filter)
                {
                    cmd.CommandText = @"SELECT * FROM ptinfo";

                    if (catalog.Length > 0)
                    {
                        cmd.CommandText += " WHERE Catalog = '" + catalog + "'";
                    }
                }
                else
                {
                    cmd.CommandText = @"SELECT * FROM ptinfo WHERE (CreateTime LIKE '%" + filter
                        + "%' OR username LIKE '%" + filter
                        + "%' OR MethodName LIKE '%" + filter
                        + "%' OR Batch LIKE '%" + filter
                        + "%' OR CV LIKE '%" + filter
                        + "%' OR ZG LIKE '%" + filter + "%')";

                    if (catalog.Length > 0)
                    {
                        cmd.CommandText += " AND Catalog = '" + catalog + "'";
                    }
                }

                cmd.CommandText += " order by CreateTime desc";

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nameList.Add(reader.GetString(0) + "_" + reader.GetString(1) + "_" + reader.GetString(2) + "_" + reader.GetString(3));
                }
            }
            catch { }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
                if (null != conn)
                {
                    conn.Close();
                }
            }

            return nameList;
        }

        /// <summary>
        /// 获取谱图列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUserPtName(string filter = "", string catalog = "")
        {
            List<string> nameList = new List<string>();

            SqlConnection conn = new SqlConnection(PublicPath.DB);
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                if ("" == filter)
                {
                    cmd.CommandText = $"SELECT * FROM ptinfo WHERE username ='{s_userName}'";

                    if (catalog.Length > 0)
                    {
                        cmd.CommandText += $" AND Catalog = '{catalog}'";
                    }
                }
                else
                {
                    cmd.CommandText = @"SELECT * FROM ptinfo WHERE (CreateTime LIKE '%" + filter
                        + "%' OR MethodName LIKE '%" + filter
                        + "%' OR Batch LIKE '%" + filter
                        + "%' OR CV LIKE '%" + filter
                        + "%' OR ZG LIKE '%" + filter + "%')" + "AND username ='" + s_userName + "'";

                    if (catalog.Length > 0)
                    {
                        cmd.CommandText += $" AND Catalog = '{catalog}'";
                    }
                }

                cmd.CommandText += " order by CreateTime desc";

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nameList.Add(reader.GetString(0) + "_" + reader.GetString(1) + "_" + reader.GetString(2) + "_" + reader.GetString(3));
                }
            }
            catch { }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                }
                if (null != conn)
                {
                    conn.Close();
                }
            }

            return nameList;
        }

        public static bool ReadTime(string name, string time, List<double> absorbanceList)
        {
            SqlConnection conn = new SqlConnection(PublicPath.PtDB);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM sc{name} WHERE [Time] = {time}", conn);
                //读取查询到的数据
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 1; i < 400 - 200 + 2; i++)
                    {
                        absorbanceList.Add(reader.GetDouble(i));
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }

        public static bool SelectTime(string name, List<double> timeList)
        {
            SqlConnection conn = new SqlConnection(PublicPath.PtDB);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT [Time] FROM pt{name}", conn);
                //读取查询到的数据
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    timeList.Add(reader.GetDouble(0));
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }

    }
}
