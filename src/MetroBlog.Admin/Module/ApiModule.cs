using System;
using System.IO;
using System.Linq;
using MetroBlog.Core.Model.ViewModel;
using Nancy;
using MetroBlog.Core.Data.IService;
using Nancy.Authentication.Token;
using MetroBlog.Core.Common;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core;

namespace MetroBlog.Admin.Module
{
    public class ApiModule : NancyModule
    {

        #region service interface
        private IArticleService ArticleService { get; set; }
        private ICategoryService CategoryService { get; set; }
        private ITagService TagService { get; set; }
        private IMenuService MenuService { get; set; }
        private ISettingService SettingService { get; set; }
        #endregion
        public ApiModule(IArticleService articleService, ICategoryService categoryService, ITagService tagService, IMenuService menuService, ISettingService settingService)
        {
            ArticleService = articleService;
            CategoryService = categoryService;
            TagService = tagService;
            MenuService = menuService;
            SettingService = settingService;


            Before.AddItemToEndOfPipeline(SetContextUserFromAuthenticationCookie);

            ModulePath = "api";
            #region  route
            Get["/"] = _ => Index();
            Get["/ArticleList"] = _ => ArticleList();
            Post["SaveArticle"] = _ => SaveArticle();

            Get["CategoryList"] = _ => CategoryList();
            Post["SaveCategory"] = _ => SaveCategory();
            Post["UpdateCategoryCount"] = _ => UpdateCategoryCount();
            Get["Tag"] = _ => Tag();
            Post["EditTag"] = _ => EditTag();
            Post["RemoveTag"] = _ => RemoveTag();

            Get["Menu"] = _ => Menu();
            Post["SaveMenu"] = _ => SaveMenu();

            Get["Setting"] = _ => Setting();
            Post["Setting"] = _ => SaveSetting();
            #endregion
        }

        #region auth
        private ITokenizer Tokenizer { get; set; } = new Tokenizer();

        private Response SetContextUserFromAuthenticationCookie(NancyContext ctx)
        {

            if (ctx == null)
            {
                return null;
            }
            var token = Convert.ToString(Session["token"]);
            if (string.IsNullOrEmpty(token))
            {
                return Response.AsJson(Rsp.Error("unauthorized"));
            }
            var userIdentity = Tokenizer.Detokenize(token, Context, new DefaultUserIdentityResolver());
            return !string.IsNullOrEmpty(userIdentity?.UserName) ? null : Response.AsJson(Rsp.Error("unauthorized "));
        }
        #endregion

        #region api action
        public dynamic Index()
        {
            return "it's working.";
        }

        public dynamic ArticleList()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Query);
            ArticleQuery query = new ArticleQuery();
            query.SetEntityValue(nvc);
            query.TagName = nvc["_tagName"];
            int pageIndex = Convert.ToInt32(nvc["pageIndex"]);
            int pageSize = Convert.ToInt32(nvc["pageSize"]);
            var result = ArticleService.SelectArticleList(query, pageIndex, pageSize);
            return Response.AsJson(Rsp.Create(result));
        }
        public dynamic CategoryList()
        {
            return Response.AsJson(CategoryService.SelectCategoryList().Where(x => !x.Delete).ToList());
        }
        public dynamic SaveCategory()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var categoryId = Convert.ToInt32(nvc["id"]);
            Rsp rsp;
            if (categoryId > 0)
            {
                var categoryInfo = CategoryService.SelectCategoryById(categoryId);
                categoryInfo.SetEntityValue(nvc);
                rsp = CategoryService.UpdateCategory(categoryInfo);
            }
            else
            {
                Category mCategory = new Category();
                mCategory.SetEntityValue(nvc);
                rsp = CategoryService.AddCategory(mCategory);

            }
            return Response.AsJson(rsp);
        }

        public dynamic UpdateCategoryCount()
        {
            var categoryId = (int)Request.Form["id"];
            CategoryService.UpdateCategoryArticleCount(categoryId);
            return Response.AsJson(Rsp.Success);
        }
        public dynamic SaveArticle()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var mArticle = new Article();
            mArticle.SetEntityValue(nvc);
            #region tag
            if (!string.IsNullOrEmpty(nvc["Tags"]))
            {
                var tags = nvc["Tags"].Split(',');
                foreach (var tag in tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        mArticle.Tags.Add(new Tag
                        {
                            TagName = tag
                        });
                    }
                }
            }
            #endregion
            var rsp = mArticle.Id > 0 ? ArticleService.UpdateArticle(mArticle) : ArticleService.AddArticle(mArticle);
            return Response.AsJson(rsp);
        }

        public dynamic Tag()
        {
            int pageIndex = Request.Query["pageIndex"];
            int pageSize = Request.Query["pageSize"];
            var tagList = TagService.SelectTagList(pageIndex, pageSize);
            return Response.AsJson(tagList);
        }
        public dynamic EditTag()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var tagId = Convert.ToInt32(nvc["id"]);
            var mTag = TagService.SelectTagById(tagId);
            mTag.SetEntityValue(nvc);
            if (TagService.SelectTagByName(mTag.TagName) != null)
            {
                return Response.AsJson(Rsp.Error("当前标签已存在"));
            }
            TagService.UpdateTag(mTag);
            return Response.AsJson(Rsp.Success);
        }
        public dynamic RemoveTag()
        {
            int tagId = Request.Query["Id"];
            var mTag = TagService.SelectTagById(tagId);
            mTag.Delete = true;
            TagService.UpdateTag(mTag);
            return Response.AsJson(Rsp.Success);

        }

        public dynamic Menu()
        {
            return Response.AsJson(MenuService.SelectMenuList());
        }
        public dynamic SaveMenu()
        {

            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var menuId = Convert.ToInt32(nvc["id"]);
            Rsp rsp;
            if (menuId > 0)
            {
                var mMemu = MenuService.SelectMenuById(menuId);
                mMemu.SetEntityValue(nvc);
                rsp = MenuService.UpdateMenu(mMemu);
            }
            else
            {
                var mMemu = new Menu();
                mMemu.SetEntityValue(nvc);
                rsp = MenuService.AddMenu(mMemu);

            }
            return Response.AsJson(rsp);
        }

        public dynamic Setting()
        {
            var options = SettingService.GetSetting();

            return Response.AsJson(options);
        }
        public dynamic SaveSetting()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var settingInfo = SettingService.GetSetting();
            settingInfo.SetEntityValue(nvc);
            SettingService.SaveSetting(settingInfo);
            return Response.AsJson(Rsp.Success);
        }

        public dynamic RestartApplication()
        {
            try
            {
                File.SetLastWriteTime(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.config"), DateTime.Now);
                return Response.AsJson(Rsp.Success);
            }
            catch (Exception e)
            {
                return Response.AsJson(Rsp.Error(e.Message));
            }
        }
        #endregion

    }
}
