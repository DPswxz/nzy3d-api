using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzy3d_winformsDemo
{
    class PublicPath
    {
        private const string C_ID = "100";
        public static string SDeviceID = "Intepure-F " + C_ID;

        public static string DBName = "HanBonDB" + C_ID;
        public static string LogDBName = "HanBonLogDB" + C_ID;
        public static string PtDBName = "HanBonPtDB" + C_ID;
        public static string DB = @"server=" + Environment.MachineName + ";database=" + DBName + ";uid=sa;pwd=123;Connection Timeout=5;";
        public static string LogDB = @"server=" + Environment.MachineName + ";database=" + LogDBName + ";uid=sa;pwd=123;Connection Timeout=5;";
        public static string PtDB = @"server=" + Environment.MachineName + ";database=" + PtDBName + ";uid=sa;pwd=123;Connection Timeout=5;";
        public static string DB2 = @"server=" + Environment.MachineName + ";database=master;uid=sa;pwd=123;Connection Timeout=5;";


        /// <summary>
        /// 创建临时文件所需路径
        /// </summary>
        public static void CreateDir()
        {
            string path = "./config";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = "./error";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = "./config/tempPdf";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
