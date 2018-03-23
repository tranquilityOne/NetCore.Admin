using Core.PetaPocoServer;
using Web.Sys.Model;

namespace Web.Sys.BLL
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_DepartmentService : BaseDAL<rms_department>
    {
        #region 单例实现
        private static RMS_DepartmentService singleInstance;

        private static object obj = new object();

        private RMS_DepartmentService()
        {

        }

        public static RMS_DepartmentService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_DepartmentService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}
