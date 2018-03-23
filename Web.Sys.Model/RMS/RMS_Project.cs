using Core.PetaPocoServer;
using System;

namespace Web.Sys.Model.RMS
{
    [TableName("rms_project")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_project
    {  /// <summary> 
       ///   主键 
       /// </summary> 
        public Int32 id
        {
            get;
            set;
        }
        /// <summary> 
        ///   公司id 
        /// </summary> 
        public Int32? companyid
        {
            get;
            set;
        }
        /// <summary> 
        ///   加密key 
        /// </summary> 
        public String keys
        {
            get;
            set;
        }
        /// <summary> 
        ///   名称 
        /// </summary> 
        public String name
        {
            get;
            set;
        }
        /// <summary> 
        ///   备注 
        /// </summary> 
        public String remarks
        {
            get;
            set;
        }
        /// <summary> 
        ///   添加时间 
        /// </summary> 
        public DateTime? createtime
        {
            get;
            set;
        }
        /// <summary> 
        ///   更新时间 
        /// </summary> 
        public DateTime? modifytime
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isvalid
        {
            get;
            set;
        }



        /// <summary>
        /// 是否删除
        /// </summary>
        public bool isdeleted
        {
            get;
            set;
        }
    }
}
