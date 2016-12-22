using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IService;
using MetroBlog.Core.Model.QueryModel;
using MetroBlog.Core.Model.ViewModel;
using System.Collections.Generic;
using MetroBlog.Core.Cache;

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
            _menuService = menuService;
            _categoryService = categoryService;
            _articleService = articleService;
            _settingService = settingService;
        }

        public PageInfo<IList<Article>> LoadArticle(ArticleQuery query, int pageIndex = 1, int pageSize = 10)
        {
            return _articleService.SelectArticleList(query, pageIndex, pageSize);
        }

        public Article GetArticleById(int articleId)
        {
            return _articleService.SelectArticleById(articleId);
        }

        public Category GetCategoryById(int categoryId)
        {
            return _categoryService.SelectCategoryById(categoryId);
        }
        public Category GetCategoryByAlias(string alias)
        {
            return _categoryService.SelectCategoryByAlias(alias);
        }

        public Setting Setting => _settingService.GetSetting();

        public IEnumerable<Category> Category => _categoryService.SelectCategoryList();

        public IEnumerable<Menu> Menu => _menuService.SelectMenuList();


        static Blog()
        {
            Cache = CoreIoCContainer.Current.Resolve<ICache>();
            Current = CoreIoCContainer.Current.Resolve<Blog>();
        }
        /// <summary>
        /// 获取当前Blog
        /// </summary>
        public static Blog Current
        {
            get;
        }

        public static ICache Cache { get; }
    }
}
