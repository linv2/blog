using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetroBlog.Core
{
    public class Blog
    {
        #region service

        private readonly IMenuService _menuService;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ISettingService _settingService;
        #endregion
        public Blog(IMenuService menuService, ICategoryService categoryService, IArticleService articleService, ISettingService settingService)
        {
            this._menuService = menuService;
            this._categoryService = categoryService;
            this._articleService = articleService;
            this._settingService = settingService;
        }

        public PageInfo<IList<Article>> LoadArticle(ArticleQuery query, int pageIndex = 1, int pageSize = 10)
        {
            return _articleService.SelectArticleList(query, pageIndex, pageSize);
        }

        public Article GetArticleById(int articleId)
        {
            return _articleService.SelectArticleById(articleId);
        }

        public Setting Setting
        {
            get
            {
                return _settingService.GetSetting();
            }
        }

        public IEnumerable<Category> Category
        {
            get
            {
                return _categoryService.SelectCategoryList();
            }
        }

        public IEnumerable<Menu> Menu
        {
            get
            {
                return _menuService.SelectMenuList();
            }
        }



        static Blog()
        {
            Current = CoreIoCContainer.Current.Resolve<Blog>();
        }
        /// <summary>
        /// 获取当前Blog
        /// </summary>
        public static Blog Current
        {
            get;
            private set;
        }
    }
}
