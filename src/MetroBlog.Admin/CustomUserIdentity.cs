﻿using Nancy.Authentication.Token;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;

namespace MetroBlog.Admin
{
    public class CustomUserIdentity : IUserIdentity
    {

        public CustomUserIdentity(string userName)
        {

            this.UserName = userName;
            Claims = new List<string>();
        }
        public IEnumerable<string> Claims
        {
            get;
            private set;
        }

        public string UserName
        {
            get;
            private set;
        }
    }
}
