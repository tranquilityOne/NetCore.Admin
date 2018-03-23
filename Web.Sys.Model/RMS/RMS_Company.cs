using Core.PetaPocoServer;
using System;

namespace Web.Sys.Model.RMS
{
    [TableName("rms_company")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_company
    {  /// <summary> 
        ///   主键 
        /// </summary> 
        public Int32 id
        {
            get;
            set;
        }
        /// <summary> 
        ///   排序 
        /// </summary> 
        public Int32? orderno
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
