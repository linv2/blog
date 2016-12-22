using RazorEngine.Templating;

namespace MetroBlog.Template.Razor
{
    public class WebTemplate : TemplateBase<Page>
    {
        public WebTemplate()
        {
            Blog = Core.Blog.Current;
        }
        public string Title => Blog.Setting.Title;
        public string KeyWord => Blog.Setting.KeyWord;
        public string Description => Blog.Setting.Description;

        public System.Web.HttpRequest Request
        {
            get { return System.Web.HttpContext.Current.Request; }
        }

        public System.Web.HttpResponse Response
        {

            get { return System.Web.HttpContext.Current.Response; }
        }

        public Core.Blog Blog { get; }

        public string UrlBuild(string url)
        {
            return string.Concat("/themes/", SessionManage.ThemeName, "/", url);
        }
        
    }
}
