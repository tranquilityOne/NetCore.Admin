﻿using Core.PetaPocoServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoServer
{
      [SingleDbFactory("Postgresql")]
    public class UserInfoBLL: BaseDAL<UserInfo> 
    {



    }
}
