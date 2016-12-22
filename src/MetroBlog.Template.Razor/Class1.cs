using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetroBlog.Core.Model.QueryModel;

namespace MetroBlog.Web
{
    public class Class1 : MetroBlog.Template.Razor.WebTemplate
    {



        public Class1()
        {
        }

        public override void Execute()
        {

            Layout = "_Layout.cshtml";

            WriteLiteral("\r\n");

            Write(ViewBag.Title);

            WriteLiteral("\r\n");


            var alias = Request.Path.Remove(0, 1);
            var categoryInfo = Blog.GetCategoryByAlias(alias);
            if (categoryInfo == null)
            {
                Model.Response.StatusCode = 404;
                return;
            }
            ViewBag.Title += ("---" + categoryInfo.Name);
            ViewBag.KeyWord = categoryInfo.KeyWord;
            ViewBag.Description = categoryInfo.Description;

            WriteLiteral("\r\n");

            Write(ViewBag.Title);

            WriteLiteral("\r\n<div");

            WriteLiteral(" class=\"posts\"");

            WriteLiteral(">\r\n    <h1");

            WriteLiteral(" class=\"content-subhead\"");

            WriteLiteral(">日志列表</h1>\r\n");







            var query = new ArticleQuery();
            query.CategoryId = categoryInfo.Id;
            var result = Blog.LoadArticle(query);

            WriteLiteral("\r\n");


            foreach (var aticleItem in result.Data)
            {

                WriteLiteral("        <section");

                WriteLiteral(" class=\"post\"");

                WriteLiteral(">\r\n            <header");

                WriteLiteral(" class=\"post-header\"");

                WriteLiteral(">\r\n                <!--<img class=\"post-avatar\" alt=\"1111\" height=\"48\" width=\"48\"" +
                " src=\"blog/img/common/1111-avatar.jpg\">-->\r\n\r\n                <h2");

                WriteLiteral(" class=\"post-title\"");

                WriteLiteral("><a");

                WriteAttribute("href", Tuple.Create(" href=\"", 1079), Tuple.Create("\"", 1112)
                , Tuple.Create(Tuple.Create("", 1086), Tuple.Create("/Article?id=", 1086), true)
                , Tuple.Create(Tuple.Create("", 1098), Tuple.Create<System.Object, System.Int32>(aticleItem.Id
                , 1098), false)
                );

                WriteLiteral(">");

                Write(aticleItem.Title);

                WriteLiteral("</a></h2>\r\n\r\n                <p");

                WriteLiteral(" class=\"post-meta\"");

                WriteLiteral(">\r\n                    <a");

                WriteLiteral(" class=\"post-author\"");

                WriteLiteral(" href=\"javascript:;\"");

                WriteLiteral(">");

                Write(Title);

                WriteLiteral("</a> 发表于 ");

                Write(aticleItem.CreateTime.ToString("yyyy-MM-dd HH:mm"));

                WriteLiteral("\r\n");


                foreach (var tagItem in aticleItem.Tags)
                {

                    WriteLiteral("                        <a");

                    WriteLiteral(" class=\"post-category post-category-design\"");

                    WriteLiteral(" href=\"javascript:;\"");

                    WriteLiteral(">");

                    Write(tagItem.TagName);

                    WriteLiteral("</a>\r\n");

                }

                WriteLiteral("                </p>\r\n            </header>\r\n\r\n            <div");

                WriteLiteral(" class=\"post-description\"");

                WriteLiteral(">\r\n                <p>\r\n");

                WriteLiteral("                    ");

                Write(aticleItem.Description);

                WriteLiteral("\r\n                </p>\r\n            </div>\r\n        </section>\r\n");

            }

            WriteLiteral("</div>");

        }
    }
}
