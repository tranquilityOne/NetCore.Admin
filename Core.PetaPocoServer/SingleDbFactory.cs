using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PetaPocoServer
{
    /// <summary>
    /// 类的特性描述特性(单体）
    /// </summary> 
    [AttributeUsage(AttributeTargets.Class)]
    public class SingleDbFactory : Attribute
    { 
        public readonly string ConnectionName;

        public SingleDbFactory(string connectionName)
        {
            ConnectionName = connectionName;
        }

        /// <summary>
        /// 获取类的描述值的第一个
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetKeyFrom(object target)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var objectType = target.GetType();
            var attributes = objectType.GetCustomAttributes(typeof(SingleDbFactory), true);
            if (attributes.Length > 0)
            {
                var attribute = (SingleDbFactory)attributes[0];              
                return attribute.ConnectionName;
            }
            return "DefaultDB";
        }
    }
}
