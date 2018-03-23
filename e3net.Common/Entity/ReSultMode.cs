using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e3net.Common.Entity
{


    public enum code
    {
        /// <summary>
        /// 成功
        /// </summary>
        Sucess = 11,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = -11,
        /// <summary>
        /// 出错
        /// </summary>
        error = -66,
        /// <summary>
        /// 正常
        /// </summary>
        normal = 0,
        /// <summary>
        /// 没权限
        /// </summary>
        UnAuthorization = -10
    };

    /// <summary>
    /// 返回信息
    /// </summary>
    [Serializable]
    public class ReSultMode
    {
        /// <summary>
        /// 代码Code 成功11 失败-11 系统出错-66 成常操作0  
        /// </summary>
        public int code { set; get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 数据
        /// </summary>
        public string data { set; get; }

        /// <summary>
        /// 构造返回一个
        /// </summary>
        /// <param name="_code">代码</param>
        /// <param name="_msg">信息</param>
        /// <param name="_data">数据</param>
        /// <returns></returns>
        public static ReSultMode GetReSultMode(int _code, string _msg, string _data)
        {
            ReSultMode info = new ReSultMode();
            info.code = _code;
            info.msg = _msg;
            info.data = _data;
            return info;
        }
    }

    /// <summary>
    ///  返回信息 泛型 单个或列表
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    [Serializable]
    public class ReSultMode<T>
    { /// <summary>
        /// 代码Code 成功11 失败-11 系统出错-66 成常操作0  
        /// </summary>
        public int code { set; get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }

        /// <summary>
        /// 对象
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 构造返回一个
        /// </summary>
        /// <param name="_code">代码</param>
        /// <param name="_msg">信息</param>
        /// <param name="_data">数据</param>
        /// <returns></returns>
        public static ReSultMode<T> GetReSultMode(int _code, string _msg, T _data)
        {
            ReSultMode<T> info = new ReSultMode<T>();
            info.code = _code;
            info.msg = _msg;
            info.data = _data;
            return info;
        }

    }

    /// <summary>
    /// 带分页 返回信息 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ReSultPageMode<T>
    {
        /// <summary>
        /// 代码Code 成功11 失败-11 系统出错-66 成常操作0  
        /// </summary>
        public int code { set; get; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 总页数输出
        /// </summary>
        public int pcount { get; set; }
        /// <summary>
        /// 总记录数输出
        /// </summary>
        public int rcunt { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int page_index { get; set; }
        /// <summary>
        /// 每页页大小
        /// </summary>

        public int page_size { get; set; }
        /// <summary>
        /// 列表
        /// </summary>
        public List<T> data { get; set; }

        /// <summary>
        /// 构造返回一个 翻页
        /// </summary>
        /// <param name="_code">代码Code 成功11 失败-11 系统出错-66 成常操作0  </param>
        /// <param name="_msg">信息</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="PageSize">每页页大小</param>
        /// <param name="Pcount">总页数输出</param>
        /// <param name="Rcunt">总记录数输出</param>
        /// <param name="_data">列表</param>
        /// <returns></returns>
        public static ReSultPageMode<T> GetReSultPageMode(int _code, string _msg, int PageIndex, int PageSize, int Pcount, int Rcunt, List<T> _data)
        {
            ReSultPageMode<T> info = new ReSultPageMode<T>();
            info.code = _code;
            info.msg = _msg;
            info.page_index = PageIndex;
            info.page_size = PageSize;
            info.pcount = Pcount;
            info.rcunt = Rcunt;
            info.data = _data;
            return info;
        }
    }
}
