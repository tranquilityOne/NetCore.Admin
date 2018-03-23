using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class V_MenuButtonsService : BaseDAL<v_menubuttons>
    {
        #region 单例实现
        private static V_MenuButtonsService singleInstance;

        private static object obj = new object();

        private V_MenuButtonsService() { }

        public static V_MenuButtonsService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new V_MenuButtonsService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}
