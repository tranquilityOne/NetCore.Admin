using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Security;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Diagnostics;
//using System.Windows.Forms;
using System.Threading;
using e3net.Common.Entity;


namespace Zephyr.Utils
{
    public partial class ZFiles
    {
        private const string PATH_SPLIT_CHAR = "\\";

        #region 工具方法：ASP.NET上传文件的方法
        /// <summary>
        /// 工具方法：上传文件的方法
        /// </summary>
        /// <param name="myFileUpload">上传控件的ID</param>
        /// <param name="allowExtensions">允许上传的扩展文件名类型,如：string[] allowExtensions = { ".doc", ".xls", ".ppt", ".jpg", ".gif" };</param>
        /// <param name="maxLength">允许上传的最大大小，以M为单位</param>
        /// <param name="savePath">保存文件的目录，注意是绝对路径,如：Server.MapPath("~/upload/");</param>
        /// <param name="saveName">保存的文件名，如果是""则以原文件名保存</param>
        public static string FilesUpload(FileUpload myFileUpload, string[] allowExtensions, int maxLength, string savePath, string saveName)
        {
            // 文件格式是否允许上传
            bool fileAllow = false;
            //检查是否有文件案
            if (myFileUpload.HasFile)
            {
                // 检查文件大小, ContentLength获取的是字节，转成M的时候要除以2次1024
                if (myFileUpload.PostedFile.ContentLength / 1024 / 1024 >= maxLength)
                {
                    return String.Format("只能上传小于{0}M的文件！", maxLength);
                }
                //取得上传文件之扩展文件名，并转换成小写字母
                string fileExtension = System.IO.Path.GetExtension(myFileUpload.FileName).ToLower();
                string tmp = "";   // 存储允许上传的文件后缀名
                //检查扩展文件名是否符合限定类型
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    tmp += i == allowExtensions.Length - 1 ? allowExtensions[i] : allowExtensions[i] + ",";
                    if (fileExtension == allowExtensions[i])
                    {
                        fileAllow = true;
                    }
                }
                if (allowExtensions.Length == 0) { fileAllow = true; }
                if (fileAllow)
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(Path.GetFullPath(savePath));
                        if (!di.Exists)
                        {
                            di.Create();
                        }
                        string path = savePath + (saveName == "" ? myFileUpload.FileName : saveName);
                        //存储文件到文件夹
                        myFileUpload.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    return "文件格式不符，可以上传的文件格式为：" + tmp;
                }
            }
            else
            {
                return "请选择要上传的文件！";
            }
            return "";
        }


        /// <summary>
        /// http文件上传
        /// </summary>
        /// <param name="hpFile">http文件  HttpPostedFile</param>
        /// <param name="allowExtensions"> 允许上传的扩展文件名类型  小写,如：string[] allowExtensions = { ".doc", ".xls", ".ppt", ".jpg", ".gif" };</param>
        /// <param name="maxLength">允许上传的最大大小，以kb为单位</param>
        /// <param name="savePath">保存文件的目录，注意是绝对路径,如：Server.MapPath("~/upload/");</param>
        /// <param name="saveName">保存的文件名，如果是""则以原文件名保存</param>
        /// <returns></returns>
        public static ReSultMode FilesUpload(HttpPostedFile hpFile, string[] allowExtensions, int maxLength, string savePath, string saveName)
        {
            ReSultMode rsm = new ReSultMode();
            string fileExtension = System.IO.Path.GetExtension(hpFile.FileName).ToLower();
            bool fileAllow = false;
            string tmp = "";   // 存储允许上传的文件后缀名

            if (allowExtensions.Length == 0)
            {
                fileAllow = true;

            }
            else
            {
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    tmp += i == allowExtensions.Length - 1 ? allowExtensions[i] : allowExtensions[i] + ",";
                    if (fileExtension == allowExtensions[i])
                    {
                        fileAllow = true;
                    }
                }
            }

            if (fileAllow)
            {
                if (hpFile.ContentLength / 1024 > maxLength)
                {
                    rsm.code = -13;
                    rsm.msg = "文件不能大于" + maxLength + "kB";
                    rsm.data = "";
                }
                else
                {
                    try
                    {
                        if (string.IsNullOrEmpty(saveName))
                        {
                            saveName = hpFile.FileName;

                        }
                        ZFiles.CreateDirectoryIsExists(savePath);//不存在就创建
                        hpFile.SaveAs(savePath + saveName);
                        rsm.code = 11;
                        rsm.data = saveName;
                        rsm.msg = "保存成功";
                    }
                    catch (Exception ex)
                    {
                        rsm.code = -11;
                        rsm.msg = ex.Message;
                        rsm.data = "";
                    }
                }
            }
            else
            {
                rsm.code = -13;
                rsm.msg = "文件格式不符，可以上传的文件格式为：" + tmp;
                rsm.data = "";
            }
            return rsm;
        }




        /// <summary>
        /// http  图片上传
        /// </summary>
        /// <param name="hpFile">http文件  HttpPostedFile</param>
        /// <param name="allowExtensions"> 允许上传的扩展文件名类型  小写,如：string[] allowExtensions = { ".doc", ".xls", ".ppt", ".jpg", ".gif" };</param>
        /// <param name="maxLength">允许上传的最大大小，以kb为单位</param>
        /// <param name="savePath">保存文件的目录，注意是绝对路径,如：Server.MapPath("~/upload/");</param>
        /// <param name="saveName">保存的文件名，如果是""则以原文件名保存</param>
        /// <returns></returns>
        public static ReSultMode ImageUpload(HttpPostedFile hpFile, int maxLength, string savePath, string saveName)
        {
            ReSultMode rsm = new ReSultMode();


            //转换成byte,读取图片MIME类型  S
            Stream stream;
            //int contentLength = file0.ContentLength; //文件长度
            byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
            stream = hpFile.InputStream;
            stream.Read(fileByte, 0, 2);//contentLength，还是取前两位  stream.Close();
            string fileFlag = "";
            if (fileByte != null && fileByte.Length > 0)//图片数据是否为空 
            {
                fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
            }
            string[] fileTypeStr = { "255216", "7173", "6677", "13780", "7370" };//对应的图片格式jpg,gif,bmp,png
            string[] allowExtensions = { ".jpg", ".gif", ".bmp", ".png" };
            bool fileAllow = false;
            for (int i = 0; i < fileTypeStr.Length; i++)
            {
                if (fileFlag == fileTypeStr[i])
                {
                    fileAllow = true;
                    break;
                }
            }
            if (fileAllow)
            {
                return FilesUpload(hpFile, allowExtensions, maxLength, savePath, saveName);
            }
            else
            {
                rsm.code = -13;
                rsm.msg = "图片类型只能为jpg,gif,bmp,png";
                rsm.data = "";
            }
            return rsm;



        }

        #region      常见文件的文件头（十进制）

        //jpg: 255,216

        //gif: 71,73

        //bmp: 66,77

        //png: 137,80

        //doc: 208,207

        //docx: 80,75

        //xls: 208,207

        //xlsx: 80,75

        //js: 239,187

        //swf: 67,87

        //txt: 70,67

        //mp3: 73,68

        //wma: 48,38

        //mid: 77,84

        //rar: 82,97

        //zip: 80,75

        //xml: 60,63

        #endregion

        #endregion
    }
}