using e3net.Common.Json;
using e3net.Common.NetWork;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace e3net.Common.Gps
{

    #region  属性
    /// <summary>
    /// 基站项
    /// </summary>
    public class LBSDataItem
    {
        /// <summary>
        /// 移动用户所属国家代码 默认460
        /// </summary>
        public int mcc { get; set; }

        /// <summary>
        /// 移动网号  移动0  联通1
        /// </summary>
        public int mnc { get; set; }
        /// <summary>
        /// 位置区域码 取值范围0—65535
        /// </summary>
        public int lac { get; set; }
        /// <summary>
        /// 基站 小区编号
        /// </summary>

        public int cellid { get; set; }
        /// <summary>
        /// 信号强度
        /// </summary>
        public double signal { get; set; }

        public string ToId()
        {


            return string.Format("{0}_{1}_{2}_{3}", mcc, mnc, lac, cellid);
        }

        public override string ToString()
        {
            return String.Format("mcc:{0},mnc:{1},lac:{2},cellid:{3}" + mcc, mnc, lac, cellid);
        }
    }

    /// <summary>
    /// wifi项
    /// </summary>
    public class WiFiItem
    {
        /// <summary>
        /// mac 地址
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 信号强度
        /// </summary>
        public int SignalValue { get; set; }
        /// <summary>
        /// wifi名称
        /// </summary>
        public string Name { get; set; }


        public WiFiItem()
        {

        }
        public WiFiItem(string mac, int value, string name)
        {
            Mac = mac;
            SignalValue = value;
            Name = name;
        }
        public override string ToString()
        {
            return Mac + "," + SignalValue + "," + Name;
        }
    }

    /// <summary>
    /// 定位结果
    /// </summary>
    public class LocData
    {


        public double lat { get; set; }
        public double lng { get; set; }

        public string address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string country
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string province
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string city
        {
            get;
            set;
        }
        /// <summary>
        /// 道路名
        /// </summary>
        public string road
        {
            get;
            set;
        }



        /// <summary>
        /// 定位精度 半径  单位：米
        /// </summary>
        public double accuracy { get; set; }

        /// <summary>
        /// 定位类型 ，0：没有得到定位结果； ：没有得到定位结果； 其他数字为：正常获取定位结果
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 附近的点
        /// </summary>
        public string poi
        {
            get;
            set;
        }



    }


    #endregion
    /// <summary>
    /// 高德 基站 wifi定位 转化为经度纬度
    /// </summary>
    public class GaodeLbsParse
    {
        /// <summary>
        /// wifi 转换
        /// </summary>
        /// <param name="WiFiItems"></param>
        /// <returns></returns>
        public LocData WiFiAnalysisPosition(String imei, List<WiFiItem> WiFiItems)
        {
            String url = "";
            try
            {
                //List<WiFiItem> WiFiItems = new List<WiFiItem>();
                //WiFiItems.Add(new WiFiItem("22:27:1d:20:08:d5", -55, "CMCC-EDU"));
                //WiFiItems.Add(new WiFiItem("5c:63:bf:a4:bf:56", -86, "TP-LINK"));
                //WiFiItems.Add(new WiFiItem("d8:c7:c8:a8:1a:13", -42, "TP-LINK"));
                //--============

                if (WiFiItems == null || WiFiItems.Count <= 0)
                {
                    return null;
                }

                string IMEI = imei;
                string SMAC = WiFiItems.First().Mac;  //手机MAC地址
                string MMAC = ""; //已连热点 mac信
                string MACS = ""; //wifi列表中信息"f0:7d:68:9e:7d:18,-41,TPLink"
                string OutPut = "json"; //返回数据格式
                WiFiItems = WiFiItems.OrderByDescending(d => d.SignalValue).ToList();

                string Key = ConfigurationManager.AppSettings["AmapPosition"].ToString();


                for (int i = 0; i < WiFiItems.Count;i++ )
                    {
                        if (WiFiItems[i].SignalValue != 0)
                        {
                            if (WiFiItems[i].SignalValue > 0)//信号为负值
                            {
                                WiFiItems[i].SignalValue = 0 - WiFiItems[i].SignalValue;
                            }
                            if (String.IsNullOrEmpty(MMAC))
                            {
                                MMAC = WiFiItems[i].ToString();
                            }
                            MACS += WiFiItems[i].ToString() + "|";
                        }
                    }

                if (MACS.Length > 0)
                {
                    MACS = MACS.Substring(0, MACS.Length - 1);
                }
                url = String.Format("http://apilocate.amap.com/position?accesstype=1&imei={0}&network=GSM&cdma=0&smac={1}&mmac={2}&macs={3}&output={4}&key={5}", IMEI, SMAC, MMAC, MACS, OutPut, Key);
                HttpUtil httpu = new HttpUtil();
                JObject obj = JsonHelper.FromJson(httpu.GetRequest(url));

                if (obj != null && obj["status"] != null && obj["status"].ToString().Equals("1"))
                {
                    JObject result = JObject.Parse(obj["result"].ToString());
                    if (result != null && !result["type"].ToString().Equals("0"))
                    {
                        string location = result["location"].ToString();
                        LocData lbsdat = new LocData();
                        lbsdat.lng = Convert.ToDouble(location.Split(',')[0]);
                        lbsdat.lat = Convert.ToDouble(location.Split(',')[1]);  //纬度
                        lbsdat.Type = int.Parse(result["type"].ToString());

                        double radius = 0;
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        lbsdat.accuracy = radius;

                        if (result["desc"] != null)
                        {
                            lbsdat.address = string.Format("{0}附近约{1}米", result["desc"].ToString().Replace(" ", ""), radius);

                        }
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        if (result["country"] != null)
                        {
                            lbsdat.country = result["country"].ToString();
                        } if (result["province"] != null)
                        {
                            lbsdat.province = result["province"].ToString();
                        }
                        if (result["city"] != null)
                        {
                            lbsdat.city = result["city"].ToString();
                        }
                        if (result["road"] != null)
                        {
                            lbsdat.road = result["road"].ToString();
                        }
                        if (result["poi"] != null)
                        {
                            lbsdat.poi = result["poi"].ToString();
                        }
                        return lbsdat;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (obj != null && obj["info"] != null)
                    {

                        log4net.LogManager.GetLogger("定位失败").Error("调用高德lbs数据解析地址错误 code:" + obj["info"].ToString() + "  url:" + url);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Type classt = this.GetType();
                string typeName = classt.ToString();//空间名.类名
                string tname = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;//方法名
                string Title = typeName + "." + tname;
                log4net.LogManager.GetLogger(Title).Error("调用高德Wifi数据解析地址错误:" + ex.Message + "  url:" + url, ex);
                return null;
            }
        }

        /// <summary>
        /// 基站转换
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        public LocData LBSAnalysisPosition(String imei, List<LBSDataItem> dataItem)
        {

            String url = "";

            try
            {
                string IMEI = imei;
                string SMAC = "";  //手机MAC地址
                string MMAC = ""; //已连热点 mac信
                string Bts = ""; //接入 基站信息
                string NearBts = "";//周边基站信息 （不 含接入 基站 信息 mcc,mnc,lac,cellid,signal）
                string OutPut = "json"; //返回数据格式 xml json

                for (int i = 0; i < dataItem.Count; i++)
                {
                    if (dataItem[i].signal > 0)//信号为负值
                    {
                        dataItem[i].signal = 0 - dataItem[i].signal;
                    }

                    //默认第一位当前接入的基站
                    if (i == 0)
                    {
                        Bts = dataItem[i].mcc + "," + dataItem[i].mnc + "," + dataItem[i].lac + "," + dataItem[i].cellid + "," + dataItem[i].signal;
                    }
                    else
                    {
                        NearBts += dataItem[i].mcc + "," + dataItem[i].mnc + "," + dataItem[i].lac + "," + dataItem[i].cellid + "," + dataItem[i].signal + "|";
                    }
                }
                if (NearBts.Trim().Length > 0)
                {
                    NearBts = NearBts.Substring(0, NearBts.Length - 1);
                }


                string Key = ConfigurationManager.AppSettings["AmapPosition"].ToString();
                url = String.Format("http://apilocate.amap.com/position?accesstype=0&network=GSM&imei={0}&smac={1}&cdma=0&bts={2}&nearbts={3}&output={4}&key={5}", IMEI, SMAC, Bts, NearBts, OutPut, Key);
                HttpUtil httpu = new HttpUtil();
                JObject obj = JsonHelper.FromJson(httpu.GetRequest(url));

                if (obj != null && obj["status"] != null && obj["status"].ToString().Equals("1"))
                {
                    JObject result = JObject.Parse(obj["result"].ToString());
                    if (result != null && !result["type"].ToString().Equals("0"))
                    {
                        string location = result["location"].ToString();
                        LocData lbsdat = new LocData();
                        lbsdat.lng = Convert.ToDouble(location.Split(',')[0]);
                        lbsdat.lat = Convert.ToDouble(location.Split(',')[1]);  //纬度
                        lbsdat.Type = int.Parse(result["type"].ToString());

                        double radius = 0;
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        lbsdat.accuracy = radius;

                        if (result["desc"] != null)
                        {
                            lbsdat.address = string.Format("{0}附近约{1}米", result["desc"].ToString().Replace(" ", ""), radius);

                        }
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        if (result["country"] != null)
                        {
                            lbsdat.country = result["country"].ToString();
                        } if (result["province"] != null)
                        {
                            lbsdat.province = result["province"].ToString();
                        }
                        if (result["city"] != null)
                        {
                            lbsdat.city = result["city"].ToString();
                        }
                        if (result["road"] != null)
                        {
                            lbsdat.road = result["road"].ToString();
                        }
                        if (result["poi"] != null)
                        {
                            lbsdat.poi = result["poi"].ToString();
                        }
                        return lbsdat;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (obj != null && obj["info"] != null)
                    {

                        log4net.LogManager.GetLogger("定位失败").Error("调用高德lbs数据解析地址错误 code:" + obj["info"].ToString() + "  url:" + url);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Type classt = this.GetType();
                string typeName = classt.ToString();//空间名.类名
                string tname = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;//方法名
                string Title = typeName + "." + tname;
                log4net.LogManager.GetLogger(Title).Error("调用高德LBS数据解析地址错误:" + ex.Message + "  url:" + url, ex);
                return null;
            }
        }

        /// <summary>
        /// lbs wifi混合转换
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="LbsItem"></param>
        /// <param name="WifiItems"></param>
        /// <returns></returns>
        public LocData GaoDeWIFILBSAnalysisPosition(String imei, List<LBSDataItem> LbsItem, List<WiFiItem> WifiItems)
        {
            String url = "";

            try
            {
                string Key = ConfigurationManager.AppSettings["AmapPosition"].ToString();
                var smac = "";
                var mmac = "";
                var macs = "";
                var nearbts = "";
                var bts = "";
                WifiItems = WifiItems.OrderByDescending(d => d.SignalValue).ToList();

                for (int i = 0; i < WifiItems.Count; i++)
                {
                    if (WifiItems[i].SignalValue > 0)//信号为负值
                    {
                        WifiItems[i].SignalValue = 0 - WifiItems[i].SignalValue;
                    }
                    if (String.IsNullOrEmpty(smac))
                    {
                        smac = WifiItems[i].Mac;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(mmac))
                        {
                            mmac = WifiItems[i].ToString();
                        }
                        macs += String.Format("{0},{1},{2}|", WifiItems[i].Mac, WifiItems[i].SignalValue, WifiItems[i].Name);
                    }
                }
                macs = macs.Trim('|');
                foreach (var item in LbsItem)
                {
                    if (item.signal > 0)//信号为负值
                    {
                        item.signal = 0 - item.signal;
                    }

                    if (String.IsNullOrEmpty(bts))
                    {
                        bts = String.Format("{0},{1},{2},{3},{4}", item.mcc, item.mnc, item.lac, item.cellid, item.signal);
                    }
                    else
                    {
                        nearbts += String.Format("{0},{1},{2},{3},{4}|", item.mcc, item.mnc, item.lac, item.cellid, item.signal);
                    }
                }
                nearbts = nearbts.Trim('|');

                url = String.Format("http://apilocate.amap.com/position?accesstype=1&imei={0}&smac={1}&mmac={2}&macs={3}&cdma=0&network=GSM&bts={4}&nearbts={5}&output=json&key={6}", imei, smac, mmac, macs, bts, nearbts, Key);
                HttpUtil httpu = new HttpUtil();
                JObject obj = JsonHelper.FromJson(httpu.GetRequest(url));

                if (obj != null && obj["status"] != null && obj["status"].ToString().Equals("1"))
                {
                    JObject result = JObject.Parse(obj["result"].ToString());
                    if (result != null && !result["type"].ToString().Equals("0"))
                    {
                        string location = result["location"].ToString();
                        LocData lbsdat = new LocData();
                        lbsdat.lng = Convert.ToDouble(location.Split(',')[0]);
                        lbsdat.lat = Convert.ToDouble(location.Split(',')[1]);  //纬度
                        lbsdat.Type = int.Parse(result["type"].ToString());

                        double radius = 0;
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        lbsdat.accuracy = radius;

                        if (result["desc"] != null)
                        {
                            lbsdat.address = string.Format("{0}附近约{1}米", result["desc"].ToString().Replace(" ", ""), radius);

                        }
                        if (result["radius"] != null)
                        {
                            radius = Convert.ToDouble(result["radius"].ToString());
                        }
                        if (result["country"] != null)
                        {
                            lbsdat.country = result["country"].ToString();
                        } if (result["province"] != null)
                        {
                            lbsdat.province = result["province"].ToString();
                        }
                        if (result["city"] != null)
                        {
                            lbsdat.city = result["city"].ToString();
                        }
                        if (result["road"] != null)
                        {
                            lbsdat.road = result["road"].ToString();
                        }
                        if (result["poi"] != null)
                        {
                            lbsdat.poi = result["poi"].ToString();
                        }
                        return lbsdat;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (obj != null && obj["info"] != null)
                    {

                        log4net.LogManager.GetLogger("定位失败").Error("调用高德lbs数据解析地址错误 code:" + obj["info"].ToString() + "  url:" + url);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Type classt = this.GetType();
                string typeName = classt.ToString();//空间名.类名
                string tname = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name;//方法名
                string Title = typeName + "." + tname;
                log4net.LogManager.GetLogger(Title).Error("调用高德Wifi数据解析地址错误:" + ex.Message + "  url:" + url, ex);
                return null;

            }

        }

    }
}
