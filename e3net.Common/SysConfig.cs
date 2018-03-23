using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e3net.Common
{
    public class SysConfig
    {
        #region log


        /// <summary>
        /// 是否记录调试信息
        /// </summary>
        public static bool IsDebugLog
        {
            get
            {
                return bool.Parse("IsDebugLog".ToAppSetting());
            }
        }




        #endregion


        #region Memcached 配置

        private static string _MemcachedServer;
        /// <summary>
        /// Memcached服务器信息
        /// </summary>
        public static string MemcachedServer
        {
            get
            {
                if (String.IsNullOrEmpty(_MemcachedServer))
                {
                    _MemcachedServer = "MemcachedServer".ToAppSetting();
                }
                return _MemcachedServer;
            }
        }

        #endregion

        #region Redis
        private static string _RedisServer;
        /// <summary>
        /// redis服务器 连接 
        /// </summary>
        public static string RedisServer
        {
            get
            {
                if (String.IsNullOrEmpty(_RedisServer))
                {
                    _RedisServer = "RedisServer".ToAppSetting();
                }
                return _RedisServer;
            }
        }
        #endregion
        #region MongoDB
        private static string[] _MDB_User_Pwd_Host_Port;
        /// <summary>
        /// mongodb数据库连接
        /// </summary>
        public static string[] MDB_User_Pwd_Host_Port
        {
            get
            {
                if (_MDB_User_Pwd_Host_Port == null)
                {
                    _MDB_User_Pwd_Host_Port = "MDB_User_Pwd_Host_Port".ToAppSetting().Split(':');
                }
                return _MDB_User_Pwd_Host_Port;
            }
        }
        #endregion
        #region

        #endregion



        #region  OAuth

        private static string _tokenTimeOut;
        /// <summary>
        /// token过期时间 秒
        /// </summary>
        public static int tokenTimeOut
        {
            get
            {
                if (String.IsNullOrEmpty(_tokenTimeOut))
                {
                    _tokenTimeOut = "tokenTimeOut".ToAppSetting();
                }
                return int.Parse(_tokenTimeOut);
            }
        }



        #endregion

    }
}
