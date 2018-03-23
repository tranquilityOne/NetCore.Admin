using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e3net.Common.Gps
{

    #region wgs84与gcj02等坐标系转换函数
    //WGS-84：是国际标准，GPS坐标（Google Earth使用、或者GPS模块）
    //GCJ-02：中国坐标偏移标准，Google Map、高德、腾讯使用
    //BD-09：百度坐标偏移标准，Baidu Map使用


    /**
 * 各地图API坐标系统比较与转换;
 * WGS84坐标系：即地球坐标系，国际上通用的坐标系。设备一般包含GPS芯片或者北斗芯片获取的经纬度为WGS84地理坐标系,
 * 谷歌地图采用的是WGS84地理坐标系（中国范围除外）;
 * GCJ02坐标系：即火星坐标系，是由中国国家测绘局制订的地理信息系统的坐标系统。由WGS84坐标系经加密后的坐标系。
 * 谷歌中国地图和搜搜中国地图采用的是GCJ02地理坐标系; BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系;
 * 搜狗坐标系、图吧坐标系等，估计也是在GCJ02基础上加密而成的。 chenhua
 */

    public class GpsItem
    {
        public double Lat;
        public double Lng;
        public GpsItem(double _lat, double _lng)
        {
            Lat = _lat;
            Lng = _lng;
        }
        public override string ToString()
        {
            return Lat + "," + Lng;
        }
    }

    public class PositionUtil
    {

        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;

        public static GpsItem gps84_To_Gcj02(double wgLat, double wgLon)
        {
            double mgLat = 0;
            double mgLon = 0;
            if (outOfChina(wgLat, wgLon))
            {
                mgLat = wgLat;
                mgLon = wgLon;
                return new GpsItem(mgLat, mgLon);
            }
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
            mgLat = wgLat + dLat;
            mgLon = wgLon + dLon;
            return new GpsItem(mgLat, mgLon);
        }

        public static GpsItem gcj02_To_Gps84(double lat, double lon)
        {
            GpsItem pl = gps84_To_Gcj02(lat, lon);
            double offsetLat = pl.Lat - lat;
            double offsetLng = pl.Lng - lon;

            return new GpsItem(lat - offsetLat, lon - offsetLng);
        }

        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        public static GpsItem gcj02_To_Bd09(double gg_lat, double gg_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new GpsItem(bd_lat, bd_lon);
        }

        public static GpsItem bd09_To_Gcj02(double bd_lat, double bd_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new GpsItem(gg_lat, gg_lon);
        }

        public static GpsItem bd09_To_Gps84(double bd_lat, double bd_lon)
        {

            GpsItem gcj02 = PositionUtil.bd09_To_Gcj02(bd_lat, bd_lon);
            GpsItem map84 = PositionUtil.gcj02_To_Gps84(gcj02.Lat,
                    gcj02.Lng);
            return map84;
        }
        public static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        public static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        public static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0
                    * Math.PI)) * 2.0 / 3.0;
            return ret;
        }


        #region
        /// <summary>
        /// 获取两个坐标之间的距离（千米）
        /// </summary>
        /// <param name="lat1">第一点的纬度坐标</param>
        /// <param name="lng1">第一点的经度坐标</param>
        /// <param name="lat2">第二点的纬度坐标</param>
        /// <param name="lng2">第二点的经度坐标</param>
        /// <returns>两个坐标之间的距离</returns>
        public static double GetDistanceKm(double lat1, double lng1, double lat2, double lng2)
        {
            return Math.Round(Distance(lat1, lng1, lat2, lng2) / 1000, 2);
        }



        /// <summary>
        /// 获取两个坐标之间的距离
        /// </summary>
        /// <param name="lat1">第一点的纬度坐标</param>
        /// <param name="lng1">第一点的经度坐标</param>
        /// <param name="lat2">第二点的纬度坐标</param>
        /// <param name="lng2">第二点的经度坐标</param>
        /// <returns>两个坐标之间的距离</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            try
            {
                var b = Math.PI / 180;
                var c = Math.Sin((lat2 - lat1) * b / 2);
                var d = Math.Sin((lng2 - lng1) * b / 2);
                var a = c * c + d * d * Math.Cos(lat1 * b) * Math.Cos(lat2 * b);
                return 12756274 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            }
            catch (Exception)
            {

                return 0;
            }
        }
        /// <summary>
        /// 通过GPS坐标计算两点间的距离
        /// </summary>
        /// <param name="lat1">第一点的纬度坐标</param>
        /// <param name="long1">第一点的经度坐标</param>
        /// <param name="lat2">第二点的纬度坐标</param>
        /// <param name="long2">第二点的经度坐标</param>
        /// <returns></returns>
        public static double Distance(double lat1, double long1, double lat2, double long2)
        {
            double a, b, R;
            R = 6378137; //地球半径
            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;
            a = lat1 - lat2;
            b = (long1 - long2) * Math.PI / 180.0;
            double d;
            double sa2, sb2;
            sa2 = Math.Sin(a / 2.0);
            sb2 = Math.Sin(b / 2.0);
            d = 2 * R * Math.Asin(Math.Sqrt(sa2 * sa2 + Math.Cos(lat1) * Math.Cos(lat2) * sb2 * sb2));
            return d;
        }


        private double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        //点是否在矩形中
        public Boolean IsInRect(double lng, double lat,
            double lng1, double lat1, double lng2, double lat2)
        {
            return lng >= Math.Min(lng1, lng2) && lng <= Math.Max(lng1, lng2)
                && lat >= Math.Min(lat1, lat2) && lat <= Math.Max(lat1, lat2);
        }
        /// <summary>
        /// 圆形区域中
        /// </summary>
        /// <param name="lng1"></param>
        /// <param name="lat1"></param>
        /// <param name="lng2"></param>
        /// <param name="lat2"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Boolean IsInCircle(double lng1, double lat1,
            double lng2, double lat2, double radius)
        {
            double d = Distance(lat1, lng1, lat2, lng2);
            return d <= radius;
        }

        //点是否在多边形中
        public Boolean IsInPolygon(GpsItem p, List<GpsItem> points)
        {
            int i, j = points.Count - 1;
            bool oddNodes = false;

            double ret;
            for (i = 0; i < points.Count; i++)
            {
                if (points[i].Lat < p.Lat && points[j].Lat >= p.Lat
                  || points[j].Lat < p.Lat && points[i].Lat >= p.Lat)
                {
                    ret = points[i].Lng + (p.Lat - points[i].Lat) / (points[j].Lat - points[i].Lat) * (points[j].Lng - points[i].Lng);
                    if (ret < p.Lng)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }
            return oddNodes;
        }

        // / <summary>
        // / 计算叉乘 |P0P1| × |P0P2|
        // / </summary>
        // / <param name="p1"></param>
        // / <param name="p2"></param>
        // / <param name="p0"></param>
        // / <returns></returns>
        private double MultiplyPoint(GpsItem p1, GpsItem p2,
                GpsItem p0)
        {
            return ((p1.Lng - p0.Lng) * (p2.Lat - p0.Lat) - (p2.Lng - p0.Lng)
                    * (p1.Lat - p0.Lat));
        }

        public double ESP = 0.000001;// (1e-5)

        // / <summary>
        // / 判断点是否在线段上，考虑可控误差
        // / </summary>
        // / <param name="p1"></param>
        // / <param name="p2"></param>
        // / <param name="p0"></param>
        // / <param name="offset"></param>
        // / <returns></returns>
        public Boolean IsPointOnLine(GpsItem p0, GpsItem p1, GpsItem p2, int offset)
        {
            if (Math.Abs(MultiplyPoint(p1, p2, p0)) >= ESP * offset)
                return false;
            if ((p0.Lng - p1.Lng) * (p0.Lng - p2.Lng) > 0)
                return false;
            if ((p0.Lat - p1.Lat) * (p0.Lat - p2.Lat) > 0)
                return false;
            return true;
        }



        #endregion

    }
    #endregion
}
