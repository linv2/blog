using Nancy;
using Nancy.Authentication.Token;
using Nancy.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Admin
{
    public class FormAuthentication
    {
        private NancyContext context { get; set; }
        private ITokenizer tokenizer { get; set; }

        public string Identity { get; private set; }
        public const string CookieName = "LOGINTOKEN";
        public FormAuthentication(NancyContext context)
        {
            tokenizer = new Tokenizer();
            this.context = context;
        }
        public void WriteToken(string Identity, DateTime? Expires = null)
        {


            var userIdentity = new DefaultUserIdentity(Identity);
            var token = tokenizer.Tokenize(userIdentity, context);
            var cookie = new NancyCookie(FormAuthentication.CookieName, token);
            cookie.Expires = Expires;
            context.Response.Cookies.Add(cookie);
        }
        public bool CheckAuth()
        {
            var value = string.Empty;
            if (context.Request.Cookies.TryGetValue(CookieName, out value))
            {
                var userIdentity = tokenizer.Detokenize(value, context, new DefaultUserIdentityResolver());
                if (userIdentity != null && string.IsNullOrEmpty(userIdentity.UserName))
                {
                    Identity = userIdentity.UserName;
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
