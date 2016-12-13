using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetroBlog.Core.Model.ViewModel;
using Nancy;
using Nancy.ModelBinding;
using MetroBlog.Core.Data.IService;
using Nancy.Authentication.Token;
using MetroBlog.Core.Data;
using System.Collections.Specialized;
using System.Net;
using MetroBlog.Core.Common;
using System.Threading;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core;

namespace MetroBlog.Admin
{
    public class AdminModule : NancyModule
    {

        #region service interface
        private IUserService userService { get; set; }
        private IArticleService articleService { get; set; }
        private ICategoryService categoryService { get; set; }
        private ITagService tagService { get; set; }
        private IMenuService menuService { get; set; }
        private ISettingService settingService { get; set; }
        #endregion
        public AdminModule(IUserService userService, IArticleService articleService, ICategoryService categoryService, ITagService tagService, IMenuService menuService, ISettingService settingService)
        {
            this.userService = userService;
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.menuService = menuService;
            this.settingService = settingService;


            this.Before.AddItemToEndOfPipeline(SetContextUserFromAuthenticationCookie);

            base.ModulePath = "api";
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
        private ITokenizer tokenizer { get; set; } = new Tokenizer();

        private Response SetContextUserFromAuthenticationCookie(NancyContext ctx)
        {

            if (ctx == null)
            {
                return null;
            }
            var token = Convert.ToString(Session["token"]);
            if (string.IsNullOrEmpty(token))
            {
                return Response.AsJson(Rsp.Error("unauthorized "));
            }
            var userIdentity = tokenizer.Detokenize(token, this.Context, new DefaultUserIdentityResolver());
            if (userIdentity != null && !string.IsNullOrEmpty(userIdentity.UserName))
            {
                return null;
            }
            return Response.AsJson(Rsp.Error("unauthorized "));

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
            var result = articleService.SelectArticleList(query, pageIndex, pageSize);
            return Response.AsJson(Rsp.Create(result));
        }
        public dynamic CategoryList()
        {
            return Response.AsJson(categoryService.SelectCategoryList().Where(x => !x.Delete).ToList());
        }
        public dynamic SaveCategory()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var categoryId = Convert.ToInt32(nvc["id"]);
            Rsp _rsp = null;
            if (categoryId > 0)
            {
                var categoryInfo = categoryService.SelectCategoryById(categoryId);
                categoryInfo.SetEntityValue(nvc);
                _rsp = categoryService.UpdateCategory(categoryInfo);
            }
            else
            {
                Category mCategory = new Category();
                mCategory.SetEntityValue(nvc);
                _rsp = categoryService.AddCategory(mCategory);

            }
            return Response.AsJson(_rsp);
        }

        public dynamic UpdateCategoryCount()
        {
            var categoryId = (int)Request.Form["id"];
            categoryService.UpdateCategoryArticleCount(categoryId);
            return Response.AsJson(Rsp.Success);
        }
        public dynamic SaveArticle()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            Article mArticle = new Article();
            mArticle.SetEntityValue(nvc);
            #region tag
            if (!string.IsNullOrEmpty(nvc["Tags"]))
            {
                var tags = nvc["Tags"].Split(',');
                foreach (var tag in tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        mArticle.Tags.Add(new Core.Model.ViewModel.Tag
                        {
                            TagName = tag
                        });
                    }
                }
            }
            #endregion
            Rsp _rsp = null;
            if (mArticle.Id > 0)
            {
                _rsp = articleService.UpdateArticle(mArticle);
            }
            else
            {
                _rsp = articleService.AddArticle(mArticle);
            }
            return Response.AsJson(_rsp);
        }

        public dynamic Tag()
        {
            int pageIndex = Request.Query["pageIndex"];
            int pageSize = Request.Query["pageSize"];
            var tagList = tagService.SelectTagList(pageIndex, pageSize);
            return Response.AsJson(tagList);
        }
        public dynamic EditTag()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var tagId = Convert.ToInt32(nvc["id"]);
            var mTag = tagService.SelectTagById(tagId);
            mTag.SetEntityValue(nvc);
            if (tagService.SelectTagByName(mTag.TagName) != null)
            {
                return Response.AsJson(Rsp.Error("当前标签已存在"));
            }
            tagService.UpdateTag(mTag);
            return Response.AsJson(Rsp.Success);
        }
        public dynamic RemoveTag()
        {
            int tagId = Request.Query["Id"];
            var mTag = tagService.SelectTagById(tagId);
            mTag.Delete = true;
            tagService.UpdateTag(mTag);
            return Response.AsJson(Rsp.Success);

        }

        public dynamic Menu()
        {
            return Response.AsJson(menuService.SelectMenuList());
        }
        public dynamic SaveMenu()
        {

            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var menuId = Convert.ToInt32(nvc["id"]);
            Rsp _rsp = null;
            if (menuId > 0)
            {
                var mMemu = menuService.SelectMenuById(menuId);
                mMemu.SetEntityValue(nvc);
                _rsp = menuService.UpdateMenu(mMemu);
            }
            else
            {
                var mMemu = new Menu();
                mMemu.SetEntityValue(nvc);
                _rsp = menuService.AddMenu(mMemu);

            }
            return Response.AsJson(_rsp);
        }

        public dynamic Setting()
        {
            var options = settingService.GetSetting();

            return Response.AsJson(options);
        }
        public dynamic SaveSetting()
        {
            var nvc = NancyDynamicDictionary.ToNameValueCollection((DynamicDictionary)Request.Form);
            var settingInfo = settingService.GetSetting();
            settingInfo.SetEntityValue(nvc);
            settingService.SaveSetting(settingInfo);
            return Response.AsJson(Rsp.Success);
        }
        #endregion

    }
}
