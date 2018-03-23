using e3net.Common.Json;
using e3net.Common.NetWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e3net.Common.Gps
{
    public class baiduParse
    {

        //const string baidu_ap = "a3wNd0COKjujWVCggydwQUIR";
        static string baidu_ap = System.Configuration.ConfigurationManager.AppSettings["baidu_ak"];
        /// <summary>
        /// 百度地图解析 地址
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static string getAddress(double lng, double lat)
        {
            String url = "http://api.map.baidu.com/geocoder/v2/?location=" + lat + ","
        + lng + "&output=json&ak=" + baidu_ap + "&pois=0";
            HttpUtil request = new HttpUtil();
            JObject obj = JsonHelper.FromJson(request.GetRequest(url));

            if (obj["status"]!=null&&obj["status"].ToString().Equals("0"))
            {
                obj = (JObject)obj["result"];
                return obj["formatted_address"].ToString()+"," + obj["sematic_description"].ToString();
            }
            else
            {
                return "";
            }

        }
           /// <summary>
        /// 百度天气  说明 http://developer.baidu.com/map/carapi-7.htm
        /// </summary>
        /// <param name="location">经度 城市名称如:北京，经纬度格式为lng,lat坐标如: location=116.305145,39.982368;</param>
        /// <param name="lat">纬度</param>
        /// <returns></returns>
        public static JObject getweather(string location)
        {
            String url = "http://api.map.baidu.com/telematics/v3/weather?location=" + location + "&output=json&ak=" + baidu_ap;
            HttpUtil request = new HttpUtil();
            JObject obj = JsonHelper.FromJson(request.GetRequest(url));

            if (obj["status"]!=null&&obj["status"].ToString().Equals("success"))
            {
                JArray res = JArray.Parse(obj["results"].ToString());
                res = JArray.Parse(res[0]["weather_data"].ToString());
                return JObject.Parse(res[0].ToString());


                //JObject obj = baiduParse.getweather("119.911676,30.98564");
                //if (obj != null)
                //{
                //    string dd = obj["date"].ToString();
                //   string 温度 = StringHelper.SubstringBetween(dd, "实时：", "℃");
                  
                //}
            }
            else
            {
                return null;
            }

        }
    }
}
