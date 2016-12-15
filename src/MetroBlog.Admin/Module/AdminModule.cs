using Nancy;
using Nancy.Authentication.Token;
using System;
using MetroBlog.Core;

namespace MetroBlog.Admin.Module
{
    public class AdminModule : NancyModule
    {

        public AdminModule() : base("Admin")
        {
            ModulePath = Blog.Current.Setting.AdminPath;
            Get["/Login"] = _ => Login();
            Before.AddItemToEndOfPipeline(SetContextUserFromAuthenticationCookie);
            Get["/"] = _ => Index();
            Get["/Main"] = _ => Index();
            Get["/Category"] = _ => Category();
            Get["/Article"] = _ => Article();
            Get["/ArticleList"] = _ => ArticleList();
            Get["/Tag"] = _ => Tag();
            Get["/Menu"] = _ => Menu();
            Get["/Themes"] = _ => Themes();
            Get["ThemeItem"] = _ => ThemeItem();
            Get["/EditTheme"] = _ => EditTheme();
            Get["/Setting"] = _ => Setting();
        }
        #region auth
        private ITokenizer Tokenizer { get; set; } = new Tokenizer();

        private Response SetContextUserFromAuthenticationCookie(NancyContext ctx)
        {
            if (ctx == null)
            {
                return null;
            }
            var manageLoginUrl = string.Concat("/", Blog.Current.Setting.AdminPath, "/", "login");
            if (ctx.Request.Path.StartsWith(manageLoginUrl, StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }
            var token = Convert.ToString(Session["token"]);
            if (string.IsNullOrEmpty(token))
            {
                return Response.AsRedirect(manageLoginUrl);
            }
            var userIdentity = Tokenizer.Detokenize(token, this.Context, new DefaultUserIdentityResolver());
            if (!string.IsNullOrEmpty(userIdentity?.UserName))
            {
                return null;
            }
            return Response.AsRedirect(manageLoginUrl);

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
        public dynamic Setting()
        {
            return View["Setting"];
        }
        public dynamic Themes()
        {
            return View["Themes"];
        }
        public dynamic ThemeItem()
        {
            return View["ThemeItem"];
        }
        public dynamic EditTheme()
        {
            return View["EditTheme"];
        }
    }
}
