/*  作者：       tianzh
*  创建时间：   2012/6/9 23:10:11
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json.Converters;

namespace e3net.Common.Json
{

    /// <summary>
    /// 提供了一个关于json的辅助类
    /// </summary>
    public static class JsonHelper
    {

        #region Method
        /// <summary>
        /// 类对像转换成json格式
        /// </summary> 
        /// <returns></returns>
        public static string ToJson(object t)
        {
            return JsonConvert.SerializeObject(t, Formatting.Indented,
        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }
        /// <summary>
        /// 类对像转换成json格式
        /// </summary>
        /// <param name="t"></param>
        /// <param name="HasNullIgnore">是否忽略NULL值</param>
        /// <returns></returns>
        public static string ToJson(object t, bool HasNullIgnore)
        {
            if (HasNullIgnore)
                return JsonConvert.SerializeObject(t, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            else
                return ToJson(t);
        }
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, new DataTableConverter());
        }
        /// <summary>
        /// json格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            if (!string.IsNullOrEmpty(strJson))
                return JsonConvert.DeserializeObject<T>(strJson);
            return null;
        }
        /// <summary>
        ///  json格式转换 jobject
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JObject FromJson(string strJson)
        {

            JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
            //string zone = jo["beijing"]["zone"].ToString();
            //string zone_en = jo["beijing"]["zone_en"].ToString(); 
            return jo;
        }
        #endregion
        #region Method2


        /// <summary>
        /// DataRow转JSON
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <returns>JSON格式对象</returns>
        public static string DataRowToJSON(DataRow row)
        {
            Dictionary<string, object> dataList = new Dictionary<string, object>();
            foreach (DataColumn column in row.Table.Columns)
            {
                dataList.Add(column.ColumnName, row[column]);
            }
            return ObjectToJSON(dataList);
        }
        /// <summary>
        /// DataRow转对象，泛型方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="row">DataRow</param>
        /// <returns>JSON格式对象</returns>
        public static T DataRowToObject<T>(DataRow row)
        {
            return JSONToObject<T>(DataRowToJSON(row).ToString());
        }
        /// <summary>
        /// DataRow转对象，泛型方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>JSON格式对象</returns>
        public static List<T> DataTableToList<T>(DataTable table)
        {
            return JSONToList<T>(DataTableToJSON(table).ToString());
        }
        /// <summary>
        /// DataRow转对象，泛型方法
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>JSON格式对象</returns>
        public static List<T> JSONToList<T>(string jsonText)
        {
            return JSONToObject<List<T>>(jsonText);
        }
        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            try
            {
                JsonSerializerSettings jset = new JsonSerializerSettings();
                jset.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                jset.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy'/'MM'/'dd' 'HH':'mm':'ss" });
                return JsonConvert.SerializeObject(obj, jset);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 数据表转JSON 
        /// </summary> 
        /// <param name="dataTable">数据表</param> 
        /// <returns>JSON字符串</returns> 
        public static string DataTableToJSON(DataTable dataTable)
        {
            return ObjectToJSON(dataTable);
        }
        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonText.Replace("undefined", "null"));
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }
        /// <summary> 
        /// JSON文本转对象 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <param name="type">类型</param>
        /// <returns>指定类型的对象</returns> 
        public static object JSONToObject(string jsonText, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(jsonText.Replace("undefined", "null"), type);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        /// <summary>
        #endregion
    }
}
