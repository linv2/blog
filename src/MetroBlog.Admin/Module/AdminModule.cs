using Nancy;
using Nancy.Authentication.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Admin.Module
{
    public class AdminModule : NancyModule
    {

        public AdminModule() : base("Admin")
        {
            Get["/Login"] = _ => Login();
            this.Before.AddItemToEndOfPipeline(SetContextUserFromAuthenticationCookie);
            Get["/"] = _ => Index();
            Get["/Main"] = _ => Index();
            Get["/Category"] = _ => Category();
            Get["/Article"] = _ => Article();
            Get["/ArticleList"] = _ => ArticleList();
            Get["/Tag"] = _ => Tag();
            Get["/Menu"] = _ => Menu();
            Get["/Setting/{key}"] = _ => Setting(_.key);
        }
        #region auth
        private ITokenizer tokenizer { get; set; } = new Tokenizer();

        private Response SetContextUserFromAuthenticationCookie(NancyContext ctx)
        {
            if (ctx == null)
            {
                return null;
            }
            if (ctx.Request.Path.StartsWith("/admin/login", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }
            var token = Convert.ToString(Session["token"]);
            if (string.IsNullOrEmpty(token))
            {
                return Response.AsRedirect("/admin/login");
            }
            var userIdentity = tokenizer.Detokenize(token, this.Context, new DefaultUserIdentityResolver());
            if (userIdentity != null && !string.IsNullOrEmpty(userIdentity.UserName))
            {
                return null;
            }
            return Response.AsRedirect("/admin/login");

        }
        #endregion
        public dynamic Login()
        {
            return View["Login"];
        }
        public dynamic Index()
        {
            return View["Index"];
        }
        public dynamic Category()
        {
            return View["Category"];
        }
        public dynamic Article()
        {
            return View["Article"];
        }
        public dynamic ArticleList()
        {
            return View["ArticleList"];
        }
        public dynamic Tag()
        {
            return View["Tag"];
        }
        public dynamic Menu()
        {
            return View["Menu"];
        }
        public dynamic Setting(string key)
        {
            return View["Setting", key];
        }
    }
}
