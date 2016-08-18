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
        IMenuService menuService;
        ICategoryService categoryService;
        IArticleService articleService;
        ISettingService settingService;
        #endregion
        public Blog(IMenuService menuService, ICategoryService categoryService, IArticleService articleService, ISettingService settingService)
        {
            this.menuService = menuService;
            this.categoryService = categoryService;
            this.articleService = articleService;
            this.settingService = settingService;
        }

        public PageInfo<IList<Article>> LoadArticle(ArticleQuery query, int pageIndex = 1, int pageSize = 10)
        {
            return articleService.SelectArticleList(query, pageIndex, pageSize);
        }

        public Article GetArticleById(int articleId)
        {
            return articleService.SelectArticleById(articleId);
        }

        public Setting Setting
        {
            get
            {
                return settingService.GetSetting();
            }
        }

        public IEnumerable<Category> Category
        {
            get
            {
                return categoryService.SelectCategoryList();
            }
        }

        public IEnumerable<Menu> Menu
        {
            get
            {
                return menuService.SelectMenuList();
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
