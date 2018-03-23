using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.Admin.MVC.Models;
using Web.Sys.Admin.MVC.Extensions;
using Core.PetaPocoServer;
using Web.Sys.BLL;
using Web.Sys.Model;
using Microsoft.AspNetCore.Authorization;
using e3net.Common;
using log4net;

namespace Web.Sys.Admin.MVC.Controllers
{
    public class LoginController : BaseController
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel mode)
        {
            try
            {
                List<v_userrole> adminRole = null;
                bool IsHaveP = false;//是否有权限登录
                mode.UserType = "1";
                #region  根据类型登录
                switch (mode.UserType)
                {
                    case "1"://账号密码登录
                        var sqllogin = new Sql("select * from v_userrole where loginname=@0 and password=@1 ", mode.LoginName, mode.Password);
                        adminRole = V_UserRoleService.Instance.Fetch(sqllogin);
                        break;
                    default:
                        break;
                }

                #endregion

                if ((adminRole != null && adminRole.Count > 0)) // 账号是否存在，添加权限配置
                {
                    #region 存用户数据
                    AdminUserInfo UserInfor = new AdminUserInfo();
                    UserInfor.UserTypes = adminRole[0].usertype;
                    UserInfor.Id = adminRole[0].id;
                    UserInfor.UserName = adminRole[0].loginname;
                    UserInfor.RoleId = adminRole[0].roleid;
                    UserInfor.Password = adminRole[0].password;
                    UserInfor.companyid = adminRole[0].companyid;
                    UserInfor.companyName = adminRole[0].companyName;
                    if (adminRole[0].departmentid != 0)
                    {
                        rms_department dpItem = RMS_DepartmentService.Instance.SingleW(" id='" + adminRole[0].departmentid + "'");
                        if (dpItem != null)
                        {
                            UserInfor.DepartmentId = dpItem.id;
                            UserInfor.DepartmentName = dpItem.name;
                        }
                    }
                    IsHaveP = true;

                    #endregion

                    #region  获取权限
                    if (IsHaveP)//可以登录
                    {
                        UserInfor.ListManusD = RMS_RoleService.Instance.GetRoleListManusD2(adminRole[0].jurisdiction);
                        if (UserInfor.ListManusD == null)
                        {
                            ViewData["IsShowAlert"] = true;
                            ModelState.AddModelError("Error", "无权限登录");
                        }
                        else
                        {
                            UserData = UserInfor;
                            return RedirectToAction("index", "home");
                        }
                    }
                    #endregion
                }
                else
                {
                    //  return RedirectToAction("index", "Login");
                    // 如果我们进行到这一步时某个地方出错，则重新显示表单
                    ModelState.AddModelError("Error", "账号或者密码有误");
                }
            }
            catch (Exception ex)
            {
                log.Error("Login error", ex);
            }                    
            return View();
        }

        /// 生成验证码图像对象
        /// </summary>
        /// <returns></returns>

        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            HttpContext.Session.Set<string>("ValidateCode",code);
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}