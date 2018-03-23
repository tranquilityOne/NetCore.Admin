using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetaPocoServer
{
    /// <summary>
    /// 使用petapoco实体样板，加表名，和主键名 自增， 哪些字段不映射，哪些字段映射
    /// </summary>
    [TableName("userinfo")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]//表示只有明确标出的列才进行映射 不加默认全部映射
    public class UserInfo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }


        //[Ignore]  加这个就不会映射此字段 Column("qq") 为映射名，不写为一至
        [Column("qq")]
        public int Qq { get; set; }

      
        [Column("sonb")]
        public string sonb { get; set; }
    }




}