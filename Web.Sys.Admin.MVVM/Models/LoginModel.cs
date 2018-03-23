using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web.Sys.Admin.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string LoginName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }


        [Required]
        [Display(Name = "验证码")]
        public string VCode { get; set; }
        /// <summary>
        /// 用户类型 
        /// </summary>
        public string UserType { get; set; }
    }
}