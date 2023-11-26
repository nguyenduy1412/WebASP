using QLHS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHS.App_Start
{
    public static class SessionConfig
    {
        
            public static void SetUser(users user)
            {
                HttpContext.Current.Session["user"] = user;
            }
            public static users GetUser()
            {
                return (users)HttpContext.Current.Session["user"];
            }
        
    }
}